using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ChartboostSDK
{
	public class Chartboost : MonoBehaviour
	{
		private static bool showingAgeGate;

		private static Chartboost instance;

		private Rect windowRect;

		private Texture2D transparentTexture;

		private GUIStyle transparent;

		private static bool isPaused;

		private static bool shouldPause;

		private static float lastTimeScale;

		[method: MethodImpl(32)]
		public static event Action<bool> didInitialize;

		[method: MethodImpl(32)]
		public static event Func<CBLocation, bool> shouldDisplayInterstitial;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didDisplayInterstitial;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didCacheInterstitial;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didClickInterstitial;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didCloseInterstitial;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didDismissInterstitial;

		[method: MethodImpl(32)]
		public static event Action<CBLocation, CBImpressionError> didFailToLoadInterstitial;

		[method: MethodImpl(32)]
		public static event Action<CBLocation, CBClickError> didFailToRecordClick;

		[method: MethodImpl(32)]
		public static event Func<CBLocation, bool> shouldDisplayMoreApps;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didDisplayMoreApps;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didCacheMoreApps;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didClickMoreApps;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didCloseMoreApps;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didDismissMoreApps;

		[method: MethodImpl(32)]
		public static event Action<CBLocation, CBImpressionError> didFailToLoadMoreApps;

		[method: MethodImpl(32)]
		public static event Func<CBLocation, bool> shouldDisplayRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didDisplayRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didCacheRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didClickRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didCloseRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didDismissRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation, int> didCompleteRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation, CBImpressionError> didFailToLoadRewardedVideo;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> didCacheInPlay;

		[method: MethodImpl(32)]
		public static event Action<CBLocation, CBImpressionError> didFailToLoadInPlay;

		[method: MethodImpl(32)]
		public static event Action<CBLocation> willDisplayVideo;

		[method: MethodImpl(32)]
		public static event Action didPauseClickForConfirmation;

		public static bool isInitialized()
		{
			return CBExternal.isInitialized();
		}

		public static bool isAnyViewVisible()
		{
			return CBExternal.isAnyViewVisible();
		}

		public static void cacheInterstitial(CBLocation location)
		{
			CBExternal.cacheInterstitial(location);
		}

		public static bool hasInterstitial(CBLocation location)
		{
			return CBExternal.hasInterstitial(location);
		}

		public static void showInterstitial(CBLocation location)
		{
			CBExternal.showInterstitial(location);
		}

		public static void cacheMoreApps(CBLocation location)
		{
			CBExternal.cacheMoreApps(location);
		}

		public static bool hasMoreApps(CBLocation location)
		{
			return CBExternal.hasMoreApps(location);
		}

		public static void showMoreApps(CBLocation location)
		{
			CBExternal.showMoreApps(location);
		}

		public static void cacheRewardedVideo(CBLocation location)
		{
			CBExternal.cacheRewardedVideo(location);
		}

		public static bool hasRewardedVideo(CBLocation location)
		{
			return CBExternal.hasRewardedVideo(location);
		}

		public static void showRewardedVideo(CBLocation location)
		{
			CBExternal.showRewardedVideo(location);
		}

		public static void cacheInPlay(CBLocation location)
		{
			CBExternal.cacheInPlay(location);
		}

		public static bool hasInPlay(CBLocation location)
		{
			return CBExternal.hasInPlay(location);
		}

		public static CBInPlay getInPlay(CBLocation location)
		{
			return CBExternal.getInPlay(location);
		}

		public static void didPassAgeGate(bool pass)
		{
			if (showingAgeGate)
			{
				doShowAgeGate(visible: false);
				CBExternal.didPassAgeGate(pass);
			}
		}

		public static void setShouldPauseClickForConfirmation(bool shouldPause)
		{
			CBExternal.setShouldPauseClickForConfirmation(shouldPause);
		}

		public static string getCustomId()
		{
			return CBExternal.getCustomId();
		}

		public static void setCustomId(string customId)
		{
			CBExternal.setCustomId(customId);
		}

		public static bool getAutoCacheAds()
		{
			return CBExternal.getAutoCacheAds();
		}

		public static void setAutoCacheAds(bool autoCacheAds)
		{
			CBExternal.setAutoCacheAds(autoCacheAds);
		}

		public static void setShouldRequestInterstitialsInFirstSession(bool shouldRequest)
		{
			CBExternal.setShouldRequestInterstitialsInFirstSession(shouldRequest);
		}

		public static void setShouldDisplayLoadingViewForMoreApps(bool shouldDisplay)
		{
			CBExternal.setShouldDisplayLoadingViewForMoreApps(shouldDisplay);
		}

		public static void setShouldPrefetchVideoContent(bool shouldPrefetch)
		{
			CBExternal.setShouldPrefetchVideoContent(shouldPrefetch);
		}

		public static void trackLevelInfo(string eventLabel, CBLevelType type, int mainLevel, int subLevel, string description)
		{
			CBExternal.trackLevelInfo(eventLabel, type, mainLevel, subLevel, description);
		}

		public static void trackLevelInfo(string eventLabel, CBLevelType type, int mainLevel, string description)
		{
			CBExternal.trackLevelInfo(eventLabel, type, mainLevel, description);
		}

		public static void trackInAppGooglePlayPurchaseEvent(string title, string description, string price, string currency, string productID, string purchaseData, string purchaseSignature)
		{
			CBExternal.trackInAppGooglePlayPurchaseEvent(title, description, price, currency, productID, purchaseData, purchaseSignature);
		}

		public static void trackInAppAmazonStorePurchaseEvent(string title, string description, string price, string currency, string productID, string userID, string purchaseToken)
		{
			CBExternal.trackInAppAmazonStorePurchaseEvent(title, description, price, currency, productID, userID, purchaseToken);
		}

		public static void setMediation(CBMediation mediator, string version)
		{
			CBExternal.setMediation(mediator, version);
		}

		public static Chartboost Create()
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject("Chartboost");
				instance = gameObject.AddComponent<Chartboost>();
			}
			else
			{
				UnityEngine.Debug.LogWarning("CHARTBOOST: Chartboost instance already exists. Create() ignored");
			}
			return instance;
		}

		public static Chartboost CreateWithAppId(string appId, string appSignature)
		{
			CBSettings.setAppId(appId, appSignature);
			return Create();
		}

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				CBExternal.init();
				CBExternal.setGameObjectName(base.gameObject.name);
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				windowRect = new Rect(0f, 0f, Screen.width, Screen.height);
				transparentTexture = new Texture2D(1, 1);
				transparentTexture.SetPixel(0, 0, Color.clear);
				transparentTexture.Apply();
				transparent = new GUIStyle();
				transparent.normal.background = transparentTexture;
				showingAgeGate = false;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnDestroy()
		{
			if (this == instance)
			{
				instance = null;
				CBExternal.destroy();
			}
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyUp(KeyCode.Escape) && !CBExternal.onBackPressed())
			{
			}
		}

		private void OnGUI()
		{
			if (!showingAgeGate && isImpressionVisible())
			{
				GUI.ModalWindow(0, windowRect, BlockerWindow, string.Empty, transparent);
			}
		}

		private void BlockerWindow(int windowID)
		{
		}

		private void OnApplicationPause(bool paused)
		{
			CBExternal.pause(paused);
		}

		private void OnDisable()
		{
			if (this == instance)
			{
				instance = null;
				CBExternal.destroy();
			}
		}

		private static CBImpressionError impressionErrorFromInt(object errorObj)
		{
			bool flag = Application.platform == RuntimePlatform.IPhonePlayer;
			int num;
			try
			{
				num = Convert.ToInt32(errorObj);
			}
			catch
			{
				num = -1;
			}
			int num2 = 10;
			if (!flag)
			{
				num2 = 18;
			}
			if (num < 0 || num > num2)
			{
				return CBImpressionError.Internal;
			}
			if (flag && num == 8)
			{
				return CBImpressionError.UserCancellation;
			}
			if (flag && num == 9)
			{
				return CBImpressionError.InvalidLocation;
			}
			if (flag && num == 10)
			{
				return CBImpressionError.PrefetchingIncomplete;
			}
			return (CBImpressionError)num;
		}

		private static CBClickError clickErrorFromInt(object errorObj)
		{
			int num;
			try
			{
				num = Convert.ToInt32(errorObj);
			}
			catch
			{
				num = -1;
			}
			int num2 = 3;
			if (num < 0 || num > num2)
			{
				return CBClickError.Internal;
			}
			return (CBClickError)num;
		}

		private void didInitializeEvent(string data)
		{
			if (Chartboost.didInitialize != null)
			{
				Chartboost.didInitialize(Convert.ToBoolean(data));
			}
		}

		private void didFailToLoadInterstitialEvent(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			CBImpressionError arg = impressionErrorFromInt(hashtable["errorCode"]);
			if (Chartboost.didFailToLoadInterstitial != null)
			{
				Chartboost.didFailToLoadInterstitial(CBLocation.locationFromName(hashtable["location"] as string), arg);
			}
		}

		private void didDismissInterstitialEvent(string location)
		{
			doUnityPause(pause: false, setShouldPause: false);
			if (CBExternal.isWebViewEnabled())
			{
				Screen.orientation = ScreenOrientation.AutoRotation;
			}
			if (Chartboost.didDismissInterstitial != null)
			{
				Chartboost.didDismissInterstitial(CBLocation.locationFromName(location));
			}
		}

		private void didClickInterstitialEvent(string location)
		{
			if (Chartboost.didClickInterstitial != null)
			{
				Chartboost.didClickInterstitial(CBLocation.locationFromName(location));
			}
		}

		private void didCloseInterstitialEvent(string location)
		{
			if (Chartboost.didCloseInterstitial != null)
			{
				Chartboost.didCloseInterstitial(CBLocation.locationFromName(location));
			}
		}

		private void didCacheInterstitialEvent(string location)
		{
			if (Chartboost.didCacheInterstitial != null)
			{
				Chartboost.didCacheInterstitial(CBLocation.locationFromName(location));
			}
		}

		private void shouldDisplayInterstitialEvent(string location)
		{
			bool flag = true;
			if (Chartboost.shouldDisplayInterstitial != null)
			{
				flag = Chartboost.shouldDisplayInterstitial(CBLocation.locationFromName(location));
			}
			CBExternal.chartBoostShouldDisplayInterstitialCallbackResult(flag);
			if (flag)
			{
				showInterstitial(CBLocation.locationFromName(location));
			}
		}

		public void didDisplayInterstitialEvent(string location)
		{
			doUnityPause(pause: true, setShouldPause: true);
			if (CBExternal.isWebViewEnabled())
			{
				Screen.orientation = Screen.orientation;
			}
			if (Chartboost.didDisplayInterstitial != null)
			{
				Chartboost.didDisplayInterstitial(CBLocation.locationFromName(location));
			}
		}

		private void didFailToLoadMoreAppsEvent(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			CBImpressionError arg = impressionErrorFromInt(hashtable["errorCode"]);
			if (Chartboost.didFailToLoadMoreApps != null)
			{
				Chartboost.didFailToLoadMoreApps(CBLocation.locationFromName(hashtable["location"] as string), arg);
			}
		}

		private void didDismissMoreAppsEvent(string location)
		{
			doUnityPause(pause: false, setShouldPause: false);
			if (Chartboost.didDismissMoreApps != null)
			{
				Chartboost.didDismissMoreApps(CBLocation.locationFromName(location));
			}
		}

		private void didClickMoreAppsEvent(string location)
		{
			if (Chartboost.didClickMoreApps != null)
			{
				Chartboost.didClickMoreApps(CBLocation.locationFromName(location));
			}
		}

		private void didCloseMoreAppsEvent(string location)
		{
			if (Chartboost.didCloseMoreApps != null)
			{
				Chartboost.didCloseMoreApps(CBLocation.locationFromName(location));
			}
		}

		private void didCacheMoreAppsEvent(string location)
		{
			if (Chartboost.didCacheMoreApps != null)
			{
				Chartboost.didCacheMoreApps(CBLocation.locationFromName(location));
			}
		}

		private void shouldDisplayMoreAppsEvent(string location)
		{
			bool flag = true;
			if (Chartboost.shouldDisplayMoreApps != null)
			{
				flag = Chartboost.shouldDisplayMoreApps(CBLocation.locationFromName(location));
			}
			CBExternal.chartBoostShouldDisplayMoreAppsCallbackResult(flag);
			if (flag)
			{
				showMoreApps(CBLocation.locationFromName(location));
			}
		}

		private void didDisplayMoreAppsEvent(string location)
		{
			doUnityPause(pause: true, setShouldPause: true);
			if (Chartboost.didDisplayMoreApps != null)
			{
				Chartboost.didDisplayMoreApps(CBLocation.locationFromName(location));
			}
		}

		private void didFailToRecordClickEvent(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			CBClickError arg = clickErrorFromInt(hashtable["errorCode"]);
			if (Chartboost.didFailToRecordClick != null)
			{
				Chartboost.didFailToRecordClick(CBLocation.locationFromName(hashtable["location"] as string), arg);
			}
		}

		private void didFailToLoadRewardedVideoEvent(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			CBImpressionError arg = impressionErrorFromInt(hashtable["errorCode"]);
			if (Chartboost.didFailToLoadRewardedVideo != null)
			{
				Chartboost.didFailToLoadRewardedVideo(CBLocation.locationFromName(hashtable["location"] as string), arg);
			}
		}

		private void didDismissRewardedVideoEvent(string location)
		{
			doUnityPause(pause: false, setShouldPause: false);
			if (CBExternal.isWebViewEnabled())
			{
				Screen.orientation = ScreenOrientation.AutoRotation;
			}
			if (Chartboost.didDismissRewardedVideo != null)
			{
				Chartboost.didDismissRewardedVideo(CBLocation.locationFromName(location));
			}
		}

		private void didClickRewardedVideoEvent(string location)
		{
			if (Chartboost.didClickRewardedVideo != null)
			{
				Chartboost.didClickRewardedVideo(CBLocation.locationFromName(location));
			}
		}

		private void didCloseRewardedVideoEvent(string location)
		{
			if (Chartboost.didCloseRewardedVideo != null)
			{
				Chartboost.didCloseRewardedVideo(CBLocation.locationFromName(location));
			}
		}

		private void didCacheRewardedVideoEvent(string location)
		{
			if (Chartboost.didCacheRewardedVideo != null)
			{
				Chartboost.didCacheRewardedVideo(CBLocation.locationFromName(location));
			}
		}

		private void shouldDisplayRewardedVideoEvent(string location)
		{
			bool flag = true;
			if (Chartboost.shouldDisplayRewardedVideo != null)
			{
				flag = Chartboost.shouldDisplayRewardedVideo(CBLocation.locationFromName(location));
			}
			CBExternal.chartBoostShouldDisplayRewardedVideoCallbackResult(flag);
			if (flag)
			{
				showRewardedVideo(CBLocation.locationFromName(location));
			}
		}

		private void didCompleteRewardedVideoEvent(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			int arg;
			try
			{
				arg = Convert.ToInt32(hashtable["reward"]);
			}
			catch
			{
				arg = 0;
			}
			if (Chartboost.didCompleteRewardedVideo != null)
			{
				Chartboost.didCompleteRewardedVideo(CBLocation.locationFromName(hashtable["location"] as string), arg);
			}
		}

		private void didDisplayRewardedVideoEvent(string location)
		{
			doUnityPause(pause: true, setShouldPause: true);
			if (CBExternal.isWebViewEnabled())
			{
				Screen.orientation = Screen.orientation;
			}
			if (Chartboost.didDisplayRewardedVideo != null)
			{
				Chartboost.didDisplayRewardedVideo(CBLocation.locationFromName(location));
			}
		}

		private void didCacheInPlayEvent(string location)
		{
			if (Chartboost.didCacheInPlay != null)
			{
				Chartboost.didCacheInPlay(CBLocation.locationFromName(location));
			}
		}

		private void didFailToLoadInPlayEvent(string dataString)
		{
			Hashtable hashtable = (Hashtable)CBJSON.Deserialize(dataString);
			CBImpressionError arg = impressionErrorFromInt(hashtable["errorCode"]);
			if (Chartboost.didFailToLoadInPlay != null)
			{
				Chartboost.didFailToLoadInPlay(CBLocation.locationFromName(hashtable["location"] as string), arg);
			}
		}

		private void didPauseClickForConfirmationEvent()
		{
			doShowAgeGate(visible: true);
			if (Chartboost.didPauseClickForConfirmation != null)
			{
				Chartboost.didPauseClickForConfirmation();
			}
		}

		private void willDisplayVideoEvent(string location)
		{
			if (Chartboost.willDisplayVideo != null)
			{
				Chartboost.willDisplayVideo(CBLocation.locationFromName(location));
			}
		}

		private static void doUnityPause(bool pause, bool setShouldPause)
		{
			shouldPause = setShouldPause;
			if (pause && !isPaused)
			{
				lastTimeScale = Time.timeScale;
				Time.timeScale = 0f;
				isPaused = true;
				disableUI(pause: true);
			}
			else if (!pause && isPaused)
			{
				Time.timeScale = lastTimeScale;
				isPaused = false;
				disableUI(pause: false);
			}
		}

		private static void doShowAgeGate(bool visible)
		{
			if (shouldPause)
			{
				doUnityPause(!visible, setShouldPause: true);
			}
			showingAgeGate = visible;
		}

		private static void disableUI(bool pause)
		{
		}

		public static bool isImpressionVisible()
		{
			return isPaused;
		}
	}
}
