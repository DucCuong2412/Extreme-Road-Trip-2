using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameFacebookManager : AutoSingleton<GameFacebookManager>
{
	private const string _apiVersion = "";

	private Maybe<bool> _facebookLike = new Maybe<bool>();

	private Maybe<bool> _publishPermissionGranted = new Maybe<bool>();

	private Action _onPublishPermissionAsked;

	private Action<int> _onInviteFriendsDialogCompleted;

	private Action _onLoginSucceeded;

	private Action<string> _onLoginFailed;

	private string _playerAlias = string.Empty;

	private string _playerId = string.Empty;

	[method: MethodImpl(32)]
	public static event Action OnRetrieveScoresFailed;

	[method: MethodImpl(32)]
	public static event Action<List<LeaderboardScore>> OnScoresLoaded;

	[method: MethodImpl(32)]
	public static event Action<string> OnRetrieveUserInfoFailed;

	[method: MethodImpl(32)]
	public static event Action OnUserInfoLoaded;

	[method: MethodImpl(32)]
	public static event Action<List<User>> OnFriendsLoaded;

	[method: MethodImpl(32)]
	public static event Action<string> OnRetrieveFriendsFailed;

	[method: MethodImpl(32)]
	public static event Action OnLogout;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		FacebookManager.reauthorizationSucceededEvent += OnPublishPermissionSucceeded;
		FacebookManager.reauthorizationFailedEvent += OnPublishPermissionFailed;
		FacebookManager.dialogCompletedWithUrlEvent += OnDialogCompletedWithUrl;
		FacebookManager.dialogFailedEvent += OnDialogFailed;
		FacebookManager.sessionOpenedEvent += OnSessionOpenedEvent;
		FacebookManager.loginFailedEvent += OnLoginFailed;
		Init();
		_facebookLike.Reset();
		_publishPermissionGranted.Reset();
		BackendManager instance = AutoSingleton<BackendManager>.Instance;
		instance.OnLeaderboardGetSucceeded += OnLeaderboardGetSucceeded;
		instance.OnLeaderboardGetFailed += OnLeaderboardGetFailed;
		base.OnAwake();
	}

	private void Init()
	{
		Facebook.instance.accessToken = FacebookAndroid.getAccessToken();
		FacebookAndroid.init();
		FacebookAndroid.setAppVersion(GameVersion.VERSION);
		ResumeLogin();
	}

	private void ResumeLogin()
	{
		if (IsLoggedIn())
		{
			_onLoginSucceeded = (Action)Delegate.Combine(_onLoginSucceeded, (Action)delegate
			{
				CheckPublishPermissions(null);
			});
			OnSessionOpenedEvent();
		}
		else if (!string.IsNullOrEmpty(FacebookAndroid.getAccessToken()))
		{
			_onLoginSucceeded = (Action)Delegate.Combine(_onLoginSucceeded, (Action)delegate
			{
				CheckPublishPermissions(null);
				AutoSingleton<BackendManager>.Instance.Authenticate(SocialPlatform.facebook);
			});
			_onLoginFailed = (Action<string>)Delegate.Combine(_onLoginFailed, (Action<string>)delegate
			{
				AutoSingleton<BackendManager>.Instance.Authenticate(SocialPlatform.facebook);
			});
			OnSessionOpenedEvent();
		}
	}

	public string GetAppLaunchUrl()
	{
		return FacebookAndroid.getAppLaunchUrl();
	}

	public bool IsAvailable()
	{
		return true;
	}

	public bool IsLoggedIn()
	{
		return FacebookAndroid.isSessionValid();
	}

	public bool IsLoggingIn()
	{
		return _onLoginSucceeded != null;
	}

	public string PlayerAlias()
	{
		return _playerAlias;
	}

	public string PlayerId()
	{
		return _playerId;
	}

	[Obsolete("We removed the like permission")]
	public void ResetUserLikes()
	{
		_facebookLike.Reset();
	}

	public bool GetFacebookLike()
	{
		return _facebookLike.Get();
	}

	[Obsolete("We removed the like permission")]
	public void RefreshUserLikes()
	{
		ResetUserLikes();
		if (Application.isEditor)
		{
			_facebookLike.Set(data: true);
		}
		if (IsLoggedIn())
		{
			Facebook.instance.graphRequest("me/likes/" + FacebookSetting.WebPageId, HTTPVerb.GET, null, GetFacebookLikesRequestHandler);
		}
	}

	[Obsolete("We removed the like permission")]
	private void GetFacebookLikesRequestHandler(string error, object result)
	{
		if (!string.IsNullOrEmpty(error))
		{
			SilentDebug.LogError("GetFacebookLikesRequestHandler error: " + error);
			return;
		}
		List<object> list = FacebookResultToList(result);
		if (list != null)
		{
			foreach (object item in list)
			{
				Hashtable data = new Hashtable(item as Dictionary<string, object>);
				if (JsonUtil.ExtractString(data, "id", string.Empty).Equals(FacebookSetting.WebPageId))
				{
					_facebookLike.Set(data: true);
				}
			}
		}
		if (!_facebookLike.IsSet())
		{
			_facebookLike.Set(data: false);
		}
	}

	[Obsolete("We removed the like permission")]
	public bool DoesUserLikesGame()
	{
		return (_facebookLike.IsSet() && _facebookLike.Get()) || AutoSingleton<PersistenceManager>.Instance.FacebookRewardCollected;
	}

	[Obsolete("We removed the like permission")]
	public bool DoesUserNotLikesGame()
	{
		return _facebookLike.IsSet() && !_facebookLike.Get();
	}

	public void Login(Action onLoginWithPermissionSucceeded, bool isPublishNeeded, Action<string> onLoginFailed = null)
	{
		Action onLoginSucceeded = delegate
		{
			AutoSingleton<PersistenceManager>.Instance.MustShowFacebookInvitationToLoginPopup = false;
			if (!isPublishNeeded)
			{
				CheckPublishPermissions(null);
				onLoginWithPermissionSucceeded();
			}
			else
			{
				CheckPublishPermissions(onLoginWithPermissionSucceeded);
			}
		};
		LoginWithPublishPermission(onLoginSucceeded, onLoginFailed);
	}

	private void LoginWithPublishPermission(Action onLoginSucceeded, Action<string> onLoginFailed)
	{
		_onLoginSucceeded = onLoginSucceeded;
		_onLoginFailed = onLoginFailed;
		_publishPermissionGranted.Reset();
		if (IsLoggedIn())
		{
			OnSessionOpenedEvent();
			return;
		}
		FacebookAndroid.logout();
		FacebookAndroid.loginWithReadPermissions(new string[1]
		{
			"user_friends"
		});
	}

	public void OnSessionOpenedEvent()
	{
		if (_onLoginSucceeded != null)
		{
			_onLoginSucceeded();
			_onLoginSucceeded = null;
		}
		_onLoginFailed = null;
	}

	private void OnLoginFailed(P31Error error)
	{
		string text = (error == null) ? string.Empty : error.message;
		SilentDebug.LogError("Facebook login error: " + text + "Prime code" + error.code);
		if (_onLoginFailed != null)
		{
			_onLoginFailed(text);
			_onLoginFailed = null;
		}
		_onLoginSucceeded = null;
	}

	private void AuthorizePublishPermissions(Action onPublishPermissionAsked)
	{
		if (IsLoggedIn())
		{
			_onPublishPermissionAsked = onPublishPermissionAsked;
			string[] permissions = new string[1]
			{
				"publish_actions"
			};
			FacebookAndroid.reauthorizeWithPublishPermissions(permissions, FacebookSessionDefaultAudience.Friends);
		}
		else
		{
			SilentDebug.LogWarning("Facebook: You must be logged in to authorize publish permission");
		}
	}

	public void CheckPublishPermissions(Action onPublishPermissionAsked)
	{
		if (_publishPermissionGranted.IsSet())
		{
			HandleCheckPublishPermission(_publishPermissionGranted.Get(), onPublishPermissionAsked);
		}
		else
		{
			Facebook.instance.graphRequest("me/permissions", HTTPVerb.GET, delegate(string error, object result)
			{
				bool flag = false;
				if (!string.IsNullOrEmpty(error))
				{
					SilentDebug.LogError("CheckPublishPermissions failed: " + error);
				}
				else
				{
					List<object> list = FacebookResultToList(result);
					if (list != null)
					{
						foreach (object item in list)
						{
							Hashtable data = new Hashtable(item as Dictionary<string, object>);
							if (JsonUtil.ExtractString(data, "permission", string.Empty) == "publish_actions")
							{
								flag = (JsonUtil.ExtractString(data, "status", string.Empty) == "granted");
								break;
							}
						}
						_publishPermissionGranted.Set(flag);
					}
				}
				HandleCheckPublishPermission(flag, onPublishPermissionAsked);
			});
		}
	}

	private void HandleCheckPublishPermission(bool isGranted, Action onGranted)
	{
		if (isGranted)
		{
			onGranted?.Invoke();
		}
		else if (onGranted != null)
		{
			AuthorizePublishPermissions(onGranted);
		}
	}

	public bool IsPublishPermissionVerify()
	{
		return _publishPermissionGranted.IsSet();
	}

	private void OnPublishPermissionSucceeded()
	{
		_publishPermissionGranted.Set(data: true);
		if (_onPublishPermissionAsked != null)
		{
			_onPublishPermissionAsked();
		}
		_onPublishPermissionAsked = null;
	}

	private void OnPublishPermissionFailed(P31Error error)
	{
		SilentDebug.LogWarning(("OnPublishPermissionFailed: " + error == null) ? string.Empty : (error.message + " Code " + error.code));
		_publishPermissionGranted.Set(data: false);
		if (_onPublishPermissionAsked != null)
		{
			_onPublishPermissionAsked();
		}
		_onPublishPermissionAsked = null;
	}

	[Obsolete("We removed the like permission")]
	public void LoginAndRefreshUserLikes(Action<string> onLoginFailed)
	{
		ResetUserLikes();
		Login(delegate
		{
			RefreshUserLikes();
		}, isPublishNeeded: false, onLoginFailed);
	}

	[Obsolete("We removed the like permission")]
	public void LaunchGameFacebookPage()
	{
		ResetUserLikes();
		Login(delegate
		{
			Application.OpenURL(FacebookSetting.FacebookGamePageUrl);
		}, isPublishNeeded: false);
	}

	public void PostScore(int score)
	{
		if (IsLoggedIn() && IsPublishPermissionGranted())
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("score", score.ToString());
			Dictionary<string, object> parameters = dictionary;
			Facebook.instance.graphRequest("me/scores", HTTPVerb.POST, parameters, null);
		}
	}

	public void OnLeaderboardGetSucceeded(List<BackendManager.LeaderboardEntry> entries)
	{
		List<LeaderboardScore> list = new List<LeaderboardScore>();
		if (entries != null)
		{
			foreach (BackendManager.LeaderboardEntry entry in entries)
			{
				BackendManager.LeaderboardEntry current = entry;
				list.Add(new LeaderboardScore(current.PlayerAlias, current.PlayerId, current.Score, SocialPlatform.facebook));
			}
		}
		if (GameFacebookManager.OnScoresLoaded != null)
		{
			GameFacebookManager.OnScoresLoaded(list);
		}
	}

	public void OnLeaderboardGetFailed(string error)
	{
		if (GameFacebookManager.OnRetrieveScoresFailed != null)
		{
			GameFacebookManager.OnRetrieveScoresFailed();
		}
	}

	private static List<object> FacebookResultToList(object result)
	{
		Hashtable data = new Hashtable(result as Dictionary<string, object>);
		return JsonUtil.ExtractList(data, "data");
	}

	public void PublishToStream(string title, string description)
	{
		Action onLoginWithPermissionSucceeded = delegate
		{
			GraphRequestPublishToStream(title, description);
		};
		Login(onLoginWithPermissionSucceeded, isPublishNeeded: true);
	}

	public void PublishImageToStream(byte[] image)
	{
		Action onLoginWithPermissionSucceeded = delegate
		{
			GraphRequestPublishToStream(image);
		};
		Login(onLoginWithPermissionSucceeded, isPublishNeeded: true);
	}

	public void InviteFriends(Action<int> onInviteCompleted)
	{
		Dictionary<string, string> parameters = new Dictionary<string, string>
		{
			{
				"title",
				"INVITE FRIENDS"
			},
			{
				"message",
				FacebookSetting.InviteMessage
			},
			{
				"filters",
				"app_non_users"
			}
		};
		Login(delegate
		{
			AutoSingleton<BackendFacebook>.Instance.Authenticate();
			ShowInviteFriendsDialog(onInviteCompleted, null, parameters);
		}, isPublishNeeded: false);
	}

	public void ChallengeFriends(Action<int> onInviteCompleted, string ids)
	{
		Dictionary<string, string> parameters = new Dictionary<string, string>
		{
			{
				"title",
				"INVITE FRIENDS"
			},
			{
				"message",
				FacebookSetting.InviteMessage
			}
		};
		Login(delegate
		{
			AutoSingleton<BackendFacebook>.Instance.Authenticate();
			ShowInviteFriendsDialog(onInviteCompleted, ids, parameters);
		}, isPublishNeeded: false);
	}

	private void ShowInviteFriendsDialog(Action<int> onInviteCompleted, string ids, Dictionary<string, string> parameters = null)
	{
		if (ids != null && ids.Length > 0)
		{
			parameters["to"] = ids;
		}
		ResetDialogCallback();
		_onInviteFriendsDialogCompleted = onInviteCompleted;
		FacebookAndroid.showDialog("apprequests", parameters);
	}

	private void ResetDialogCallback()
	{
		_onInviteFriendsDialogCompleted = null;
	}

	public void OnDialogCompletedWithUrl(string url)
	{
		if (_onInviteFriendsDialogCompleted != null)
		{
			string value = "to";
			if (url.Contains("request=") && url.Contains(value))
			{
				int num = 0;
				for (int num2 = url.IndexOf(value); num2 > -1; num2 = url.IndexOf(value, num2 + 1))
				{
					num++;
				}
				_onInviteFriendsDialogCompleted(num);
			}
		}
		ResetDialogCallback();
	}

	public void OnDialogFailed(P31Error error)
	{
		SilentDebug.LogWarning(("Facebook OnDialogFailed: " + error == null) ? string.Empty : (error.message + " Prime " + error.code));
		ResetDialogCallback();
	}

	public bool IsPublishPermissionGranted()
	{
		return _publishPermissionGranted.IsSet() && _publishPermissionGranted.Get();
	}

	public void GraphRequestPublishToStream(string title, string description)
	{
		if (IsLoggedIn() && IsPublishPermissionGranted())
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("link", FacebookSetting.LinkURL);
			dictionary.Add("name", title);
			dictionary.Add("picture", FacebookSetting.Picture);
			dictionary.Add("caption", FacebookSetting.ApplicationName);
			dictionary.Add("description", description);
			Dictionary<string, string> parameters = dictionary;
			ResetDialogCallback();
			FacebookAndroid.showDialog("feed", parameters);
		}
	}

	public void GraphRequestPublishToStream(byte[] image)
	{
		if (IsLoggedIn() && IsPublishPermissionGranted())
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("message", FacebookSetting.ApplicationName);
			dictionary.Add("picture", image);
			Dictionary<string, object> parameters = dictionary;
			Facebook.instance.graphRequest("me/photos", HTTPVerb.POST, parameters, OnImagePosted);
		}
	}

	public void OnImagePosted(string error, object result)
	{
		if (!string.IsNullOrEmpty(error))
		{
			SilentDebug.LogError("Error while posintg facebook picture: " + error);
		}
	}

	private IEnumerator LoadJpgCR(string filename, string url)
	{
		WWW req = new WWW(url);
		yield return req;
		File.WriteAllBytes(filename, req.bytes);
	}

	public void RetrieveLocalUserInfo()
	{
		if (!IsLoggedIn())
		{
			return;
		}
		if (_playerId != string.Empty && _playerAlias != string.Empty)
		{
			if (GameFacebookManager.OnUserInfoLoaded != null)
			{
				GameFacebookManager.OnUserInfoLoaded();
			}
		}
		else
		{
			string str = "me?fields=id,name,picture";
			Facebook.instance.graphRequest(string.Empty + str, HTTPVerb.GET, null, OnUserInfoRetrieved);
		}
	}

	public void OnUserInfoRetrieved(string error, object result)
	{
		if (!string.IsNullOrEmpty(error))
		{
			SilentDebug.LogError(error);
			if (GameFacebookManager.OnRetrieveUserInfoFailed != null)
			{
				GameFacebookManager.OnRetrieveUserInfoFailed(error);
			}
			return;
		}
		Hashtable hashtable = new Hashtable(result as Dictionary<string, object>);
		if (hashtable != null)
		{
			_playerAlias = StringUtil.Cleanup(JsonUtil.ExtractString(hashtable, "name", string.Empty));
			_playerId = JsonUtil.ExtractString(hashtable, "id", string.Empty);
			Hashtable data = new Hashtable(JsonUtil.ExtractDictionary(hashtable, "picture", new Dictionary<string, object>()));
			Hashtable data2 = new Hashtable(JsonUtil.ExtractDictionary(data, "data", new Dictionary<string, object>()));
			string url = JsonUtil.ExtractString(data2, "url", string.Empty);
			if (IsValidPictureUrl(_playerId, url))
			{
				PictureManager.StorePicture(_playerId, url);
			}
		}
		if (GameFacebookManager.OnUserInfoLoaded != null)
		{
			GameFacebookManager.OnUserInfoLoaded();
		}
	}

	public void Logout()
	{
		FacebookAndroid.logout();
		Facebook.instance.accessToken = FacebookAndroid.getAccessToken();
		ResetData();
		if (GameFacebookManager.OnLogout != null)
		{
			GameFacebookManager.OnLogout();
		}
	}

	public void RetrieveFriends()
	{
		if (IsLoggedIn())
		{
			string str = "me/friends?fields=id,name,first_name,picture&limit=" + 25;
			Facebook.instance.graphRequest(string.Empty + str, HTTPVerb.GET, null, OnFriendsReceived);
		}
	}

	public void OnFriendsReceived(string error, object result)
	{
		if (!string.IsNullOrEmpty(error))
		{
			SilentDebug.LogError("Error while loading facebook friends: " + error);
			if (GameFacebookManager.OnRetrieveFriendsFailed != null)
			{
				GameFacebookManager.OnRetrieveFriendsFailed(error);
			}
			return;
		}
		List<User> list = new List<User>();
		List<object> list2 = FacebookResultToList(result);
		if (list2 != null)
		{
			foreach (object item in list2)
			{
				Hashtable hashtable = new Hashtable(item as Dictionary<string, object>);
				if (hashtable != null)
				{
					list.Add(User.FromFacebookJson(hashtable));
				}
			}
		}
		if (GameFacebookManager.OnFriendsLoaded != null)
		{
			GameFacebookManager.OnFriendsLoaded(list);
		}
	}

	private bool IsValidPictureUrl(string userId, string url)
	{
		return !string.IsNullOrEmpty(url) && url.Contains(".jpg");
	}

	private void ResetData()
	{
		_facebookLike.Reset();
		_publishPermissionGranted.Reset();
		_playerAlias = string.Empty;
		_playerId = string.Empty;
	}
}
