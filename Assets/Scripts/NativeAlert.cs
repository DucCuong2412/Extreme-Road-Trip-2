using System;
using System.Runtime.CompilerServices;

public class NativeAlert
{
	[method: MethodImpl(32)]
	public static event Action<string> AlertButtonClickedEvent;

	[method: MethodImpl(32)]
	public static event Action AlertCancelledEvent;

	public static void Show(string title, string msg, string button1Label)
	{
		RegisterEvents();
		string title2 = title.Localize();
		string message = msg.Localize();
		string positiveButton = button1Label.Localize();
		EtceteraAndroid.showAlert(title2, message, positiveButton);
	}

	private static void OnAlertButtonClicked(string button)
	{
		if (NativeAlert.AlertButtonClickedEvent != null)
		{
			NativeAlert.AlertButtonClickedEvent(button);
		}
		UnregisterEvents();
	}

	private static void OnAlertCancelled()
	{
		if (NativeAlert.AlertCancelledEvent != null)
		{
			NativeAlert.AlertCancelledEvent();
		}
		UnregisterEvents();
	}

	private static void RegisterEvents()
	{
		EtceteraAndroidManager.alertButtonClickedEvent += OnAlertButtonClicked;
		EtceteraAndroidManager.alertCancelledEvent += OnAlertCancelled;
	}

	private static void UnregisterEvents()
	{
		EtceteraAndroidManager.alertButtonClickedEvent -= OnAlertButtonClicked;
		EtceteraAndroidManager.alertCancelledEvent -= OnAlertCancelled;
	}
}
