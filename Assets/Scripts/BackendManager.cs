using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackendManager : AutoSingleton<BackendManager>
{
	private struct LoginRequestMessage
	{
		public string PlayerID;

		public string PlayerAlias;

		public string DeviceID;

		public bool GetSave;

		public string DeviceToken;

		public string Version;

		public int Coins;

		public int Bucks;

		public int PrestigeTokens;

		public int PrestigePoints;

		public int Level;

		public List<string> Cars;

		public List<int> Powerups;

		public ArrayList StorePerks;

		public string Platform;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PlayerID"] = PlayerID;
			hashtable["PlayerAlias"] = PlayerAlias;
			hashtable["DeviceID"] = DeviceID;
			hashtable["GetSave"] = GetSave;
			hashtable["DeviceToken"] = DeviceToken;
			hashtable["Version"] = Version;
			hashtable["Coins"] = Coins;
			hashtable["Bucks"] = Bucks;
			hashtable["PrestigeTokens"] = PrestigeTokens;
			hashtable["PrestigePoints"] = PrestigePoints;
			hashtable["Level"] = Level;
			hashtable["Cars"] = new ArrayList(Cars);
			hashtable["Powerups"] = new ArrayList(Powerups);
			hashtable["StorePerks"] = StorePerks;
			hashtable["Platform"] = Platform;
			return hashtable;
		}
	}

	private struct LoginReplyMessage
	{
		public string Status;

		public BackendSaveGame.GameSaveInfo Save;

		public List<string> Messages;

		public LoginReplyMessage(string json)
		{
			Hashtable data = json.hashtableFromJson();
			Status = JsonUtil.ExtractString(data, "Status", "invalid");
			Hashtable ht = JsonUtil.ExtractHashtable(data, "Save", new Hashtable());
			Save = BackendSaveGame.GameSaveInfo.FromJsonData(ht);
			Messages = new List<string>();
			ArrayList arrayList = JsonUtil.ExtractArrayList(data, "Messages");
			if (arrayList != null)
			{
				foreach (string item in arrayList)
				{
					Messages.Add(item);
				}
			}
		}
	}

	private struct LogPurchaseRequestMessage
	{
		public string PlayerID;

		public string DeviceID;

		public string Identifier;

		public string Receipt;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PlayerID"] = PlayerID;
			hashtable["DeviceID"] = DeviceID;
			hashtable["Identifier"] = Identifier;
			hashtable["Receipt"] = Receipt;
			return hashtable;
		}
	}

	private struct ReplayRequestMessage
	{
		public List<string> Friends;

		public int Seed;

		public int Limit;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Friends"] = new ArrayList(Friends);
			hashtable["Seed"] = Seed;
			hashtable["Limit"] = Limit;
			return hashtable;
		}
	}

	private struct ReplayResponseMessage
	{
		public List<Replay> Replays;

		public ReplayResponseMessage(string json)
		{
			Replays = new List<Replay>();
			Hashtable data = json.hashtableFromJson();
			ArrayList arrayList = JsonUtil.ExtractArrayList(data, "Replays");
			if (arrayList != null)
			{
				foreach (Hashtable item in arrayList)
				{
					Replay replay = Replay.FromJsonData(item);
					if (replay != null)
					{
						Replays.Add(replay);
					}
				}
			}
		}
	}

	private struct ShowroomSaveRequestMessage
	{
		public string PlayerId;

		public string Json;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PlayerId"] = PlayerId;
			hashtable["Json"] = Json;
			return hashtable;
		}
	}

	private struct SpecialOfferResolutionRequestMessage
	{
		public string PlayerId;

		public string PlayerAlias;

		public string DeviceId;

		public string DeviceType;

		public string Version;

		public string OfferId;

		public string ResolutionType;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PlayerId"] = PlayerId;
			hashtable["PlayerAlias"] = PlayerAlias;
			hashtable["DeviceId"] = DeviceId;
			hashtable["DeviceType"] = DeviceType;
			hashtable["Version"] = Version;
			hashtable["OfferId"] = OfferId;
			hashtable["ResolutionType"] = ResolutionType;
			return hashtable;
		}
	}

	public struct RegisterDeviceRequestMessage
	{
		public string DeviceId;

		public string DeviceToken;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["DeviceId"] = DeviceId;
			hashtable["DeviceToken"] = DeviceToken;
			return hashtable;
		}
	}

	public struct LeaderboardEntry
	{
		public string Date;

		public string PlayerId;

		public string PlayerAlias;

		public int Score;

		public static LeaderboardEntry FromJsonData(Hashtable data)
		{
			LeaderboardEntry result = default(LeaderboardEntry);
			result.Date = JsonUtil.ExtractString(data, "Date", "*Date*");
			result.PlayerId = JsonUtil.ExtractString(data, "PlayerId", "*PlayerId*");
			result.PlayerAlias = JsonUtil.ExtractString(data, "PlayerAlias", "*PlayerAlias*");
			result.Score = JsonUtil.ExtractInt(data, "Score", 0);
			return result;
		}
	}

	public struct LeaderboardPostRequestMessage
	{
		public string LeaderboardId;

		public string PlayerId;

		public string PlayerAlias;

		public int Score;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["LeaderboardId"] = LeaderboardId;
			hashtable["PlayerID"] = PlayerId;
			hashtable["PlayerAlias"] = PlayerAlias;
			hashtable["Score"] = Score;
			return hashtable;
		}
	}

	public struct LeaderboardGetRequestMessage
	{
		public string LeaderboardId;

		public List<string> PlayersId;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["LeaderboardId"] = LeaderboardId;
			hashtable["PlayersId"] = new ArrayList(PlayersId);
			return hashtable;
		}
	}

	public struct LeaderboardGetResponseMessage
	{
		public string LeaderboardId;

		public List<LeaderboardEntry> Entries;

		public static LeaderboardGetResponseMessage FromJsonData(Hashtable data)
		{
			LeaderboardGetResponseMessage result = default(LeaderboardGetResponseMessage);
			result.LeaderboardId = JsonUtil.ExtractString(data, "LeaderboardId", "*LeaderboardId");
			result.Entries = new List<LeaderboardEntry>();
			foreach (object item in JsonUtil.ExtractArrayList(data, "Entries", new ArrayList()))
			{
				Hashtable data2 = item as Hashtable;
				result.Entries.Add(LeaderboardEntry.FromJsonData(data2));
			}
			return result;
		}
	}

	private Dictionary<SocialPlatform, IBackend> _backendList;

	private bool _loginRequested;

	private bool _loggedIn;

	public SocialPlatform MainSocialPlatform
	{
		get;
		private set;
	}

	[method: MethodImpl(32)]
	public event Action OnRetrieveFriendsCallback;

	[method: MethodImpl(32)]
	public event Action OnRetrieveFriendsFailedCallback;

	[method: MethodImpl(32)]
	public event Action<string> OnShowroomLoadingSucceeded;

	[method: MethodImpl(32)]
	public event Action<string> OnShowroomLoadingFailed;

	[method: MethodImpl(32)]
	public event Action<string> OnGetSpecialOffersSucceeded;

	[method: MethodImpl(32)]
	public event Action<string> OnGetSpecialOffersFailed;

	[method: MethodImpl(32)]
	public event Action<List<LeaderboardEntry>> OnLeaderboardGetSucceeded;

	[method: MethodImpl(32)]
	public event Action<string> OnLeaderboardGetFailed;

	private string Host()
	{
		return "https://ert2backroad.appspot.com";
	}

	public string PlayerAlias()
	{
		return PlayerAlias(MainSocialPlatform);
	}

	public string PlayerAlias(SocialPlatform platform)
	{
		return _backendList[platform].PlayerAlias();
	}

	public string PlayerIdentifier()
	{
		return PlayerIdentifier(MainSocialPlatform);
	}

	public string PlayerIdentifier(SocialPlatform platform)
	{
		return _backendList[platform].PlayerIdentifier();
	}

	public List<User> Friends()
	{
		return Friends(MainSocialPlatform);
	}

	public int GetAllFriendCount()
	{
		int num = 0;
		foreach (IBackend value in _backendList.Values)
		{
			num += value.GetFriendCount();
		}
		return num;
	}

	public List<User> Friends(SocialPlatform platform)
	{
		if (_backendList.ContainsKey(platform))
		{
			return _backendList[platform].Friends();
		}
		return null;
	}

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_backendList = new Dictionary<SocialPlatform, IBackend>();
		MainSocialPlatform = SocialPlatform.facebook;
		_backendList[SocialPlatform.facebook] = AutoSingleton<BackendFacebook>.Instance;
		Authenticate();
		base.OnAwake();
	}

	public void Authenticate()
	{
		foreach (SocialPlatform key in _backendList.Keys)
		{
			Authenticate(key);
		}
	}

	public void Authenticate(SocialPlatform platform)
	{
		if (_backendList.ContainsKey(platform))
		{
			_backendList[platform].Authenticate();
		}
		else
		{
			UnityEngine.Debug.LogError("Social Platform " + platform.ToString() + " isn't supported on this build.");
		}
	}

	public void OnPlayerAuthenticated()
	{
	}

	public bool IsLoggedIn()
	{
		return _backendList[MainSocialPlatform].IsLoggedIn();
	}

	public bool IsLoggedUser(string userId)
	{
		foreach (SocialPlatform key in _backendList.Keys)
		{
			if (PlayerIdentifier(key) == userId)
			{
				return true;
			}
		}
		return false;
	}

	public bool LoggedIn()
	{
		return _loggedIn;
	}

	public void LoginToBackend()
	{
		if (!_loginRequested)
		{
			BackendSessionApiClient.StartSession(null, null);
			_loginRequested = true;
			PlayerProfile playerProfile = AutoSingleton<PlayerDatabase>.Instance.GetPlayerProfile();
			LoginRequestMessage loginRequestMessage = default(LoginRequestMessage);
			loginRequestMessage.PlayerID = PlayerIdentifier();
			loginRequestMessage.PlayerAlias = PlayerAlias();
			loginRequestMessage.DeviceID = GetUniqueDeviceID();
			loginRequestMessage.DeviceToken = AutoSingleton<NotificationManager>.Instance.GetDeviceToken();
			loginRequestMessage.Version = GameVersion.VERSION;
			loginRequestMessage.Level = playerProfile.XPProfile.Level;
			loginRequestMessage.Coins = playerProfile.Coins;
			loginRequestMessage.Bucks = playerProfile.Bucks;
			loginRequestMessage.PrestigeTokens = playerProfile.PrestigeTokens;
			loginRequestMessage.PrestigePoints = playerProfile.PrestigePoints;
			loginRequestMessage.Cars = AutoSingleton<CarManager>.Instance.GetAllUnlockedCarsName();
			loginRequestMessage.Powerups = playerProfile.GetPowerupsInventory();
			loginRequestMessage.StorePerks = AutoSingleton<StorePerkManager>.Instance.GetStringArrayList();
			loginRequestMessage.Platform = Device.GetDeviceType();
			LoginRequestMessage loginRequestMessage2 = loginRequestMessage;
			Post("/login", loginRequestMessage2.ToJsonData(), OnLoginToBackend, OnLoginToBackendFailed);
		}
	}

	private string GetUniqueDeviceID()
	{
		return Device.GetDeviceId();
	}

	private void OnLoginToBackend(string response)
	{
		_loggedIn = true;
		LoginReplyMessage loginReplyMessage = new LoginReplyMessage(response);
		List<string> messages = loginReplyMessage.Messages;
		foreach (string item in messages)
		{
			OnMessageReceived(item.ToString());
		}
		_loginRequested = false;
	}

	private void OnLoginToBackendFailed(string error)
	{
		UnityEngine.Debug.LogWarning("OnLoginToBackendFailed: error = " + error);
		_loginRequested = false;
	}

	private void OnMessageReceived(string message)
	{
		if (message != null)
		{
			Hashtable data = message.hashtableFromJson();
			string text = JsonUtil.ExtractString(data, "MessageType", null);
			switch (text)
			{
			case "Alert":
			{
				string title = JsonUtil.ExtractString(data, "Title", "Message Received");
				string msg = JsonUtil.ExtractString(data, "Message", string.Empty);
				NativeAlert.Show(title, msg, "OK");
				break;
			}
			case "Reward":
				AutoSingleton<MissionRewardsManager>.Instance.OnServerRewardReceived(data);
				break;
			default:
				UnityEngine.Debug.LogWarning("unknown message received, type: " + text);
				break;
			}
		}
	}

	public void LogPurchase(string identifier, string receipt)
	{
		LogPurchaseRequestMessage logPurchaseRequestMessage = default(LogPurchaseRequestMessage);
		logPurchaseRequestMessage.PlayerID = PlayerIdentifier();
		logPurchaseRequestMessage.DeviceID = Device.GetDeviceId();
		logPurchaseRequestMessage.Identifier = identifier;
		logPurchaseRequestMessage.Receipt = receipt;
		LogPurchaseRequestMessage logPurchaseRequestMessage2 = logPurchaseRequestMessage;
		Post("/purchase", logPurchaseRequestMessage2.ToJsonData());
	}

	public void RetrieveFriends()
	{
		_backendList[MainSocialPlatform].RetrieveFriends();
	}

	public void OnRetrieveFriends(List<User> users)
	{
		foreach (User user in users)
		{
			user._alias = StringUtil.Cleanup(user._alias);
			user._displayName = StringUtil.Cleanup(user._displayName);
		}
		if (this.OnRetrieveFriendsCallback != null)
		{
			this.OnRetrieveFriendsCallback();
		}
	}

	public void OnRetrieveFriendsFailed(string error)
	{
		UnityEngine.Debug.LogWarning("=== Retrieve friends failed: " + error);
		if (this.OnRetrieveFriendsFailedCallback != null)
		{
			this.OnRetrieveFriendsFailedCallback();
		}
	}

	public void SaveReplay(Replay replay)
	{
		Post("/record", replay.ToJsonData(), delegate(string text)
		{
			StartCoroutine(UploadReplay(text, replay));
		});
	}

	private IEnumerator UploadReplay(string text, Replay replay)
	{
		yield return null;
		Hashtable uploadInfo = text.hashtableFromJson();
		string uploadURL = JsonUtil.ExtractString(uploadInfo, "URL", string.Empty);
		string uploadKey = JsonUtil.ExtractString(uploadInfo, "Key", string.Empty);
		WWWForm blobForm = new WWWForm();
		blobForm.AddField("key", uploadKey);
		blobForm.AddBinaryData("file", replay.GetByteArray());
		WWW blobRequest = new WWW(uploadURL, blobForm);
		yield return null;
		while (!blobRequest.isDone)
		{
			yield return new WaitForSeconds(1f);
		}
		if (!string.IsNullOrEmpty(blobRequest.error))
		{
			UnityEngine.Debug.LogWarning("=== SaveReplayToNetworkCR blob upload error: " + blobRequest.error);
		}
	}

	public void LoadReplays(Action<Replay> OnReplayLoaded)
	{
		StartCoroutine(LoadReplaysFromNetworkCR(OnReplayLoaded));
	}

	private IEnumerator LoadReplaysFromNetworkCR(Action<Replay> OnReplayLoaded)
	{
		yield return null;
		Post("/replay", new ReplayRequestMessage
		{
			Friends = AutoSingleton<LeaderboardsManager>.Instance.FriendsActiveToday(),
			Seed = SeedUtil.GetSeed(),
			Limit = 10
		}.ToJsonData(), delegate(string text)
		{
			ReplayResponseMessage replayResponseMessage = new ReplayResponseMessage(text);
			foreach (Replay replay in replayResponseMessage.Replays)
			{
				//base.OnReplayLoaded(replay);
			}
		});
	}

	public void LoadReplayDataBlob(Replay replay)
	{
		if (replay != null)
		{
			string url = "/download?blobKey=" + replay.BlobKey();
			Get(url, replay.LoadData);
		}
	}

	public void SaveShowroom(string json)
	{
		string text = PlayerIdentifier();
		ShowroomSaveRequestMessage showroomSaveRequestMessage = default(ShowroomSaveRequestMessage);
		showroomSaveRequestMessage.PlayerId = text;
		showroomSaveRequestMessage.Json = json;
		ShowroomSaveRequestMessage showroomSaveRequestMessage2 = showroomSaveRequestMessage;
		if (text != null)
		{
			Post("/saveshowroom", showroomSaveRequestMessage2.ToJsonData());
		}
	}

	public void LoadShowroom(string playerId)
	{
		if (playerId != null)
		{
			string url = "/loadshowroom?playerId=" + playerId;
			Action<string> onSuccess = delegate(string response)
			{
				if (this.OnShowroomLoadingSucceeded != null)
				{
					this.OnShowroomLoadingSucceeded(response);
				}
			};
			Action<string> onFailed = delegate(string error)
			{
				if (this.OnShowroomLoadingFailed != null)
				{
					this.OnShowroomLoadingFailed(error);
				}
			};
			Get(url, onSuccess, onFailed);
		}
	}

	public void GetSpecialOffers()
	{
		string url = "/specialoffer/get";
		Action<string> onSuccess = delegate(string response)
		{
			if (this.OnGetSpecialOffersSucceeded != null)
			{
				this.OnGetSpecialOffersSucceeded(response);
			}
		};
		Action<string> onFailed = delegate(string error)
		{
			if (this.OnGetSpecialOffersFailed != null)
			{
				this.OnGetSpecialOffersFailed(error);
			}
		};
		Get(url, onSuccess, onFailed);
	}

	public void ReportSpecialOfferResolution(string offerId, SpecialOfferResolutionType resolutionType)
	{
		string url = "/specialoffer/resolve";
		SpecialOfferResolutionRequestMessage specialOfferResolutionRequestMessage = default(SpecialOfferResolutionRequestMessage);
		specialOfferResolutionRequestMessage.PlayerId = PlayerIdentifier();
		specialOfferResolutionRequestMessage.PlayerAlias = PlayerAlias();
		specialOfferResolutionRequestMessage.DeviceId = Device.GetDeviceId();
		specialOfferResolutionRequestMessage.DeviceType = Device.GetDeviceType();
		specialOfferResolutionRequestMessage.Version = GameVersion.VERSION;
		specialOfferResolutionRequestMessage.OfferId = offerId;
		specialOfferResolutionRequestMessage.ResolutionType = resolutionType.ToString();
		SpecialOfferResolutionRequestMessage specialOfferResolutionRequestMessage2 = specialOfferResolutionRequestMessage;
		Post(url, specialOfferResolutionRequestMessage2.ToJsonData());
	}

	public void RegisterDeviceToken(string deviceToken)
	{
		RegisterDeviceRequestMessage registerDeviceRequestMessage = default(RegisterDeviceRequestMessage);
		registerDeviceRequestMessage.DeviceId = Device.GetDeviceId();
		registerDeviceRequestMessage.DeviceToken = deviceToken;
		RegisterDeviceRequestMessage registerDeviceRequestMessage2 = registerDeviceRequestMessage;
		Post("/notification/register", registerDeviceRequestMessage2.ToJsonData());
	}

	public void PostToLeaderboard(string leaderboardId, int score)
	{
		SocialPlatform platform = MainSocialPlatform;
		if (_backendList.ContainsKey(SocialPlatform.facebook))
		{
			platform = SocialPlatform.facebook;
		}
		string playerAlias = PlayerAlias(platform);
		string playerId = PlayerIdentifier(platform);
		PostToLeaderboard(leaderboardId, score, playerAlias, playerId);
	}

	public void PostToLeaderboard(string leaderboardId, int score, string playerAlias, string playerId)
	{
		string url = "/leaderboard/post";
		LeaderboardPostRequestMessage leaderboardPostRequestMessage = default(LeaderboardPostRequestMessage);
		leaderboardPostRequestMessage.LeaderboardId = leaderboardId;
		leaderboardPostRequestMessage.PlayerId = playerId;
		leaderboardPostRequestMessage.PlayerAlias = playerAlias;
		leaderboardPostRequestMessage.Score = score;
		LeaderboardPostRequestMessage leaderboardPostRequestMessage2 = leaderboardPostRequestMessage;
		Post(url, leaderboardPostRequestMessage2.ToJsonData());
	}

	public void GetFriendsLeaderboardEntries(string leaderboardId)
	{
		SocialPlatform platform = MainSocialPlatform;
		if (_backendList.ContainsKey(SocialPlatform.facebook))
		{
			platform = SocialPlatform.facebook;
		}
		List<User> list = Friends(platform);
		if (list != null)
		{
			List<string> usersId = new List<string>();
			list.ForEach(delegate(User u)
			{
				usersId.Add(u._id);
			});
			usersId.Add(PlayerIdentifier(platform));
			GetLeaderboardEntries(leaderboardId, usersId);
		}
		else if (this.OnLeaderboardGetFailed != null)
		{
			this.OnLeaderboardGetFailed("GetLeaderboardEntries failed. Load the friends list first");
		}
	}

	public void GetLeaderboardEntries(string leaderboardId, List<string> playersId)
	{
		string url = "/leaderboard/get";
		Action<string> onSuccess = delegate(string response)
		{
			Hashtable data = response.hashtableFromJson();
			LeaderboardGetResponseMessage leaderboardGetResponseMessage = LeaderboardGetResponseMessage.FromJsonData(data);
			if (this.OnLeaderboardGetSucceeded != null)
			{
				this.OnLeaderboardGetSucceeded(leaderboardGetResponseMessage.Entries);
			}
		};
		Action<string> onFailed = delegate(string error)
		{
			if (this.OnLeaderboardGetFailed != null)
			{
				this.OnLeaderboardGetFailed(error);
			}
		};
		LeaderboardGetRequestMessage leaderboardGetRequestMessage = default(LeaderboardGetRequestMessage);
		leaderboardGetRequestMessage.LeaderboardId = leaderboardId;
		leaderboardGetRequestMessage.PlayersId = playersId;
		LeaderboardGetRequestMessage leaderboardGetRequestMessage2 = leaderboardGetRequestMessage;
		Post(url, leaderboardGetRequestMessage2.ToJsonData(), onSuccess, onFailed);
	}

	private void Get(string url, Action<string> onSuccess = null, Action<string> onFailed = null)
	{
		StartCoroutine(GetCR(url, onSuccess, onFailed));
	}

	private IEnumerator GetCR(string url, Action<string> onSuccess, Action<string> onFailed)
	{
		WWW request = new WWW(Host() + url);
		yield return request;
		if (!string.IsNullOrEmpty(request.error))
		{
			if (onFailed != null)
			{
				onFailed(request.error);
			}
			else
			{
				UnityEngine.Debug.LogWarning("GET " + url + " request failed: " + request.error);
			}
		}
		else
		{
			onSuccess?.Invoke(request.text);
		}
	}

	private void Get(string url, Action<byte[]> onSuccess = null, Action<string> onFailed = null)
	{
		StartCoroutine(GetCR(url, onSuccess, onFailed));
	}

	private IEnumerator GetCR(string url, Action<byte[]> onSuccess, Action<string> onFailed)
	{
		WWW request = new WWW(Host() + url);
		yield return request;
		if (!string.IsNullOrEmpty(request.error))
		{
			if (onFailed != null)
			{
				onFailed(request.error);
			}
			else
			{
				UnityEngine.Debug.LogWarning("GET " + url + " request failed: " + request.error);
			}
		}
		else
		{
			onSuccess?.Invoke(request.bytes);
		}
	}

	public void Post(string url, Hashtable jsonData, Action<string> onSuccess = null, Action<string> onFailed = null)
	{
		StartCoroutine(PostCR(url, jsonData, onSuccess, onFailed));
	}

	private IEnumerator PostCR(string url, Hashtable jsonData, Action<string> onSuccess, Action<string> onFailed)
	{
		WWWForm form = new WWWForm();
		jsonData["Timestamp"] = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
		jsonData["Salt"] = RandomUtil.RandomString(6);
		string json = jsonData.toJson();
		form.AddField("data", json);
		form.AddField("hash", Security.ComputeHash(json));
		WWW request = new WWW(Host() + url, form);
		yield return request;
		if (!string.IsNullOrEmpty(request.error))
		{
			if (onFailed != null)
			{
				onFailed(request.error);
			}
			else
			{
				UnityEngine.Debug.LogWarning("POST " + url + " request failed: " + request.error);
			}
		}
		else if (request.text.Length > 28)
		{
			string hash = request.text.Substring(0, 28);
			string text = request.text.Substring(28);
			if (hash != Security.ComputeHash(text))
			{
				UnityEngine.Debug.LogWarning("POST " + url + " has an invalid hash");
				onFailed?.Invoke("Invalid or missing hash");
			}
			else
			{
				onSuccess?.Invoke(text);
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("POST " + url + " has an invalid or missing hash");
			onFailed?.Invoke("Invalid or missing hash");
		}
	}

	public string GetGameCenterID()
	{
		return string.Empty;
	}

	public string GetFacebookID()
	{
		if (AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
		{
			return AutoSingleton<GameFacebookManager>.Instance.PlayerId();
		}
		return string.Empty;
	}
}
