using System;
using System.Collections.Generic;
using UnityEngine;

public class GameAdProvider : AdProvider<GameAdProvider>
{
	private const int _sessionCountBeforeShowingInterstitial = 5;

	public static Action OnVideoAvailable;

	public static Action OnVideoNotAvailable;

	private PersistentInt _gameSessionCount;

	private PersistentBool _interstitialEnabled;

	private DateTime _lastUpdateTime;

	private int _adFrequency = 3;

	private int _adFrequencyCheckCounter;

	private MetroBadge _chooseCarButtonBadge;

	private Action OnNoContentAvailablePopupClose = delegate
	{
	};

	protected override void OnAwake()
	{
		base.OnAwake();
		_gameSessionCount = new PersistentInt("_gameSessionCount", 0);
		_interstitialEnabled = new PersistentBool("_interstitialEnabled", def: true);
		IncrementGameSessionCount();
	}

	protected override void RegisterAdEvents()
	{
		AdEvents.AdAvailableEvent += OnAdAvailable;
		AdEvents.AdNotAvailableEvent += OnAdNotAvailable;
		AdEvents.AdClosedEvent += OnAdClosed;
		AdEvents.AdShownEvent += OnAdShown;
		AdEvents.SendAdRewardEvent += OnSendAdReward;
		AdEvents.VideoFullyViewedEvent += OnVideoFullyViewed;
		AdEvents.VideoInterruptedEvent += OnVideoInterrupted;
	}

	protected override void UnregisterAdEvents()
	{
		AdEvents.AdAvailableEvent -= OnAdAvailable;
		AdEvents.AdNotAvailableEvent -= OnAdNotAvailable;
		AdEvents.AdClosedEvent -= OnAdClosed;
		AdEvents.AdShownEvent -= OnAdShown;
		AdEvents.SendAdRewardEvent -= OnSendAdReward;
		AdEvents.VideoFullyViewedEvent -= OnVideoFullyViewed;
		AdEvents.VideoInterruptedEvent -= OnVideoInterrupted;
	}

	protected override void GameSpecificAddProviders()
	{
		if (GameChartboostManager.IsSupported())
		{
			_adProviders.Add(AutoSingleton<GameChartboostManager>.Instance);
		}
		if (GameTapjoyManager.IsSupported())
		{
			_adProviders.Add(AutoSingleton<GameTapjoyManager>.Instance);
		}
		if (GameSupersonicManager.IsSupported())
		{
			_adProviders.Add(new GameSupersonicManager());
		}
	}

	protected override void GameSpecificOnNoContentAvailable(PlacementId placementId)
	{
		ShowNoAdsAvailablePopup();
	}

	protected override bool GameSpecificCanDisplayResumePlacement()
	{
		MetroMenu metroMenu = AutoSingleton<MetroMenuStack>.Instance.Peek();
		bool flag = metroMenu is GameHud || metroMenu is MetroMenuPause || metroMenu is MetroMenuEndRun || metroMenu == null;
		return !flag;
	}

	protected override void GameSpecificOnApplicationPause(bool pause)
	{
		if (!pause && (DateTime.Now - _pauseTime).TotalSeconds > 60.0)
		{
			IncrementGameSessionCount();
		}
	}

	protected override void OnCustomAdEvent(string eventName)
	{
		base.OnCustomAdEvent(eventName);
	}

	private void OnAdClosed(PlacementId placementId)
	{
		PrefabSingleton<GameMusicManager>.Instance.PlayTitleMusic();
	}

	private void OnAdShown(PlacementId placementId)
	{
		PrefabSingleton<GameMusicManager>.Instance.StopMusic();
	}

	private void OnAdAvailable(PlacementId placementId)
	{
		UpdateChooseCarButtonBadge();
		if (OnVideoAvailable != null)
		{
			OnVideoAvailable();
		}
	}

	private void OnAdNotAvailable(PlacementId placementId)
	{
		UpdateChooseCarButtonBadge();
		if (OnVideoNotAvailable != null)
		{
			OnVideoNotAvailable();
		}
	}

	private void OnVideoFullyViewed(PlacementId placementId)
	{
		AutoSingleton<Rooflog>.Instance.OnVideoViewed();
	}

	private void OnVideoInterrupted(PlacementId placementId)
	{
	}

	private void OnSendAdReward(PlacementId placementId)
	{
		OnLocalRewardReady(placementId);
	}

	private void UpdateChooseCarButtonBadge()
	{
	}

	private MetroButton CreateFreeCrateButton(Action onPopupDismiss, MetroButton button)
	{
		if (AutoSingleton<PlatformCapabilities>.Instance.IsIncentivedVideoSupported())
		{
			button.OnButtonClicked += delegate
			{
				ShowRewardedPlacement(PlacementId.FreeCratesPlacement);
			};
		}
		return button;
	}

