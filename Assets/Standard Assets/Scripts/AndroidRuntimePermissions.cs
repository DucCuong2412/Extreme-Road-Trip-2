using System;
using UnityEngine;

public class AndroidRuntimePermissions : MonoBehaviour
{
	private const string WRITE_EXTERNAL_STORAGE = "WRITE_EXTERNAL_STORAGE";

	private const string PERMISSION_GRANTED = "PERMISSION_GRANTED";

	private const string PERMISSION_DENIED = "PERMISSION_DENIED";

	private const string PERMISSION_GRANTER_NAME = "AndroidRuntimePermissions";

	private static Action _onPermissionGranted;

	private static Action _onPermissionDenied;

	public static Action<bool> PermissionRequestCallback;

	private static AndroidRuntimePermissions instance;

	private static bool initialized;

	private static AndroidJavaClass javaPermissionGranterClass;

	private static AndroidJavaObject activity;

	public static void ExecuteWithPermission(AndroidPermission permission, Action onGranted, Action onDenied)
	{
		_onPermissionGranted = onGranted;
		_onPermissionDenied = onDenied;
		GrantPermission(permission);
	}

	private static void GrantPermission(AndroidPermission permission)
	{
		if (!initialized)
		{
			Initialize();
		}
		javaPermissionGranterClass.CallStatic("grantPermission", activity, (int)permission);
	}

	public static string GetExternalCachePath()
	{
		if (!initialized)
		{
			Initialize();
		}
		return javaPermissionGranterClass.CallStatic<string>("GetExternalCachePath", new object[1]
		{
			activity
		});
	}

	public static string GetExternalFilesPath()
	{
		if (!initialized)
		{
			Initialize();
		}
		return javaPermissionGranterClass.CallStatic<string>("GetExternalFilesPath", new object[1]
		{
			activity
		});
	}

	public void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (base.name != "AndroidRuntimePermissions")
		{
			base.name = "AndroidRuntimePermissions";
		}
	}

	private static void Initialize()
	{
		if (instance == null)
		{
			GameObject gameObject = new GameObject();
			instance = gameObject.AddComponent<AndroidRuntimePermissions>();
			gameObject.name = "AndroidRuntimePermissions";
		}
		javaPermissionGranterClass = new AndroidJavaClass("ca.roofdog.unityplugins.PermissionGranter");
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		initialized = true;
	}

	private void PermissionRequestCallbackInternal(string message)
	{
		if (message == "PERMISSION_GRANTED")
		{
			if (_onPermissionGranted != null)
			{
				_onPermissionGranted();
			}
		}
		else if (_onPermissionDenied != null)
		{
			_onPermissionDenied();
		}
		_onPermissionGranted = null;
		_onPermissionDenied = null;
	}
}
