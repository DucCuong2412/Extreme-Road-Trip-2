using System;
using System.Collections.Generic;
using UnityEngine;

public class MetroPanelLeaderboards : MetroWidget
{
	private const int _maxLeaderboardEntry = 30;

	private LeaderboardType _leaderboardType;

	private MetroWidget _mainPanel;

	private List<LeaderboardScore> _facebookScores;

	private List<LeaderboardScore> _gameCenterScores;

	public static MetroPanelLeaderboards Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroPanelLeaderboards).ToString());
		return gameObject.AddComponent<MetroPanelLeaderboards>().Setup();
	}

	private void OnDestroy()
	{
		HideLoadingWheel();
		UnregisterCallback();
	}

	private void UnregisterCallback()
	{
		if (AutoSingleton<LeaderboardsManager>.IsCreated())
		{
			AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedCallback -= OnScoresRetrievedCallback;
			AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedFailedCallback -= OnScoresRetrievedFailedCallback;
		}
		if (AutoSingleton<GameFacebookManager>.IsCreated())
		{
			GameFacebookManager.OnRetrieveUserInfoFailed -= OnFacebookLoginFailed;
		}
	}

	private MetroPanelLeaderboards Setup()
	{
		LeaderboardsManager instance = AutoSingleton<LeaderboardsManager>.Instance;
		LoadConfigGame loadConfigGame = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigGame;
		if (loadConfigGame != null && loadConfigGame.GameMode == GameMode.frenzy)
		{
			_leaderboardType = LeaderboardType.mostBucksFrenzyMode;
		}
		else
		{
			_leaderboardType = LeaderboardType.longestRoadTrip;
		}
		_facebookScores = instance.GetLeaderboardScores(_leaderboardType, SocialPlatform.facebook);
		_gameCenterScores = instance.GetLeaderboardScores(_leaderboardType, SocialPlatform.gameCenter);
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		_mainPanel = MetroSpacer.Create(5f);
		_mainPanel.Add(CreateScoresPanel());
		metroLayout.Add(_mainPanel);
		return this;
	}

	private void OnScoresRetrievedCallback(SocialPlatform platform, LeaderboardType leaderboardType, List<LeaderboardScore> scores)
	{
		if (leaderboardType == _leaderboardType)
		{
			if (scores != null)
			{
				if (platform == SocialPlatform.facebook)
				{
					UnregisterCallback();
					_facebookScores = scores;
					RefreshScorePanel();
				}
			}
			else
			{
				OnError("Can't retrieves the leaderboard.");
			}
		}
		HideLoadingWheel();
	}

	private void OnScoresRetrievedFailedCallback(SocialPlatform platform, LeaderboardType leaderboardType)
	{
		UnregisterCallback();
		if (leaderboardType == _leaderboardType)
		{
			OnError("Can't retrieves the leaderboard.");
		}
		HideLoadingWheel();
	}

	private void OnFacebookLoginFailed(string error)
	{
		HideLoadingWheel();
		UnregisterCallback();
		OnError("Facebook login failed.");
	}

	private MetroWidget CreateFacebookLoginButton()
	{
		if (_facebookScores == null || !AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
		{
			Action onButtonClicked = delegate
			{
				AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedCallback += OnScoresRetrievedCallback;
				AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedFailedCallback += OnScoresRetrievedFailedCallback;
				ShowLoadingWheel();
				if (AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
				{
					AutoSingleton<LeaderboardsManager>.Instance.CacheLeaderboards(SocialPlatform.facebook);
				}
				else
				{
					GameFacebookManager.OnRetrieveUserInfoFailed += OnFacebookLoginFailed;
					AutoSingleton<GameFacebookManager>.Instance.Login(delegate
					{
						AutoSingleton<BackendManager>.Instance.Authenticate(SocialPlatform.facebook);
					}, isPublishNeeded: false, OnFacebookLoginFailed);
				}
			};
			return CreateLoadScoresButton(MetroSkin.Slice9ButtonBlue, "Play with your friends on Facebook", onButtonClicked);
		}
		return null;
	}

	private MetroWidget CreateLoadScoresButton(string bkg, string buttonLabel, Action onButtonClicked)
	{
		MetroButton metroButton = MetroButton.Create();
		metroButton.Add(MetroLabel.Create(buttonLabel).SetFont(MetroSkin.SmallFont));
		metroButton.OnButtonClicked += onButtonClicked;
		metroButton.AddSlice9Background(bkg);
		return metroButton;
	}

	private MetroWidget CreateScoresPanel()
	{
		MetroViewport metroViewport = MetroViewport.Create(MetroSkin.ClippedGUILayer1);
		MetroSlider metroSlider = MetroSlider.Create(Direction.vertical, 4.5f);
		metroViewport.Add(metroSlider);
		List<LeaderboardScore> list = new List<LeaderboardScore>();
		if (_gameCenterScores != null)
		{
			list.AddRange(_gameCenterScores);
		}
		if (_facebookScores != null)
		{
			list.AddRange(_facebookScores);
		}
		if (list.Count == 0)
		{
			list = GetOfflineScores();
		}
		list.Sort();
		MetroWidget metroWidget = null;
		int num = 1;
		foreach (LeaderboardScore item in list)
		{
			if (item != null)
			{
				if (num > 30)
				{
					break;
				}
				if (item._value > 0)
				{
					MetroPanelLeaderboardEntry metroPanelLeaderboardEntry = MetroPanelLeaderboardEntry.Create(_leaderboardType, num, item);
					metroPanelLeaderboardEntry.enabled = (!item._userId.Equals(AutoSingleton<GameFacebookManager>.Instance.PlayerId()) && item._userPlatform != SocialPlatform.fake);
					metroSlider.Add(metroPanelLeaderboardEntry);
					num++;
					if (AutoSingleton<BackendManager>.Instance.IsLoggedUser(item._userId))
					{
						metroWidget = metroPanelLeaderboardEntry;
						if (AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported() && !AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
						{
							metroSlider.Add(CreateFacebookLoginButton());
						}
					}
				}
			}
		}
		if (metroWidget != null)
		{
			metroSlider.Focus(metroWidget);
		}
		return metroViewport;
	}

	private List<LeaderboardScore> GetOfflineScores()
	{
		List<LeaderboardScore> list = new List<LeaderboardScore>();
		long val = 0L;
		if (_leaderboardType == LeaderboardType.longestRoadTrip)
		{
			list.Add(new LeaderboardScore("Ethan", "IconAvatar01", 19875L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Emma", "IconAvatar02", 12540L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Jayden", "IconAvatar03", 7345L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Richard", "IconAvatar04", 2980L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Oliver", "IconAvatar05", 750L, SocialPlatform.fake));
			val = AutoSingleton<GameStatsManager>.Instance.Overall.GetMax(GameStats.CarStats.Type.maxDistance);
		}
		else if (_leaderboardType == LeaderboardType.mostBucksFrenzyMode)
		{
			list.Add(new LeaderboardScore("Ethan", "IconAvatar01", 5L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Emma", "IconAvatar02", 3L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Jayden", "IconAvatar03", 3L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Richard", "IconAvatar04", 2L, SocialPlatform.fake));
			list.Add(new LeaderboardScore("Oliver", "IconAvatar05", 1L, SocialPlatform.fake));
			val = AutoSingleton<GameStatsManager>.Instance.Overall.GetMax(GameStats.CarStats.Type.bucksPickedUp);
		}
		string userId = AutoSingleton<BackendManager>.Instance.PlayerIdentifier();
		LeaderboardScore item = new LeaderboardScore("You", userId, val, SocialPlatform.fake);
		list.Add(item);
		return list;
	}

	private void OnError(string message)
	{
	}

	private void RefreshScorePanel()
	{
		_mainPanel.Clear().Add(CreateScoresPanel());
		_mainPanel.Parent.Reflow();
	}

	private void ShowLoadingWheel()
	{
	}

	private void HideLoadingWheel()
	{
	}
}