	public bool IsPopupDisplayed()
	{
		return AutoSingleton<MetroMenuStack>.Instance.Peek() is MetroPopupMessage;
	}

	private void ShowNoAdsAvailablePopup()
	{
		if (!IsPopupDisplayed())
		{
			_isAdDisplayed = false;
			string titleString = "No Content".Localize();
			string messageString = "No Ads are available at this time. Please try again later.".Localize();
			string buttonString = "OK".Localize();
			string slice9ButtonRed = MetroSkin.Slice9ButtonRed;
			Action buttonAction = delegate
			{
				OnNoContentAvailablePopupClose();
				AutoSingleton<MetroMenuStack>.Instance.Pop();
			};
			MetroPopupMessage page = MetroMenuPage.Create<MetroPopupMessage>().Setup(titleString, messageString, buttonString, slice9ButtonRed, buttonAction);
			AutoSingleton<MetroMenuStack>.Instance.Push(page);
		}
	}

	private void OnLocalRewardReady(PlacementId placementId)
	{
		WeightedList<int> weightedList = new WeightedList<int>();
		weightedList.Add(1, 14);
		weightedList.Add(2, 5);
		weightedList.Add(3, 1);
		ServerReward item = new ServerReward(new Reward(RewardType.bucks, weightedList.Pick()), "Here's a delivery!", "From your friends at Roofdog");
		List<ServerReward> list = new List<ServerReward>();
		list.Add(item);
		AutoSingleton<MissionRewardsManager>.Instance.OnServerRewardReceived(list);
	}

	private bool ShowFreeCrateInterstitialPopup(PlacementId placementId)
	{
		if (IsIncentiveVideoSupported() && AreAdsEnabled())
		{
			ShowRewardedPlacement(placementId);
			return true;
		}
		return false;
	}

	private bool IsStackReady()
	{
		return AutoSingleton<MetroMenuStack>.Instance.Peek() as PopupBikeTripPromo == null;
	}

	private bool AreAdsEnabled()
	{
		return _gameSessionCount.Get() > 5 && _interstitialEnabled.Get();
	}

	private void IncrementGameSessionCount()
	{
		int num = _gameSessionCount.Get();
		num++;
		_gameSessionCount.Set(num);
	}

	public MetroButton CreateFreeCrateButtonMenuStore(float iconScale)
	{
		return CreateFreeCrateButtonMenuChooseCar(iconScale, MetroSkin.Slice9StoreSquare, Color.black);
	}

	public MetroButton CreateFreeCrateButtonMenuChooseCar(float iconScale, string bgSlice9, Color textColor)
	{
		string text = "FREE BUCKS!";
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconWatchVideo, text, textColor, bgSlice9, iconScale);
		_chooseCarButtonBadge = MetroBadge.Create();
		metroButton.Add(_chooseCarButtonBadge);
		UpdateChooseCarButtonBadge();
		OnNoContentAvailablePopupClose = delegate
		{
			UpdateChooseCarButtonBadge();
		};
		Action onPopupDismiss = delegate
		{
			UpdateChooseCarButtonBadge();
		};
		return CreateFreeCrateButton(onPopupDismiss, metroButton);
	}

	public MetroButton CreateFreeCrateButtonMenuMain(Action onPopupDismiss)
	{
		string text = "FREE BUCKS!";
		MetroButton button = MetroSkin.CreateMenuButton(MetroSkin.IconWatchVideo, text, MetroSkin.Slice9ButtonRed, 0.6f);
		OnNoContentAvailablePopupClose = delegate
		{
		};
		return CreateFreeCrateButton(onPopupDismiss, button);
	}

	public bool MaybeShowInterstitialEndRun()
	{
		_adFrequencyCheckCounter++;
		if (_adFrequencyCheckCounter >= _adFrequency)
		{
			bool flag = false;
			_adFrequencyCheckCounter = 0;
			return ShowFreeCrateInterstitialPopup(PlacementId.EndRunVideoPopupPlacement);
		}
		return false;
	}

	public void DisableInterstitial()
	{
		_interstitialEnabled.Set(v: false);
	}

	public void EnableInterstitial()
	{
		_interstitialEnabled.Set(v: true);
	}

	public bool IsInterstialEnabled()
	{
		return _interstitialEnabled.Get();
	}

	public static bool IsIncentiveVideoSupported()
	{
		return true;
	}

	public static MetroWidget GetOfferWallButton()
	{
		if (GameTapjoyManager.IsSupported())
		{
			MetroButton metroButton = null;
			metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconBucksPackA, "FREE BUCKS!");
			metroButton.OnButtonClicked += delegate
			{
				AutoSingleton<GameAdProvider>.Instance.ShowRewardedPlacement(PlacementId.OfferwallPlacement);
			};
			return metroButton;
		}
		return null;
	}
}
