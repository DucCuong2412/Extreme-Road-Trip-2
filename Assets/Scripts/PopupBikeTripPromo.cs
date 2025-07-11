using System;
using System.Collections;
using UnityEngine;

public class PopupBikeTripPromo : MetroPopupPage
{
	private int _closeButtonDelay;

	public PopupBikeTripPromo Setup(string matURL, int closeDelay)
	{
		_onClose = (Action)Delegate.Combine(_onClose, (Action)delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop();
		});
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.5f));
		string content = "PLAY WITH YOUR FRIENDS!";
		metroLayout.Add(MetroLabel.Create(content));
		metroLayout.Add(MetroSpacer.Create());
		MetroIcon metroIcon = MetroIcon.Create(MetroSkin.BikeTripPromoPopup);
		metroIcon.SetScale(0.98f);
		metroIcon.SetMass(6f);
		metroLayout.Add(metroIcon);
		metroLayout.Add(MetroSpacer.Create());
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		metroLayout2.SetMass(1.3f);
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		MetroButton metroButton = MetroButton.Create("PLAY NOW");
		metroLayout2.Add(metroButton);
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop();
			AutoSingleton<BikeTripCrossPromoManager>.Instance.ConsumePromo();
			Application.OpenURL(matURL);
		};
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroSpacer.Create(0.6f));
		_closeButtonDelay = closeDelay;
		return this;
	}

	public override void Layout(Rect zone)
	{
		base.Layout(zone);
		AddCloseButtonTimer(_closeButtonDelay);
	}

	private IEnumerator ShowCloseButtonCR(float time)
	{
		yield return new WaitForSeconds(time);
		_closeXButton.SetActive(active: true);
		_closeXButton.gameObject.SetActive(value: true);
	}

	public void AddCloseButtonTimer(float time)
	{
		if (time > 0f)
		{
			_closeXButton.SetActive(active: false);
			_closeXButton.gameObject.SetActive(value: false);
			StartCoroutine(ShowCloseButtonCR(time));
		}
	}
}
