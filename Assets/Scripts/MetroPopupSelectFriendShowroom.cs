using System.Collections.Generic;
using UnityEngine;

public class MetroPopupSelectFriendShowroom : MetroPopupPage
{
	private const int _friendMax = 20;

	private MetroButton _closeButton;

	private MetroLabel _messageLabel;

	private MetroWidget _friendsPanel;

	private Color _oddRowColor;

	private Color _evenRowColor;

	private User _selectedFriend;

	private bool _isFacebookMainSocialPlatform;

	protected override void OnAwake()
	{
		_width = 0.95f;
		_height = 0.95f;
		_isFacebookMainSocialPlatform = AutoSingleton<PlatformCapabilities>.Instance.UseFacebookAsSocialPlatform();
		_oddRowColor = ((!_isFacebookMainSocialPlatform) ? MetroSkin.GameCenterPlayerOddRowColor : MetroSkin.FacebookOddRowColor);
		_evenRowColor = ((!_isFacebookMainSocialPlatform) ? MetroSkin.GameCenterPlayerEvenRowColor : MetroSkin.FacebookEvenRowColor);
		if (_isFacebookMainSocialPlatform)
		{
			AutoSingleton<BackendManager>.Instance.OnRetrieveFriendsCallback += OnFriendsLoaded;
			AutoSingleton<BackendManager>.Instance.OnRetrieveFriendsFailedCallback += OnRetrieveFriendsFailed;
		}
		else
		{
			AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedCallback += OnScoresRetrievedCallback;
			AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedFailedCallback += OnScoresRetrievedFailedCallback;
		}
		base.OnAwake();
	}

	protected override void Cleanup()
	{
		UnregisterShowroomCallback();
		UnregisterFriendsCallback();
		base.Cleanup();
	}

	public override void OnFocus()
	{
		base.OnFocus();
		HandleBackendConnection();
	}

	private void UnregisterFriendsCallback()
	{
		if (_isFacebookMainSocialPlatform)
		{
			AutoSingleton<BackendManager>.Instance.OnRetrieveFriendsCallback -= OnFriendsLoaded;
			AutoSingleton<BackendManager>.Instance.OnRetrieveFriendsFailedCallback -= OnRetrieveFriendsFailed;
		}
		else
		{
			AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedCallback -= OnScoresRetrievedCallback;
			AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedFailedCallback -= OnScoresRetrievedFailedCallback;
		}
	}

	private void UnregisterShowroomCallback()
	{
		AutoSingleton<BackendManager>.Instance.OnShowroomLoadingSucceeded -= OnShowroomLoadingSucceeded;
		AutoSingleton<BackendManager>.Instance.OnShowroomLoadingFailed -= OnShowroomLoadingFailed;
	}

	private void HandleBackendConnection()
	{
		if (AutoSingleton<BackendManager>.Instance.IsLoggedIn())
		{
			if (_isFacebookMainSocialPlatform)
			{
				List<User> list = AutoSingleton<BackendManager>.Instance.Friends();
				if (list == null)
				{
					AutoSingleton<BackendManager>.Instance.RetrieveFriends();
				}
			}
			else
			{
				LeaderboardsManager instance = AutoSingleton<LeaderboardsManager>.Instance;
				if (instance.GetMainLeaderboardScores(LeaderboardType.showroomValue) == null)
				{
					instance.RetrieveShowroomLeaderboard();
				}
			}
		}
		else if (_isFacebookMainSocialPlatform)
		{
			AutoSingleton<GameFacebookManager>.Instance.Login(delegate
			{
				AutoSingleton<BackendManager>.Instance.Authenticate(SocialPlatform.facebook);
			}, isPublishNeeded: false, OnLoginFailed);
		}
	}

	private void OnScoresRetrievedCallback(SocialPlatform platform, LeaderboardType leaderboardType, List<LeaderboardScore> scores)
	{
		if ((platform == SocialPlatform.gameCenter && leaderboardType == LeaderboardType.showroomValue) || (platform == SocialPlatform.facebook && leaderboardType == LeaderboardType.longestRoadTrip))
		{
			OnFriendsLoaded();
		}
	}

