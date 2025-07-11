using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDatabase : AutoSingleton<CarDatabase>
{
	private List<Car> _allCars;

	private Dictionary<string, Car> _carCache;

	private Dictionary<CarCategory, List<Price>> _upgradePrices;

	protected override void OnAwake()
	{
		_allCars = new List<Car>();
		_carCache = new Dictionary<string, Car>();
		LoadCarsFromCSV();
		_allCars.Sort();
		_upgradePrices = new Dictionary<CarCategory, List<Price>>();
		LoadUpgradePrices();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.OnAwake();
	}

	private void LoadCarsFromCSV()
	{
		CSVData cSVData = new CSVData("cars.csv");
		int count = cSVData.Count;
		for (int i = 0; i < count; i++)
		{
			int @int = cSVData.GetInt(i, "rank", -1);
			if (@int != -1)
			{
				string @string = cSVData.GetString(i, "prefab");
				Car car = new Car(@string);
				car.Rank = @int;
				car.DisplayName = cSVData.GetString(i, "displayName");
				car.Description = cSVData.GetString(i, "description");
				string string2 = cSVData.GetString(i, "price", string.Empty);
				Hashtable hashtable = string2.hashtableFromJson();
				if (hashtable != null)
				{
					car.Prices.Add(new Price(hashtable));
				}
				if (car.Prices.Count == 0)
				{
					UnityEngine.Debug.LogWarning("Something went wrong when loading " + car.DisplayName);
					car.Prices.Add(new Price(10000, Currency.bucks));
				}
				switch (cSVData.GetInt(i, "category"))
				{
				case 8:
					car.Category = CarCategory.pocketMine3;
					break;
				case 7:
					car.Category = CarCategory.fishingBreak;
					break;
				case 6:
					car.Category = CarCategory.prt;
					break;
				case 5:
					car.Category = CarCategory.pocketMine;
					break;
				case 4:
					car.Category = CarCategory.prestige;
					break;
				case 3:
					car.Category = CarCategory.super;
					break;
				case 2:
					car.Category = CarCategory.notForSale;
					break;
				case 1:
					car.Category = CarCategory.soldForBucks;
					break;
				default:
					car.Category = CarCategory.soldForCoins;
					break;
				}
				car.GasLevel = cSVData.GetInt(i, "gas");
				car.SpeedLevel = cSVData.GetInt(i, "speed");
				car.FlipLevel = cSVData.GetInt(i, "flip");
				car.BoostLevel = cSVData.GetInt(i, "boost");
				car.SlamLevel = cSVData.GetInt(i, "slam");
				car.MaxSpeed = cSVData.GetFloat(i, "maxSpeed", 0f);
				car.Mass = cSVData.GetFloat(i, "mass", 0f);
				car.SuspensionDistance = cSVData.GetFloat(i, "suspensionDistance", 0f);
				car.SuspensionSpring = cSVData.GetFloat(i, "suspensionSpring", 0f);
				car.SuspensionDamper = cSVData.GetFloat(i, "suspensionDamper", 0f);
				_allCars.Add(car);
				_carCache[@string] = car;
			}
		}
	}

	private void LoadUpgradePrices()
	{
		TextAsset textAsset = Resources.Load("upgrades.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			Hashtable data = textAsset.text.hashtableFromJson();
			foreach (int value in Enum.GetValues(typeof(CarCategory)))
			{
				ArrayList arrayList = JsonUtil.ExtractArrayList(data, ((CarCategory)value).ToString(), new ArrayList());
				_upgradePrices[(CarCategory)value] = new List<Price>();
				foreach (Hashtable item in arrayList)
				{
					_upgradePrices[(CarCategory)value].Add(new Price(item));
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file upgrades.json");
		}
	}

	public Price GetUpgradePrice(int level, CarCategory category)
	{
		if (_upgradePrices != null && Enum.IsDefined(typeof(CarCategory), category) && _upgradePrices[category] != null && _upgradePrices[category].Count > level)
		{
			return _upgradePrices[category][level];
		}
		return new Price(1000, Currency.coins);
	}

	public List<Car> GetAllCars()
	{
		return _allCars;
	}

	public Car GetCar(string carName)
	{
		if (_carCache.ContainsKey(carName))
		{
			return _carCache[carName];
		}
		return null;
	}

	public bool ContainsCar(string carName)
	{
		return _carCache.ContainsKey(carName);
	}

	public Car GetCarByRank(int rank)
	{
		return _allCars.Find((Car c) => c.Rank == rank);
	}
}
