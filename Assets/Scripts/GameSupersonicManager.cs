using System;

public class GameSupersonicManager : IAdProvider
{
	private PlacementId _placementId;

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
		Init();
	}

	void IAdProvider.OnPause()
	{
		Supersonic.Agent.onPause();
	}

	void IAdProvider.OnResume()
	{
		Supersonic.Agent.onResume();
	}

	bool IAdProvider.IsRewardedAdAvailable()
	{
		return IsVideoAvailable();
	}

	bool IAdProvider.IsSupported(PlacementId placementId)
	{
		return placementId == PlacementId.EndRunVideoPopupPlacement || placementId == PlacementId.FreeCratesPlacement;
	}

	bool IAdProvider.ShowRewardedPlacement(PlacementId placementId)
	{
		if (IsVideoAvailable())
		{
			Supersonic.Agent.showRewardedVideo(placementId.ToString());
			_placementId = placementId;
			return true;
		}
		return false;
	}

	private void Init()
	{
		Supersonic.Agent.start();
		Supersonic.Agent.initRewardedVideo("4650fd25", Device.GetDeviceId());
		RegisterEvents();
	}

	public static bool IsSupported()
	{
		return true;
	}

	public bool IsVideoAvailable()
	{
		return Supersonic.Agent.isRewardedVideoAvailable();
	}

	private void RegisterEvents()
	{
		SupersonicEvents.onRewardedVideoAdRewardedEvent += OnVideoViewed;
		SupersonicEvents.onRewardedVideoAdOpenedEvent += OnVideoOpened;
		SupersonicEvents.onRewardedVideoAdClosedEvent += OnVideoClosed;
		SupersonicEvents.onRewardedVideoInitFailEvent += OnInitFail;
		SupersonicEvents.onRewardedVideoInitSuccessEvent += OnInitSuccess;
	}

	public void OnDispose()
	{
		SupersonicEvents.onRewardedVideoAdRewardedEvent -= OnVideoViewed;
		SupersonicEvents.onRewardedVideoAdOpenedEvent -= OnVideoOpened;
		SupersonicEvents.onRewardedVideoAdClosedEvent -= OnVideoClosed;
		SupersonicEvents.onRewardedVideoInitFailEvent -= OnInitFail;
		SupersonicEvents.onRewardedVideoInitSuccessEvent -= OnInitSuccess;
	}

	private void OnVideoViewed(SupersonicPlacement placement)
	{
		PlacementId placementId = EnumUtil.Parse(placement.getPlacementName(), PlacementId.Undefined);
		TriggerOnVideoFullyViewed(placementId);
		TriggerVideoReward(placementId);
	}

	private void OnVideoOpened()
	{
		TriggerOnAdShown(_placementId);
	}

	private void OnVideoClosed()
	{
		TriggerOnAdClosed(_placementId);
	}

	private void OnInitFail(SupersonicError err)
	{
		SilentDebug.LogWarning("Supersonic : Failed to intitialize video ad rewards - " + err);
	}

	private void OnInitSuccess()
	{
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
