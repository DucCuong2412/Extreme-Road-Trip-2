using UnityEngine;

public class Device
{
	private enum DeviceForm
	{
		tablet,
		phone
	}

	private static int _deviceAPIVersion;

	private static string _deviceForm;

	private static Maybe<bool> _isTablet;

	private static int _fixedUpdateRate;

	private static int _targetFrameRate;

	static Device()
	{
		_deviceAPIVersion = -1;
		_isTablet = new Maybe<bool>();
		_fixedUpdateRate = 60;
		_targetFrameRate = 60;
		_fixedUpdateRate = 30;
		_targetFrameRate = 60;
	}

	public static bool IsIPad()
	{
		return SystemInfo.deviceModel.Contains("iPad");
	}

	public static bool IsTablet()
	{
		if (!_isTablet.IsSet())
		{
			_isTablet.Set(data: false);
			if (Screen.dpi > 10f && Screen.dpi < 1000f)
			{
				float num = (float)Screen.width / Screen.dpi;
				float num2 = (float)Screen.height / Screen.dpi;
				float num3 = Mathf.Sqrt(num * num + num2 * num2);
				_isTablet.Set(num3 >= 6f);
			}
		}
		return _isTablet.Get();
	}

	public static string GetDeviceForm()
	{
		if (string.IsNullOrEmpty(_deviceForm))
		{
			if (IsTablet())
			{
				_deviceForm = DeviceForm.tablet.ToString();
			}
			else
			{
				_deviceForm = DeviceForm.phone.ToString();
			}
		}
		return _deviceForm;
	}

	public static string GetDeviceType()
	{
		return "android";
	}

	public static int GetFixedUpdateRate()
	{
		return _fixedUpdateRate;
	}

	public static int GetTargetFrameRate()
	{
		return _targetFrameRate;
	}

	public static string GetDeviceId()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	public static string GetDeviceAPIVersionString()
	{
		return GetDeviceAPIVersionInt().ToString();
	}

	public static int GetDeviceAPIVersionInt()
	{
		if (_deviceAPIVersion == -1)
		{
			_deviceAPIVersion = AndroidNative.GetAndroidSDKVersion();
		}
		return _deviceAPIVersion;
	}
}
