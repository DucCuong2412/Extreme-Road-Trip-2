using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FishingBreakPromoManager : CrossPromoManager<FishingBreakPromoManager>
{
	private struct GetFishingCarConfigRequest
	{
		public string PromoId;

		public string Platform;

		public string GameVersion;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PromoId"] = "FishingERT2FreeCars";
			hashtable["Platform"] = Platform;
			hashtable["GameVersion"] = GameVersion;
			return hashtable;
		}
	}

	private struct GetFishingCarConfigAnswer
	{
		public string Status;

		public bool IsEnable()
		{
			return Status != "DISABLED" && (Status == "OK" || Status == "NOT_FOUND");
		}

		public bool IsDataValid()
		{
			return Status == "OK";
		}

		public static GetFishingCarConfigAnswer FromJsonData(Hashtable ht)
		{
			GetFishingCarConfigAnswer result = default(GetFishingCarConfigAnswer);
			result.Status = JsonUtil.ExtractString(ht, "Status", "DISABLED");
			return result;
		}
	}

	private const int _deltaTimeUpdateSeconds = 15;

	private PersistentBool _promoAvailable;

	private DateTime _pauseTime;

	private int _launchCount;

	[method: MethodImpl(32)]
	public static event Action<Car> CarMissionCompletedEvent;

	[method: MethodImpl(32)]
	public static event Action<Car, Price> CarPriceUpdatedEvent;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Setup("FishingCrossPromoProgression", "crosspromo/fishing");
		_promoAvailable = new PersistentBool("FishingPromoAvailable", def: false);
		UpdatePromoConfig();
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

	private void UpdatePromoConfig()
	{
		GetFishingCarConfigRequest getFishingCarConfigRequest = default(GetFishingCarConfigRequest);
		getFishingCarConfigRequest.Platform = Device.GetDeviceType();
		getFishingCarConfigRequest.GameVersion = GameVersion.VERSION;
		AutoSingleton<BackendManager>.Instance.Post("/crosspromo/fishing", getFishingCarConfigRequest.ToJsonData(), OnConfigRequestSuccess, OnConfigRequestFail);
	}

	public void OnConfigRequestSuccess(string jsonData)
	{
		Hashtable ht = jsonData.hashtableFromJson();
		GetFishingCarConfigAnswer json = GetFishingCarConfigAnswer.FromJsonData(ht);
		UpdateConfig(json);
	}

	public void OnConfigRequestFail(string error)
	{
		UnityEngine.Debug.LogWarning("Refresh fishing car config failed: " + error);
	}

	private void UpdateConfig(GetFishingCarConfigAnswer json)
	{
		if (!_promoAvailable.Get())
		{
			_promoAvailable.Set(json.IsEnable());
		}
	}

	private void RefreshCarEvent()
	{
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			if (IsPromoCar(allCar) && CanUnlock(allCar))
			{
				CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(allCar);
				if (carProfile != null && !carProfile.IsUnlocked() && FishingBreakPromoManager.CarMissionCompletedEvent != null)
				{
					FishingBreakPromoManager.CarMissionCompletedEvent(allCar);
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

	protected override void OnProgressLoaded()
	{
		UpdateCarPrice();
	}

	protected override void OnDeepLinkParsed()
	{
		RefreshCarEvent();
		UpdateCarPrice();
	}

	protected override void OnApplicationPause(bool pause)
	{
		base.OnApplicationPause(pause);
		if (pause)
		{
			_pauseTime = DateTime.Now;
		}
		else if ((DateTime.Now - _pauseTime).TotalSeconds >= 15.0)
		{
			UpdatePromoConfig();
		}
	}

	private void UpdateCarPrice()
	{
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			if (IsPromoCar(allCar))
			{
				Price arg = new Price(GetAmountMissing(allCar), allCar.Price.Currency);
				if (FishingBreakPromoManager.CarPriceUpdatedEvent != null)
				{
					FishingBreakPromoManager.CarPriceUpdatedEvent(allCar, arg);
				}
			}
		}
	}

	private bool CanUnlock(Car car)
	{
		string currency = car.Price.Currency.ToString();
		int currentAmount = GetCurrentAmount(currency);
		return currentAmount >= car.Price.Amount;
	}

	private int GetAmountMissing(Car car)
	{
		string currency = car.Price.Currency.ToString();
		int currentAmount = GetCurrentAmount(currency);
		return Math.Max(0, car.Price.Amount - currentAmount);
	}

	private int GetCurrentAmount(string currency)
	{
		object sharedValue = GetSharedValue(currency);
		try
		{
			return Convert.ToInt32(sharedValue);
			IL_0014:;
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.LogWarning("Unable to format shared value : " + arg);
		}
		return 0;
	}

	public bool IsPromoCar(Car car)
	{
		return IsValidData(car.Price.Currency.ToString());
	}

	public bool AreCarsAvailable()
	{
		return _promoAvailable.Get();
	}

	public bool IsMissionCompleted(Car car)
	{
		return CanUnlock(car);
	}

	public int GetProgressLeft(Car car)
	{
		return GetAmountMissing(car);
	}

	public bool UnlockPromoCar(Car car)
	{
		bool result = false;
		if (CanUnlock(car))
		{
			result = UnlockCar(car);
		}
		return result;
	}

	public void LaunchOrStoreRedirect()
	{
		_launchCount++;
		string param = $"crosspromo/roadtrip2/{_launchCount}";
		AutoSingleton<ExternalAppLauncher>.Instance.LaunchOrStoreRedirect("fishing", param);
	}
}