	private void OnFriendsLoaded()
	{
		UpdateFriendsPanel();
		_friendsPanel.Reflow();
		UnregisterFriendsCallback();
	}

	private void OnScoresRetrievedFailedCallback(SocialPlatform platform, LeaderboardType leaderboardType)
	{
		if ((platform == SocialPlatform.gameCenter && leaderboardType == LeaderboardType.showroomValue) || (platform == SocialPlatform.facebook && leaderboardType == LeaderboardType.longestRoadTrip))
		{
			OnRetrieveFriendsFailed();
		}
	}

	private void OnRetrieveFriendsFailed()
	{
		string messageText = (!_isFacebookMainSocialPlatform) ? "CAN'T RETRIEVE YOUR GAME CENTER FRIENDS LIST.\n\nPLEASE TRY AGAIN." : "CAN'T RETRIEVE YOUR FACEBOOK FRIENDS LIST.\n\nPLEASE TRY AGAIN.";
		SetMessageText(messageText);
		UnregisterFriendsCallback();
	}

	private void OnLoginFailed(string error)
	{
		SetMessageText("Facebook login failed.");
	}

	public MetroPopupSelectFriendShowroom Setup()
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("Content", Direction.vertical);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel child = MetroLabel.Create("FRIENDS' SHOWROOM");
		metroLayout2.Add(child);
		_friendsPanel = MetroLayout.Create("FriendsPanel", Direction.vertical).SetMass(6f);
		metroLayout.Add(_friendsPanel);
		UpdateFriendsPanel();
		metroLayout.Add(MetroSpacer.Create(0.2f));
		return this;
	}

	private List<User> GetFriends()
	{
		List<User> result = null;
		if (_isFacebookMainSocialPlatform)
		{
			result = AutoSingleton<BackendManager>.Instance.Friends();
		}
		return result;
	}

	private void UpdateFriendsPanel()
	{
		_friendsPanel.Clear();
		List<User> friends = GetFriends();
		if (HaveFriends(friends))
		{
			MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).AddSlice9Background(MetroSkin.Slice9PopupBackground).SetPadding(2f, 0.2f);
			_friendsPanel.Add(metroWidget);
			MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
			metroWidget.Add(MetroSpacer.Create(0.05f));
			metroWidget.Add(metroLayout);
			metroWidget.Add(MetroSpacer.Create(0.05f));
			MetroViewport metroViewport = MetroViewport.Create(MetroSkin.ClippedGUILayer1);
			metroLayout.Add(MetroSpacer.Create(0.05f));
			metroLayout.Add(metroViewport);
			metroLayout.Add(MetroSpacer.Create(0.05f));
			float onScreenCount = 3.5f;
			MetroSlider metroSlider = MetroSlider.Create(Direction.vertical, onScreenCount);
			metroViewport.Add(metroSlider);
			int num = 0;
			foreach (User item in friends)
			{
				if (!IsLocalUser(item))
				{
					MetroWidget child = CreateFriendWidget(item, num);
					metroSlider.Add(child);
					num++;
					if (num >= 20)
					{
						break;
					}
				}
			}
		}
		else
		{
			string empty = string.Empty;
			empty = ((friends != null) ? ((!_isFacebookMainSocialPlatform) ? "NONE OF YOUR GAME CENTER FRIENDS HAVE A SHOWROOM SETUP." : "NONE OF YOUR FACEBOOK FRIENDS ARE PLAYING EXTREME ROAD TRIP 2.") : ((!_isFacebookMainSocialPlatform) ? "LOG INTO GAME CENTER TO SEE YOUR FRIEND'S SHOWROOM." : "WAITING FOR FACEBOOK..."));
			CreateMessageLabel();
			SetMessageText(empty);
		}
	}

	private void CreateMessageLabel()
	{
		_messageLabel = MetroLabel.Create(string.Empty);
		_messageLabel.SetFont(MetroSkin.MediumFont);
		_messageLabel.SetLineSpacing(0f);
		_friendsPanel.Clear();
		_friendsPanel.Add(_messageLabel);
		_friendsPanel.Add(MetroSpacer.Create());
	}

	private void SetMessageText(string message)
	{
		if (_messageLabel != null)
		{
			string text = message.Localize().Wrap(40);
			_messageLabel.SetText(text);
		}
	}

	private MetroWidget CreateFriendWidget(User user, int index)
	{
		Color color = (index % 2 != 0) ? _oddRowColor : _evenRowColor;
		MetroButton metroButton = MetroButton.Create();
		metroButton.AddSolidBackground().SetColor(color);
		metroButton.OnButtonClicked += delegate
		{
			_selectedFriend = user;
			CreateMessageLabel();
			_friendsPanel.Reflow();
			SetMessageText("LOADING...");
			AutoSingleton<BackendManager>.Instance.OnShowroomLoadingSucceeded += OnShowroomLoadingSucceeded;
			AutoSingleton<BackendManager>.Instance.OnShowroomLoadingFailed += OnShowroomLoadingFailed;
			AutoSingleton<BackendManager>.Instance.LoadShowroom(user._id);
		};
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal);
		metroButton.Add(metroWidget);
		metroWidget.Add(MetroSpacer.Create());
		if (PictureManager.IsPictureLoaded(user._id))
		{
			WidgetPlayerPicture child = WidgetPlayerPicture.Create(user._id, 0.1f);
			metroWidget.Add(child);
		}
		else
		{
			metroWidget.Add(MetroSpacer.Create());
		}
		metroWidget.Add(MetroSpacer.Create(0.2f));
		MetroLabel metroLabel = MetroLabel.Create(user._displayName);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetAlignment(MetroAlign.Left);
		metroLabel.SetMass(5f);
		metroWidget.Add(metroLabel);
		metroWidget.Add(MetroSpacer.Create());
		return metroButton;
	}

	private void OnShowroomLoadingSucceeded(string response)
	{
		UnregisterShowroomCallback();
		AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		ShowroomSetup showroomSetup = new ShowroomSetup(response);
		showroomSetup.OwnerName = ((_selectedFriend == null) ? string.Empty : _selectedFriend._displayName);
		showroomSetup.LocalShowroom = false;
		AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(showroomSetup));
		_selectedFriend = null;
	}

	private void OnShowroomLoadingFailed(string error)
	{
		UnregisterShowroomCallback();
		UnityEngine.Debug.Log(error);
		SetMessageText(_selectedFriend._displayName.ToUpper() + " HAS'T SETUP ANY SHOWROOM YET.".Localize());
		MetroButton metroButton = MetroButton.Create("FRIEND LIST");
		metroButton.AddSolidBackground().SetColor((!_isFacebookMainSocialPlatform) ? MetroSkin.GameCenterPlayerOddRowColor : MetroSkin.FacebookOddRowColor);
		metroButton.SetPadding(10f, 0f);
		metroButton.OnButtonClicked += delegate
		{
			UpdateFriendsPanel();
			_friendsPanel.Reflow();
		};
		_friendsPanel.Add(metroButton);
		_friendsPanel.Add(MetroSpacer.Create());
		_friendsPanel.Reflow();
		_selectedFriend = null;
	}

	private bool IsLocalUser(User user)
	{
		return user._id == AutoSingleton<BackendManager>.Instance.PlayerIdentifier();
	}

	private bool HaveFriends(List<User> friends)
	{
		if (friends == null || friends.Count == 0)
		{
			return false;
		}
		if (friends.Count == 1)
		{
			return !IsLocalUser(friends[0]);
		}
		return true;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.8f, (0f - base.Camera.HalfScreenHeight) * 0.9f, base.Camera.ScreenWidth * 0.8f, base.Camera.ScreenHeight * 0.9f);
	}
}
