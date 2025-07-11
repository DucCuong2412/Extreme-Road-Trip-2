using System;
using System.Collections;
using System.Collections.Generic;
using TapjoyUnity;
using UnityEngine;

public class GameTapjoyManager : AutoSingleton<GameTapjoyManager>, IAdProvider
{
	private int _amountTryingToSpend;

	private List<PlacementId> _supportedPlacements;

	private Dictionary<PlacementId, TJPlacement> _placements;

	private TJPlacement _requestedPlacement;

	public Action<PlacementId> AdAvailableEvent
	{
		get;
		set;
	}

	public Action<PlacementId> AdNotAvailableEvent
	{
		get;
		set;
	}

	public Action<PlacementId> AdClosedEvent
	{
		get;
		set;
	}

	public Action<PlacementId> AdShownEvent
	{
		get;
		set;
	}

	public Action<PlacementId> NoContentAvailableEvent
	{
		get;
		set;
	}

	public Action<PlacementId> ForceLocalRewardEvent
	{
		get;
		set;
	}

	public Action<PlacementId> VideoFullyViewedEvent
	{
		get;
		set;
	}

	public Action<PlacementId> VideoInterruptedEvent
	{
		get;
		set;
	}

	public Action<PlacementId> TriggerVideoRewardEvent
	{
		get;
		set;
	}

	void IAdProvider.OnStart()
	{
	}

	void IAdProvider.OnPause()
	{
	}

	void IAdProvider.OnResume()
	{
		TryConnect();
		if (IsConnected())
		{
			CreatePlacements();
			PreloadPlacements();
		}
	}

	bool IAdProvider.IsRewardedAdAvailable()
	{
		return true;
	}

	bool IAdProvider.IsSupported(PlacementId placementId)
	{
		return _supportedPlacements.Contains(placementId);
	}

	bool IAdProvider.ShowRewardedPlacement(PlacementId placementId)
	{
		return ShowRewardedPlacement(placementId);
	}

