using System;
using UnityEngine;

public class MetroPopupNewFeature : MetroPopupPage
{
	public MetroPopupNewFeature Setup(string titleString, string messageString, Action openButtonAction, string okButtonString = "OPEN", string promoIcon = "")
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel child = MetroLabel.Create(titleString);
		metroLayout2.Add(child);
		metroLayout.Add(MetroSpacer.Create(0.05f));
		bool flag = !string.IsNullOrEmpty(promoIcon);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		metroLayout3.SetMass((!flag) ? 3f : 5f);
		metroLayout.Add(metroLayout3);
		metroLayout.Add(MetroSpacer.Create(0.1f));
		metroLayout3.Add(MetroSpacer.Create((!flag) ? 0.1f : 0f));
		string content = messageString.Localize().Wrap((!flag) ? 40 : 50);
		MetroLabel metroLabel = MetroLabel.Create(content);
		metroLayout3.Add(metroLabel);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetLineSpacing(0f);
		if (flag)
		{
			_width = 1.05f;
			_height = 1.05f;
			_useCloseButton = false;
			metroLayout3.Add(MetroSpacer.Create(0.5f));
			MetroIcon metroIcon = MetroIcon.Create(UnityEngine.Object.Instantiate(Resources.Load(promoIcon)) as GameObject);
			metroIcon.SetMass(2f);
			metroLayout3.Add(metroIcon);
		}
		metroLayout3.Add(MetroSpacer.Create((!flag) ? 0.1f : 0.5f));
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		metroLayout4.SetMass(1.3f);
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		MetroButton laterButton = MetroButton.Create("LATER");
		metroLayout4.Add(laterButton);
		laterButton.AddSlice9Background(MetroSkin.Slice9Button);
		laterButton.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		};
		laterButton.OnKeyFocusGained += delegate
		{
			laterButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
			laterButton.Berp();
		};
		laterButton.OnKeyFocusLost += delegate
		{
			laterButton.AddSlice9Background(MetroSkin.Slice9Button);
		};
		MetroButton openButton = MetroButton.Create(okButtonString);
		metroLayout4.Add(openButton);
		openButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		openButton.OnButtonClicked += openButtonAction;
		openButton.OnKeyFocusGained += delegate
		{
			openButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
			openButton.Berp();
		};
		openButton.OnKeyFocusLost += delegate
		{
			openButton.AddSlice9Background(MetroSkin.Slice9Button);
		};
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroSpacer.Create(0.6f));
		return this;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.8f, (0f - base.Camera.HalfScreenHeight) * 0.9f, base.Camera.ScreenWidth * 0.8f, base.Camera.ScreenHeight * 0.9f);
	}
}
