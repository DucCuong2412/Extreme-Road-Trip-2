using System;
using UnityEngine;

public class MetroPopupMessage : MetroPopupPage
{
	private void DefaultAction()
	{
		AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
	}

	public MetroPopupMessage Setup(string titleString, string messageString)
	{
		return Setup(titleString, messageString, "CLOSE", MetroSkin.Slice9ButtonRed, DefaultAction);
	}

	public MetroPopupMessage Setup(string titleString, string messageString, string buttonString, string buttonSlice9, Action buttonAction)
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
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout3.SetMass(3f);
		metroLayout.Add(metroLayout3);
		metroLayout.Add(MetroSpacer.Create(0.1f));
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		string content = messageString.Localize().Wrap(40);
		MetroLabel metroLabel = MetroLabel.Create(content);
		metroLayout3.Add(metroLabel);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetLineSpacing(0f);
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		metroLayout4.SetMass(1.3f);
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		MetroButton metroButton = MetroButton.Create(buttonString);
		metroLayout4.Add(metroButton);
		metroButton.AddSlice9Background(buttonSlice9);
		metroButton.OnButtonClicked += buttonAction;
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroSpacer.Create(0.6f));
		return this;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.8f, (0f - base.Camera.HalfScreenHeight) * 0.9f, base.Camera.ScreenWidth * 0.8f, base.Camera.ScreenHeight * 0.9f);
	}
}
