using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProfile
{
	private bool _unlocked;

	private int _upgradeLevel;

	public CarProfile()
	{
		_unlocked = false;
		_upgradeLevel = 0;
	}

	public CarProfile(string json)
	{
		Load(json);
	}

	public string ToJson()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Type"] = GetType().ToString();
		hashtable["Unlocked"] = _unlocked;
		hashtable["UpgradeLevel"] = _upgradeLevel;
		return hashtable.toJson();
	}

	public void Load(string json)
	{
		Hashtable data = json.hashtableFromJson();
		_unlocked = JsonUtil.ExtractBool(data, "Unlocked", def: false);
		_upgradeLevel = JsonUtil.ExtractInt(data, "UpgradeLevel", 0);
	}

	public bool IsUnlocked()
	{
		return _unlocked;
	}

	public int GetUpgradeLevel()
	{
		return _upgradeLevel;
	}

	public Price GetUpgradePrice(Car car)
	{
		return AutoSingleton<CarDatabase>.Instance.GetUpgradePrice(_upgradeLevel, car.Category);
	}

	public Price GetUpgradePrice(Car car, int level)
	{
		return AutoSingleton<CarDatabase>.Instance.GetUpgradePrice(level - 1, car.Category);
	}

	public bool CanUpgrade()
	{
		return _upgradeLevel < 5;
	}

	public bool TryUpgrade(Car car)
	{
		Price upgradePrice = GetUpgradePrice(car);
		if (CanUpgrade() && AutoSingleton<CashManager>.Instance.CanBuy(upgradePrice))
		{
			AutoSingleton<CashManager>.Instance.Buy(upgradePrice);
			_upgradeLevel++;
			return true;
		}
		return false;
	}

	public void UnlockForFree()
	{
		UnlockCar();
	}

	public bool TryUnlock(Price price)
	{
		List<Price> list = new List<Price>();
		list.Add(price);
		return TryUnlock(list);
	}

	public bool TryUnlock(List<Price> prices)
	{
		if (_unlocked)
		{
			UnityEngine.Debug.Log("Already unlocked");
			return false;
		}
		if (AutoSingleton<CashManager>.Instance.CanBuy(prices))
		{
			AutoSingleton<CashManager>.Instance.Buy(prices);
			UnlockCar();
			return true;
		}
		UnityEngine.Debug.Log("Not enough cash");
		return false;
	}

	private void UnlockCar()
	{
		_unlocked = true;
		AutoSingleton<AchievementsManager>.Instance.CheckUnlockCarAchievement();
	}
}
