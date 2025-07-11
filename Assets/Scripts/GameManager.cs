using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private LoadConfigGame _configGame;

	private float _startTime;

	private bool _gameOver;

	public Car CarRef
	{
		get;
		private set;
	}

	public CarController Car
	{
		get;
		private set;
	}

	[method: MethodImpl(32)]
	public event Action<CarController> OnCarControllerChanged;

	[method: MethodImpl(32)]
	public event Action OnGameEnded;

	[method: MethodImpl(32)]
	public event Action OnGameSetupStarted;

	[method: MethodImpl(32)]
	public event Action OnGameSetupEnded;

	protected override void OnAwake()
	{
		base.OnAwake();
		AutoSingleton<PersistenceManager>.Instance.RunCount++;
		_configGame = (AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigGame);
		AutoSingleton<PowerupManager>.Instance.Create();
		Car = SpawnCar();
		UnityEngine.Random.seed = Mathf.RoundToInt(Time.realtimeSinceStartup * 1000f);
		SeedUtil.ResetSeed();
		AutoSingleton<WorldManager>.Instance.Create(_configGame.GameMode);
		AutoSingleton<ExplosionManager>.Instance.Create();
		PrefabSingleton<GameMusicManager>.Instance.PlayGameMusic();
		StartCoroutine(PlayGameCR());
	}

	private void OnDisable()
	{
		if (AutoSingleton<PowerupManager>.IsCreated())
		{
			AutoSingleton<PowerupManager>.Instance.DisablePowerUp();
		}
	}

	private CarController SpawnCar()
	{
		if (_configGame != null)
		{
			CarRef = _configGame.Car;
		}
		else
		{
			CarRef = AutoSingleton<CarDatabase>.Instance.GetAllCars()[0];
		}
		CarController carController = null;
		carController = AutoSingleton<EpicPowerupManager>.Instance.GetCarController(CarRef, (_configGame != null) ? _configGame.GameMode : GameMode.normal);
		if (carController == null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(CarRef.GamePrefab, Vector3.zero, Quaternion.identity) as GameObject;
			carController = gameObject.GetComponent<CarController>();
		}
		carController.Setup(CarRef);
		return carController;
	}

	public bool IsGameOver()
	{
		return _gameOver;
	}

	private void StartGameTimer()
	{
		_startTime = Time.time;
	}

	public float GetGameTimer()
	{
		return Time.time - _startTime;
	}

	public void SetCarController(CarController car)
	{
		StartCoroutine(ResetCarControllerCR(car));
	}

	private IEnumerator ResetCarControllerCR(CarController car)
	{
		Car = car;
		car.Setup(CarRef);
		AutoSingleton<ReplayManager>.Instance.RecordReplay(Car);
		PrefabSingleton<CameraGame>.Instance.SetTarget(Car.transform);
		if (this.OnCarControllerChanged != null)
		{
			this.OnCarControllerChanged(Car);
		}
		List<Powerup> enabledPowerups = AutoSingleton<PowerupManager>.Instance.GetEnabledPowerups();
		foreach (Powerup p2 in enabledPowerups)
		{
			p2.Disable();
		}
		yield return null;
		foreach (Powerup p in enabledPowerups)
		{
			p.Enable();
		}
	}

	private IEnumerator PlayGameCR()
	{
		GameMode gameMode = GameMode.normal;
		if (_configGame != null)
		{
			gameMode = _configGame.GameMode;
		}
		_gameOver = false;
		yield return new WaitForFixedUpdate();
		StartGameTimer();
		if (gameMode == GameMode.normal && AutoSingleton<EpicPowerupManager>.Instance.CanRecordReplay())
		{
			AutoSingleton<ReplayManager>.Instance.RecordReplay(Car);
		}
		if (gameMode == GameMode.normal && AutoSingleton<EpicPowerupManager>.Instance.CanPlayReplay())
		{
			AutoSingleton<ReplayManager>.Instance.PlayReplay();
		}
		GameStatsManager statsManager = AutoSingleton<GameStatsManager>.Instance;
		PrefabSingleton<CameraGame>.Instance.SetTarget(Car.transform);
		statsManager.OnGameStarted();
		AutoSingleton<MissionManager>.Instance.OnGameStarted(CarRef, (_configGame != null) ? _configGame.GameMode : GameMode.normal);
		AutoSingleton<AchievementsManager>.Instance.OnGameStarted();
		int best = (gameMode != 0) ? statsManager.Overall.GetMax(GameStats.CarStats.Type.bucksPickedUp) : statsManager.Overall.GetMaxDistance(CarRef);
		if (this.OnGameSetupStarted != null)
		{
			this.OnGameSetupStarted();
		}
		Car.OnGameSetupStarted();
		yield return new WaitForSeconds(0.5f);
		float gameSetupDistance = (gameMode != 0) ? 80f : AutoSingleton<EpicPowerupManager>.Instance.GetGameSetupDistance();
		while (!Car.IsCrashed())
		{
			Vector3 position = Car.Position;
			if (!(position.x <= gameSetupDistance))
			{
				break;
			}
			yield return null;
		}
		Car.OnGameSetupEnded();
		TriggerGameSetupEnded();
		Maybe<float> best2kTimer = new Maybe<float>();
		Maybe<float> best5kTimer = new Maybe<float>();
		Maybe<float> best10kTimer = new Maybe<float>();
		Maybe<int> bestOneMinuteSprint = new Maybe<int>();
		bool trackBests = gameMode == GameMode.normal && AutoSingleton<EpicPowerupManager>.Instance.TrackBests();
		float timer;
		while (!Car.IsCrashed())
		{
			timer = GetGameTimer();
			if (trackBests)
			{
				Vector3 position2 = Car.Position;
				float currentMaxDistance = position2.x;
				if (!bestOneMinuteSprint.IsSet() && timer >= 60f)
				{
					int distance = Mathf.RoundToInt(currentMaxDistance);
					AutoSingleton<MetroWidgetNotificationManager>.Instance.ShowMessage("One Minute Distance".Localize() + ": " + $"{distance}" + "m".Localize());
					bestOneMinuteSprint.Set(distance);
				}
				if (!best2kTimer.IsSet() && currentMaxDistance >= 2000f)
				{
					best2kTimer.Set(timer);
					statsManager.CurrentRun.Record2kTime(CarRef, best2kTimer.Get());
					AutoSingleton<RoadSignManager>.Instance.ShowBestTime(LeaderboardType.best2kTime, timer);
				}
				if (!best5kTimer.IsSet() && currentMaxDistance >= 5000f)
				{
					best5kTimer.Set(timer);
					statsManager.CurrentRun.Record5kTime(CarRef, best5kTimer.Get());
					AutoSingleton<RoadSignManager>.Instance.ShowBestTime(LeaderboardType.best5kTime, timer);
				}
				if (!best10kTimer.IsSet() && currentMaxDistance >= 10000f)
				{
					best10kTimer.Set(timer);
					statsManager.CurrentRun.Record10kTime(CarRef, best10kTimer.Get());
					AutoSingleton<RoadSignManager>.Instance.ShowBestTime(LeaderboardType.best10kTime, timer);
				}
			}
			if (gameMode == GameMode.frenzy && timer >= 35f)
			{
				Car.Crash();
			}
			yield return null;
		}
		int score = Mathf.RoundToInt(statsManager.CurrentRun.GetValue(CarRef, GameStats.CarStats.Type.maxDistance));
		timer = GetGameTimer();
		_gameOver = true;
		AutoSingleton<AchievementsManager>.Instance.CheckScoreAchievement(score);
		if (AutoSingleton<MetroMenuStack>.Instance.Peek().GetType() == typeof(GameHud))
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop();
		}
		if (this.OnGameEnded != null)
		{
			this.OnGameEnded();
		}
		statsManager.CurrentRun.RecordLongestRaceInTime(CarRef, timer);
		int currentRunCoins = statsManager.CurrentRun.GetValue(CarRef, GameStats.CarStats.Type.coinsPickedUp);
		AutoSingleton<CashManager>.Instance.AddCoins(currentRunCoins);
		int currentRunBucks = statsManager.CurrentRun.GetValue(CarRef, GameStats.CarStats.Type.bucksPickedUp);
		AutoSingleton<CashManager>.Instance.AddBucks(currentRunBucks);
		yield return new WaitForSeconds(2f);
		statsManager.EndGame(CarRef);
		AutoSingleton<MissionManager>.Instance.EndGame(CarRef, (_configGame != null) ? _configGame.GameMode : GameMode.normal);
		AutoSingleton<LeaderboardsManager>.Instance.OnGameEnded(CarRef, bestOneMinuteSprint, gameMode);
		yield return null;
		int stunts = statsManager.CurrentRun.GetValue(CarRef, GameStats.CarStats.Type.numberOfStunts);
		PlayerProfile p = AutoSingleton<Player>.Instance.Profile;
		int previousBucks = p.Bucks;
		int previousCoins = p.Coins;
		EpicPowerup ep = AutoSingleton<EpicPowerupManager>.Instance.OnGameEnded(gameMode);
		XPProfile xpp = p.XPProfile;
		float previousXP = xpp.XP;
		xpp.RegisterXP((float)stunts * 5f + (float)score * 0.1f);
		MissionManager mm = AutoSingleton<MissionManager>.Instance;
		List<Mission> previousMissions = new List<Mission>(mm.GetMissions(CarRef));
		int numMissionsCompleted = mm.AchieveCompletedMission(CarRef);
		List<Mission> nextMissions = new List<Mission>(mm.GetMissions(CarRef));
		int numTotalMissionsCompleted = mm.GetCompletedMissionCount();
		List<Reward> rewards = AutoSingleton<MissionRewardsManager>.Instance.GetReward(numTotalMissionsCompleted, numMissionsCompleted);
		rewards?.ForEach(delegate(Reward r)
		{
			r.Activate();
		});
		Preference.Save();
		MetroMenuEndRun metroMenuEndRun = MetroMenuPage.Create<MetroMenuEndRun>();
		metroMenuEndRun.Setup(CarRef, score, best, currentRunBucks, currentRunCoins, stunts, previousXP, previousBucks, previousCoins, numTotalMissionsCompleted, numMissionsCompleted, rewards, previousMissions, nextMissions, ep);
		AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuEndRun, MetroAnimation.slideDown);
	}

	public void TriggerGameSetupEnded()
	{
		if (this.OnGameSetupEnded != null)
		{
			this.OnGameSetupEnded();
		}
	}
}
