// using ChartboostSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChartboostManager : AutoSingleton<GameChartboostManager>, IAdProvider
{
	private List<PlacementId> _supportedPlacements;

	// private CBLocation _requestedPlacement;

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
		AddLog("Show placement: " + placementId);
		return ShowRewardedPlacement(placementId);
	}

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_supportedPlacements = new List<PlacementId>();
		_supportedPlacements.Add(PlacementId.BootPlacement);
		_supportedPlacements.Add(PlacementId.ResumePlacement);
	}

	private void OnEnable()
	{
		// Chartboost.didInitialize += OnInitialized;
		// Chartboost.didFailToLoadInterstitial += OnFailedToLoadInterstitial;
		// Chartboost.didDismissInterstitial += OnDismissedInterstitial;
		// Chartboost.didCloseInterstitial += OnClosedInterstitial;
		// Chartboost.didClickInterstitial += OnClickedInterstitial;
		// Chartboost.didCacheInterstitial += OnCachedInterstitial;
		// Chartboost.shouldDisplayInterstitial += OnShouldDisplayInterstitial;
		// Chartboost.didDisplayInterstitial += OnDisplayedInterstitial;
	}

	private void OnDisable()
	{
		// Chartboost.didInitialize -= OnInitialized;
		// Chartboost.didFailToLoadInterstitial -= OnFailedToLoadInterstitial;
		// Chartboost.didDismissInterstitial -= OnDismissedInterstitial;
		// Chartboost.didCloseInterstitial -= OnClosedInterstitial;
		// Chartboost.didClickInterstitial -= OnClickedInterstitial;
		// Chartboost.didCacheInterstitial -= OnCachedInterstitial;
		// Chartboost.shouldDisplayInterstitial -= OnShouldDisplayInterstitial;
		// Chartboost.didDisplayInterstitial -= OnDisplayedInterstitial;
	}

	// private CBLocation GetPlacement(PlacementId placementId)
	// {
	// 	return CBLocation.locationFromName(placementId.ToString());
	// }

	// private PlacementId CBLocationToPlacementId(CBLocation location)
	// {
	// 	return EnumUtil.Parse(location.ToString(), PlacementId.Undefined);
	// }

	public static bool IsSupported()
	{
		return true;
	}

	private void OnInitSuccess()
	{
		// Chartboost.setAutoCacheAds(autoCacheAds: true);
		PreloadPlacements();
	}

	private void PreloadPlacements()
	{
		foreach (PlacementId supportedPlacement in _supportedPlacements)
		{
			PreloadPlacement(supportedPlacement);
		}
	}

	private void PreloadPlacement(PlacementId id)
	{
		// Chartboost.cacheInterstitial(GetPlacement(id));
	}

	private bool ShowRewardedPlacement(PlacementId placementId)
	{
		// if (!Chartboost.isInitialized())
		// {
		// 	return false;
		// }
		// ShowOrRequestContent(GetPlacement(placementId));
		return true;
	}

	// private void ShowOrRequestContent(CBLocation placement)
	// {
	// 	if (Chartboost.hasInterstitial(placement))
	// 	{
	// 		StartCoroutine(ShowContentCR(placement));
	// 		return;
	// 	}
	// 	_requestedPlacement = placement;
	// 	Chartboost.cacheInterstitial(placement);
	// }

	// private IEnumerator ShowContentCR(CBLocation placement)
	// {
	// 	yield return null;
	// 	Chartboost.showInterstitial(placement);
	// }

	private void OnInitialized(bool status)
	{
		AddLog($"didInitialize: {status}");
		if (status)
		{
			OnInitSuccess();
		}
	}

	// private void OnFailedToLoadInterstitial(CBLocation location, CBImpressionError error)
	// {
	// 	AddLog($"didFailToLoadInterstitial: {error} at location {location}");
	// }

	// private void OnDismissedInterstitial(CBLocation location)
	// {
	// 	AddLog("didDismissInterstitial: " + location);
	// }

	// private void OnClosedInterstitial(CBLocation location)
	// {
	// 	AddLog("didCloseInterstitial: " + location);
	// 	TriggerOnAdClosed(CBLocationToPlacementId(location));
	// }

	// private void OnClickedInterstitial(CBLocation location)
	// {
	// 	AddLog("didClickInterstitial: " + location);
	// }

	// private void OnCachedInterstitial(CBLocation location)
	// {
	// 	AddLog("didCacheInterstitial: " + location);
	// 	if (_requestedPlacement != null && _requestedPlacement == location)
	// 	{
	// 		Chartboost.showInterstitial(location);
	// 		_requestedPlacement = null;
	// 	}
	// }

	// private bool OnShouldDisplayInterstitial(CBLocation location)
	// {
	// 	AddLog("shouldDisplayInterstitial @" + location);
	// 	return true;
	// }

	// private void OnDisplayedInterstitial(CBLocation location)
	// {
	// 	AddLog("didDisplayInterstitial: " + location);
	// 	TriggerOnAdShown(CBLocationToPlacementId(location));
	// }

	private void AddLog(string text)
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
}
