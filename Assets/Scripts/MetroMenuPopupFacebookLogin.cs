using System;

public class MetroMenuPopupFacebookLogin : MetroMenuPage
{
	private MetroButton _yes;

	private MetroButton _no;

	private MetroButton _later;

	public MetroMenuPopupFacebookLogin Setup(bool publish = false)
	{
		string label = "LOG IN";
		string label2 = "NO THANKS";
		string label3 = "LATER";
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		Add(metroLayout);
		if (publish)
		{
			label = "SHARE";
			MetroLabel metroLabel = MetroLabel.Create("Let your friends know!");
			metroLabel.SetFont(MetroSkin.BigFont);
			metroLayout.Add(metroLabel);
			metroLabel.SetMass(2f);
			MetroLayout metroLayout2 = MetroLayout.Create("features", Direction.horizontal);
			metroLayout.Add(metroLayout2);
			metroLayout2.SetMass(4f);
			MetroLayout metroLayout3 = MetroLayout.Create("achievement", Direction.vertical);
			metroLayout2.Add(metroLayout3);
			MetroLabel child = MetroLabel.Create("Achievements");
			metroLayout3.Add(child);
			metroLayout3.Add(MetroSpacer.Create(0.2f));
			MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
			metroLayout3.Add(metroLayout4);
			metroLayout4.Add(MetroIcon.Create("FacebookAchievement"));
			metroLayout4.SetMass(4f);
			MetroLayout metroLayout5 = MetroLayout.Create("score", Direction.vertical);
			metroLayout2.Add(metroLayout5);
			MetroLabel child2 = MetroLabel.Create("My best score!");
			metroLayout5.Add(child2);
			metroLayout5.Add(MetroSpacer.Create(0.2f));
			MetroLayout metroLayout6 = MetroLayout.Create(Direction.horizontal);
			metroLayout5.Add(metroLayout6);
			metroLayout6.Add(MetroIcon.Create("IconHighscore"));
			metroLayout6.SetMass(4f);
		}
		else
		{
			MetroLabel metroLabel2 = MetroLabel.Create("Log to Facebook now!");
			metroLabel2.SetFont(MetroSkin.BigFont);
			metroLayout.Add(metroLabel2);
			metroLabel2.SetMass(2f);
			MetroLayout metroLayout7 = MetroLayout.Create("features", Direction.horizontal);
			metroLayout.Add(metroLayout7);
			metroLayout7.SetMass(4f);
			MetroLayout metroLayout8 = MetroLayout.Create("roadSign", Direction.vertical);
			metroLayout7.Add(metroLayout8);
			MetroLabel metroLabel3 = MetroLabel.Create("Friend Signs");
			metroLabel3.SetLineSpacing(0f);
			metroLayout8.Add(metroLabel3);
			metroLayout8.Add(MetroSpacer.Create(0.2f));
			MetroLayout metroLayout9 = MetroLayout.Create(Direction.horizontal);
			metroLayout8.Add(metroLayout9);
			metroLayout9.Add(MetroIcon.Create("friendRoadSigns"));
			metroLayout9.SetMass(4f);
			MetroLayout metroLayout10 = MetroLayout.Create("leaderboard", Direction.vertical);
			metroLayout7.Add(metroLayout10);
			MetroLabel metroLabel4 = MetroLabel.Create("Leaderboards");
			metroLabel4.SetLineSpacing(0f);
			metroLayout10.Add(metroLabel4);
			metroLayout10.Add(MetroSpacer.Create(0.2f));
			MetroLayout metroLayout11 = MetroLayout.Create(Direction.horizontal);
			metroLayout10.Add(metroLayout11);
			metroLayout11.Add(MetroIcon.Create(MetroSkin.IconLeaderboard).SetScale(1.25f));
			metroLayout11.SetMass(4f);
			MetroLayout metroLayout12 = MetroLayout.Create("ghost", Direction.vertical);
			metroLayout7.Add(metroLayout12);
			MetroLabel metroLabel5 = MetroLabel.Create("Friend Ghosts");
			metroLabel5.SetLineSpacing(0f);
			metroLayout12.Add(metroLabel5);
			metroLayout12.Add(MetroSpacer.Create(0.2f));
			MetroLayout metroLayout13 = MetroLayout.Create(Direction.horizontal);
			metroLayout12.Add(metroLayout13);
			metroLayout13.Add(MetroIcon.Create("friendGhosts"));
			metroLayout13.SetMass(4f);
		}
		metroLayout.Add(MetroSpacer.Create(1f));
		MetroLayout metroLayout14 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout14);
		metroLayout14.Add(MetroSpacer.Create(0.1f));
		_later = MetroButton.Create(label3);
		metroLayout14.Add(_later);
		_later.AddSlice9Background(MetroSkin.Slice9Button);
		_later.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		};
		metroLayout14.Add(MetroSpacer.Create(0.05f));
		_no = MetroButton.Create(label2);
		metroLayout14.Add(_no);
		metroLayout14.Add(MetroSpacer.Create(0.05f));
		_no.AddSlice9Background(MetroSkin.Slice9Button);
		if (publish)
		{
			_no.OnButtonClicked += delegate
			{
				AutoSingleton<PersistenceManager>.Instance.MustShowFacebookInvitationToPublishPopup = false;
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
		}
		else
		{
			_no.OnButtonClicked += delegate
			{
				AutoSingleton<PersistenceManager>.Instance.MustShowFacebookInvitationToLoginPopup = false;
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
		}
		_yes = MetroButton.Create(label);
		metroLayout14.Add(_yes);
		_yes.AddSlice9Background(MetroSkin.Slice9ButtonBlue);
		if (publish)
		{
			_yes.OnButtonClicked += delegate
			{
				AutoSingleton<GameFacebookManager>.Instance.CheckPublishPermissions(delegate
				{
				});
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
		}
		else
		{
			_yes.OnButtonClicked += delegate
			{
				Action onLoginWithPermissionSucceeded = delegate
				{
					AutoSingleton<BackendManager>.Instance.Authenticate(SocialPlatform.facebook);
				};
				AutoSingleton<GameFacebookManager>.Instance.Login(onLoginWithPermissionSucceeded, isPublishNeeded: false);
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
		}
		metroLayout14.Add(MetroSpacer.Create(0.1f));
		metroLayout.Add(MetroSpacer.Create(0.5f));
		return this;
	}
}
