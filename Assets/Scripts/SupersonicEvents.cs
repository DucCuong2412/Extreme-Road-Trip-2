using SupersonicJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SupersonicEvents : MonoBehaviour
{
	private const string ERROR_CODE = "error_code";

	private const string ERROR_DESCRIPTION = "error_description";

	[method: MethodImpl(32)]
	private static event Action _onRewardedVideoInitSuccessEvent;

	public static event Action onRewardedVideoInitSuccessEvent;

	[method: MethodImpl(32)]
	private static event Action<SupersonicError> _onRewardedVideoInitFailEvent;

	public static event Action<SupersonicError> onRewardedVideoInitFailEvent;

	[method: MethodImpl(32)]
	private static event Action _onRewardedVideoAdOpenedEvent;

	public static event Action onRewardedVideoAdOpenedEvent;

	[method: MethodImpl(32)]
	private static event Action _onRewardedVideoAdClosedEvent;

	public static event Action onRewardedVideoAdClosedEvent;

	[method: MethodImpl(32)]
	private static event Action _onVideoStartEvent;

	public static event Action onVideoStartEvent;

	[method: MethodImpl(32)]
	private static event Action _onVideoEndEvent;

	public static event Action onVideoEndEvent;

	[method: MethodImpl(32)]
	private static event Action<SupersonicPlacement> _onRewardedVideoAdRewardedEvent;

	public static event Action<SupersonicPlacement> onRewardedVideoAdRewardedEvent;

	[method: MethodImpl(32)]
	private static event Action<bool> _onVideoAvailabilityChangedEvent;

	public static event Action<bool> onVideoAvailabilityChangedEvent;

	[method: MethodImpl(32)]
	private static event Action _onInterstitialInitSuccessEvent;

	public static event Action onInterstitialInitSuccessEvent;

	[method: MethodImpl(32)]
	private static event Action<SupersonicError> _onInterstitialInitFailEvent;

	public static event Action<SupersonicError> onInterstitialInitFailEvent;

	[method: MethodImpl(32)]
	private static event Action<bool> _onInterstitialAvailabilityEvent;

	public static event Action<bool> onInterstitialAvailabilityEvent;

	[method: MethodImpl(32)]
	private static event Action _onInterstitialShowSuccessEvent;

	public static event Action onInterstitialShowSuccessEvent;

	[method: MethodImpl(32)]
	private static event Action<SupersonicError> _onInterstitialShowFailEvent;

	public static event Action<SupersonicError> onInterstitialShowFailEvent;

	[method: MethodImpl(32)]
	private static event Action _onInterstitialAdClickedEvent;

	public static event Action onInterstitialAdClickedEvent;

	[method: MethodImpl(32)]
	private static event Action _onInterstitialAdClosedEvent;

	public static event Action onInterstitialAdClosedEvent;

	[method: MethodImpl(32)]
	private static event Action _onOfferwallInitSuccessEvent;

	public static event Action onOfferwallInitSuccessEvent;

	[method: MethodImpl(32)]
	private static event Action<SupersonicError> _onOfferwallInitFailEvent;

	public static event Action<SupersonicError> onOfferwallInitFailEvent;

	[method: MethodImpl(32)]
	private static event Action _onOfferwallOpenedEvent;

	public static event Action onOfferwallOpenedEvent;

	[method: MethodImpl(32)]
	private static event Action<SupersonicError> _onOfferwallShowFailEvent;

	public static event Action<SupersonicError> onOfferwallShowFailEvent;

	[method: MethodImpl(32)]
	private static event Action _onOfferwallClosedEvent;

	public static event Action onOfferwallClosedEvent;

	[method: MethodImpl(32)]
	private static event Action<SupersonicError> _onGetOfferwallCreditsFailEvent;

	public static event Action<SupersonicError> onGetOfferwallCreditsFailEvent;

	[method: MethodImpl(32)]
	private static event Action<Dictionary<string, object>> _onOfferwallAdCreditedEvent;

	public static event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent;

	private void Awake()
	{
		base.gameObject.name = "SupersonicEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onRewardedVideoInitSuccess(string empty)
	{
		if (SupersonicEvents._onRewardedVideoInitSuccessEvent != null)
		{
			SupersonicEvents._onRewardedVideoInitSuccessEvent();
		}
	}

	public void onRewardedVideoInitFail(string description)
	{
		if (SupersonicEvents._onRewardedVideoInitFailEvent != null)
		{
			UnityEngine.Debug.Log("entered onRewardedVideoInitFail 1");
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onRewardedVideoInitFailEvent(errorFromErrorString);
		}
	}

	public void onRewardedVideoAdOpened(string empty)
	{
		if (SupersonicEvents._onRewardedVideoAdOpenedEvent != null)
		{
			SupersonicEvents._onRewardedVideoAdOpenedEvent();
		}
	}

	public void onRewardedVideoAdClosed(string empty)
	{
		if (SupersonicEvents._onRewardedVideoAdClosedEvent != null)
		{
			SupersonicEvents._onRewardedVideoAdClosedEvent();
		}
	}

	public void onVideoStart(string empty)
	{
		if (SupersonicEvents._onVideoStartEvent != null)
		{
			SupersonicEvents._onVideoStartEvent();
		}
	}

	public void onVideoEnd(string empty)
	{
		if (SupersonicEvents._onVideoEndEvent != null)
		{
			SupersonicEvents._onVideoEndEvent();
		}
	}

	public void onRewardedVideoAdRewarded(string description)
	{
		if (SupersonicEvents._onRewardedVideoAdRewardedEvent != null)
		{
			SupersonicPlacement placementFromString = getPlacementFromString(description);
			SupersonicEvents._onRewardedVideoAdRewardedEvent(placementFromString);
		}
	}

	public void onVideoAvailabilityChanged(string stringAvailable)
	{
		bool obj = (stringAvailable == "true") ? true : false;
		if (SupersonicEvents._onVideoAvailabilityChangedEvent != null)
		{
			SupersonicEvents._onVideoAvailabilityChangedEvent(obj);
		}
	}

	public void onInterstitialInitSuccess(string empty)
	{
		if (SupersonicEvents._onInterstitialInitSuccessEvent != null)
		{
			SupersonicEvents._onInterstitialInitSuccessEvent();
		}
	}

	public void onInterstitialInitFail(string description)
	{
		if (SupersonicEvents._onInterstitialInitFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialInitFailEvent(errorFromErrorString);
		}
	}

	public void onInterstitialAvailability(string stringAvailable)
	{
		bool obj = (stringAvailable == "true") ? true : false;
		if (SupersonicEvents._onInterstitialAvailabilityEvent != null)
		{
			SupersonicEvents._onInterstitialAvailabilityEvent(obj);
		}
	}

	public void onInterstitialShowSuccess(string empty)
	{
		if (SupersonicEvents._onInterstitialShowSuccessEvent != null)
		{
			SupersonicEvents._onInterstitialShowSuccessEvent();
		}
	}

	public void onInterstitialShowFail(string description)
	{
		if (SupersonicEvents._onInterstitialShowFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onInterstitialShowFailEvent(errorFromErrorString);
		}
	}

	public void onInterstitialAdClicked(string empty)
	{
		if (SupersonicEvents._onInterstitialAdClickedEvent != null)
		{
			SupersonicEvents._onInterstitialAdClickedEvent();
		}
	}

	public void onInterstitialAdClosed(string empty)
	{
		if (SupersonicEvents._onInterstitialAdClosedEvent != null)
		{
			SupersonicEvents._onInterstitialAdClosedEvent();
		}
	}

	public void onOfferwallInitSuccess(string empty)
	{
		if (SupersonicEvents._onOfferwallInitSuccessEvent != null)
		{
			SupersonicEvents._onOfferwallInitSuccessEvent();
		}
	}

	public void onOfferwallInitFail(string description)
	{
		if (SupersonicEvents._onOfferwallInitFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onOfferwallInitFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallOpened(string empty)
	{
		if (SupersonicEvents._onOfferwallOpenedEvent != null)
		{
			SupersonicEvents._onOfferwallOpenedEvent();
		}
	}

	public void onOfferwallShowFail(string description)
	{
		if (SupersonicEvents._onOfferwallShowFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onOfferwallShowFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallClosed(string empty)
	{
		if (SupersonicEvents._onOfferwallClosedEvent != null)
		{
			SupersonicEvents._onOfferwallClosedEvent();
		}
	}

	public void onGetOfferwallCreditsFail(string description)
	{
		if (SupersonicEvents._onGetOfferwallCreditsFailEvent != null)
		{
			SupersonicError errorFromErrorString = getErrorFromErrorString(description);
			SupersonicEvents._onGetOfferwallCreditsFailEvent(errorFromErrorString);
		}
	}

	public void onOfferwallAdCredited(string json)
	{
		if (SupersonicEvents._onOfferwallAdCreditedEvent != null)
		{
			SupersonicEvents._onOfferwallAdCreditedEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	public SupersonicError getErrorFromErrorString(string description)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(description) as Dictionary<string, object>;
		if (dictionary != null)
		{
			int errCode = Convert.ToInt32(dictionary["error_code"].ToString());
			string errDescription = dictionary["error_description"].ToString();
			return new SupersonicError(errCode, errDescription);
		}
		return new SupersonicError(-1, string.Empty);
	}

	public SupersonicPlacement getPlacementFromString(string jsonPlacement)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(jsonPlacement) as Dictionary<string, object>;
		int rAmount = Convert.ToInt32(dictionary["placement_reward_amount"].ToString());
		string rName = dictionary["placement_reward_name"].ToString();
		string pName = dictionary["placement_name"].ToString();
		return new SupersonicPlacement(pName, rName, rAmount);
	}
}
