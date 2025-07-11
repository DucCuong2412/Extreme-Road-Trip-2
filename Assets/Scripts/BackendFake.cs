using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendFake : AutoSingleton<BackendFake>, IBackend
{
	private Action PlayerAuthenticated;

	private Action<string> PlayerFailedToAuthenticate;

	private Action<List<User>> OnFriendsLoaded;

	private Action<string> OnFriendsLoadFailed;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.OnAwake();
	}

	public void Authenticate()
	{
		FakeAuthentication();
	}

	public void RegisterAuthentication()
	{
		PlayerAuthenticated = (Action)Delegate.Combine(PlayerAuthenticated, new Action(OnPlayerAuthenticated));
		PlayerFailedToAuthenticate = (Action<string>)Delegate.Combine(PlayerFailedToAuthenticate, new Action<string>(OnPlayerAuthenticationFailed));
	}

	public void UnregisterAuthentication()
	{
		PlayerAuthenticated = (Action)Delegate.Remove(PlayerAuthenticated, new Action(OnPlayerAuthenticated));
		PlayerFailedToAuthenticate = (Action<string>)Delegate.Remove(PlayerFailedToAuthenticate, new Action<string>(OnPlayerAuthenticationFailed));
	}

	private void FakeAuthentication()
	{
		StartCoroutine(FakeAuthenticationCR());
	}

	private IEnumerator FakeAuthenticationCR()
	{
		RegisterAuthentication();
		yield return new WaitForSeconds(0.5f);
		if (PlayerAuthenticated != null)
		{
			PlayerAuthenticated();
		}
	}

	private void OnPlayerAuthenticated()
	{
		UnregisterAuthentication();
		AutoSingleton<BackendManager>.Instance.LoginToBackend();
		RetrieveFriends();
		AutoSingleton<LeaderboardsManager>.Instance.CacheLeaderboards(SocialPlatform.fake);
		AutoSingleton<BackendManager>.Instance.OnPlayerAuthenticated();
	}

	private void OnPlayerAuthenticationFailed(string error)
	{
		UnityEngine.Debug.LogWarning("=== Fake User authentication failed: " + error);
		UnregisterAuthentication();
	}

	public bool IsLoggedIn()
	{
		return true;
	}

	public void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			AutoSingleton<BackendManager>.Instance.Authenticate();
		}
	}

	public string PlayerAlias()
	{
		return SystemInfo.deviceName;
	}

	public string PlayerIdentifier()
	{
		return Device.GetDeviceId();
	}

	public void LoadLocalProfilePhoto()
	{
	}

	public List<User> Friends()
	{
		return null;
	}

	public void RetrieveFriends()
	{
		RegisterRetrieveFriends();
		StartCoroutine(FakeRetrieveFriendsCR());
	}

	private IEnumerator FakeRetrieveFriendsCR()
	{
		yield return new WaitForSeconds(1f);
		User user9 = new User
		{
			_id = "id1",
			_alias = "player1",
			_displayName = "Real Name 1"
		};
		User user8 = new User
		{
			_id = "id2",
			_alias = "player2",
			_displayName = "Real Name 2"
		};
		User user7 = new User
		{
			_id = "id3",
			_alias = "player3",
			_displayName = "Real Name 3"
		};
		User user6 = new User
		{
			_id = "id4",
			_alias = "player4",
			_displayName = "Real Name 4"
		};
		User user5 = new User
		{
			_id = "id5",
			_alias = "player5",
			_displayName = "Real Name 5"
		};
		List<User> users = new List<User>
		{
			user9,
			user8,
			user7,
			user6,
			user5
		};
		if (OnFriendsLoaded != null)
		{
			OnFriendsLoaded(users);
		}
	}

	public void RegisterRetrieveFriends()
	{
		OnFriendsLoaded = (Action<List<User>>)Delegate.Combine(OnFriendsLoaded, new Action<List<User>>(OnRetrieveFriends));
		OnFriendsLoadFailed = (Action<string>)Delegate.Combine(OnFriendsLoadFailed, new Action<string>(OnRetrieveFriendsFailed));
	}

	public void UnregisterRetrieveFriends()
	{
		OnFriendsLoaded = (Action<List<User>>)Delegate.Remove(OnFriendsLoaded, new Action<List<User>>(OnRetrieveFriends));
		OnFriendsLoadFailed = (Action<string>)Delegate.Remove(OnFriendsLoadFailed, new Action<string>(OnRetrieveFriendsFailed));
	}

	private void OnRetrieveFriends(List<User> friends)
	{
		UnregisterRetrieveFriends();
		AutoSingleton<BackendManager>.Instance.OnRetrieveFriends(friends);
	}

	private void OnRetrieveFriendsFailed(string error)
	{
		UnregisterRetrieveFriends();
		AutoSingleton<BackendManager>.Instance.OnRetrieveFriendsFailed(error);
	}

	public int GetFriendCount()
	{
		return 0;
	}
}
