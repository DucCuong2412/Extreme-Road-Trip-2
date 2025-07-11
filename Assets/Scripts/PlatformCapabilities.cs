using System.Collections;
using UnityEngine;

public class PlatformCapabilities : AutoSingleton<PlatformCapabilities>
{
	private const string _platformCapabilitiesKey = "";

	private bool _useCurrencyPurchase;

	private bool _useFacebookInvite;

	private bool _useGameCenterChallenge;

	private bool _useNativeSocialSharing;

	protected override void OnAwake()
	{
		TextAsset textAsset = Resources.Load("platformCapabilities.json", typeof(TextAsset)) as TextAsset;
		string json = "{}";
		if (textAsset != null)
		{
			json = textAsset.text;
		}
		Hashtable data = json.hashtableFromJson();
		_useCurrencyPurchase = JsonUtil.ExtractBool(data, "useCurrencyPurchase", def: true);
		_useFacebookInvite = JsonUtil.ExtractBool(data, "useFacebookInvite", def: true);
		_useGameCenterChallenge = JsonUtil.ExtractBool(data, "useGameCenterChallenge", def: true);
		_useNativeSocialSharing = JsonUtil.ExtractBool(data, "useNativeSocialSharing", def: true);
		base.OnAwake();
	}

	public bool IsiCloudSupported()
	{
		return false;
	}

	public bool IsCurrencyPurchaseSupported()
	{
		return _useCurrencyPurchase;
	}

	public bool IsGameTouchControlSupported()
	{
		return true;
	}

	public bool IsGameCenterSupported()
	{
		return false;
	}

	private bool IsGameCenterChallengeSupported()
	{
		return false;
	}

	public bool UseGameCenterChallenge()
	{
		return IsGameCenterChallengeSupported() && _useGameCenterChallenge;
	}

	public bool IsAchievementsNotificationSupported()
	{
		return false;
	}

	public bool IsShowFullGameCenterScreenSupported()
	{
		return false;
	}

	public bool IsFacebookSupported()
	{
		return AutoSingleton<GameFacebookManager>.Instance.IsAvailable();
	}

	public bool UseFacebookInvite()
	{
		return IsFacebookSupported() && _useFacebookInvite;
	}

	public bool UseFacebookAsSocialPlatform()
	{
		return true;
	}

	public bool IsTwitterSupported()
	{
		return AutoSingleton<GameTwitterManager>.Instance.IsAvailable();
	}

	private bool IsNativeSocialSharingSupported()
	{
		return false;
	}

	public bool UseNativeSocialSharing()
	{
		return IsNativeSocialSharingSupported() && _useNativeSocialSharing;
	}

	public bool IsFacebookAccessSettingAvailable()
	{
		return false;
	}

	public bool AreInvertedControlsSupported()
	{
		return true;
	}

	public bool IsFeedbackSupported()
	{
		return true;
	}

	public bool UseShowroom()
	{
		return true;
	}

	public bool UseSocialSharing()
	{
		return IsFacebookSupported() || IsTwitterSupported() || UseNativeSocialSharing();
	}

	public bool IsIncentivedVideoSupported()
	{
		return GameAdProvider.IsIncentiveVideoSupported();
	}
}
