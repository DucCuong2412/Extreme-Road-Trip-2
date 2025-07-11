using System;

public class MetroPopupInterstitial : MetroPopupPage
{
	public MetroPopupInterstitial Setup(Action onAccept, Action onDecline)
	{
		_onClose = (Action)Delegate.Combine(_onClose, (Action)delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop();
			onDecline();
		});
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.5f));
		string content = "Watch a video and earn a free crate";
		metroLayout.Add(MetroLabel.Create(content));
		metroLayout.Add(MetroSpacer.Create());
		metroLayout.Add(MetroIcon.Create(MetroSkin.IconInterstitial));
		metroLayout.Add(MetroSpacer.Create());
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		metroLayout2.SetMass(1.3f);
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		MetroButton metroButton = MetroButton.Create("Watch now");
		metroLayout2.Add(metroButton);
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop();
			onAccept();
		};
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroSpacer.Create(0.6f));
		return this;
	}
}
