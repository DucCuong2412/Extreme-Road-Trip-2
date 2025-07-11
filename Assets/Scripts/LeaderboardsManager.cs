using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeaderboardsManager : AutoSingleton<LeaderboardsManager>
{
	private static Dictionary<LeaderboardType, string> _boardId;

	private Dictionary<LeaderboardType, DateTime> _leaderboardsTimestamp;

	private Dictionary<SocialPlatform, Dictionary<LeaderboardType, List<LeaderboardScore>>> _scoresCache;

	private bool _scoresRetrieved;

	private LeaderboardType _currentLeaderboard;

	private bool _cachingLeaderboard;

	[method: MethodImpl(32)]
	public event Action<SocialPlatform, LeaderboardType, List<LeaderboardScore>> OnScoresRetrievedCallback;

	[method: MethodImpl(32)]
	public event Action<SocialPlatform, LeaderboardType> OnScoresRetrievedFailedCallback;

	static LeaderboardsManager()
	{
		_boardId = new Dictionary<LeaderboardType, string>();
		_boardId[LeaderboardType.longestRoadTrip] = "grp.ca.roofdog.roadtrip2.longestRoadTrip";
		_boardId[LeaderboardType.bestOneMinuteSprint] = "grp.ca.roofdog.roadtrip2.bestOneMinuteSprint";
		_boardId[LeaderboardType.best2kTime] = "grp.ca.roofdog.roadtrip2.best2kTime";
		_boardId[LeaderboardType.best5kTime] = "grp.ca.roofdog.roadtrip2.best5kTime";
		_boardId[LeaderboardType.best10kTime] = "grp.ca.roofdog.roadtrip2.best10kTime";
		_boardId[LeaderboardType.mostCoinsPickedUp] = "grp.ca.roofdog.roadtrip2.mostCoinsPickedUp";
		_boardId[LeaderboardType.mostCoinsPickedUpSingleRun] = "grp.ca.roofdog.roadtrip2.mostCoinsPickedUpSingleRun";
		_boardId[LeaderboardType.bestStuntJump] = "grp.ca.roofdog.roadtrip2.bestStuntJump";
		_boardId[LeaderboardType.highestLevel] = "grp.ca.roofdog.roadtrip2.highestLevel";
		_boardId[LeaderboardType.longestJump] = "grp.ca.roofdog.roadtrip2.longestJump";
		_boardId[LeaderboardType.showroomValue] = "grp.ca.roofdog.roadtrip2.showroomValue";
		_boardId[LeaderboardType.mostBucksFrenzyMode] = "grp.ca.roofdog.roadtrip2.mostBucksFrenzyMode";
		_boardId[LeaderboardType.highestPrestigePoints] = "grp.ca.roofdog.roadtrip2.highestPrestigePoints";
	}

	public string GetLeaderboardId(LeaderboardType leaderboard)
	{
		return leaderboard.ToString();
	}

	protected override void OnDestroy()
	{
		GameFacebookManager.OnLogout -= OnFacebookLogout;
		base.OnDestroy();
	}

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_leaderboardsTimestamp = new Dictionary<LeaderboardType, DateTime>();
		GameFacebookManager.OnLogout += OnFacebookLogout;
		base.OnAwake();
	}

	public void SubmitScore(LeaderboardType type, int score)
	{
	}

	public void SubmitTime(LeaderboardType type, float seconds)
	{
	}

	private void SubmitFBScore(LeaderboardType type, int score)
	{
		if ((type == LeaderboardType.longestRoadTrip || type == LeaderboardType.mostBucksFrenzyMode) && score > 0 && UpdateFBLocalScore(type, score))
		{
			if (type == LeaderboardType.longestRoadTrip)
			{
				AutoSingleton<GameFacebookManager>.Instance.PostScore(score);
			}
			AutoSingleton<BackendManager>.Instance.PostToLeaderboard(type.ToString(), score);
		}
	}

	public void ShowLeaderboard()
	{
	}

	public void OnGameEnded(Car currentCar, Maybe<int> bestOneMinuteSprint, GameMode gameMode)
	{
		GameStats overall = AutoSingleton<GameStatsManager>.Instance.Overall;
		GameStats currentRun = AutoSingleton<GameStatsManager>.Instance.CurrentRun;
		int total = overall.GetTotal(GameStats.CarStats.Type.coinsPickedUp);
		int value = currentRun.GetValue(currentCar, GameStats.CarStats.Type.coinsPickedUp);
		int max = overall.GetMax(GameStats.CarStats.Type.maxDistance);
		int value2 = currentRun.GetValue(currentCar, GameStats.CarStats.Type.bucksPickedUp);
		SubmitScore(LeaderboardType.longestRoadTrip, max);
		SubmitScore(LeaderboardType.mostCoinsPickedUp, total);
		SubmitScore(LeaderboardType.mostCoinsPickedUpSingleRun, value);
		if (gameMode == GameMode.frenzy && value2 > 0)
		{
			SubmitScore(LeaderboardType.mostBucksFrenzyMode, value2);
			SubmitFBScore(LeaderboardType.mostBucksFrenzyMode, value2);
		}
		int max2 = overall.GetMax(GameStats.CarStats.Type.mostStuntOnJump);
		SubmitScore(LeaderboardType.bestStuntJump, max2);
		int max3 = overall.GetMax(GameStats.CarStats.Type.maxJumpLength);
		SubmitScore(LeaderboardType.longestJump, max3);
		float min = overall.GetMin(GameStats.CarStats.Type.best2kTime);
		if (min != 0f)
		{
			SubmitBest2kTime(min);
		}
		min = overall.GetMin(GameStats.CarStats.Type.best5kTime);
		if (min != 0f)
		{
			SubmitBest5kTime(min);
		}
		min = overall.GetMin(GameStats.CarStats.Type.best10kTime);
		if (min != 0f)
		{
			SubmitBest10kTime(min);
		}
		if (bestOneMinuteSprint.IsSet())
		{
			float timer = overall.GetMax(GameStats.CarStats.Type.longestRaceInTime);
			SubmitBestOneMinuteSprint(timer, bestOneMinuteSprint.Get());
		}
		int maxDistance = currentRun.GetMaxDistance(currentCar);
		SubmitFBScore(LeaderboardType.longestRoadTrip, maxDistance);
	}

	public void SubmitBest2kTime(float best2kTimer)
	{
		SubmitTime(LeaderboardType.best2kTime, best2kTimer);
	}

	public void SubmitBest5kTime(float best5kTimer)
	{
		SubmitTime(LeaderboardType.best5kTime, best5kTimer);
	}

	public void SubmitBest10kTime(float best10kTimer)
	{
		SubmitTime(LeaderboardType.best10kTime, best10kTimer);
	}

	public void SubmitBestOneMinuteSprint(float timer, int bestOneMinuteSprint)
	{
		if (timer >= 60f)
		{
			SubmitScore(LeaderboardType.bestOneMinuteSprint, bestOneMinuteSprint);
		}
	}

	public void SubmitShowroomValue(int showroomValue)
	{
		SubmitScore(LeaderboardType.showroomValue, showroomValue);
	}

	public void SubmitPrestigePoints(int amount)
	{
	}

	public void CacheLeaderboards(SocialPlatform platform)
	{
		if (_scoresCache == null)
		{
			_scoresCache = new Dictionary<SocialPlatform, Dictionary<LeaderboardType, List<LeaderboardScore>>>();
		}
		if (!_scoresCache.ContainsKey(platform))
		{
			_scoresCache[platform] = new Dictionary<LeaderboardType, List<LeaderboardScore>>();
		}
		StartCoroutine(CacheLeaderboardsCR(platform));
	}

	public List<LeaderboardScore> GetMainLeaderboardScores(LeaderboardType leaderboard)
	{
		return GetLeaderboardScores(leaderboard, AutoSingleton<BackendManager>.Instance.MainSocialPlatform);
	}

	public List<LeaderboardScore> GetLeaderboardScores(LeaderboardType leaderboard, SocialPlatform platform)
	{
		if (_scoresCache != null && _scoresCache.ContainsKey(platform) && _scoresCache[platform] != null && _scoresCache[platform].ContainsKey(leaderboard))
		{
			return _scoresCache[platform][leaderboard];
		}
		return null;
	}

	public Dictionary<SocialPlatform, List<LeaderboardScore>> GetLeaderboardScoresDict(LeaderboardType leaderboardType)
	{
		Dictionary<SocialPlatform, List<LeaderboardScore>> dictionary = new Dictionary<SocialPlatform, List<LeaderboardScore>>();
		if (_scoresCache != null)
		{
			foreach (KeyValuePair<SocialPlatform, Dictionary<LeaderboardType, List<LeaderboardScore>>> item in _scoresCache)
			{
				Dictionary<LeaderboardType, List<LeaderboardScore>> value = item.Value;
				if (value.ContainsKey(leaderboardType))
				{
					dictionary[item.Key] = value[leaderboardType];
				}
			}
			return dictionary;
		}
		return dictionary;
	}

	public void RetrieveShowroomLeaderboard()
	{
	}

	private IEnumerator CacheLeaderboardsCR(SocialPlatform platform)
	{
		if (platform == SocialPlatform.facebook)
		{
			if (_scoresCache != null)
			{
				_scoresCache.ContainsKey(platform);
			}
			Action<LeaderboardType> downloadLeaderboard = delegate(LeaderboardType type)
			{
				_003CCacheLeaderboardsCR_003Ec__Iterator31 _003CCacheLeaderboardsCR_003Ec__Iterator = this;
				bool flag = true;
				if (base._003C_003Ef__this._leaderboardsTimestamp.ContainsKey(type))
				{
					flag = ((DateTime.Now - base._003C_003Ef__this._leaderboardsTimestamp[type]).TotalMinutes > 30.0);
				}
				if (!base._003CfacebookLeaderboardAvailable_003E__0 || !base._003C_003Ef__this._scoresCache[base.platform].ContainsKey(type) || flag)
				{
					Action retrieveScores = delegate
					{
						AutoSingleton<BackendManager>.Instance.GetFriendsLeaderboardEntries(type.ToString());
					};
					base._003C_003Ef__this.StartCoroutine(base._003C_003Ef__this.CacheLeaderboardCR(type, retrieveScores, base.platform));
				}
				else if (base._003C_003Ef__this.OnScoresRetrievedCallback != null)
				{
					List<LeaderboardScore> leaderboardScores = base._003C_003Ef__this.GetLeaderboardScores(type, base.platform);
					base._003C_003Ef__this.OnScoresRetrievedCallback(SocialPlatform.facebook, type, leaderboardScores);
				}
			};
			downloadLeaderboard(LeaderboardType.longestRoadTrip);
			downloadLeaderboard(LeaderboardType.mostBucksFrenzyMode);
		}
		yield return null;
	}

	private IEnumerator CacheLeaderboardCR(LeaderboardType leaderboard, Action retrieveScores, SocialPlatform platform)
	{
		while (_cachingLeaderboard)
		{
			yield return new WaitForSeconds(0.1f);
		}
		_cachingLeaderboard = true;
		RegisterRetrieveScores(platform);
		_currentLeaderboard = leaderboard;
		_scoresRetrieved = false;
		retrieveScores();
		while (!_scoresRetrieved)
		{
			yield return null;
		}
		UnregisterRetrieveScores(platform);
		yield return null;
		_cachingLeaderboard = false;
	}

	private void RegisterRetrieveScores(SocialPlatform platform)
	{
		if (platform == SocialPlatform.facebook)
		{
			GameFacebookManager.OnScoresLoaded += OnFacebookRetrieveScores;
			GameFacebookManager.OnRetrieveScoresFailed += OnFacebookRetrieveScoresFailed;
		}
	}

	private void UnregisterRetrieveScores(SocialPlatform platform)
	{
		if (platform == SocialPlatform.facebook)
		{
			GameFacebookManager.OnScoresLoaded -= OnFacebookRetrieveScores;
			GameFacebookManager.OnRetrieveScoresFailed -= OnFacebookRetrieveScoresFailed;
		}
	}

	private void OnFacebookRetrieveScores(List<LeaderboardScore> fbScores)
	{
		_leaderboardsTimestamp[_currentLeaderboard] = DateTime.Now;
		_scoresRetrieved = true;
		_scoresCache[SocialPlatform.facebook][_currentLeaderboard] = fbScores;
		if (this.OnScoresRetrievedCallback != null)
		{
			this.OnScoresRetrievedCallback(SocialPlatform.facebook, _currentLeaderboard, fbScores);
		}
	}

	private void OnFacebookRetrieveScoresFailed()
	{
		_scoresRetrieved = true;
		if (this.OnScoresRetrievedFailedCallback != null)
		{
			this.OnScoresRetrievedFailedCallback(SocialPlatform.facebook, _currentLeaderboard);
		}
	}

	private bool UpdateFBLocalScore(LeaderboardType type, int score)
	{
		bool result = false;
		if (_scoresCache != null && _scoresCache.ContainsKey(SocialPlatform.facebook) && _scoresCache[SocialPlatform.facebook] != null)
		{
			string playerIdentifier = AutoSingleton<BackendManager>.Instance.PlayerIdentifier(SocialPlatform.facebook);
			string username = AutoSingleton<BackendManager>.Instance.PlayerAlias(SocialPlatform.facebook);
			LeaderboardScore item = new LeaderboardScore(username, playerIdentifier, score, SocialPlatform.facebook);
			if (_scoresCache[SocialPlatform.facebook].ContainsKey(type))
			{
				List<LeaderboardScore> list = _scoresCache[SocialPlatform.facebook][type];
				LeaderboardScore leaderboardScore = list.Find((LeaderboardScore player) => player._userId == playerIdentifier);
				if (leaderboardScore == null)
				{
					list.Add(item);
					result = true;
				}
				else if (score > leaderboardScore._value)
				{
					leaderboardScore._value = score;
					result = true;
				}
				list.Sort();
			}
			else
			{
				_scoresCache[SocialPlatform.facebook][type] = new List<LeaderboardScore>();
				_scoresCache[SocialPlatform.facebook][type].Add(item);
				result = true;
			}
		}
		return result;
	}

	public void UpdateLocalPlayerBestTime(LeaderboardType leaderboard, long newTime)
	{
		SocialPlatform mainSocialPlatform = AutoSingleton<BackendManager>.Instance.MainSocialPlatform;
		if ((leaderboard == LeaderboardType.best2kTime || leaderboard == LeaderboardType.best5kTime || leaderboard == LeaderboardType.best10kTime) && _scoresCache != null && _scoresCache.ContainsKey(mainSocialPlatform) && _scoresCache[mainSocialPlatform] != null && _scoresCache[mainSocialPlatform].ContainsKey(leaderboard))
		{
			string playerIdentifier = AutoSingleton<BackendManager>.Instance.PlayerIdentifier();
			string userId = AutoSingleton<BackendManager>.Instance.PlayerAlias();
			LeaderboardScore leaderboardScore = _scoresCache[mainSocialPlatform][leaderboard].Find((LeaderboardScore player) => player._userId == playerIdentifier);
			if (leaderboardScore == null)
			{
				LeaderboardScore item = new LeaderboardScore(playerIdentifier, userId, newTime, mainSocialPlatform);
				_scoresCache[mainSocialPlatform][leaderboard].Add(item);
			}
			else if (newTime < leaderboardScore._value)
			{
				leaderboardScore._value = newTime;
			}
			_scoresCache[mainSocialPlatform][leaderboard].Sort((LeaderboardScore s1, LeaderboardScore s2) => (int)s1._value - (int)s2._value);
		}
	}

	public List<string> FriendsActiveToday()
	{
		List<string> list = new List<string>();
		LeaderboardType key = LeaderboardType.highestLevel;
		if (AutoSingleton<PlatformCapabilities>.Instance.UseFacebookAsSocialPlatform())
		{
			key = LeaderboardType.longestRoadTrip;
		}
		SocialPlatform mainSocialPlatform = AutoSingleton<BackendManager>.Instance.MainSocialPlatform;
		if (_scoresCache != null && _scoresCache.ContainsKey(mainSocialPlatform) && _scoresCache[mainSocialPlatform] != null && _scoresCache[mainSocialPlatform].ContainsKey(key))
		{
			List<LeaderboardScore> list2 = _scoresCache[mainSocialPlatform][key];
			if (list2 != null)
			{
				foreach (LeaderboardScore item in list2)
				{
					list.Add(item._userId);
				}
				return list;
			}
		}
		return list;
	}

	public void OnFacebookLogout()
	{
		ClearLeaderboardCache(SocialPlatform.facebook);
	}

	private void ClearLeaderboardCache(SocialPlatform platform)
	{
		if (_scoresCache != null && _scoresCache.ContainsKey(platform))
		{
			_scoresCache.Remove(platform);
		}
	}
}
