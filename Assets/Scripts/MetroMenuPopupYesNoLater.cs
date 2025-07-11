using System;
using UnityEngine;

public class MetroMenuPopupYesNoLater : MetroPopupPage
{
	private MetroButton _yes;

	private MetroButton _no;

	private MetroButton _later;

	public MetroMenuPopupYesNoLater Setup(string title, string message, string yes, string no, string yesButtonSlice9, MetroFont msgFont = null, string later = null, int charPerLine = 40)
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel child = MetroLabel.Create(title);
		metroLayout2.Add(child);
		metroLayout.Add(MetroSpacer.Create(0.05f));
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout3.SetMass(3f);
		metroLayout.Add(metroLayout3);
		metroLayout.Add(MetroSpacer.Create(0.1f));
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		string content = message.Localize().Wrap(charPerLine);
		MetroLabel metroLabel = MetroLabel.Create(content);
		metroLayout3.Add(metroLabel);
		metroLabel.SetFont((msgFont != null) ? msgFont : MetroSkin.MediumFont);
		metroLabel.SetLineSpacing(0f);
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		metroLayout4.SetMass(1.3f);
		metroLayout4.Add(MetroSpacer.Create(0.05f));
		if (later != null)
		{
			_later = MetroButton.Create(later);
			metroLayout4.Add(_later);
			_later.AddSlice9Background(MetroSkin.Slice9Button);
			MetroMenu.AddKeyNavigationBehaviour(_later, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
		}
		metroLayout4.Add(MetroSpacer.Create(0.05f));
		_no = MetroButton.Create(no);
		MetroMenu.AddKeyNavigationBehaviour(_no, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
		metroLayout4.Add(_no);
		metroLayout4.Add(MetroSpacer.Create(0.05f));
		_no.AddSlice9Background(MetroSkin.Slice9Button);
		_yes = MetroButton.Create(yes);
		MetroMenu.AddKeyNavigationBehaviour(_yes, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
		metroLayout4.Add(_yes);
		_yes.AddSlice9Background(yesButtonSlice9);
		metroLayout4.Add(MetroSpacer.Create(0.05f));
		metroLayout.Add(MetroSpacer.Create(0.6f));
		return this;
	}

	public void OnButtonLater(Action action)
	{
		if (_later != null)
		{
			_later.OnButtonClicked += action;
		}
	}

	public void OnButtonNo(Action action)
	{
		_no.OnButtonClicked += action;
	}

	public void OnButtonYes(Action action)
	{
		_yes.OnButtonClicked += action;
	}

	public void OnClose(Action action)
	{
		_onClose = action;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.95f, (0f - base.Camera.HalfScreenHeight) * 0.9f, base.Camera.ScreenWidth * 0.95f, base.Camera.ScreenHeight * 0.9f);
	}
}
