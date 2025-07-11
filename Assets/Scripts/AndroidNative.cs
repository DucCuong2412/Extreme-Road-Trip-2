using UnityEngine;

public class AndroidNative
{
	private static int _androidSDKVersionCache = 42;

	private static string _androidIdCache;

	public static int GetAndroidSDKVersion()
	{
		if (Application.isEditor)
		{
			return 0;
		}
		if (_androidSDKVersionCache == 42)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build$VERSION"))
			{
				_androidSDKVersionCache = int.Parse(androidJavaClass.GetStatic<string>("SDK"));
			}
		}
		return _androidSDKVersionCache;
	}

	public static bool IsBeforeICS()
	{
		return GetAndroidSDKVersion() < 14;
	}

	public static bool IsValidUrl(string url)
	{
		if (Application.isEditor)
		{
			return false;
		}
		bool flag = false;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.webkit.URLUtil"))
		{
			return androidJavaClass.CallStatic<bool>("isValidUrl", new object[1]
			{
				url
			});
		}
	}

	public static bool IsAppInstalled(string url)
	{
		if (Application.isEditor)
		{
			return false;
		}
		bool flag = false;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[1]
			{
				url
			});
			AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW", androidJavaObject2);
			int static2 = androidJavaObject.GetStatic<int>("MATCH_DEFAULT_ONLY");
			AndroidJavaObject androidJavaObject4 = androidJavaObject.Call<AndroidJavaObject>("queryIntentActivities", new object[2]
			{
				androidJavaObject3,
				static2
			});
			return androidJavaObject4.Call<int>("size", new object[0]) > 0;
		}
	}

	public static string GetAndroidId()
	{
		if (Application.isEditor)
		{
			return "7e0c00a547ec1358";
		}
		if (Application.platform != RuntimePlatform.Android)
		{
			return Device.GetDeviceId();
		}
		if (_androidIdCache != null)
		{
			return _androidIdCache;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.provider.Settings$Secure"))
			{
				return _androidIdCache = androidJavaClass2.CallStatic<string>("getString", new object[2]
				{
					androidJavaObject,
					androidJavaClass2.GetStatic<string>("ANDROID_ID")
				});
				IL_009b:
				string result;
				return result;
			}
		}
	}

	public static void Share(string content, string subject, string imagePath = null)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", "android.intent.action.SEND");
				if (!string.IsNullOrEmpty(imagePath))
				{
					AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
					AndroidJavaObject androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[1]
					{
						"file://" + imagePath
					});
					androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("setType", new object[1]
					{
						"image/PNG"
					});
					androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
					{
						"android.intent.extra.STREAM",
						androidJavaObject2
					});
				}
				else
				{
					androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("setType", new object[1]
					{
						"text/plain"
					});
				}
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
				{
					"android.intent.extra.TEXT",
					content
				});
				androidJavaObject = androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
				{
					"android.intent.extra.SUBJECT",
					subject
				});
				AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("android.content.Intent");
				AndroidJavaObject androidJavaObject3 = androidJavaClass3.CallStatic<AndroidJavaObject>("createChooser", new object[2]
				{
					androidJavaObject,
					FacebookSetting.ApplicationName
				});
				@static.Call("startActivity", androidJavaObject3);
			}
		}
	}
}
