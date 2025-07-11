using UnityEngine;

public class MetroPopupGameCenter : MetroPopupPage
{
	public MetroPopupGameCenter Setup()
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.3f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel metroLabel = MetroLabel.Create("GAME CENTER");
		metroLabel.SetFont(MetroSkin.BigFont);
		metroLayout2.Add(metroLabel);
		metroLayout.Add(MetroSpacer.Create(0.6f));
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).SetMass(3f);
		metroLayout.Add(metroWidget);
		metroLayout.Add(MetroSpacer.Create(0.6f));
		metroWidget.Add(MetroSpacer.Create(0.5f));
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconAchievements, "ACHIEVEMENTS");
		metroWidget.Add(metroButton);
		metroButton.OnButtonClicked += delegate
		{
		};
		metroWidget.Add(MetroSpacer.Create(0.3f));
		MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconLeaderboards, "LEADERBOARDS");
		metroWidget.Add(metroButton2);
		metroButton2.OnButtonClicked += delegate
		{
		};
		metroWidget.Add(MetroSpacer.Create(0.5f));
		return this;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.8f, (0f - base.Camera.HalfScreenHeight) * 0.5f, base.Camera.ScreenWidth * 0.8f, base.Camera.ScreenHeight * 0.5f);
	}
}
