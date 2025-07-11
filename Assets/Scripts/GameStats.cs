using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameStats
{
	public class CarStats
	{
		public enum Type
		{
			best2kTime,
			best5kTime,
			best10kTime,
			coinsPickedUp,
			bucksPickedUp,
			longestRaceInTime,
			maxDistance,
			maxDistanceOnGround,
			maxHeight,
			maxJumpLength,
			mostProfitableRun,
			mostFlipOnJump,
			mostStuntOnJump,
			numberOfStunts,
			numberOfFlips,
			numberOfPerfectLandings,
			numberOfPerfectSlamLandings,
			numberOfRace,
			numberOfSlamUsed,
			totalDistance,
			totalDistanceOnBoost,
			totalDistanceOnGround,
			totalDistanceOnWheelie,
			totalJumpLength,
			totalMegaBoost,
			totalTimePlayed
		}

		public Dictionary<Type, float> _stats;

		public CarStats()
		{
			_stats = new Dictionary<Type, float>();
			Reset();
		}

		public void Reset()
		{
			foreach (int value in Enum.GetValues(typeof(Type)))
			{
				_stats[(Type)value] = 0f;
			}
		}
	}

	private const string _carStatsKey = "CarStats";

	private const string _coinsSpentKey = "CoinsSpent";

	private const string _bucksSpentKey = "BucksSpent";

	private const string _prestigeTokensSpentKey = "PrestigeTokensSpent";

	private Dictionary<Car, CarStats> _carStats;

	private int _coinsSpent;

	private int _bucksSpent;

	private int _prestigeTokensSpent;

	[method: MethodImpl(32)]
	public event Action<float> OnCurrentRunDistanceRecorded;

	[method: MethodImpl(32)]
	public event Action<int> OnCoinsPickedup;

	[method: MethodImpl(32)]
	public event Action<int, StuntEvent> OnConsecutiveLanding;

	[method: MethodImpl(32)]
	public event Action<int> OnMegaBoostActivated;

	[method: MethodImpl(32)]
	public event Action<float> OnJumpLengthRecorded;

	[method: MethodImpl(32)]
	public event Action<int> OnSlamRecorded;

	[method: MethodImpl(32)]
	public event Action<int> OnMostFlipRecorded;

	[method: MethodImpl(32)]
	public event Action<int> OnMostStuntRecorded;

	[method: MethodImpl(32)]
	public event Action<int> OnMineExplosion;

	[method: MethodImpl(32)]
	public event Action<float> On2kTimeRecorded;

	[method: MethodImpl(32)]
	public event Action<PowerupType> OnPowerUpActivated;

	[method: MethodImpl(32)]
	public event Action<float> OnGameEnded;

	[method: MethodImpl(32)]
	public event Action<float> OnUpsideDownDistanceRecorded;

	[method: MethodImpl(32)]
	public event Action OnUpsideDownSlam;

	public GameStats()
	{
		_carStats = new Dictionary<Car, CarStats>();
	}

	public GameStats(List<Car> cars)
		: this()
	{
		foreach (Car car in cars)
		{
			_carStats[car] = new CarStats();
		}
	}

	public GameStats(Hashtable gameStats, List<Car> cars)
		: this(cars)
	{
		if (gameStats != null)
		{
			if (gameStats.ContainsKey("CarStats"))
			{
				Hashtable hashtable = gameStats["CarStats"] as Hashtable;
				foreach (Car car in cars)
				{
					if (hashtable.ContainsKey(car.Id))
					{
						Hashtable hashtable2 = hashtable[car.Id] as Hashtable;
						foreach (int value in Enum.GetValues(typeof(CarStats.Type)))
						{
							if (hashtable2.ContainsKey(((CarStats.Type)value).ToString()))
							{
								_carStats[car]._stats[(CarStats.Type)value] = JsonUtil.ExtractFloat(hashtable2, ((CarStats.Type)value).ToString(), 0f);
							}
						}
					}
				}
			}
			_coinsSpent = JsonUtil.ExtractInt(gameStats, "CoinsSpent", 0);
			_bucksSpent = JsonUtil.ExtractInt(gameStats, "BucksSpent", 0);
			_prestigeTokensSpent = JsonUtil.ExtractInt(gameStats, "PrestigeTokensSpent", 0);
		}
	}

	public string ToJson()
	{
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable3 = (Hashtable)(hashtable["CarStats"] = new Hashtable());
		foreach (KeyValuePair<Car, CarStats> carStat in _carStats)
		{
			Car key = carStat.Key;
			CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(key);
			if (carProfile.IsUnlocked() || key.Category == CarCategory.notForSale)
			{
				Hashtable hashtable4 = new Hashtable();
				hashtable3[key.Id] = hashtable4;
				foreach (int value in Enum.GetValues(typeof(CarStats.Type)))
				{
					if (carStat.Value._stats[(CarStats.Type)value] != 0f)
					{
						hashtable4[((CarStats.Type)value).ToString()] = carStat.Value._stats[(CarStats.Type)value];
					}
				}
			}
		}
		hashtable["CoinsSpent"] = _coinsSpent;
		hashtable["BucksSpent"] = _bucksSpent;
		hashtable["PrestigeTokensSpent"] = _prestigeTokensSpent;
		return hashtable.toJson();
	}

	public void Clear()
	{
		foreach (CarStats value in _carStats.Values)
		{
			value.Reset();
		}
		Reset();
	}

	public void AddCar(Car car)
	{
		_carStats[car] = new CarStats();
	}

	public void EndGame(GameStats currentGameStat, Car currentCar)
	{
		Integrate(currentGameStat, currentCar);
		if (this.OnGameEnded != null)
		{
			this.OnGameEnded(currentGameStat.GetValue(currentCar, CarStats.Type.maxDistance));
		}
	}

	public void Integrate(GameStats currentGameStat, Car currentCar)
	{
		RecordMaxDistance(currentCar, currentGameStat.GetMaxDistance(currentCar));
		RecordTotalDistance(currentCar, currentGameStat.GetMaxDistance(currentCar));
		RecordJumpLength(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.maxJumpLength));
		RecordTotalJumpLength(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.totalJumpLength));
		RecordHeight(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.maxHeight));
		RecordCoinsMadeByCar(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.coinsPickedUp));
		RecordLongestRaceInTime(currentCar, currentGameStat.GetLongestRaceInTime(currentCar));
		RecordTotalTimePlayed(currentCar, currentGameStat.GetLongestRaceInTime(currentCar));
		RecordNumberOfFlips(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.numberOfFlips));
		RecordNumberOfStunts(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.numberOfStunts));
		RecordPerfectLanding(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.numberOfPerfectLandings));
		RecordPerfectSlamLanding(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.numberOfPerfectSlamLandings));
		RecordSlam(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.numberOfSlamUsed));
		RecordMostProfitableRun(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.coinsPickedUp));
		RecordMax(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.bucksPickedUp), CarStats.Type.bucksPickedUp);
		RecordTotalDistanceOnBoost(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.totalDistanceOnBoost));
		RecordMostFlip(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.mostFlipOnJump));
		RecordMostStunt(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.mostStuntOnJump));
		RecordMegaBoost(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.totalMegaBoost));
		RecordTotalDistanceOnGround(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.totalDistanceOnGround));
		RecordTotalDistanceOnWheelie(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.totalDistanceOnWheelie));
		RecordMaxDistanceOnGround(currentCar, currentGameStat.GetValue(currentCar, CarStats.Type.maxDistanceOnGround));
		Record2kTime(currentCar, currentGameStat.GetFloatValue(currentCar, CarStats.Type.best2kTime));
		Record5kTime(currentCar, currentGameStat.GetFloatValue(currentCar, CarStats.Type.best5kTime));
		Record10kTime(currentCar, currentGameStat.GetFloatValue(currentCar, CarStats.Type.best10kTime));
		RecordRaceCount(currentCar);
	}

	public int GetScore(Car car)
	{
		return GetMaxDistance(car);
	}

	public void RecordLongestRaceInTime(Car car, float time)
	{
		RecordMax(car, time, CarStats.Type.longestRaceInTime);
	}

	public float GetLongestRaceInTime(Car car)
	{
		return GetValue(car, CarStats.Type.longestRaceInTime);
	}

	public void RecordTotalTimePlayed(Car car, float time)
	{
		RecordTotal(car, time, CarStats.Type.totalTimePlayed);
	}

	public void RecordMaxDistance(Car car, float distance)
	{
		RecordMax(car, distance, CarStats.Type.maxDistance);
		if (this.OnCurrentRunDistanceRecorded != null)
		{
			this.OnCurrentRunDistanceRecorded(distance);
		}
	}

	public int GetMaxDistance(Car car)
	{
		return GetValue(car, CarStats.Type.maxDistance);
	}

	public void RecordTotalDistanceOnBoost(Car car, float distance)
	{
		RecordTotal(car, distance, CarStats.Type.totalDistanceOnBoost);
	}

	public void RecordTotalDistance(Car car, float distance)
	{
		RecordTotal(car, distance, CarStats.Type.totalDistance);
	}

	public void RecordTotalDistanceOnGround(Car car, float distance)
	{
		RecordTotal(car, distance, CarStats.Type.totalDistanceOnGround);
	}

	public void RecordTotalDistanceOnWheelie(Car car, float distance)
	{
		RecordTotal(car, distance, CarStats.Type.totalDistanceOnWheelie);
	}

	public void RecordMaxDistanceOnGround(Car car, float distance)
	{
		RecordMax(car, distance, CarStats.Type.maxDistanceOnGround);
	}

	public void RecordMostProfitableRun(Car car, float amount)
	{
		RecordMax(car, amount, CarStats.Type.mostProfitableRun);
	}

	public void RecordSlam(Car car, int slamCount)
	{
		RecordTotal(car, slamCount, CarStats.Type.numberOfSlamUsed);
		if (this.OnSlamRecorded != null)
		{
			this.OnSlamRecorded(slamCount);
		}
	}

	public void RecordMostFlip(Car car, int flipCount)
	{
		RecordNumberOfFlips(car, flipCount);
		RecordMax(car, flipCount, CarStats.Type.mostFlipOnJump);
		if (this.OnMostFlipRecorded != null)
		{
			this.OnMostFlipRecorded(flipCount);
		}
	}

	public void RecordMostStunt(Car car, int stuntCount)
	{
		RecordMax(car, stuntCount, CarStats.Type.mostStuntOnJump);
		if (this.OnMostStuntRecorded != null)
		{
			this.OnMostStuntRecorded(stuntCount);
		}
	}

	public void RecordConsecutiveLanding(Car car, int landingCount, StuntEvent landingType)
	{
		if (this.OnConsecutiveLanding != null)
		{
			this.OnConsecutiveLanding(landingCount, landingType);
		}
	}

	public void RecordMineExplosion(int explosionCount)
	{
		if (this.OnMineExplosion != null)
		{
			this.OnMineExplosion(explosionCount);
		}
	}

	public void RecordUpsideDownSlam()
	{
		if (this.OnUpsideDownSlam != null)
		{
			this.OnUpsideDownSlam();
		}
	}

	public void RecordUpsideDownDistance(float distance)
	{
		if (this.OnUpsideDownDistanceRecorded != null)
		{
			this.OnUpsideDownDistanceRecorded(distance);
		}
	}

	public void Record2kTime(Car car, float time)
	{
		RecordMin(car, time, CarStats.Type.best2kTime);
		if (this.On2kTimeRecorded != null)
		{
			this.On2kTimeRecorded(time);
		}
	}

	public void Record5kTime(Car car, float time)
	{
		RecordMin(car, time, CarStats.Type.best5kTime);
	}

	public void Record10kTime(Car car, float time)
	{
		RecordMin(car, time, CarStats.Type.best10kTime);
	}

	public void RecordPowerUpUsage(PowerupType powerupType)
	{
		if (this.OnPowerUpActivated != null)
		{
			this.OnPowerUpActivated(powerupType);
		}
	}

	public void RecordMegaBoost(Car car, float megaBoostCount)
	{
		RecordTotal(car, megaBoostCount, CarStats.Type.totalMegaBoost);
		if (this.OnMegaBoostActivated != null)
		{
			this.OnMegaBoostActivated(GetTotal(car, CarStats.Type.totalMegaBoost));
		}
	}

	public void RecordHeight(Car car, float height)
	{
		RecordMax(car, height, CarStats.Type.maxHeight);
	}

	public void RecordTotalJumpLength(Car car, float length)
	{
		RecordTotal(car, length, CarStats.Type.totalJumpLength);
	}

	public void RecordJumpLength(Car car, float length)
	{
		RecordMax(car, length, CarStats.Type.maxJumpLength);
		if (this.OnJumpLengthRecorded != null)
		{
			this.OnJumpLengthRecorded(length);
		}
	}

	public void RecordRaceCount(Car car)
	{
		RecordTotal(car, 1f, CarStats.Type.numberOfRace);
	}

	public void RecordPerfectSlamLanding(Car car, int count)
	{
		RecordTotal(car, count, CarStats.Type.numberOfPerfectSlamLandings);
	}

	public void RecordPerfectLanding(Car car, int count)
	{
		RecordTotal(car, count, CarStats.Type.numberOfPerfectLandings);
	}

	public void RecordNumberOfStunts(Car car, int count)
	{
		RecordTotal(car, count, CarStats.Type.numberOfStunts);
	}

	public void RecordNumberOfFlips(Car car, int count)
	{
		RecordTotal(car, count, CarStats.Type.numberOfFlips);
	}

	public void RecordCoinsMadeByCar(Car car, int amount)
	{
		RecordTotal(car, amount, CarStats.Type.coinsPickedUp);
		if (this.OnCoinsPickedup != null)
		{
			this.OnCoinsPickedup(amount);
		}
	}

	public void RecordBucksMadeByCar(Car car, int amount)
	{
		RecordTotal(car, amount, CarStats.Type.bucksPickedUp);
	}

	public void RecordCoinsSpent(int amount)
	{
		_coinsSpent += amount;
	}

	public int GetCoinsSpent()
	{
		return _coinsSpent;
	}

	public void RecordBucksSpent(int amount)
	{
		_bucksSpent += amount;
	}

	public int GetBucksSpent()
	{
		return _bucksSpent;
	}

	public void RecordPrestigeTokensSpent(int amount)
	{
		_prestigeTokensSpent += amount;
	}

	public int GetPrestigeTokensSpent()
	{
		return _prestigeTokensSpent;
	}

	private CarStats GetCarStats(Car car)
	{
		return (!_carStats.ContainsKey(car)) ? null : _carStats[car];
	}

	private void RecordMax(Car currentCar, float data, CarStats.Type statType)
	{
		CarStats carStats = GetCarStats(currentCar);
		if (carStats != null)
		{
			carStats._stats[statType] = Mathf.Max(data, carStats._stats[statType]);
		}
	}

	private void RecordMin(Car currentCar, float newTime, CarStats.Type statType)
	{
		CarStats carStats = GetCarStats(currentCar);
		if (carStats != null)
		{
			float num = carStats._stats[statType];
			if (num <= 0f)
			{
				num = newTime;
			}
			else if (newTime > 0f)
			{
				num = Mathf.Min(num, newTime);
			}
			carStats._stats[statType] = num;
		}
	}

	public int GetValue(Car currentCar, CarStats.Type statType)
	{
		return Mathf.RoundToInt(GetFloatValue(currentCar, statType));
	}

	public float GetFloatValue(Car currentCar, CarStats.Type statType)
	{
		float result = 0f;
		CarStats carStats = GetCarStats(currentCar);
		if (carStats != null)
		{
			result = carStats._stats[statType];
		}
		return result;
	}

	public int GetMax(CarStats.Type statType)
	{
		float num = 0f;
		foreach (CarStats value in _carStats.Values)
		{
			num = Mathf.Max(num, value._stats[statType]);
		}
		return Mathf.RoundToInt(num);
	}

	public float GetMin(CarStats.Type statType)
	{
		float num = 0f;
		foreach (CarStats value in _carStats.Values)
		{
			float num2 = value._stats[statType];
			if (num <= 0f)
			{
				num = num2;
			}
			else if (num2 > 0f)
			{
				num = Mathf.Min(num, num2);
			}
		}
		return num;
	}

	private void RecordTotal(Car currentCar, float data, CarStats.Type statType)
	{
		CarStats carStats = GetCarStats(currentCar);
		if (carStats != null)
		{
			Dictionary<CarStats.Type, float> stats;
			Dictionary<CarStats.Type, float> dictionary = stats = carStats._stats;
			CarStats.Type key;
			CarStats.Type key2 = key = statType;
			float num = stats[key];
			dictionary[key2] = num + data;
		}
	}

	public int GetTotal(CarStats.Type statType)
	{
		float num = 0f;
		foreach (CarStats value in _carStats.Values)
		{
			num += value._stats[statType];
		}
		return Mathf.RoundToInt(num);
	}

	public int GetTotal(Car car, CarStats.Type statType)
	{
		float num = 0f;
		if (_carStats.ContainsKey(car))
		{
			num += _carStats[car]._stats[statType];
		}
		return Mathf.RoundToInt(num);
	}

	public void Reset()
	{
		foreach (CarStats value in _carStats.Values)
		{
			value.Reset();
		}
	}
}
