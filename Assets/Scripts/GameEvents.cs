using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class GameEvents
{
	[method: MethodImpl(32)]
	public static event Action FacebookLoginEvent;

	[method: MethodImpl(32)]
	public static event Action<string> FacebookLoginFailedEvent;

	[method: MethodImpl(32)]
	public static event Action FacebookLogoutEvent;

	[method: MethodImpl(32)]
	public static event Action<SocialPlatform, List<User>> SocialFriendsLoadedEvent;

	[method: MethodImpl(32)]
	public static event Action SocialFriendsLoadingFailedEvent;

	[method: MethodImpl(32)]
	public static event Action<int, int, float> PlayerXPEvent;

	public static void OnFacebookLogin()
	{
		if (GameEvents.FacebookLoginEvent != null)
		{
			GameEvents.FacebookLoginEvent();
		}
	}

	public static void OnFacebookLoginFailed(string error)
	{
		if (GameEvents.FacebookLoginFailedEvent != null)
		{
			GameEvents.FacebookLoginFailedEvent(error);
		}
	}

	public static void OnFacebookLogout()
	{
		if (GameEvents.FacebookLogoutEvent != null)
		{
			GameEvents.FacebookLogoutEvent();
		}
	}

	public static void OnSocialFriendsLoaded(SocialPlatform platform, List<User> friends)
	{
		if (GameEvents.SocialFriendsLoadedEvent != null)
		{
			GameEvents.SocialFriendsLoadedEvent(platform, friends);
		}
	}

	public static void OnSocialFriendsLoadingFailed()
	{
		if (GameEvents.SocialFriendsLoadingFailedEvent != null)
		{
			GameEvents.SocialFriendsLoadingFailedEvent();
		}
	}

	public static void OnPlayerXP(int previousLevel, int currentLevel, float xpGained)
	{
		if (GameEvents.PlayerXPEvent != null)
		{
			GameEvents.PlayerXPEvent(previousLevel, currentLevel, xpGained);
		}
	}
}
