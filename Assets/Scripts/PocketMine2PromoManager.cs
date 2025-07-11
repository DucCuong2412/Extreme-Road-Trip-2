using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PocketMine2PromoManager : CrossPromoManager<PocketMine2PromoManager>
{
	private struct GetPocketCarConfigRequest
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
			hashtable["PromoId"] = "ERT2CarFactory1";
			hashtable["Platform"] = Platform;
			hashtable["GameVersion"] = GameVersion;
			return hashtable;
		}
	}

	private struct GetPocketCarConfigAnswer
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

		public static GetPocketCarConfigAnswer FromJsonData(Hashtable ht)
		{
			GetPocketCarConfigAnswer result = default(GetPocketCarConfigAnswer);
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
		Setup("PM2CrossPromoProgression", "crosspromo/pocketmine2");
		_promoAvailable = new PersistentBool("PocketMinePromoAvailable", def: false);
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
		GetPocketCarConfigRequest getPocketCarConfigRequest = default(GetPocketCarConfigRequest);
		getPocketCarConfigRequest.Platform = Device.GetDeviceType();
		getPocketCarConfigRequest.GameVersion = GameVersion.VERSION;
		AutoSingleton<BackendManager>.Instance.Post("/crosspromo/pocketmine2", getPocketCarConfigRequest.ToJsonData(), OnConfigRequestSuccess, OnConfigRequestFail);
	}

	public void OnConfigRequestSuccess(string jsonData)
	{
		Hashtable ht = jsonData.hashtableFromJson();
		GetPocketCarConfigAnswer json = GetPocketCarConfigAnswer.FromJsonData(ht);
		UpdateConfig(json);
	}

	public void OnConfigRequestFail(string error)
	{
		SilentDebug.LogWarning("Refresh Pocket mine 2 car config " + error);
	}

	private void UpdateConfig(GetPocketCarConfigAnswer json)
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
				if (carProfile != null && !carProfile.IsUnlocked() && PocketMine2PromoManager.CarMissionCompletedEvent != null)
				{
					PocketMine2PromoManager.CarMissionCompletedEvent(allCar);
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
				if (PocketMine2PromoManager.CarPriceUpdatedEvent != null)
				{
					PocketMine2PromoManager.CarPriceUpdatedEvent(allCar, arg);
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
			SilentDebug.LogWarning("Unable to format shared value : " + arg);
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
		AutoSingleton<ExternalAppLauncher>.Instance.LaunchOrStoreRedirect("pocketMine2", param);
	}
}
