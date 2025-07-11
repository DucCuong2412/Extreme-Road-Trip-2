using UnityEngine;

public class MetroPanelLeaderboardEntry : MetroButton
{
	private string _scoreSuffix;

	public static MetroPanelLeaderboardEntry Create(LeaderboardType leaderboardType, int index, LeaderboardScore score)
	{
		GameObject gameObject = new GameObject(typeof(MetroPanelLeaderboardEntry).ToString());
		return gameObject.AddComponent<MetroPanelLeaderboardEntry>().Setup(leaderboardType, index, score);
	}

	private MetroPanelLeaderboardEntry Setup(LeaderboardType leaderboardType, int index, LeaderboardScore score)
	{
		object scoreSuffix;
		switch (leaderboardType)
		{
		case LeaderboardType.mostBucksFrenzyMode:
			scoreSuffix = " bucks";
			break;
		case LeaderboardType.longestRoadTrip:
			scoreSuffix = "m";
			break;
		default:
			scoreSuffix = string.Empty;
			break;
		}
		_scoreSuffix = (string)scoreSuffix;
		AddSlice9Background((!AutoSingleton<BackendManager>.Instance.IsLoggedUser(score._userId)) ? MetroSkin.Slice9RoundedSemiTransparent : MetroSkin.Slice9RoundedSemiTransparentRed);
		base.OnButtonClicked += delegate
		{
			string message = string.Format("Send a challenge to {0}?".Localize(), score._username);
			MetroMenuPopupYesNoLater metroMenuPopupYesNoLater = MetroMenuPage.Create<MetroMenuPopupYesNoLater>().Setup("GAME CENTER CHALLENGE", message, "SEND", "CANCEL", MetroSkin.Slice9ButtonRed, MetroSkin.SmallFont, null, 50);
			metroMenuPopupYesNoLater.OnButtonYes(delegate
			{
				if (score._userPlatform == SocialPlatform.facebook)
				{
					AutoSingleton<GameFacebookManager>.Instance.ChallengeFriends(null, score._userId);
				}
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			});
			metroMenuPopupYesNoLater.OnButtonNo(delegate
			{
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			});
			metroMenuPopupYesNoLater.OnClose(delegate
			{
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			});
			AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPopupYesNoLater, MetroAnimation.popup);
		};
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroLabel.Create(index.ToString()));
		MetroSpacer metroSpacer = MetroSpacer.Create(3f);
		metroLayout.Add(metroSpacer);
		if (PictureManager.IsPictureLoaded(score._userId))
		{
			WidgetPlayerPicture child = WidgetPlayerPicture.Create(score._userId, 0.15f);
			metroSpacer.Add(child);
		}
		else if (score._userId.Contains("IconAvatar"))
		{
			metroSpacer.Add(MetroIcon.Create(score._userId).SetScale(0.9f));
		}
		metroLayout.Add(MetroSpacer.Create(0.1f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		metroLayout2.SetMass(8f);
		metroLayout.Add(metroLayout2);
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		MetroLabel metroLabel = MetroLabel.Create(StringUtil.Trunc(score._username, 15));
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.AddOutline();
		metroLabel.SetAlignment(MetroAlign.Left);
		metroLayout2.Add(metroLabel);
		MetroLabel metroLabel2 = MetroLabel.Create(score._value.ToString() + _scoreSuffix.Localize());
		metroLabel2.SetFont(MetroSkin.SmallFont);
		metroLabel2.SetColor(Color.yellow);
		metroLabel2.AddOutline();
		metroLabel2.SetAlignment(MetroAlign.Left);
		metroLayout2.Add(metroLabel2);
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		return this;
	}
}
