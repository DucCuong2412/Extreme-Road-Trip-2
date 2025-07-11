using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestigeManager : AutoSingleton<PrestigeManager>
{
	public enum LogPrestigeEventName
	{
		popupPreConfPrestige,
		popupConfPrestige
	}

	private const int _numberOfPrestigeLevelMax = 2;

	private const int _firstPrestigeLevelTokenAmount = 2;

	private const int _secondPrestigeLevelTokenAmount = 1;

	private const string _carsPrestigeLevelKey = "PrestigeLevels";

	private const int _firstPrestigeLevelPointAmount = 2;

	private const int _secondPrestigeLevelPointAmount = 1;

	private bool _isDestroyed;

	private Dictionary<Car, int> _carsPrestigeLevel;

	private PrestigeDatabase _database;

	protected override void OnAwake()
	{
		_isDestroyed = false;
		_database = new PrestigeDatabase();
		_carsPrestigeLevel = new Dictionary<Car, int>();
		BuildPrestigeDictionary();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.OnAwake();
	}

	public override void Destroy()
	{
		_isDestroyed = true;
		base.Destroy();
	}

	private void BuildPrestigeDictionary()
	{
		string json = _database.Load();
		Hashtable hashtable = json.hashtableFromJson();
		Hashtable carsPrestigeLevelSaved = null;
		if (hashtable != null && hashtable.ContainsKey("PrestigeLevels"))
		{
			carsPrestigeLevelSaved = (hashtable["PrestigeLevels"] as Hashtable);
		}
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		foreach (Car item in allCars)
		{
			BuildCarPrestigeLevels(item, carsPrestigeLevelSaved);
		}
	}

	private void BuildCarPrestigeLevels(Car car, Hashtable carsPrestigeLevelSaved)
	{
		_carsPrestigeLevel[car] = 0;
		if (carsPrestigeLevelSaved != null && carsPrestigeLevelSaved.ContainsKey(car.Id))
		{
			_carsPrestigeLevel[car] = JsonUtil.ExtractInt(carsPrestigeLevelSaved, car.Id, 0);
		}
	}

	public int GetCarPrestigeLevel(Car currentCar)
	{
		return _carsPrestigeLevel[currentCar];
	}

	public int GetPrestigeMaximumLevel()
	{
		return 2;
	}

	public string GetPrestigeLevelBadge(int prestigeLevel)
	{
		string result = string.Empty;
		switch (prestigeLevel)
		{
		case 1:
			result = MetroSkin.SpriteCardPrestigeBadge1;
			break;
		case 2:
			result = MetroSkin.SpriteCardPrestigeBadge2;
			break;
		}
		return result;
	}

	public bool IsNextPrestigeLevelMax(Car currentCar)
	{
		return GetCarPrestigeLevel(currentCar) == 1;
	}

	public bool IsCarPrestigeAvailable(Car currentCar)
	{
		return GetCarPrestigeLevel(currentCar) < 2 && AutoSingleton<MissionManager>.Instance.AreAllMissionsCompleted(currentCar);
	}

	public int GetPrestigeLevel(Car car)
	{
		return _carsPrestigeLevel[car];
	}

	public void IncrementPrestigeLevel(Car currentCar)
	{
		if (_isDestroyed)
		{
			return;
		}
		int num = _carsPrestigeLevel[currentCar];
		if (num < 2)
		{
			Dictionary<Car, int> carsPrestigeLevel;
			Dictionary<Car, int> dictionary = carsPrestigeLevel = _carsPrestigeLevel;
			Car key;
			Car key2 = key = currentCar;
			int num2 = carsPrestigeLevel[key];
			num2 = (dictionary[key2] = num2 + 1);
			int num4 = num2;
			AutoSingleton<CashManager>.Instance.AddPrestigeTokens(GetPrestigeLevelTokenReward(currentCar));
			int prestigePoints = AutoSingleton<Player>.Instance.Profile.PrestigePoints;
			prestigePoints += GetPrestigePointsReward(currentCar);
			AutoSingleton<Player>.Instance.Profile.OnPrestigePointsChanged(prestigePoints);
			AutoSingleton<LeaderboardsManager>.Instance.SubmitPrestigePoints(prestigePoints);
			if (num4 < 2)
			{
				AutoSingleton<MissionManager>.Instance.ResetAllMissions(currentCar);
			}
			_database.Save(ToJson());
		}
	}

	private int GetPrestigeLevelTokenReward(Car car)
	{
		int num = 0;
		if (_carsPrestigeLevel[car] == 1)
		{
			return 2;
		}
		return 1;
	}

	private int GetPrestigePointsReward(Car car)
	{
		int num = 0;
		if (_carsPrestigeLevel[car] == 1)
		{
			return 2;
		}
		return 1;
	}

	public List<Reward> GetRewards(Car car)
	{
		List<Reward> list = new List<Reward>();
		if (_carsPrestigeLevel[car] == 1)
		{
			list.Add(new Reward(RewardType.prestigeBadge1, 1));
		}
		else
		{
			list.Add(new Reward(RewardType.prestigeBadge2, 1));
		}
		list.Add(new Reward(RewardType.prestigeTokens, GetPrestigeLevelTokenReward(car)));
		return list;
	}

	public void ShowPrestigeTokenPopupSequence(Car car, Action OnDismissedPrestigeRewardPopup, string menuNameData)
	{
		if (AutoSingleton<PrestigeManager>.Instance.IsCarPrestigeAvailable(car))
		{
			string empty = string.Empty;
			empty = ((!AutoSingleton<PrestigeManager>.Instance.IsNextPrestigeLevelMax(car)) ? (empty + "Performing a Prestige will reset your missions completion for this car to 0%.".Localize() + " " + "Are you sure you want to continue?".Localize()) : (empty + "This is the last Prestige level for this car. Your missions completion percentage will remain at 100%.".Localize() + " " + "Are you sure you want to continue?".Localize()));
			MetroMenuPopupYesNoLater metroMenuPopupYesNoLater = MetroMenuPage.Create<MetroMenuPopupYesNoLater>().Setup("PRESTIGE".Localize() + "!", empty, "CONTINUE", "CANCEL", MetroSkin.Slice9ButtonRed);
			metroMenuPopupYesNoLater.OnButtonYes(delegate
			{
				LogPrestige(LogPrestigeEventName.popupConfPrestige, accept: true, menuNameData, car);
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
				AutoSingleton<PrestigeManager>.Instance.IncrementPrestigeLevel(car);
				List<Reward> rewards = AutoSingleton<PrestigeManager>.Instance.GetRewards(car);
				MetroPopupRewards page = MetroMenuPage.Create<MetroPopupRewards>().Setup("PRESTIGE!", string.Format("Congrats! You've earned {0} Prestige Tokens and a new Prestige Badge.".Localize(), GetPrestigeLevelTokenReward(car)), rewards, OnDismissedPrestigeRewardPopup, MetroSkin.IconFlag);
				AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
			});
			metroMenuPopupYesNoLater.OnButtonNo(delegate
			{
				LogPrestige(LogPrestigeEventName.popupConfPrestige, accept: false, menuNameData, car);
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			});
			metroMenuPopupYesNoLater.OnClose(delegate
			{
				LogPrestige(LogPrestigeEventName.popupConfPrestige, accept: false, menuNameData, car);
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			});
			AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPopupYesNoLater, MetroAnimation.popup);
		}
	}

	public string ToJson()
	{
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable3 = (Hashtable)(hashtable["PrestigeLevels"] = new Hashtable());
		foreach (KeyValuePair<Car, int> item in _carsPrestigeLevel)
		{
			hashtable3[item.Key.Id] = item.Value;
		}
		return hashtable.toJson();
	}

	public void LogPrestige(LogPrestigeEventName eventPopName, bool accept, string location, Car car)
	{
	}
}
