using System;
using System.Collections.Generic;
using UnityEngine;

public class AdProvider<T> : AutoSingleton<T> where T : MonoBehaviour
{
	protected List<IAdProvider> _adProviders;

	protected bool _isAdDisplayed;

	protected bool _bootPlacementShown;

	protected double _resumeDelay = 60.0;

	protected DateTime _pauseTime;

	protected void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_adProviders = new List<IAdProvider>();
		GameSpecificAddProviders();
		_adProviders.ForEach(delegate(IAdProvider provider)
		{
			RegisterAdProvider(provider);
		});
		_adProviders.ForEach(delegate(IAdProvider provider)
		{
			provider.OnStart();
		});
		base.OnAwake();
	}

	private void RegisterAdProvider(IAdProvider adProvider)
	{
		adProvider.AdAvailableEvent = (Action<PlacementId>)Delegate.Combine(adProvider.AdAvailableEvent, new Action<PlacementId>(OnAdAvailable));
		adProvider.AdNotAvailableEvent = (Action<PlacementId>)Delegate.Combine(adProvider.AdNotAvailableEvent, new Action<PlacementId>(OnAdNotAvailable));
		adProvider.NoContentAvailableEvent = (Action<PlacementId>)Delegate.Combine(adProvider.NoContentAvailableEvent, new Action<PlacementId>(OnNoContentAvailable));
		adProvider.AdClosedEvent = (Action<PlacementId>)Delegate.Combine(adProvider.AdClosedEvent, new Action<PlacementId>(OnAdClosed));
		adProvider.AdShownEvent = (Action<PlacementId>)Delegate.Combine(adProvider.AdShownEvent, new Action<PlacementId>(OnAdShown));
		adProvider.VideoFullyViewedEvent = (Action<PlacementId>)Delegate.Combine(adProvider.VideoFullyViewedEvent, new Action<PlacementId>(OnVideoFullyViewed));
		adProvider.VideoInterruptedEvent = (Action<PlacementId>)Delegate.Combine(adProvider.VideoInterruptedEvent, new Action<PlacementId>(OnVideoInterrupted));
		adProvider.TriggerVideoRewardEvent = (Action<PlacementId>)Delegate.Combine(adProvider.TriggerVideoRewardEvent, new Action<PlacementId>(OnSendVideoReward));
	}

	private void UnRegisterAdProvider(IAdProvider adProvider)
	{
		adProvider.AdAvailableEvent = (Action<PlacementId>)Delegate.Remove(adProvider.AdAvailableEvent, new Action<PlacementId>(OnAdAvailable));
		adProvider.AdNotAvailableEvent = (Action<PlacementId>)Delegate.Remove(adProvider.AdNotAvailableEvent, new Action<PlacementId>(OnAdNotAvailable));
		adProvider.NoContentAvailableEvent = (Action<PlacementId>)Delegate.Remove(adProvider.NoContentAvailableEvent, new Action<PlacementId>(OnNoContentAvailable));
		adProvider.AdClosedEvent = (Action<PlacementId>)Delegate.Remove(adProvider.AdClosedEvent, new Action<PlacementId>(OnAdClosed));
		adProvider.AdShownEvent = (Action<PlacementId>)Delegate.Remove(adProvider.AdShownEvent, new Action<PlacementId>(OnAdShown));
		adProvider.VideoFullyViewedEvent = (Action<PlacementId>)Delegate.Remove(adProvider.VideoFullyViewedEvent, new Action<PlacementId>(OnVideoFullyViewed));
		adProvider.VideoInterruptedEvent = (Action<PlacementId>)Delegate.Remove(adProvider.VideoInterruptedEvent, new Action<PlacementId>(OnVideoInterrupted));
		adProvider.TriggerVideoRewardEvent = (Action<PlacementId>)Delegate.Remove(adProvider.TriggerVideoRewardEvent, new Action<PlacementId>(OnSendVideoReward));
	}

	private void OnEnable()
	{
		AdEvents.BootEvent += OnBoot;
		AdEvents.ResumeEvent += OnResume;
		AdEvents.CustomAdEvent += OnCustomAdEvent;
		RegisterAdEvents();
	}

	private void OnDisable()
	{
		AdEvents.BootEvent += OnBoot;
		AdEvents.ResumeEvent += OnResume;
		AdEvents.CustomAdEvent += OnCustomAdEvent;
		UnregisterAdEvents();
	}

	protected virtual void RegisterAdEvents()
	{
	}

	protected virtual void UnregisterAdEvents()
	{
	}

	protected virtual void OnBoot()
	{
		if (!_bootPlacementShown)
		{
			_bootPlacementShown = true;
			ShowRewardedPlacement(PlacementId.BootPlacement);
		}
	}

	protected virtual void OnResume()
	{
		ShowRewardedPlacement(PlacementId.ResumePlacement);
	}

	protected virtual void OnCustomAdEvent(string eventName)
	{
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			_pauseTime = DateTime.Now;
			_adProviders.ForEach(delegate(IAdProvider provider)
			{
				provider.OnPause();
			});
		}
		else
		{
			_adProviders.ForEach(delegate(IAdProvider provider)
			{
				provider.OnResume();
			});
			TimeSpan timeSpan = DateTime.Now - _pauseTime;
			if (!_isAdDisplayed && GameSpecificCanDisplayResumePlacement() && timeSpan.TotalSeconds > _resumeDelay)
			{
				AdEvents.OnResume();
			}
		}
		GameSpecificOnApplicationPause(pause);
	}

	public bool IsEnabled(string placementId)
	{
		return IsEnabled(PlacementIdFromString(placementId));
	}

	public void ShowRewardedPlacement(string placementId)
	{
		ShowRewardedPlacement(PlacementIdFromString(placementId));
	}

	private PlacementId PlacementIdFromString(string placementId)
	{
		return EnumUtil.Parse(placementId, PlacementId.Undefined);
	}

	public bool IsEnabled(PlacementId placementId)
	{
		return _adProviders.Find((IAdProvider p) => p.IsSupported(placementId)) != null;
	}

	public bool IsRewardedAdAvailable(PlacementId placementId)
	{
		return _adProviders.Find((IAdProvider p) => p.IsSupported(placementId) && p.IsRewardedAdAvailable()) != null;
	}

	public void ShowRewardedPlacement(PlacementId placementId)
	{
		if (!IsEnabled(placementId))
		{
			SilentDebug.LogWarning(placementId.ToString() + " is disabled, user should not be able to trigger it.");
			return;
		}
		Action onGranted = delegate
		{
			bool flag = false;
			foreach (IAdProvider adProvider in _adProviders)
			{
				if (adProvider.IsRewardedAdAvailable() && adProvider.IsSupported(placementId))
				{
					flag |= adProvider.ShowRewardedPlacement(placementId);
				}
			}
			if (!flag)
			{
				OnNoContentAvailable(placementId);
			}
		};
		CheckPermissions(placementId, onGranted, delegate
		{
			ShowPermissionsDeniedPopup(placementId);
		});
	}

	private void ShowPermissionsDeniedPopup(PlacementId placementId)
	{
		string titleString = "Videos";
		string messageString = "You need to allow storage acces to view videos.\nIf you don't see the permissions popup, check your app settings";
		string buttonString = "Retry";
		Action buttonAction = delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			ShowRewardedPlacement(placementId);
		};
		MetroPopupMessage page = MetroMenuPage.Create<MetroPopupMessage>().Setup(titleString, messageString, buttonString, MetroSkin.Slice9ButtonBlue, buttonAction);
		AutoSingleton<MetroMenuStack>.Instance.Push(page);
	}

	public void CheckPermissions(PlacementId placementId, Action onGranted, Action onDenied)
	{
		if (placementId == PlacementId.FreeCratesPlacement || placementId == PlacementId.EndRunVideoPopupPlacement || placementId == PlacementId.OfferwallPlacement)
		{
			if (placementId == PlacementId.EndRunVideoPopupPlacement)
			{
				onDenied = delegate
				{
				};
			}
			AndroidRuntimePermissions.ExecuteWithPermission(AndroidPermission.WRITE_EXTERNAL_STORAGE, onGranted, onDenied);
		}
		else
		{
			onGranted?.Invoke();
		}
	}

	private void OnNoContentAvailable(PlacementId placementId)
	{
		if (placementId != PlacementId.EndRunVideoPopupPlacement)
		{
			_isAdDisplayed = false;
			GameSpecificOnNoContentAvailable(placementId);
		}
	}

	private void OnAdAvailable(PlacementId placementId)
	{
		AdEvents.OnAdAvailable(placementId);
	}

	private void OnAdNotAvailable(PlacementId placementId)
	{
		AdEvents.OnAdNotAvailable(placementId);
	}

	private void OnAdClosed(PlacementId placementId)
	{
		_isAdDisplayed = false;
		AdEvents.OnAdClosed(placementId);
	}

	private void OnAdShown(PlacementId placementId)
	{
		_isAdDisplayed = true;
		AdEvents.OnAdShown(placementId);
	}

	private void OnVideoFullyViewed(PlacementId placementId)
	{
		AdEvents.OnVideoFullyViewed(placementId);
	}

	private void OnVideoInterrupted(PlacementId placementId)
	{
		AdEvents.OnVideoInterrupted(placementId);
	}

	private void OnSendVideoReward(PlacementId placementId)
	{
		AdEvents.OnSendAdReward(placementId);
	}

	protected virtual void GameSpecificAddProviders()
	{
	}

	protected virtual void GameSpecificOnApplicationPause(bool pause)
	{
	}

	protected virtual bool GameSpecificCanDisplayResumePlacement()
	{
		return true;
	}

	protected virtual void GameSpecificOnNoContentAvailable(PlacementId placementId)
	{
	}
}
