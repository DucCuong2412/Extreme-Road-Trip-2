//using ChartboostSDK;
using System.Collections;
using UnityEngine;

public class Boot : Singleton<Boot>
{
	public GameObject _tapjoy;

	public LoadConfigMenu.NextMenuPage NextMenuPage
	{
		get;
		set;
	}

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		Time.fixedDeltaTime = 1f / (float)Device.GetFixedUpdateRate();
		Time.maximumDeltaTime = 1f / (float)Device.GetFixedUpdateRate();
		Application.targetFrameRate = Device.GetTargetFrameRate();
		AutoSingleton<Preference>.Instance.Create();
		StartCoroutine(YieldAndBoot());
		base.OnAwake();
	}

	private IEnumerator YieldAndBoot()
	{
		NextMenuPage = LoadConfigMenu.NextMenuPage.main;
		yield return null;
		AutoSingleton<CrashManager>.Instance.Create();
		while (!AutoSingleton<CrashManager>.Instance.IsDone())
		{
			yield return null;
		}
		AutoSingleton<MessageHandler>.Instance.Create();
		yield return null;
		AutoSingleton<BackendManager>.Instance.Create();
		yield return null;
		if (GameTapjoyManager.IsSupported())
		{
			AutoSingleton<GameTapjoyManager>.Instance.Create();
		}
		AutoSingleton<GameFacebookManager>.Instance.Create();
		yield return null;
		AutoSingleton<StorePerkManager>.Instance.Create();
		yield return null;
		AutoSingleton<PurchaseManager>.Instance.Create();
		yield return null;
		AutoSingleton<MemoryManager>.Instance.Create();
		yield return null;
		AutoSingleton<NotificationManager>.Instance.Create();
		yield return null;
		AutoSingleton<Rooflog>.Instance.Create();
		yield return null;
		InitTapjoy();
		yield return null;
		if (GameChartboostManager.IsSupported())
		{
			//Chartboost.Create();
			yield return null;
		}
		AutoSingleton<GameAdProvider>.Instance.Create();
		yield return null;
		if (BikeTripCrossPromoManager.IsSupported())
		{
			AutoSingleton<BikeTripCrossPromoManager>.Instance.Create();
			yield return null;
		}
		AutoSingleton<ReplayManager>.Instance.Create();
		yield return null;
		AutoSingleton<GameTwitterManager>.Instance.Create();
		yield return null;
		AutoSingleton<MissionManager>.Instance.Create();
		yield return null;
		AutoSingleton<PrestigeManager>.Instance.Create();
		yield return null;
		AutoSingleton<CarSpriteManager>.Instance.Create();
		yield return null;
		AutoSingleton<SpecialOfferManager>.Instance.Create();
		yield return null;
		AutoSingleton<EpicPowerupManager>.Instance.Create();
		yield return null;
		AutoSingleton<PocketMine2PromoManager>.Instance.Create();
		yield return null;
		AutoSingleton<PRTPromoManager>.Instance.Create();
		yield return null;
		AutoSingleton<FishingBreakPromoManager>.Instance.Create();
		yield return null;
		AutoSingleton<PocketMine3PromoManager>.Instance.Create();
		yield return null;
		AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(NextMenuPage));
		yield return null;
	}

	private void InitTapjoy()
	{
		if (!GameTapjoyManager.IsSupported())
		{
			return;
		}
		if (_tapjoy != null)
		{
			if (_tapjoy.activeSelf)
			{
				UnityEngine.Debug.LogWarning("Boot scene tapjoy game object shouldn't be active right upon start. We want to delay its init");
			}
			_tapjoy.SetActive(value: true);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Boot scene Tapjoy object reference is missing. Tapjoy might not work properly...");
		}
		AutoSingleton<GameTapjoyManager>.Instance.Create();
	}
}
