using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : AutoSingleton<AchievementsManager>
{
	private static Dictionary<AchievementType, string> _achId;

	private float _overallDistance;

	static AchievementsManager()
	{
		_achId = new Dictionary<AchievementType, string>();
		_achId[AchievementType.activateMegaboost] = "grp.ca.roofdog.roadtrip2.overdrive";
		_achId[AchievementType.closeCall] = "grp.ca.roofdog.roadtrip2.closeCall";
		_achId[AchievementType.tenCars10k] = "grp.ca.roofdog.roadtrip2.10Cars10k";
		_achId[AchievementType.handsfreeLanding] = "grp.ca.roofdog.roadtrip2.handsfreeLanding";
		_achId[AchievementType.perfectSlamLanding] = "grp.ca.roofdog.roadtrip2.perfectSlamLanding";
		_achId[AchievementType.doubleFlip] = "grp.ca.roofdog.roadtrip2.doubleFlip";
		_achId[AchievementType.tripleFlip] = "grp.ca.roofdog.roadtrip2.tripleFlip";
		_achId[AchievementType.quadrupleFlip] = "grp.ca.roofdog.roadtrip2.quadrupleFlip";
		_achId[AchievementType.quintupleFlip] = "grp.ca.roofdog.roadtrip2.quintupleFlip";
		_achId[AchievementType.sextupleFlip] = "grp.ca.roofdog.roadtrip2.sextupleFlip";
		_achId[AchievementType.tumbler] = "grp.ca.roofdog.roadtrip2.tumbler";
		_achId[AchievementType.doubleTumbler] = "grp.ca.roofdog.roadtrip2.doubleTumbler";
		_achId[AchievementType.tripleTumbler] = "grp.ca.roofdog.roadtrip2.tripleTumbler";
		_achId[AchievementType.completeAllMissions] = "grp.ca.roofdog.roadtrip2.completeAllMissions";
		_achId[AchievementType.reachLevel10] = "grp.ca.roofdog.roadtrip2.reachLevel10";
		_achId[AchievementType.reachLevel20] = "grp.ca.roofdog.roadtrip2.reachLevel20";
		_achId[AchievementType.reachLevel50] = "grp.ca.roofdog.roadtrip2.reachLevel50";
		_achId[AchievementType.travel1kHandsfree] = "grp.ca.roofdog.roadtrip2.travel1kHandsfree";
		_achId[AchievementType.stuntX8] = "grp.ca.roofdog.roadtrip2.stuntX8";
		_achId[AchievementType.stuntX12] = "grp.ca.roofdog.roadtrip2.stuntX12";
		_achId[AchievementType.unlock5Cars] = "grp.ca.roofdog.roadtrip2.unlock5Cars";
		_achId[AchievementType.distance5k] = "grp.ca.roofdog.roadtrip2.distance5k";
		_achId[AchievementType.distance10k] = "grp.ca.roofdog.roadtrip2.distance10k";
		_achId[AchievementType.distance20k] = "grp.ca.roofdog.roadtrip2.distance20k";
		_achId[AchievementType.totalDistance500k] = "grp.ca.roofdog.roadtrip2.totalDistance500k";
		_achId[AchievementType.totalDistance1MM] = "grp.ca.roofdog.roadtrip2.totalDistance1MM";
		_achId[AchievementType.leftie] = "grp.ca.roofdog.roadtrip2.leftie";
		_achId[AchievementType.collector] = "grp.ca.roofdog.roadtrip2.collector";
		_achId[AchievementType.heavyweight] = "grp.ca.roofdog.roadtrip2.heavyweight";
		_achId[AchievementType.topRacer] = "grp.ca.roofdog.roadtrip2.topRacer";
		_achId[AchievementType.blockbuster] = "grp.ca.roofdog.roadtrip2.blockbuster";
		_achId[AchievementType.showOff] = "grp.ca.roofdog.roadtrip2.showOff";
		_achId[AchievementType.museum] = "grp.ca.roofdog.roadtrip2.museum";
		_achId[AchievementType.kicktheBall] = "grp.ca.roofdog.roadtrip2.kicktheBall";
		_achId[AchievementType.rocketship] = "grp.ca.roofdog.roadtrip2.rocketship";
		_achId[AchievementType.hotAirBalloon] = "grp.ca.roofdog.roadtrip2.hotAirBalloon";
		_achId[AchievementType.grandMaster] = "grp.ca.roofdog.roadtrip2.grandMaster";
	}

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_overallDistance = 0f;
		base.OnAwake();
	}

	protected override void OnStart()
	{
		base.OnStart();
	}

	public void CreateFakeMetadataAchievement()
	{
	}

	public void CheckLevelAchievements(int currentLevel)
	{
		UpdateLevelAchievements(AchievementType.reachLevel10, currentLevel, 10);
		UpdateLevelAchievements(AchievementType.reachLevel20, currentLevel, 20);
		UpdateLevelAchievements(AchievementType.reachLevel50, currentLevel, 50);
	}

	private void UpdateLevelAchievements(AchievementType type, int currentLevel, int levelToReach)
	{
		if (currentLevel < levelToReach)
		{
			UpdateAchievement(type, 100f * (float)currentLevel / (float)levelToReach);
		}
		else
		{
			CompleteAchievement(type);
		}
	}

	public void UpdateAchievement(AchievementType achievement, float percentage)
	{
		if (!Singleton<AttractModeManager>.IsCreated() && !_achId.ContainsKey(achievement))
		{
		}
	}

	public void CompleteAchievement(AchievementType achievement)
	{
		if (!Singleton<AttractModeManager>.IsCreated() && !_achId.ContainsKey(achievement))
		{
		}
	}

	public void ShowAchievements()
	{
	}

	public void OnGameStarted()
	{
		_overallDistance = AutoSingleton<GameStatsManager>.Instance.Overall.GetTotal(GameStats.CarStats.Type.totalDistance);
	}

	public void CheckFlipAchievements(int flipCount)
	{
		if (flipCount == 2)
		{
			CompleteAchievement(AchievementType.doubleFlip);
		}
		else if (flipCount == 3)
		{
			CompleteAchievement(AchievementType.tripleFlip);
		}
		else if (flipCount == 4)
		{
			CompleteAchievement(AchievementType.quadrupleFlip);
		}
		else if (flipCount == 5)
		{
			CompleteAchievement(AchievementType.quintupleFlip);
		}
		else if (flipCount >= 6)
		{
			CompleteAchievement(AchievementType.sextupleFlip);
		}
	}

	public void CheckHandsfreeMaxDistance(float carPositionX)
	{
		if (carPositionX >= 1000f)
		{
			CompleteAchievement(AchievementType.travel1kHandsfree);
		}
	}

	public void CheckCloseCallAchievements(int closeCallCount)
	{
		if (closeCallCount > 0)
		{
			CompleteAchievement(AchievementType.closeCall);
		}
	}

	public void CheckTumblerAchievements(int tumblerCounter)
	{
		if (tumblerCounter == 1)
		{
			CompleteAchievement(AchievementType.tumbler);
		}
		else if (tumblerCounter == 2)
		{
			CompleteAchievement(AchievementType.doubleTumbler);
		}
		else if (tumblerCounter >= 3)
		{
			CompleteAchievement(AchievementType.tripleTumbler);
		}
	}

	public void CheckScoreAchievement(float score)
	{
		if (score >= 5000f)
		{
			CompleteAchievement(AchievementType.distance5k);
		}
		if (score >= 10000f)
		{
			CompleteAchievement(AchievementType.distance10k);
		}
		if (score >= 20000f)
		{
			CompleteAchievement(AchievementType.distance20k);
		}
		Check10kScoreAchievement();
		CheckTotalDistanceAchievement(score);
	}

	private void Check10kScoreAchievement()
	{
		int num = 0;
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			int total = AutoSingleton<GameStatsManager>.Instance.Overall.GetTotal(allCar, GameStats.CarStats.Type.totalDistance);
			if (total >= 10000)
			{
				num++;
			}
		}
		if (num >= 10)
		{
			CompleteAchievement(AchievementType.tenCars10k);
		}
	}

	private void CheckTotalDistanceAchievement(float currentScore)
	{
		float overallDistance = _overallDistance;
		overallDistance += currentScore;
		UpdateTotalDistance(AchievementType.totalDistance500k, overallDistance, 500000f);
		UpdateTotalDistance(AchievementType.totalDistance1MM, overallDistance, 1000000f);
	}

	private void UpdateTotalDistance(AchievementType type, float total, float totalDistanceToReach)
	{
		if (total < totalDistanceToReach)
		{
			UpdateAchievement(type, 100f * total / totalDistanceToReach);
		}
		else
		{
			CompleteAchievement(type);
		}
	}

	public void CheckUnlockCarAchievement()
	{
		int num = 0;
		int num2 = 0;
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(allCar);
			if (carProfile.IsUnlocked())
			{
				num++;
				if (allCar.Category == CarCategory.prestige)
				{
					num2++;
				}
			}
		}
		if (num >= 5)
		{
			CompleteAchievement(AchievementType.unlock5Cars);
		}
		if (num >= 20)
		{
			CompleteAchievement(AchievementType.collector);
		}
		if (num2 >= 6)
		{
			CompleteAchievement(AchievementType.grandMaster);
		}
		List<string> allUnlockedCarsName = AutoSingleton<CarManager>.Instance.GetAllUnlockedCarsName();
		if (allUnlockedCarsName.Contains("Hippie") && allUnlockedCarsName.Contains("Roadie") && allUnlockedCarsName.Contains("Optimal") && allUnlockedCarsName.Contains("Monster"))
		{
			CompleteAchievement(AchievementType.heavyweight);
		}
		if (allUnlockedCarsName.Contains("Impressive") && allUnlockedCarsName.Contains("Formula") && allUnlockedCarsName.Contains("Peak") && allUnlockedCarsName.Contains("Aerial"))
		{
			CompleteAchievement(AchievementType.topRacer);
		}
		if (allUnlockedCarsName.Contains("Charge") && allUnlockedCarsName.Contains("The88") && allUnlockedCarsName.Contains("SpyCar") && allUnlockedCarsName.Contains("Ghost"))
		{
			CompleteAchievement(AchievementType.blockbuster);
		}
	}

	public void CheckLeftieAchievement(float distance)
	{
	}

	public void OnMostStuntRecorded(int stuntCount)
	{
		if (stuntCount >= 12)
		{
			CompleteAchievement(AchievementType.stuntX12);
		}
		else if (stuntCount >= 8)
		{
			CompleteAchievement(AchievementType.stuntX8);
		}
	}

	public void ShareShowroom(int showroomValue)
	{
		if (showroomValue >= 100000)
		{
			CompleteAchievement(AchievementType.showOff);
		}
	}

	public void ShowroomUnlock()
	{
		int unlockedShowroomCount = AutoSingleton<ShowroomDatabase>.Instance.GetUnlockedShowroomCount();
		if (unlockedShowroomCount >= 4)
		{
			CompleteAchievement(AchievementType.museum);
		}
	}

	public void OnConsecutiveLanding(int landingCount, StuntEvent landingType)
	{
		if (landingType == StuntEvent.handsFreeLanding)
		{
			CompleteAchievement(AchievementType.handsfreeLanding);
		}
		if (landingType == StuntEvent.perfectSlamLanding && landingCount >= 1)
		{
			CompleteAchievement(AchievementType.perfectSlamLanding);
		}
	}

	public void OnMegaBoostActivated()
	{
		CompleteAchievement(AchievementType.activateMegaboost);
	}
}
