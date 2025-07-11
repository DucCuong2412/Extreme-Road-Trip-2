using Prime31;
using System.Collections;
using System.Collections.Generic;

public class FakeFacebook
{
	private static string _accessToken = "BAAD80zjdIGABAE4FCLcP77i11y53luDQDYs9qPhvTalSbJrgeZAaTb83uDoi37X3SsVojU7DsZCPGhec9m7dJiVLhtqPm0kKTZBYTGAXV4jUXtKNEZB4TTTccjLboFeEKU4p6ZBq22cUI8u8RGiMkGR4sgWz0YABYPXCdiBcOqYegaCFDQaCEZA9Ee2jazRHupbxCGmPzMKs6WGXnZAkbOY";

	private static bool _isLoggedIn;

	public static void init()
	{
		Facebook.instance.accessToken = _accessToken;
	}

	public static string getAppLaunchUrl()
	{
		return string.Empty;
	}

	public static bool isSessionValid()
	{
		return _isLoggedIn;
	}

	public static string getAccessToken()
	{
		return _accessToken;
	}

	public static void login()
	{
		loginWithRequestedPermissions(new string[0]);
	}

	public static void loginWithRequestedPermissions(string[] permissions)
	{
		loginWithRequestedPermissions(permissions, null);
	}

	public static void loginWithReadPermissions(string[] permissions)
	{
		_isLoggedIn = true;
		AutoSingleton<GameFacebookManager>.Instance.OnSessionOpenedEvent();
	}

	public static void loginWithRequestedReadPermissions(string[] permissions, string urlSchemeSuffix)
	{
		_isLoggedIn = true;
		AutoSingleton<GameFacebookManager>.Instance.OnSessionOpenedEvent();
	}

	public static void loginWithRequestedPermissions(string[] permissions, string urlSchemeSuffix)
	{
		_isLoggedIn = true;
		AutoSingleton<GameFacebookManager>.Instance.OnSessionOpenedEvent();
	}

	public static void reauthorizeWithPublishPermissions(string[] permissions, FacebookSessionDefaultAudience defaultAudience)
	{
		_isLoggedIn = true;
		AutoSingleton<GameFacebookManager>.Instance.OnSessionOpenedEvent();
	}

	public static void logout()
	{
		_isLoggedIn = false;
	}

	public static void showDialog(string dialogType, Dictionary<string, string> options)
	{
		if (dialogType == "apprequests")
		{
			AutoSingleton<GameFacebookManager>.Instance.OnDialogCompletedWithUrl("fbconnect://success?request=489644101055399&to%5B0%5D=100004374901082&to%5B1%5D=670235359");
		}
	}

	public static void restRequest(string restMethod, string httpMethod, Hashtable keyValueHash)
	{
	}

	public static void graphRequest(string graphPath, string httpMethod, Hashtable keyValueHash)
	{
	}

	public static bool isFacebookComposerSupported()
	{
		return false;
	}

	public static bool canUserUseFacebookComposer()
	{
		return false;
	}

	public static void showFacebookComposer(string message)
	{
		showFacebookComposer(message, null, null);
	}

	public static void showFacebookComposer(string message, string imagePath, string link)
	{
	}

	public static void setAppVersion(string version)
	{
	}
}
