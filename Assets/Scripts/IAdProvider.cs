using System;

public interface IAdProvider
{
	Action<PlacementId> AdAvailableEvent
	{
		get;
		set;
	}

	Action<PlacementId> AdNotAvailableEvent
	{
		get;
		set;
	}

	Action<PlacementId> AdClosedEvent
	{
		get;
		set;
	}

	Action<PlacementId> AdShownEvent
	{
		get;
		set;
	}

	Action<PlacementId> NoContentAvailableEvent
	{
		get;
		set;
	}

	Action<PlacementId> ForceLocalRewardEvent
	{
		get;
		set;
	}

	Action<PlacementId> VideoFullyViewedEvent
	{
		get;
		set;
	}

	Action<PlacementId> VideoInterruptedEvent
	{
		get;
		set;
	}

	Action<PlacementId> TriggerVideoRewardEvent
	{
		get;
		set;
	}

	void OnStart();

	void OnPause();

	void OnResume();

	bool IsRewardedAdAvailable();

	bool IsSupported(PlacementId placementId);

	bool ShowRewardedPlacement(PlacementId placementId);
}
