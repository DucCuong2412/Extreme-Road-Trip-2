using System;
using System.Collections.Generic;
using UnityEngine;

public class BackendFacebook : AutoSingleton<BackendFacebook>, IBackend
{
	private List<User> _friends;

	private bool _needToUpdateFriends;

	private DateTime _friendsTimestamp;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		GameFacebookManager.OnLogout += OnLogout;
		BackendSessionManager.OnSessionStartedEvent -= UpdateFriendsBackend;
		BackendSessionManager.OnSessionStartedEvent += UpdateFriendsBackend;
		base.OnAwake();
	}

	protected override void OnDestroy()
	{
		GameFacebookManager.OnLogout -= OnLogout;
		BackendSessionManager.OnSessionStartedEvent -= UpdateFriendsBackend;
		base.OnDestroy();
	}

	public void Authenticate()
	{
		if (AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
		{
			RegisterAuthentication();
			AutoSingleton<GameFacebookManager>.Instance.RetrieveLocalUserInfo();
		}
		else if (!AutoSingleton<GameFacebookManager>.Instance.IsLoggingIn())
		{
			OnPlayerAuthenticated();
		}
	}

	public void OnLogout()
	{
		if (IsLoggedIn())
		{
			AutoSingleton<BackendManager>.Instance.LoginToBackend();
		}
	}

	public void RegisterAuthentication()
	{
		GameFacebookManager.OnUserInfoLoaded += OnPlayerAuthenticated;
		GameFacebookManager.OnRetrieveUserInfoFailed += OnPlayerAuthenticationFailed;
	}

	public void UnregisterAuthentication()
	{
		GameFacebookManager.OnUserInfoLoaded -= OnPlayerAuthenticated;
		GameFacebookManager.OnRetrieveUserInfoFailed -= OnPlayerAuthenticationFailed;
	}

	private void OnPlayerAuthenticated()
	{
		UnregisterAuthentication();
		bool flag = AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn();
		if (AutoSingleton<PlatformCapabilities>.Instance.UseFacebookAsSocialPlatform())
		{
			AutoSingleton<BackendManager>.Instance.LoginToBackend();
		}
		if (flag)
		{
			RetrieveFriends();
		}
		if (AutoSingleton<PlatformCapabilities>.Instance.UseFacebookAsSocialPlatform())
		{
			AutoSingleton<BackendManager>.Instance.OnPlayerAuthenticated();
		}
	}

	private void OnPlayerAuthenticationFailed(string error)
	{
		SilentDebug.LogWarning("=== Facebook User authentication failed: " + error);
		UnregisterAuthentication();
	}

	public bool IsLoggedIn()
	{
		return AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn();
	}

	public void OnApplicationPause(bool pause)
	{
		if (!pause && !AutoSingleton<GameFacebookManager>.Instance.IsLoggingIn())
		{
			Authenticate();
			if (AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
			{
				AutoSingleton<GameFacebookManager>.Instance.CheckPublishPermissions(null);
			}
		}
	}

	public string PlayerAlias()
	{
		return AutoSingleton<GameFacebookManager>.Instance.PlayerAlias();
	}

	public string PlayerIdentifier()
	{
		if (AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
		{
			string text = AutoSingleton<GameFacebookManager>.Instance.PlayerId();
			if (text != string.Empty)
			{
				return text;
			}
			SilentDebug.LogWarning("Warning: User is logged in to Facebook, but doesn't have a player id set!");
		}
		return Device.GetDeviceId();
	}

	public List<User> Friends()
	{
		return _friends;
	}

	public void RetrieveFriends()
	{
		TimeSpan timeSpan = DateTime.Now - _friendsTimestamp;
		if (_friends == null || timeSpan.TotalMinutes > 30.0)
		{
			RegisterRetrieveFriends();
			AutoSingleton<GameFacebookManager>.Instance.RetrieveFriends();
		}
		else
		{
			OnRetrieveFriends(_friends);
		}
	}

	public void RegisterRetrieveFriends()
	{
		_needToUpdateFriends = false;
		GameFacebookManager.OnFriendsLoaded += OnRetrieveFriends;
		GameFacebookManager.OnRetrieveFriendsFailed += OnRetrieveFriendsFailed;
	}

	public void UnregisterRetrieveFriends()
	{
		GameFacebookManager.OnFriendsLoaded -= OnRetrieveFriends;
		GameFacebookManager.OnRetrieveFriendsFailed -= OnRetrieveFriendsFailed;
	}

	public void OnRetrieveFriends(List<User> users)
	{
		_needToUpdateFriends = true;
		UnregisterRetrieveFriends();
		_friendsTimestamp = DateTime.Now;
		_friends = users;
		AutoSingleton<BackendManager>.Instance.OnRetrieveFriends(users);
		AutoSingleton<LeaderboardsManager>.Instance.CacheLeaderboards(SocialPlatform.facebook);
	}

	private void UpdateFriendsBackend()
	{
		if (_friends != null && _friends.Count > 0 && _needToUpdateFriends && AutoSingleton<BackendSessionManager>.Instance.IsSessionValid())
		{
			BackendFriendsApiClient.UpdateFriends(new FriendIds(_friends.ConvertAll((User u) => u._id), new List<string>()), null, null);
			_needToUpdateFriends = false;
		}
	}

	public int GetFriendCount()
	{
		if (_friends != null)
		{
			return _friends.Count;
		}
		return 0;
	}

	private void OnRetrieveFriendsFailed(string error)
	{
		UnregisterRetrieveFriends();
		AutoSingleton<BackendManager>.Instance.OnRetrieveFriendsFailed(error);
	}
}
