using UnityEngine;

public static class ExternalAppLaunch
{
	public static bool IsGameInstalled(string appId)
	{
		if (!string.IsNullOrEmpty(appId))
		{
			string uRL = GetURL(appId, string.Empty);
			return AndroidNative.IsAppInstalled(uRL);
		}
		UnityEngine.Debug.LogWarning("app Id null or empty");
		return false;
	}

	public static bool LaunchExternalApp(string appId, string param)
	{
		bool flag = IsGameInstalled(appId);
		if (flag)
		{
			string uRL = GetURL(appId, param);
			Application.OpenURL(uRL);
		}
		return flag;
	}

	public static void LaunchOrRedirect(string appId, string param, string redirectURL)
	{
		if (!LaunchExternalApp(appId, param))
		{
			Application.OpenURL(redirectURL);
		}
	}

	private static string GetURL(string id, string param)
	{
		string str = string.IsNullOrEmpty(param) ? string.Empty : param;
		return id + "://" + str;
	}
}
