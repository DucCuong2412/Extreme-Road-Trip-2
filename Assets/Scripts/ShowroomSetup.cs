using System.Collections;
using System.Collections.Generic;

public class ShowroomSetup
{
	public List<CarLevel> Cars
	{
		get;
		private set;
	}

	public Showroom Showroom
	{
		get;
		private set;
	}

	public string OwnerName
	{
		get;
		set;
	}

	public bool LocalShowroom
	{
		get;
		set;
	}

	public ShowroomSetup()
	{
		Init();
	}

	public ShowroomSetup(string json)
	{
		Init();
		Hashtable hashtable = json.hashtableFromJson();
		if (hashtable != null)
		{
			string a = JsonUtil.ExtractString(hashtable, "Type", string.Empty);
			if (a == "Showroom")
			{
				string text = JsonUtil.ExtractString(hashtable, "Name", string.Empty);
				if (text != string.Empty)
				{
					SetShowroom(AutoSingleton<ShowroomDatabase>.Instance.GetShowroom(text));
					Cars.Clear();
				}
				if (hashtable.ContainsKey("Cars"))
				{
					ArrayList arrayList = hashtable["Cars"] as ArrayList;
					foreach (object item in arrayList)
					{
						Car car = null;
						int level = 0;
						if (item != null && item is Hashtable)
						{
							Hashtable data = item as Hashtable;
							string carName = JsonUtil.ExtractString(data, "Name", string.Empty);
							car = AutoSingleton<CarManager>.Instance.GetCar(carName);
							level = JsonUtil.ExtractInt(data, "Level", 0);
						}
						Cars.Add(new CarLevel(car, level));
					}
				}
			}
		}
		FillEmptySlot();
	}

	private void Init()
	{
		Cars = new List<CarLevel>();
		SetShowroom(AutoSingleton<ShowroomDatabase>.Instance.GetAllShowroom()[0]);
		OwnerName = string.Empty;
		LocalShowroom = true;
	}

	private void FillEmptySlot()
	{
		int num = Showroom.SlotCount();
		while (Cars.Count < num)
		{
			Cars.Add(new CarLevel(null, 0));
		}
	}

	public void SetShowroom(Showroom showroom)
	{
		if (!(Showroom != showroom))
		{
			return;
		}
		Showroom = showroom;
		int num = Cars.Count - 1;
		while (num >= 0 && Cars.Count > Showroom.SlotCount())
		{
			CarLevel carLevel = Cars[num];
			if (carLevel._car == null)
			{
				Cars.RemoveAt(num);
			}
			num--;
		}
		int num2 = Cars.Count - Showroom.SlotCount();
		if (num2 > 0)
		{
			Cars.RemoveRange(Showroom.SlotCount(), num2);
		}
	}

	public string ToJson()
	{
		Hashtable hashtable = new Hashtable();
		ArrayList arrayList = new ArrayList();
		foreach (CarLevel car2 in Cars)
		{
			CarLevel current = car2;
			Hashtable hashtable2 = null;
			Car car = current._car;
			if (car != null)
			{
				hashtable2 = new Hashtable();
				hashtable2["Name"] = car.Id;
				hashtable2["Level"] = current._level;
			}
			arrayList.Add(hashtable2);
		}
		hashtable["Type"] = "Showroom";
		hashtable["Name"] = Showroom.name;
		hashtable["Cars"] = arrayList;
		return hashtable.toJson();
	}

	public int GetValue()
	{
		int num = (!Showroom.Price.IsBucks()) ? 1 : 10;
		int num2 = GetValueFromPrice(Showroom.Price) * num;
		foreach (CarLevel car in Cars)
		{
			CarLevel current = car;
			if (current._car != null)
			{
				int level = (!LocalShowroom) ? current._level : AutoSingleton<CarManager>.Instance.GetCarProfile(current._car).GetUpgradeLevel();
				num2 += GetValueFromPrice(current._car.Prices);
				num2 += GetCarUpgradeValue(current._car, level);
			}
		}
		return num2;
	}

	private int GetValueFromPrice(Price price)
	{
		List<Price> list = new List<Price>();
		list.Add(price);
		return GetValueFromPrice(list);
	}

	private int GetValueFromPrice(List<Price> prices)
	{
		int num = 0;
		foreach (Price price in prices)
		{
			num += ((!price.IsCoins()) ? ((!price.IsBucks()) ? (price.Amount * 1000) : (price.Amount * 300)) : price.Amount);
		}
		return num;
	}

	private int GetCarUpgradeValue(Car car, int level)
	{
		int num = 0;
		CarDatabase instance = AutoSingleton<CarDatabase>.Instance;
		for (int i = 0; i < level; i++)
		{
			num += GetValueFromPrice(instance.GetUpgradePrice(i, car.Category));
		}
		return num;
	}

	public void SetCar(Car car, int selectedSlotIndex)
	{
		CarLevel item = new CarLevel(car, AutoSingleton<CarManager>.Instance.GetCarProfile(car).GetUpgradeLevel());
		int num = Cars.IndexOf(item);
		if (selectedSlotIndex != num)
		{
			if (Showroom.SlotCount() > 1 && num != -1)
			{
				SwapPosition(num, selectedSlotIndex);
				return;
			}
			Cars.RemoveAt(selectedSlotIndex);
			Cars.Insert(selectedSlotIndex, item);
		}
	}

	private void SwapPosition(int oldPosition, int newPosition)
	{
		List<CarLevel> cars = Cars;
		CarLevel value = cars[newPosition];
		cars[newPosition] = cars[oldPosition];
		cars[oldPosition] = value;
	}
}
