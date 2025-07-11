using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpicPowerupManager : AutoSingleton<EpicPowerupManager>
{
	private struct EpicPowerupConfigRequestMessage
	{
		public string Platform;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["DeviceId"] = Device.GetDeviceId();
			hashtable["GameVersion"] = GameVersion.VERSION;
			hashtable["Platform"] = Device.GetDeviceType();
			return hashtable;
		}
	}

	private struct EpicPowerupConfigResponseMessage
	{
		public int TimeUpdateMinute;

		public EpicPowerUpConfig Config;

		public static EpicPowerupConfigResponseMessage FromJsonData(Hashtable ht)
		{
			EpicPowerupConfigResponseMessage result = default(EpicPowerupConfigResponseMessage);
			if (ht != null)
			{
				result.TimeUpdateMinute = JsonUtil.ExtractInt(ht, "TimeUpdateMinute", 60);
				Hashtable hashtable = JsonUtil.ExtractHashtable(ht, "Config");
				if (hashtable != null)
				{
					result.Config = EpicPowerUpConfig.FromJsonData(hashtable);
				}
			}
			return result;
		}
	}

	private struct EpicPowerUpConfig
	{
		public string Version;

		public RandomRange NumGames;

		public Dictionary<EpicPowerupType, EpicPowerup> AllPowerups;

		public WeightedList<EpicPowerup> WeightedPowerups;

		public ArrayList StreakDiscounts;

		public bool IsValid()
		{
			return NumGames != null && AllPowerups != null && WeightedPowerups != null && StreakDiscounts != null;
		}

		public static EpicPowerUpConfig FromJsonData(Hashtable jsonData)
		{
			EpicPowerUpConfig result = default(EpicPowerUpConfig);
			if (jsonData != null)
			{
				result.AllPowerups = new Dictionary<EpicPowerupType, EpicPowerup>();
				result.WeightedPowerups = new WeightedList<EpicPowerup>();
				result.Version = JsonUtil.ExtractString(jsonData, "Version", string.Empty);
				Hashtable hashtable = JsonUtil.ExtractHashtable(jsonData, "NumGames");
				if (hashtable != null)
				{
					result.NumGames = new RandomRange(JsonUtil.ExtractFloat(hashtable, "Min", 5f), JsonUtil.ExtractFloat(hashtable, "Max", 5f));
				}
				ArrayList arrayList = JsonUtil.ExtractArrayList(jsonData, "StreakDiscounts");
				if (arrayList != null)
				{
					result.StreakDiscounts = arrayList;
				}
				ArrayList arrayList2 = JsonUtil.ExtractArrayList(jsonData, "EpicPowerUpList");
				if (arrayList2 != null)
				{
					Dictionary<string, EpicPowerupType> dictionary = new Dictionary<string, EpicPowerupType>();
					foreach (int value in Enum.GetValues(typeof(EpicPowerupType)))
					{
						dictionary.Add(((EpicPowerupType)value).ToString(), (EpicPowerupType)value);
					}
					{
						foreach (Hashtable item in arrayList2)
						{
							string text = JsonUtil.ExtractString(item, "Type", string.Empty);
							if (text != string.Empty)
							{
								Price price = new Price(JsonUtil.ExtractHashtable(item, "Price"));
								int weight = JsonUtil.ExtractInt(item, "Weight", 0);
								EpicPowerup epicPowerup = null;
								if (dictionary.ContainsKey(text))
								{
									EpicPowerupType epicPowerupType2 = dictionary[text];
									switch (epicPowerupType2)
									{
									case EpicPowerupType.rocket:
										epicPowerup = new RocketEpicPowerup(price);
										break;
									case EpicPowerupType.transport:
										epicPowerup = new TransportEpicPowerup(price);
										break;
									case EpicPowerupType.choppa:
										epicPowerup = new ChoppaEpicPowerup(price);
										break;
									}
									if (epicPowerup != null)
									{
										result.WeightedPowerups.Add(epicPowerup, weight);
										result.AllPowerups.Add(epicPowerupType2, epicPowerup);
									}
								}
							}
						}
						return result;
					}
				}
			}
			return result;
		}
	}

	private RandomRange _numGames;

	private Dictionary<EpicPowerupType, EpicPowerup> _allPowerups;

	private WeightedList<EpicPowerup> _weightedPowerups;

	private ArrayList _streakDiscounts;

	private DateTime _lastUpdateTime;

	private int _deltaUpdateTimeMin = 60;

	public EpicPowerup GetEpicPowerup(EpicPowerupType type)
	{
		return (type == EpicPowerupType.none) ? null : _allPowerups[type];
	}

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_allPowerups = new Dictionary<EpicPowerupType, EpicPowerup>();
		_weightedPowerups = new WeightedList<EpicPowerup>();
		TextAsset textAsset = Resources.Load("epicPowerups.json", typeof(TextAsset)) as TextAsset;
		EpicPowerUpConfig config = EpicPowerUpConfig.FromJsonData(textAsset.text.hashtableFromJson());
		UpdateConfig(config);
		GetEpicPowerupServerConfig();
		base.OnAwake();
	}

	private void UpdateConfig(EpicPowerUpConfig config)
	{
		if (config.IsValid())
		{
			_numGames = config.NumGames;
			_weightedPowerups = config.WeightedPowerups;
			_streakDiscounts = config.StreakDiscounts;
			if (config.AllPowerups.Count >= _allPowerups.Count)
			{
				_allPowerups = config.AllPowerups;
			}
		}
	}

	private void GetEpicPowerupServerConfig()
	{
		_lastUpdateTime = DateTime.Now;
		AutoSingleton<BackendManager>.Instance.Post("/epicPowerup/getConfig", default(EpicPowerupConfigRequestMessage).ToJsonData(), OnUpdateEpicPowerupConfigSuccess, OnUpdateEpicPowerupConfigFail);
	}

	private void OnUpdateEpicPowerupConfigSuccess(string answer)
	{
		EpicPowerupConfigResponseMessage epicPowerupConfigResponseMessage = EpicPowerupConfigResponseMessage.FromJsonData(answer.hashtableFromJson());
		UpdateConfig(epicPowerupConfigResponseMessage.Config);
		PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
		string epicPowerupServerConfigVersion = profile.EpicPowerupServerConfigVersion;
		if (epicPowerupServerConfigVersion != epicPowerupConfigResponseMessage.Config.Version)
		{
			int gamesUntilEpicPowerup = profile.GamesUntilEpicPowerup;
			int val = Mathf.CeilToInt(_numGames.Pick());
			int gamesUntilEpicPowerup2 = Math.Min(gamesUntilEpicPowerup, val);
			profile.SetGamesUntilEpicPowerup(gamesUntilEpicPowerup2);
			profile.EpicPowerupServerConfigVersion = epicPowerupConfigResponseMessage.Config.Version;
			AutoSingleton<Player>.Instance.SaveProfile();
		}
		_deltaUpdateTimeMin = epicPowerupConfigResponseMessage.TimeUpdateMinute;
	}

	private void OnUpdateEpicPowerupConfigFail(string error)
	{
	}

	public Price GetEpicPowerupPrice(EpicPowerup ep)
	{
		PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
		if (_streakDiscounts != null && profile.EpicPowerupStreak < _streakDiscounts.Count)
		{
			float num = Convert.ToSingle(_streakDiscounts[profile.EpicPowerupStreak]);
			return new Price((int)((float)ep.Price.Amount * num), ep.Price.Currency);
		}
		return new Price(0, Currency.coins);
	}

	public EpicPowerup OnGameEnded(GameMode mode)
	{
		EpicPowerup result = null;
		PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
		if (mode == GameMode.normal)
		{
			if (profile.NextEpicPowerup != 0 && profile.EpicPowerupStreak > 0 && profile.EpicPowerupStreak < _streakDiscounts.Count)
			{
				result = _allPowerups[profile.NextEpicPowerup];
			}
			else
			{
				int gamesUntilEpicPowerup = profile.GamesUntilEpicPowerup;
				if (--gamesUntilEpicPowerup <= 0)
				{
					result = PickEpicPowerUp();
					gamesUntilEpicPowerup = Mathf.CeilToInt(_numGames.Pick());
				}
				profile.SetEpicPowerupStreak(0);
				profile.SetGamesUntilEpicPowerup(gamesUntilEpicPowerup);
			}
			profile.SetNextEpicPowerup(EpicPowerupType.none);
		}
		return result;
	}

	public EpicPowerup GetCurrentEpicPowerUp()
	{
		return GetEpicPowerup(AutoSingleton<Player>.Instance.Profile.NextEpicPowerup);
	}

	public bool CanRecordReplay()
	{
		return GetCurrentEpicPowerUp()?.CanRecordReplay() ?? true;
	}

	public bool CanPlayReplay()
	{
		return GetCurrentEpicPowerUp()?.CanPlayReplay() ?? true;
	}

	public bool TrackBests()
	{
		return GetCurrentEpicPowerUp()?.TrackBests() ?? true;
	}

	public CarController GetCarController(Car car, GameMode mode)
	{
		EpicPowerup currentEpicPowerUp = GetCurrentEpicPowerUp();
		if (currentEpicPowerUp != null && mode == GameMode.normal)
		{
			return currentEpicPowerUp.Setup(car);
		}
		return null;
	}

	public float GetGameSetupDistance()
	{
		return GetCurrentEpicPowerUp()?.GetGameSetupDistance() ?? 80f;
	}

	private EpicPowerup PickEpicPowerUp()
	{
		if (!AutoSingleton<PersistenceManager>.Instance.HasUsedEpicChoppa)
		{
			AutoSingleton<PersistenceManager>.Instance.HasUsedEpicChoppa = true;
			return GetEpicPowerup(EpicPowerupType.choppa);
		}
		return _weightedPowerups.Pick();
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			CheckForConfigUpdate();
		}
	}

	public void CheckForConfigUpdate()
	{
		if ((DateTime.Now - _lastUpdateTime).TotalMinutes >= (double)_deltaUpdateTimeMin)
		{
			GetEpicPowerupServerConfig();
		}
	}
}
