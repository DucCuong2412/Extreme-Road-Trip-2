using System.Collections.Generic;
using UnityEngine;

public class CarManager : AutoSingleton<CarManager>
{
	private Dictionary<Car, CarProfile> _carProfileCache = new Dictionary<Car, CarProfile>();

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			if (allCar.IsFree())
			{
				CarProfile carProfile = GetCarProfile(allCar);
				carProfile.UnlockForFree();
			}
		}
		AutoSingleton<AchievementsManager>.Instance.CheckUnlockCarAchievement();
		base.OnAwake();
	}

	public void UnlockCarForFree(int rank)
	{
		Car carByRank = AutoSingleton<CarDatabase>.Instance.GetCarByRank(rank);
		if (carByRank != null)
		{
			CarProfile carProfile = GetCarProfile(carByRank);
			carProfile.UnlockForFree();
			SaveCarProfile(carByRank, carProfile);
		}
	}

	private bool IsCarForSale(Car car)
	{
		if (car.Category == CarCategory.notForSale)
		{
			return false;
		}
		if (car.Category == CarCategory.pocketMine)
		{
			return AutoSingleton<PocketMine2PromoManager>.Instance.AreCarsAvailable();
		}
		if (car.Category == CarCategory.pocketMine3)
		{
			return AutoSingleton<PocketMine3PromoManager>.Instance.AreCarsAvailable();
		}
		if (car.Category == CarCategory.fishingBreak)
		{
			return AutoSingleton<FishingBreakPromoManager>.Instance.AreCarsAvailable();
		}
		if (car.Category == CarCategory.prt)
		{
			return AutoSingleton<PRTPromoManager>.Instance.AreCarsAvailable();
		}
		return true;
	}

	public List<Car> GetAllForSaleCars()
	{
		List<Car> list = new List<Car>();
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			if (IsCarForSale(allCar))
			{
				list.Add(allCar);
			}
		}
		return list;
	}

	public List<string> GetAllUnlockedCarsName()
	{
		List<string> list = new List<string>();
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			if (GetCarProfile(allCar).IsUnlocked())
			{
				list.Add(allCar.Id);
			}
		}
		return list;
	}

	public List<Car> GetAllUnlockedForSaleCars()
	{
		List<Car> list = new List<Car>();
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			if (IsCarForSale(allCar) && GetCarProfile(allCar).IsUnlocked())
			{
				list.Add(allCar);
			}
		}
		return list;
	}

	public Car GetCar(string carName)
	{
		Car car = AutoSingleton<CarDatabase>.Instance.GetCar(carName);
		if (car != null)
		{
			return car;
		}
		UnityEngine.Debug.LogWarning("Unknown car: " + carName);
		return GetRandomCar();
	}

	public Car GetRandomCar()
	{
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		return allCars[Random.Range(0, allCars.Count)];
	}

	public Car GetRandomUnlockedCar()
	{
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		List<Car> list = allCars.FindAll((Car c) => GetCarProfile(c).IsUnlocked() && c.Category != CarCategory.notForSale);
		return (list.Count <= 0) ? GetRandomCar() : list[Random.Range(0, list.Count)];
	}

	public Car GetRandomLockedCar()
	{
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		List<Car> list = allCars.FindAll((Car c) => !GetCarProfile(c).IsUnlocked() && c.Category != CarCategory.notForSale);
		return (list.Count <= 0) ? GetRandomCar() : list[Random.Range(0, list.Count)];
	}

	public int GetNumAffordableCars(int coins, int bucks, int prestigeTokens)
	{
		int num = 0;
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		foreach (Car item in allCars)
		{
			bool flag = true;
			foreach (Price price in item.Prices)
			{
				if (GetCarProfile(item).IsUnlocked() || (price.IsCoins() && price.Amount > coins) || (price.IsBucks() && price.Amount > bucks) || (price.IsPrestigeTokens() && price.Amount > prestigeTokens))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				num++;
			}
		}
		return num;
	}

	private string CarProfilePrefId(Car car)
	{
		return car.Id + "Profile";
	}

	public CarProfile GetCarProfile(Car car)
	{
		if (!_carProfileCache.ContainsKey(car))
		{
			string @string = Preference.GetString(CarProfilePrefId(car), string.Empty);
			if (@string == string.Empty)
			{
				_carProfileCache[car] = new CarProfile();
			}
			else
			{
				_carProfileCache[car] = new CarProfile(@string);
			}
		}
		return _carProfileCache[car];
	}

	public void SaveCarProfile(Car car, CarProfile carProfile)
	{
		string v = carProfile.ToJson();
		Preference.SetString(CarProfilePrefId(car), v);
		Preference.Save();
	}

	public void Refresh()
	{
		foreach (KeyValuePair<Car, CarProfile> item in _carProfileCache)
		{
			string @string = Preference.GetString(CarProfilePrefId(item.Key), string.Empty);
			if (item.Value != null)
			{
				item.Value.Load(@string);
			}
		}
	}
}
