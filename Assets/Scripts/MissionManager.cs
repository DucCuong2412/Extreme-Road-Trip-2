using Missions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : AutoSingleton<MissionManager>
{
	private const int _numberOfCurrentMissionMax = 3;

	private const string _allMissionsKey = "AllMissions";

	private const string _currentMissionsKey = "CurrentMissions";

	private const string _customDataKey = "CustomData";

	private const string _isCompletedKey = "IsCompleted";

	private Dictionary<Car, List<List<Mission>>> _allMissions;

	private Dictionary<Car, List<Mission>> _currentMissions;

	private MissionDatabase _database;

	protected override void OnAwake()
	{
		_database = new MissionDatabase();
		_allMissions = new Dictionary<Car, List<List<Mission>>>();
		_currentMissions = new Dictionary<Car, List<Mission>>();
		BuildMissionsDictionary();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.OnAwake();
	}

	public void Refresh()
	{
		_database = new MissionDatabase();
		_allMissions = new Dictionary<Car, List<List<Mission>>>();
		_currentMissions = new Dictionary<Car, List<Mission>>();
		BuildMissionsDictionary();
	}

	private void BuildMissionsDictionary()
	{
		string json = _database.Load();
		Hashtable hashtable = json.hashtableFromJson();
		Hashtable allMissionsSaved = null;
		Hashtable currentMissionsSaved = null;
		if (hashtable != null)
		{
			if (hashtable.ContainsKey("AllMissions"))
			{
				allMissionsSaved = (hashtable["AllMissions"] as Hashtable);
			}
			if (hashtable.ContainsKey("CurrentMissions"))
			{
				currentMissionsSaved = (hashtable["CurrentMissions"] as Hashtable);
			}
		}
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		foreach (Car item in allCars)
		{
			BuildAllMissionGroups(item, allMissionsSaved);
			BuildCurrentMissionList(item, currentMissionsSaved);
		}
	}

	private void CreateMissions<T>(Car car, ArrayList jsonContent, ArrayList missionsTablePlayerPref) where T : Mission, new()
	{
		if (jsonContent != null)
		{
			foreach (Hashtable item in jsonContent)
			{
				T mission = new T();
				mission.Load(item);
				mission.OnMissionCompletedEvent += OnMissionCompleted;
				mission.Completed = (missionsTablePlayerPref?.Contains(mission.Id) ?? false);
				if (IsMissionIDUnique(car, mission.Id))
				{
					AddMissionToGroup(car, mission);
				}
				else
				{
					UnityEngine.Debug.LogWarning("MissionID " + mission.Id + " isn't unique!! Desc: " + mission.Description);
				}
			}
		}
	}

	private bool IsMissionIDUnique(Car car, string missionID)
	{
		foreach (List<Mission> item in _allMissions[car])
		{
			foreach (Mission item2 in item)
			{
				if (item2.Id == missionID)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void BuildAllMissionGroups(Car car, Hashtable allMissionsSaved)
	{
		ArrayList missionsTablePlayerPref = null;
		if (allMissionsSaved != null && allMissionsSaved.ContainsKey(car.Id))
		{
			missionsTablePlayerPref = (allMissionsSaved[car.Id] as ArrayList);
		}
		List<List<Mission>> value = new List<List<Mission>>();
		_allMissions[car] = value;
		TextAsset textAsset = Resources.Load("missions.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			ArrayList arrayList = textAsset.text.arrayListFromJson();
			if (arrayList != null)
			{
				foreach (object item in arrayList)
				{
					Hashtable hashtable = item as Hashtable;
					ArrayList arrayList2 = null;
					if (hashtable.ContainsKey("TravelMission"))
					{
						arrayList2 = (hashtable["TravelMission"] as ArrayList);
						CreateMissions<TravelMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("TravelMissionSingleRun"))
					{
						arrayList2 = (hashtable["TravelMissionSingleRun"] as ArrayList);
						CreateMissions<TravelMissionSingleRun>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("CoinsMission"))
					{
						arrayList2 = (hashtable["CoinsMission"] as ArrayList);
						CreateMissions<CoinsMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("CoinsMissionSingleRun"))
					{
						arrayList2 = (hashtable["CoinsMissionSingleRun"] as ArrayList);
						CreateMissions<CoinsMissionSingleRun>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("ConsecutiveLandingMission"))
					{
						arrayList2 = (hashtable["ConsecutiveLandingMission"] as ArrayList);
						CreateMissions<ConsecutiveLandingMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("MegaBoostMission"))
					{
						arrayList2 = (hashtable["MegaBoostMission"] as ArrayList);
						CreateMissions<MegaBoostMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("AirMission"))
					{
						arrayList2 = (hashtable["AirMission"] as ArrayList);
						CreateMissions<AirMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("SlamMission"))
					{
						arrayList2 = (hashtable["SlamMission"] as ArrayList);
						CreateMissions<SlamMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("MostFlipJumpMission"))
					{
						arrayList2 = (hashtable["MostFlipJumpMission"] as ArrayList);
						CreateMissions<MostFlipJumpMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("MostFlipRunMission"))
					{
						arrayList2 = (hashtable["MostFlipRunMission"] as ArrayList);
						CreateMissions<MostFlipRunMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("MineExplosionMission"))
					{
						arrayList2 = (hashtable["MineExplosionMission"] as ArrayList);
						CreateMissions<MineExplosionMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("TwoKMission"))
					{
						arrayList2 = (hashtable["TwoKMission"] as ArrayList);
						CreateMissions<TwoKMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("PowerUpMission"))
					{
						arrayList2 = (hashtable["PowerUpMission"] as ArrayList);
						CreateMissions<PowerUpMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("CrashMission"))
					{
						arrayList2 = (hashtable["CrashMission"] as ArrayList);
						CreateMissions<CrashMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("UpsideDownSlamMission"))
					{
						arrayList2 = (hashtable["UpsideDownSlamMission"] as ArrayList);
						CreateMissions<UpsideDownSlamMission>(car, arrayList2, missionsTablePlayerPref);
					}
					else if (hashtable.ContainsKey("UpsideDownDistanceMission"))
					{
						arrayList2 = (hashtable["UpsideDownDistanceMission"] as ArrayList);
						CreateMissions<UpsideDownDistanceMission>(car, arrayList2, missionsTablePlayerPref);
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file missions.json");
		}
	}

	private List<Mission> AddMissionGroup(Car car)
	{
		List<Mission> list = new List<Mission>();
		_allMissions[car].Add(list);
		return list;
	}

	private List<Mission> GetMissionGroup(Car car, Type type)
	{
		foreach (List<Mission> item in _allMissions[car])
		{
			if (item[0].GetType() == type)
			{
				return item;
			}
		}
		return AddMissionGroup(car);
	}

	private void AddMissionToGroup(Car car, Mission mission)
	{
		List<Mission> missionGroup = GetMissionGroup(car, mission.GetType());
		missionGroup.Add(mission);
	}

	public void ForceComplete(Car car)
	{
		foreach (List<Mission> item in _allMissions[car])
		{
			foreach (Mission item2 in item)
			{
				item2.Completed = true;
			}
		}
		_currentMissions[car].Clear();
	}

	public void ResetAllMissions(Car car)
	{
		_allMissions[car].Clear();
		BuildAllMissionGroups(car, null);
		_currentMissions[car].Clear();
		UpdateCurrentMissions(car);
		_database.Save(ToJson());
	}

	private void BuildCurrentMissionList(Car car, Hashtable currentMissionsSaved)
	{
		_currentMissions[car] = new List<Mission>();
		if (currentMissionsSaved != null && currentMissionsSaved.ContainsKey(car.Id))
		{
			ArrayList arrayList = currentMissionsSaved[car.Id] as ArrayList;
			foreach (Hashtable item in arrayList)
			{
				string missionId = JsonUtil.ExtractString(item, "id", string.Empty);
				foreach (List<Mission> item2 in _allMissions[car])
				{
					Mission mission = item2.Find((Mission m) => m.Id == missionId);
					if (mission != null)
					{
						mission.SetCustomData(JsonUtil.ExtractHashtable(item, "CustomData"));
						mission.Completed = JsonUtil.ExtractBool(item, "IsCompleted", def: false);
						_currentMissions[car].Add(mission);
						break;
					}
				}
			}
		}
		UpdateCurrentMissions(car);
	}

	public List<Mission> GetMissions(Car car)
	{
		return _currentMissions[car];
	}

	public string ToJson()
	{
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable3 = (Hashtable)(hashtable["AllMissions"] = new Hashtable());
		foreach (KeyValuePair<Car, List<List<Mission>>> allMission in _allMissions)
		{
			CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(allMission.Key);
			if (carProfile.IsUnlocked())
			{
				ArrayList arrayList = new ArrayList();
				foreach (List<Mission> item in allMission.Value)
				{
					foreach (Mission item2 in item)
					{
						if (item2.Completed)
						{
							arrayList.Add(item2.Id.ToString());
						}
					}
				}
				hashtable3[allMission.Key.Id] = arrayList;
			}
		}
		Hashtable hashtable5 = (Hashtable)(hashtable["CurrentMissions"] = new Hashtable());
		foreach (KeyValuePair<Car, List<Mission>> currentMission in _currentMissions)
		{
			CarProfile carProfile2 = AutoSingleton<CarManager>.Instance.GetCarProfile(currentMission.Key);
			if (carProfile2.IsUnlocked())
			{
				ArrayList arrayList2 = new ArrayList();
				foreach (Mission item3 in currentMission.Value)
				{
					Hashtable hashtable6 = new Hashtable();
					hashtable6["id"] = item3.Id;
					Hashtable customData = item3.GetCustomData();
					if (customData != null)
					{
						hashtable6["CustomData"] = customData;
					}
					if (item3.Completed)
					{
						hashtable6["IsCompleted"] = item3.Completed;
					}
					arrayList2.Add(hashtable6);
				}
				hashtable5[currentMission.Key.Id] = arrayList2;
			}
		}
		return hashtable.toJson();
	}

	public bool AreAllMissionsCompleted(Car currentCar)
	{
		foreach (List<Mission> item in _allMissions[currentCar])
		{
			foreach (Mission item2 in item)
			{
				if (!item2.Completed)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void OnMissionCompleted(Mission mission)
	{
		AutoSingleton<MetroWidgetNotificationManager>.Instance.ShowMission(mission.Description);
		if (AreAllMissionsCompleted(Singleton<GameManager>.Instance.CarRef))
		{
			AutoSingleton<AchievementsManager>.Instance.CompleteAchievement(AchievementType.completeAllMissions);
		}
		_database.Save(ToJson());
	}

	public void OnGameStarted(Car currentCar, GameMode gameMode)
	{
		if (gameMode == GameMode.normal)
		{
			RegisterGameEvents(currentCar);
		}
	}

	public void EndGame(Car currentCar, GameMode gameMode)
	{
		if (gameMode == GameMode.normal)
		{
			UnRegisterGameEvents(currentCar);
		}
	}

	public void RegisterGameEvents(Car currentCar)
	{
		if (_currentMissions.ContainsKey(currentCar))
		{
			foreach (Mission item in _currentMissions[currentCar])
			{
				item.RegisterEvents(currentCar);
			}
		}
	}

	public void UnRegisterGameEvents(Car currentCar)
	{
		if (_currentMissions.ContainsKey(currentCar))
		{
			foreach (Mission item in _currentMissions[currentCar])
			{
				item.UnRegisterEvents();
			}
		}
	}

	public bool IsCurrentListContainsMissionType(Car currentCar, Type type)
	{
		foreach (Mission item in _currentMissions[currentCar])
		{
			if (!item.Completed && item.GetType() == type)
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateCurrentMissions(Car currentCar)
	{
		Mission newMission = GetNewMission(currentCar);
		while (!IsCurrentMissionListFull(currentCar) && newMission != null)
		{
			AddCurrentMission(newMission, currentCar, _currentMissions[currentCar].Count);
			newMission = GetNewMission(currentCar);
		}
		_database.Save(ToJson());
	}

	private Mission GetNewMission(Car currentCar)
	{
		List<List<Mission>> list = _allMissions[currentCar];
		int count = list.Count;
		int num = UnityEngine.Random.Range(0, count);
		int num2 = 3 - _currentMissions[currentCar].Count;
		for (int i = 0; i < count; i++)
		{
			if (IsCurrentMissionListFull(currentCar))
			{
				break;
			}
			List<Mission> list2 = list[num];
			if (IsAvailable(currentCar, list2))
			{
				for (int j = GetCompletedMissionCountInGroup(currentCar, list2); j < list2.Count && IsHighestPriority(currentCar, list2[j].Priority, num2); j++)
				{
					if (IsCurrentListContainsMissionType(currentCar, list2[0].GetType()) && !OtherGroupHasLessThen(currentCar, num2))
					{
						break;
					}
					Mission mission = list2[j];
					if (!_currentMissions[currentCar].Contains(mission))
					{
						return mission;
					}
				}
			}
			num = ++num % count;
		}
		return null;
	}

	private Mission GetFirstNonCompletedMission(List<Mission> missionGroup)
	{
		foreach (Mission item in missionGroup)
		{
			if (!item.Completed)
			{
				return item;
			}
		}
		return null;
	}

	private bool IsAvailable(Car currentCar, List<Mission> mg)
	{
		if (IsPowerUpMissionGroup(mg))
		{
			PowerUpMission powerUpMission = GetNextMission(currentCar, mg) as PowerUpMission;
			if (powerUpMission != null)
			{
				return AutoSingleton<Player>.Instance.Profile.HasPowerup(powerUpMission.PowerUpToActivate);
			}
		}
		return true;
	}

	private bool IsCurrentMissionListFull(Car currentCar)
	{
		return _currentMissions[currentCar].Count == 3;
	}

	private void AddCurrentMission(Mission mission, Car car, int index)
	{
		mission.OnSetCurrent(car);
		_currentMissions[car].Insert(index, mission);
	}

	private bool IsHighestPriority(Car currentCar, int priority, int missionSlotAvailable)
	{
		int num = 0;
		foreach (List<Mission> item in _allMissions[currentCar])
		{
			if (IsAvailable(currentCar, item))
			{
				Mission firstNonCompletedMission = GetFirstNonCompletedMission(item);
				if (firstNonCompletedMission != null && !_currentMissions[currentCar].Contains(firstNonCompletedMission) && firstNonCompletedMission.Priority < priority)
				{
					num++;
				}
			}
		}
		return num < missionSlotAvailable;
	}

	private bool OtherGroupHasLessThen(Car currentCar, int missionNeededCount)
	{
		int num = 0;
		foreach (List<Mission> item in _allMissions[currentCar])
		{
			if (!IsCurrentListContainsMissionType(currentCar, item[0].GetType()))
			{
				num += GetNonCompletedMissionCount(currentCar, item);
			}
		}
		return num < missionNeededCount;
	}

	public int GetNonCompletedMissionCount(Car currentCar, List<Mission> missionGroup)
	{
		int nonCompletedCount = 0;
		missionGroup.ForEach(delegate(Mission m)
		{
			nonCompletedCount += ((!m.Completed) ? 1 : 0);
		});
		return nonCompletedCount;
	}

	private int GetCompletedMissionCountInGroup(Car currentCar, List<Mission> missionGroup)
	{
		return missionGroup.Count - GetNonCompletedMissionCount(currentCar, missionGroup);
	}

	public int AchieveCompletedMission(Car currentCar)
	{
		int num = 0;
		List<Mission> list = _currentMissions[currentCar];
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			if (list[num2].Completed)
			{
				num++;
				list.RemoveAt(num2);
				Mission newMission = GetNewMission(currentCar);
				if (newMission != null)
				{
					AddCurrentMission(newMission, currentCar, num2);
				}
			}
		}
		_database.Save(ToJson());
		return num;
	}

	public int GetCurrentCompletedMissionCount(Car car)
	{
		int missionsCompletedCount = 0;
		_currentMissions[car].ForEach(delegate(Mission m)
		{
			missionsCompletedCount += (m.Completed ? 1 : 0);
		});
		return missionsCompletedCount;
	}

	public int GetCompletedMissionCount()
	{
		int missionsCompletedCount = 0;
		AutoSingleton<CarDatabase>.Instance.GetAllCars().ForEach(delegate(Car car)
		{
			missionsCompletedCount += GetCompletedMissionCount(car);
		});
		return missionsCompletedCount;
	}

	public int GetCompletedMissionCount(Car car)
	{
		int missionsCompletedCount = 0;
		_allMissions[car].ForEach(delegate(List<Mission> mg)
		{
			missionsCompletedCount += GetCompletedMissionCountInGroup(car, mg);
		});
		return missionsCompletedCount;
	}

	public int GetMissionCount(Car car)
	{
		int missionsCount = 0;
		_allMissions[car].ForEach(delegate(List<Mission> mg)
		{
			missionsCount += mg.Count;
		});
		return missionsCount;
	}

	public bool IsPowerUpMissionGroup(List<Mission> mg)
	{
		return mg[0].GetType() == typeof(PowerUpMission);
	}

	public Mission GetNextMission(Car currentCar, List<Mission> mg)
	{
		int completedMissionCountInGroup = GetCompletedMissionCountInGroup(currentCar, mg);
		if (completedMissionCountInGroup < mg.Count)
		{
			return mg[completedMissionCountInGroup];
		}
		return null;
	}
}
