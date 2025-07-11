using System;
using UnityEngine;

public class MetroPopupEpicPowerup : MetroPopupPage
{
	public MetroPopupEpicPowerup Setup(EpicPowerup ep, Action onRefuse, Action onAccept)
	{
		PlayerProfile p = AutoSingleton<Player>.Instance.Profile;
		_onClose = onRefuse;
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.5f));
		string empty = string.Empty;
		switch (p.EpicPowerupStreak)
		{
		case 1:
			empty = "YOU'RE ON A ROLL!";
			break;
		case 2:
			empty = "IT'S YOUR LUCKY DAY!";
			break;
		default:
			empty = "CHECK THIS OUT!";
			break;
		}
		metroLayout.Add(MetroLabel.Create(empty));
		metroLayout.Add(MetroSpacer.Create());
		metroLayout.Add(MetroIcon.Create(ep.GetIconPath()));
		metroLayout.Add(MetroSpacer.Create());
		string content = ep.GetDescription(p.EpicPowerupStreak).Wrap(40);
		MetroLabel metroLabel = MetroLabel.Create(content);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetLineSpacing(0f);
		metroLayout.Add(metroLabel);
		metroLayout.Add(MetroSpacer.Create());
		Price price = AutoSingleton<EpicPowerupManager>.Instance.GetEpicPowerupPrice(ep);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(2f);
		metroLayout.Add(metroLayout2);
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		MetroButton metroButton = MetroButton.Create();
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			if (AutoSingleton<EpicPowerupManager>.Instance.GetCurrentEpicPowerUp() == null)
			{
				if (AutoSingleton<CashManager>.Instance.CanBuy(price))
				{
					AutoSingleton<CashManager>.Instance.Buy(price);
					p.SetNextEpicPowerup(ep.GetEpicPowerupType());
					p.SetEpicPowerupStreak(p.EpicPowerupStreak + 1);
					onAccept();
				}
				else
				{
					AutoSingleton<CashManager>.Instance.HandleNotEnoughCurrency(price, pauseOnFocus: false);
				}
			}
		};
		metroLayout2.Add(metroButton);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		metroLayout3.Add(MetroLabel.Create("PURCHASE").SetFont(MetroSkin.MediumFont).SetMass(1.2f));
		metroLayout3.Add(MetroWidgetPrice.Create(price, MetroSkin.SmallFont));
		metroButton.Add(metroLayout3);
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroSpacer.Create(0.5f));
		return this;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.8f, (0f - base.Camera.HalfScreenHeight) * 0.9f, base.Camera.ScreenWidth * 0.8f, base.Camera.ScreenHeight * 0.9f);
	}
}