	public static bool IsSupported()
	{
		return true;
	}

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_supportedPlacements = new List<PlacementId>();
		_supportedPlacements.Add(PlacementId.BootPlacement);
		_supportedPlacements.Add(PlacementId.ResumePlacement);
		_supportedPlacements.Add(PlacementId.OfferwallPlacement);
		_placements = new Dictionary<PlacementId, TJPlacement>();
		TapjoyRoofdogCustom.Validate();
	}

	private void OnEnable()
	{
		GameEvents.PlayerXPEvent += OnPlayerXP;
		Tapjoy.OnConnectSuccess += OnConnectSuccess;
		Tapjoy.OnConnectFailure += OnConnectFailure;
		Tapjoy.OnAwardCurrencyResponse += OnTapPointsEarnedEvent;
		Tapjoy.OnAwardCurrencyResponseFailure += OnTapPointsEarnedFailed;
		Tapjoy.OnSpendCurrencyResponse += OnTapPointSpendSuccessfull;
		Tapjoy.OnSpendCurrencyResponseFailure += OnTapPointSpendFailed;
		Tapjoy.OnGetCurrencyBalanceResponse += OnGetCurrencyBalance;
		TJPlacement.OnRequestSuccess += HandleOnPlacementRequestSuccess;
		TJPlacement.OnRequestFailure += HandleOnPlacementRequestFailure;
		TJPlacement.OnContentReady += HandleOnPlacementContentReady;
		TJPlacement.OnContentShow += HandleOnPlacementContentShow;
		TJPlacement.OnContentDismiss += HandleOnPlacementContentDismiss;
		TJPlacement.OnPurchaseRequest += HandleOnPurchaseRequest;
		TJPlacement.OnRewardRequest += HandleOnRewardRequest;
		Tapjoy.GetCurrencyBalance();
	}

	private void OnDisable()
	{
		GameEvents.PlayerXPEvent -= OnPlayerXP;
		Tapjoy.OnConnectSuccess -= OnConnectSuccess;
		Tapjoy.OnConnectFailure -= OnConnectFailure;
		Tapjoy.OnAwardCurrencyResponse -= OnTapPointsEarnedEvent;
		Tapjoy.OnAwardCurrencyResponseFailure -= OnTapPointsEarnedFailed;
		Tapjoy.OnSpendCurrencyResponse -= OnTapPointSpendSuccessfull;
		Tapjoy.OnSpendCurrencyResponseFailure -= OnTapPointSpendFailed;
		Tapjoy.OnGetCurrencyBalanceResponse -= OnGetCurrencyBalance;
		TJPlacement.OnRequestSuccess -= HandleOnPlacementRequestSuccess;
		TJPlacement.OnRequestFailure -= HandleOnPlacementRequestFailure;
		TJPlacement.OnContentReady -= HandleOnPlacementContentReady;
		TJPlacement.OnContentShow -= HandleOnPlacementContentShow;
		TJPlacement.OnContentDismiss -= HandleOnPlacementContentDismiss;
		TJPlacement.OnPurchaseRequest -= HandleOnPurchaseRequest;
		TJPlacement.OnRewardRequest -= HandleOnRewardRequest;
	}

	private PlacementId GetPlacementId(TJPlacement placement)
	{
		return EnumUtil.Parse(placement.GetName(), PlacementId.Undefined);
	}

	private void OnConnectSuccess()
	{
		InitUser();
		CreatePlacements();
		PreloadPlacements();
	}

	private void OnConnectFailure()
	{
	}

	private bool IsConnected()
	{
		return Tapjoy.IsConnected;
	}

	public void TryConnect()
	{
		if (!Tapjoy.IsConnected)
		{
			Tapjoy.Connect();
		}
	}

	private void CreatePlacements()
	{
		foreach (PlacementId supportedPlacement in _supportedPlacements)
		{
			if (!_placements.ContainsKey(supportedPlacement))
			{
				_placements[supportedPlacement] = TJPlacement.CreatePlacement(supportedPlacement.ToString());
			}
		}
	}

	private void PreloadPlacements()
	{
		foreach (KeyValuePair<PlacementId, TJPlacement> placement in _placements)
		{
			PreloadPlacement(placement.Value);
		}
	}

	private void PreloadPlacement(TJPlacement placement)
	{
		if (placement != null && IsConnected())
		{
			placement.RequestContent();
		}
	}

	private void InitUser()
	{
		Tapjoy.SetUserLevel(AutoSingleton<Player>.Instance.Profile.XPProfile.Level);
		UpdateUserId();
	}

	private void OnPlayerXP(int previousLevel, int currentLevel, float xpGained)
	{
		if (IsConnected())
		{
			Tapjoy.SetUserLevel(currentLevel);
		}
	}

	private void UpdateUserId()
	{
		if (IsConnected())
		{
			string text = AutoSingleton<BackendManager>.Instance.PlayerIdentifier();
			if (!string.IsNullOrEmpty(text))
			{
				Tapjoy.SetUserID(text);
			}
		}
	}

	public void OnGetCurrencyBalance(string currencyName, int balance)
	{
		TapPointReceived(balance);
	}

	public void OnTapPointsReceivedEvent(int amount)
	{
		TapPointReceived(amount);
	}

	private void TapPointReceived(int amount)
	{
		if (amount > 0)
		{
			TryToSpend(amount);
		}
	}

	public void OnTapPointReceivedFail(string error = "EMPTY")
	{
	}

	public void OnTapPointsEarnedEvent(string currencyName, int balance)
	{
	}

	public void OnTapPointsEarnedFailed(string error = "EMPTY")
	{
	}

	public void OnTapPointSpendSuccessfull(string currencyName, int amountLeft = 0)
	{
		ConsumeTapPoint();
	}

	public void OnTapPointSpendFailed(string error = "EMPTY")
	{
	}

	private void ConsumeTapPoint()
	{
		if (_amountTryingToSpend > 0)
		{
			ServerReward serverReward = new ServerReward(new Reward(RewardType.bucks, _amountTryingToSpend), "Here's a delivery!", "From your friends at Roofdog");
			AutoSingleton<Rooflog>.Instance.OnTapjoyReward(serverReward.Reward);
			AutoSingleton<MissionRewardsManager>.Instance.OnServerRewardReceived(new List<ServerReward>
			{
				serverReward
			});
		}
		_amountTryingToSpend = 0;
	}

	private void TryToSpend(int amount)
	{
		_amountTryingToSpend = amount;
		this.After(1f, delegate
		{
			Tapjoy.SpendCurrency(_amountTryingToSpend);
		});
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			Tapjoy.GetCurrencyBalance();
		}
	}

	private void HandleOnPlacementRequestSuccess(TJPlacement placement)
	{
	}

	private void HandleOnPlacementRequestFailure(TJPlacement placement, string error)
	{
	}

	private void HandleOnPlacementContentReady(TJPlacement placement)
	{
		if (_requestedPlacement != null && _requestedPlacement == placement && placement.IsContentReady())
		{
			_requestedPlacement.ShowContent();
			_requestedPlacement = null;
		}
	}

	private void HandleOnPlacementContentShow(TJPlacement placement)
	{
		TriggerOnAdShown(GetPlacementId(placement));
	}

	private void HandleOnPlacementContentDismiss(TJPlacement placement)
	{
		if (_placements.ContainsKey(PlacementId.OfferwallPlacement) && placement != _placements[PlacementId.OfferwallPlacement])
		{
			PreloadPlacement(placement);
		}
		TriggerOnAdClosed(GetPlacementId(placement));
		Tapjoy.GetCurrencyBalance();
	}

	private void HandleOnPurchaseRequest(TJPlacement placement, TJActionRequest request, string productId)
	{
	}

	private void HandleOnRewardRequest(TJPlacement placement, TJActionRequest request, string itemId, int quantity)
	{
	}

	private bool ShowRewardedPlacement(PlacementId placementId)
	{
		if (_placements.ContainsKey(placementId))
		{
			return TryShowPlacement(_placements[placementId]);
		}
		this.After(2f, delegate
		{
			ShowRewardedPlacement(placementId);
		});
		return false;
	}

	private bool TryShowPlacement(TJPlacement placement)
	{
		TryConnect();
		if (placement != null)
		{
			if (IsConnected())
			{
				ShowOrRequestContent(placement);
				return true;
			}
			this.After(2f, delegate
			{
				ShowOrRequestContent(placement);
			});
		}
		return false;
	}

	private void ShowOrRequestContent(TJPlacement placement)
	{
		if (placement.IsContentReady())
		{
			StartCoroutine(ShowContentCR(placement));
			return;
		}
		_requestedPlacement = placement;
		placement.RequestContent();
	}

	private IEnumerator ShowContentCR(TJPlacement placement)
	{
		yield return null;
		placement.ShowContent();
	}

	private void TriggerOnAdAvailable(PlacementId placementId)
	{
		if (AdAvailableEvent != null)
		{
			AdAvailableEvent(placementId);
		}
	}

	private void TriggerOnAdNotAvailable(PlacementId placementId)
	{
		if (AdNotAvailableEvent != null)
		{
			AdNotAvailableEvent(placementId);
		}
	}

	private void TriggerOnNoContentAvailable(PlacementId placementId)
	{
		if (NoContentAvailableEvent != null)
		{
			NoContentAvailableEvent(placementId);
		}
	}

	private void TriggerOnAdClosed(PlacementId placementId)
	{
		if (AdClosedEvent != null)
		{
			AdClosedEvent(placementId);
		}
	}

	private void TriggerOnAdShown(PlacementId placementId)
	{
		if (AdShownEvent != null)
		{
			AdShownEvent(placementId);
		}
	}

	private void TriggerOnVideoFullyViewed(PlacementId placementId)
	{
		if (VideoFullyViewedEvent != null)
		{
			VideoFullyViewedEvent(placementId);
		}
	}

	private void TriggerOnVideoInterrupted(PlacementId placementId)
	{
		if (VideoInterruptedEvent != null)
		{
			VideoInterruptedEvent(placementId);
		}
	}

	private void TriggerVideoReward(PlacementId placementId)
	{
		if (TriggerVideoRewardEvent != null)
		{
			TriggerVideoRewardEvent(placementId);
		}
	}
}
