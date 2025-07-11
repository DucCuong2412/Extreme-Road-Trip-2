using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : AutoSingleton<NotificationManager>
{
	private PersistentString _cachedDeviceToken;

	private List<int> _notificationIDList;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_cachedDeviceToken = new PersistentString("CachedDeviceToken", string.Empty);
		_notificationIDList = new List<int>();
		ArrayList arrayList = Preference.GetString("NotificationIDList", "[]").arrayListFromJson();
		if (arrayList != null)
		{
			foreach (string item in arrayList)
			{
				if (int.TryParse(item, out int result))
				{
					_notificationIDList.Add(result);
				}
			}
		}
		base.OnAwake();
	}

	private void SaveNotificationID()
	{
		ArrayList arrayList = new ArrayList();
		foreach (int notificationID in _notificationIDList)
		{
			arrayList.Add(Convert.ToString(notificationID));
		}
		Preference.SetString("NotificationIDList", arrayList.toJson());
	}

	protected override void OnStart()
	{
		CleanupNotifications();
		//GoogleCloudMessaging.register("108352960072");
		//GoogleCloudMessagingManager.registrationSucceededEvent += OnRegisterNotificationSuccessEvent;
		//GoogleCloudMessaging.checkForNotifications();
	}

	private void OnRegisterNotificationSuccessEvent(string registrationId)
	{
		_cachedDeviceToken.Set(registrationId);
	}

	public void ScheduleLocalNotification(DateTime fireDate, string title, string message)
	{
		TimeSpan timeSpan = fireDate - DateTime.Now;
		_notificationIDList.Add(EtceteraAndroid.scheduleNotification(Convert.ToInt64(timeSpan.TotalSeconds), title.Localize(), message.Localize(), message.Localize(), string.Empty));
		SaveNotificationID();
	}

	public void CancelAllLocalNotifications()
	{
		if (_notificationIDList.Count > 0)
		{
			foreach (int notificationID in _notificationIDList)
			{
				EtceteraAndroid.cancelNotification(notificationID);
			}
			_notificationIDList.Clear();
			SaveNotificationID();
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			ScheduleGameSpecificNotification();
		}
		else
		{
			CleanupNotifications();
		}
	}

	public void OnApplicationQuit()
	{
		CleanupNotifications();
		ScheduleGameSpecificNotification();
	}

	private void CleanupNotifications()
	{
		CancelAllLocalNotifications();
	}

	private void ScheduleGameSpecificNotification()
	{
		if (!Preference.UseNotifications())
		{
			CleanupNotifications();
			return;
		}
		string title = "It's been a while!";
		ScheduleLocalNotification(DateTime.Now.AddDays(7.0), title, "Time to ride again! Get back on the road!");
		if (!AutoSingleton<FrenzyModeManager>.Instance.HasFreeAccess())
		{
			string title2 = "Frenzy Run Ready!";
			ScheduleLocalNotification(AutoSingleton<FrenzyModeManager>.Instance.NextRunDate(), title2, "A Frenzy Run awaits you! Grab free bucks now!");
		}
	}

	public string GetDeviceToken()
	{
		return _cachedDeviceToken.Get();
	}
}
