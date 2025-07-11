using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PRTPromoManager : AutoSingleton<PRTPromoManager>
{
	private class MissionProgress
	{
		public Currency Item;

		public int Current;

		public int Wanted;

		public MissionProgress()
		{
			Current = -2;
			Wanted = -1;
		}

		public bool IsCompleted()
		{
			return Current >= Wanted;
		}

		public int ProgressLeft()
		{
			return Wanted - Current;
		}

		public bool IsValid()
		{
			return Wanted >= 0;
		}

		public static MissionProgress FromJsonData(Hashtable ht)
		{
			MissionProgress missionProgress = new MissionProgress();
			missionProgress.Item = EnumUtil.Parse(JsonUtil.ExtractString(ht, "Item", "distance"), Currency.distance);
			missionProgress.Current = JsonUtil.ExtractInt(ht, "Current", -2);
			missionProgress.Wanted = JsonUtil.ExtractInt(ht, "Wanted", -1);
			return missionProgress;
		}

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Item"] = Item.ToString();
			hashtable["Current"] = Current;
			hashtable["Wanted"] = Wanted;
			return hashtable;
		}
	}

	private class PromoProgress
	{
		public Dictionary<string, MissionProgress> Progress;

		public PromoProgress()
		{
			Progress = new Dictionary<string, MissionProgress>();
		}

		public static PromoProgress FromJsonData(Hashtable ht)
		{
			PromoProgress promoProgress = new PromoProgress();
			foreach (DictionaryEntry item in ht)
			{
				Hashtable hashtable = item.Value as Hashtable;
				if (hashtable != null)
				{
					MissionProgress value = MissionProgress.FromJsonData(hashtable);
					promoProgress.Progress[item.Key as string] = value;
				}
			}
			return promoProgress;
		}

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			foreach (KeyValuePair<string, MissionProgress> item in Progress)
			{
				hashtable[item.Key] = item.Value.ToJsonData();
			}
			return hashtable;
		}
	}

	private struct GetPRTCarConfigRequest
	{
		public string PromoId;

		public string GameCenterId;

		public string FacebookId;

		public string DeviceId;

		public string Platform;

		public string GameVersion;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PromoId"] = "PRTPromo1";
			hashtable["GameCenterId"] = GameCenterId;
			hashtable["FacebookId"] = FacebookId;
			hashtable["DeviceId"] = DeviceId;
			hashtable["Platform"] = Platform;
			hashtable["GameVersion"] = GameVersion;
			return hashtable;
		}
	}

	private struct GetPRTCarConfigAnswer
	{
		public string Status;

		public PromoProgress Progress;

		public bool IsEnable()
		{
			return Status != "DISABLED" && (Status == "OK" || Status == "NOT_FOUND");
		}

		public bool IsDataValid()
		{
			return Status == "OK";
		}

		public static GetPRTCarConfigAnswer FromJsonData(Hashtable ht)
		{
			GetPRTCarConfigAnswer result = default(GetPRTCarConfigAnswer);
			result.Progress = new PromoProgress();
			result.Status = JsonUtil.ExtractString(ht, "Status", "DISABLED");
			if (result.Status == "OK")
			{
				string json = JsonUtil.ExtractString(ht, "Data", "{}");
				Hashtable ht2 = json.hashtableFromJson();
				result.Progress = PromoProgress.FromJsonData(ht2);
			}
			return result;
		}
	}

	private const int _deltaTimeUpdateSeconde = 15;

	private PersistentBool _promoAvailable;

	private DateTime _pauseTime;

	private PromoProgress _promo;

	[method: MethodImpl(32)]
	public static event Action<Car> OnCarMissionCompleted;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_promoAvailable = new PersistentBool("PRTPromoAvailable", def: false);
		LoadProgress();
		GetConfigData();
		base.OnAwake();
	}

	public void OnPromoPushReceived(bool onLaunch)
	{
		_promoAvailable.Set(v: true);
		if (onLaunch)
		{
			Singleton<Boot>.Instance.NextMenuPage = LoadConfigMenu.NextMenuPage.chooseCar;
		}
	}

	private void GetConfigData()
	{
		GetPRTCarConfigRequest getPRTCarConfigRequest = default(GetPRTCarConfigRequest);
		getPRTCarConfigRequest.DeviceId = Device.GetDeviceId();
		getPRTCarConfigRequest.GameCenterId = AutoSingleton<BackendManager>.Instance.GetGameCenterID();
		getPRTCarConfigRequest.FacebookId = AutoSingleton<BackendManager>.Instance.GetFacebookID();
		getPRTCarConfigRequest.Platform = Device.GetDeviceType();
		getPRTCarConfigRequest.GameVersion = GameVersion.VERSION;
		AutoSingleton<BackendManager>.Instance.Post("/crosspromo/prt/load", getPRTCarConfigRequest.ToJsonData(), OnConfigRequestSuccess, OnConfigRequestFail);
	}

	public void OnConfigRequestSuccess(string jsonData)
	{
		Hashtable ht = jsonData.hashtableFromJson();
		GetPRTCarConfigAnswer json = GetPRTCarConfigAnswer.FromJsonData(ht);
		UpdateConfig(json);
	}

	public void OnConfigRequestFail(string error)
	{
	}

	private void UpdateConfig(GetPRTCarConfigAnswer json)
	{
		if (!_promoAvailable.Get())
		{
			_promoAvailable.Set(json.IsEnable());
		}
		if (json.IsDataValid())
		{
			_promo = json.Progress;
			RefreshCarEvent();
			SaveProgress();
			UpdateCarPrice();
		}
	}

	private void RefreshCarEvent()
	{
		foreach (KeyValuePair<string, MissionProgress> item in _promo.Progress)
		{
			if (item.Value.IsCompleted() && AutoSingleton<CarDatabase>.Instance.ContainsCar(item.Key))
			{
				Car car = AutoSingleton<CarManager>.Instance.GetCar(item.Key);
				CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(car);
				if (!carProfile.IsUnlocked())
				{
					TriggerOnCarMissionCompleted(car);
				}
			}
		}
	}

	private bool UnlockCar(Car car)
	{
		CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(car);
		if (carProfile != null && !carProfile.IsUnlocked())
		{
			carProfile.UnlockForFree();
			AutoSingleton<CarManager>.Instance.SaveCarProfile(car, carProfile);
		}
		return carProfile?.IsUnlocked() ?? false;
	}

	public bool IsPromoCar(Car _car)
	{
		return _promo.Progress.ContainsKey(_car.Id);
	}

	private MissionProgress GetProgress(Car car)
	{
		MissionProgress missionProgress = null;
		if (_promo.Progress.ContainsKey(car.Id))
		{
			return _promo.Progress[car.Id];
		}
		return new MissionProgress();
	}

	private void SaveProgress()
	{
		Preference.SetString("PRTCrossPromoProgression", _promo.ToJsonData().toJson());
	}

	private void LoadProgress()
	{
		string @string = Preference.GetString("PRTCrossPromoProgression", "{}");
		Hashtable hashtable = @string.hashtableFromJson();
		if (hashtable != null)
		{
			_promo = PromoProgress.FromJsonData(@string.hashtableFromJson());
			UpdateCarPrice();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			_pauseTime = DateTime.Now;
		}
		else if ((DateTime.Now - _pauseTime).TotalSeconds >= 15.0)
		{
			GetConfigData();
		}
	}

	private void UpdateCarPrice()
	{
		foreach (KeyValuePair<string, MissionProgress> item2 in _promo.Progress)
		{
			if (AutoSingleton<CarDatabase>.Instance.ContainsCar(item2.Key))
			{
				Price item = new Price(item2.Value.Wanted, item2.Value.Item);
				Car car = AutoSingleton<CarManager>.Instance.GetCar(item2.Key);
				car.Prices.Clear();
				car.Prices.Add(item);
			}
		}
	}

	private void TriggerOnCarMissionCompleted(Car car)
	{
		if (PRTPromoManager.OnCarMissionCompleted != null)
		{
			PRTPromoManager.OnCarMissionCompleted(car);
		}
	}

	public bool AreCarsAvailable()
	{
		return _promoAvailable.Get();
	}

	public bool IsMissionCompleted(Car car)
	{
		MissionProgress progress = GetProgress(car);
		return progress.IsCompleted();
	}

	public int GetProgressLeft(Car car)
	{
		int num = 0;
		foreach (Price price in car.Prices)
		{
			num += price.Amount;
		}
		if (_promo.Progress.ContainsKey(car.Id))
		{
			MissionProgress missionProgress = _promo.Progress[car.Id];
			if (missionProgress != null && missionProgress.IsValid())
			{
				num = missionProgress.ProgressLeft();
			}
		}
		return num;
	}

	public bool UnlockPromoCar(Car car)
	{
		bool result = false;
		if (GetProgress(car).IsCompleted())
		{
			result = UnlockCar(car);
		}
		return result;
	}

	public string GetPRTCarCategoryFromId(string carId)
	{
		switch (carId)
		{
		case "PRTCarStreet":
			return "STREET".Localize();
		case "PRTCarMuscle":
			return "MUSCLE".Localize();
		case "PRTCarTruck":
			return "TRUCK".Localize();
		case "PRTCarPrestige":
			return "PRESTIGE".Localize();
		case "PRTCarNovelty":
			return "NOVELTY".Localize();
		case "PRTCarGarage":
			return "GARAGE".Localize();
		default:
			return string.Empty;
		}
	}
}
