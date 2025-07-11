using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AdEvents : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action BootEvent;

	[method: MethodImpl(32)]
	public static event Action ResumeEvent;

	[method: MethodImpl(32)]
	public static event Action<string> CustomAdEvent;

	[method: MethodImpl(32)]
	public static event Action<PlacementId> AdAvailableEvent;

	[method: MethodImpl(32)]
	public static event Action<PlacementId> AdNotAvailableEvent;

	[method: MethodImpl(32)]
	public static event Action<PlacementId> AdClosedEvent;

	[method: MethodImpl(32)]
	public static event Action<PlacementId> AdShownEvent;

	[method: MethodImpl(32)]
	public static event Action<PlacementId> VideoFullyViewedEvent;

	[method: MethodImpl(32)]
	public static event Action<PlacementId> VideoInterruptedEvent;

	[method: MethodImpl(32)]
	public static event Action<PlacementId> SendAdRewardEvent;

	public static void OnBoot()
	{
		if (AdEvents.BootEvent != null)
		{
			AdEvents.BootEvent();
		}
	}

	public static void OnResume()
	{
		if (AdEvents.ResumeEvent != null)
		{
			AdEvents.ResumeEvent();
		}
	}

	public static void OnCustomAdEvent(string eventName)
	{
		if (AdEvents.CustomAdEvent != null)
		{
			AdEvents.CustomAdEvent(eventName);
		}
	}

	public static void OnAdAvailable(PlacementId placementId)
	{
		if (AdEvents.AdAvailableEvent != null)
		{
			AdEvents.AdAvailableEvent(placementId);
		}
	}

	public static void OnAdNotAvailable(PlacementId placementId)
	{
		if (AdEvents.AdNotAvailableEvent != null)
		{
			AdEvents.AdNotAvailableEvent(placementId);
		}
	}

	public static void OnAdClosed(PlacementId placementId)
	{
		if (AdEvents.AdClosedEvent != null)
		{
			AdEvents.AdClosedEvent(placementId);
		}
	}

	public static void OnAdShown(PlacementId placementId)
	{
		if (AdEvents.AdShownEvent != null)
		{
			AdEvents.AdShownEvent(placementId);
		}
	}

	public static void OnVideoFullyViewed(PlacementId placementId)
	{
		if (AdEvents.VideoFullyViewedEvent != null)
		{
			AdEvents.VideoFullyViewedEvent(placementId);
		}
	}

	public static void OnVideoInterrupted(PlacementId placementId)
	{
		if (AdEvents.VideoInterruptedEvent != null)
		{
			AdEvents.VideoInterruptedEvent(placementId);
		}
	}

	public static void OnSendAdReward(PlacementId placementId)
	{
		if (AdEvents.SendAdRewardEvent != null)
		{
			AdEvents.SendAdRewardEvent(placementId);
		}
	}
}
