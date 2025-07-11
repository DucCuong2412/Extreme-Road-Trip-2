using System;
using UnityEngine;

public class MetroPopupSocialSharing : MetroPopupPage
{
	public MetroPopupSocialSharing Setup(Action onFacebookButtonClicked, Action onTwitterButtonClicked)
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.5f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel metroLabel = MetroLabel.Create("SHARE");
		metroLabel.SetFont(MetroSkin.BigFont);
		metroLayout2.Add(metroLabel);
		metroLayout.Add(MetroSpacer.Create(1.4f));
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).SetMass(3f);
		metroLayout.Add(metroWidget);
		metroLayout.Add(MetroSpacer.Create(0.3f));
		metroWidget.Add(MetroSpacer.Create(0.5f));
		if (AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported())
		{
			Action onButtonClicked = delegate
			{
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
				onFacebookButtonClicked();
			};
			MetroButtonSocial child = MetroButtonSocial.Create(SocialNetwork.facebook, onButtonClicked, 1.2f);
			metroWidget.Add(child);
		}
		if (AutoSingleton<PlatformCapabilities>.Instance.IsTwitterSupported())
		{
			Action onButtonClicked2 = delegate
			{
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
				onTwitterButtonClicked();
			};
			MetroButtonSocial child2 = MetroButtonSocial.Create(SocialNetwork.twitter, onButtonClicked2, 1.2f);
			metroWidget.Add(child2);
		}
		metroWidget.Add(MetroSpacer.Create(0.5f));
		MetroWidget metroWidget2 = MetroLayout.Create(Direction.horizontal).SetPadding(0f);
		metroLayout.Add(metroWidget2);
		metroWidget2.Add(MetroSpacer.Create(0.5f));
		if (AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported())
		{
			MetroLabel metroLabel2 = MetroLabel.Create("Facebook");
			metroLabel2.SetFont(MetroSkin.MediumFont);
			metroWidget2.Add(metroLabel2);
		}
		if (AutoSingleton<PlatformCapabilities>.Instance.IsTwitterSupported())
		{
			MetroLabel metroLabel3 = MetroLabel.Create("Twitter");
			metroLabel3.SetFont(MetroSkin.MediumFont);
			metroWidget2.Add(metroLabel3);
		}
		metroWidget2.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroSpacer.Create(0.5f));
		return this;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.8f, (0f - base.Camera.HalfScreenHeight) * 0.6f, base.Camera.ScreenWidth * 0.8f, base.Camera.ScreenHeight * 0.6f);
	}
}
