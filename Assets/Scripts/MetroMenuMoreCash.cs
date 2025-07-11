using System;
using System.Collections.Generic;
using UnityEngine;

public class MetroMenuMoreCash : MetroMenuPage
{
	private MetroLabel _messageLabel;

	public MetroMenuPage Setup(Price price)
	{
		return Setup(price.Currency);
	}

	public MetroMenuPage Setup(Currency currency)
	{
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		Add(metroLayout);
		MetroStatusBar metroStatusBar = MetroStatusBar.Create();
		metroStatusBar.Bucks.SetActive(active: false);
		metroStatusBar.Coins.SetActive(active: false);
		metroLayout.Add(metroStatusBar);
		MetroWidget metroWidget = MetroLayout.Create(Direction.vertical).SetMass(9f);
		metroLayout.Add(metroWidget);
		MetroLabel metroLabel = MetroLabel.Create("PURCHASE MORE".Localize() + " " + currency.ToString().ToUpper().Localize());
		metroLabel.SetFont(MetroSkin.BigFont);
		metroWidget.Add(metroLabel);
		if (AutoSingleton<PlatformCapabilities>.Instance.IsCurrencyPurchaseSupported())
		{
			MetroWidget metroWidget2 = MetroLayout.Create(Direction.vertical).SetMass(2f);
			metroWidget.Add(metroWidget2);
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
			metroWidget2.Add(metroLayout2);
			_messageLabel = MetroLabel.Create(string.Empty);
			_messageLabel.SetMass(3f);
			if (currency == Currency.bucks && AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported())
			{
				CreateFacebookFreeOfferButton(metroLayout2);
			}
			List<PurchaseManager.PurchasedCurrency> purchasesByCurrency = AutoSingleton<PurchaseManager>.Instance.GetPurchasesByCurrency(currency);
			foreach (PurchaseManager.PurchasedCurrency item in purchasesByCurrency)
			{
				PurchaseManager.PurchasedCurrency p = item;
				MetroButton metroButton = MetroButton.Create();
				metroButton.AddSlice9Background(MetroSkin.Slice9Button);
				metroLayout2.Add(metroButton);
				MetroMenu.AddKeyNavigationBehaviour(metroButton, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
				MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
				metroButton.Add(metroLayout3);
				MetroIcon metroIcon = MetroIcon.Create(p.Icon);
				metroIcon.SetMass(2f);
				metroLayout3.Add(metroIcon);
				MetroWidgetPrice child = MetroWidgetPrice.Create(new Price(p.Amount + p.Bonus, p.Currency));
				metroLayout3.Add(child);
				if (p.Bonus > 0)
				{
					MetroLabel metroLabel2 = MetroLabel.Create(Mathf.RoundToInt(100f * (float)p.Bonus / (float)p.Amount).ToString() + "% BONUS!");
					metroLabel2.SetFont(MetroSkin.SmallFont);
					metroLabel2.SetColor(Color.yellow);
					metroLabel2.SetMass(0.5f);
					metroLayout3.Add(metroLabel2);
				}
				else
				{
					metroLayout3.Add(MetroSpacer.Create(0.5f));
				}
				MetroLabel child2 = MetroLabel.Create(AutoSingleton<PurchaseManager>.Instance.GetPurchasePriceString(p));
				metroLayout3.Add(child2);
				metroButton.OnButtonClicked += delegate
				{
					AutoSingleton<PurchaseManager>.Instance.Buy(p);
				};
			}
			AutoSingleton<PurchaseManager>.Instance.RefreshProductDataIfNeeded();
		}
		else
		{
			metroWidget.Add(MetroSpacer.Create(1f));
			_messageLabel = MetroLabel.Create("You don't have enough of the required currency\nto purchase this item.\n\nPurchasing currency is currently not\navailable on this platform.");
			_messageLabel.SetMass(3f);
			metroWidget.Add(_messageLabel);
			metroWidget.Add(MetroSpacer.Create(1f));
		}
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroWidget.Add(metroLayout4);
		MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconBack, "BACK");
		metroButton2.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.slideUp);
		};
		metroLayout4.Add(metroButton2);
		if (AutoSingleton<PlatformCapabilities>.Instance.IsCurrencyPurchaseSupported())
		{
			metroLayout4.Add(_messageLabel);
			metroLayout4.Add(MetroSpacer.Create());
		}
		if (GameTapjoyManager.IsSupported())
		{
			MetroWidget offerWallButton = GameAdProvider.GetOfferWallButton();
			if (offerWallButton != null)
			{
				metroLayout4.Add(offerWallButton);
			}
		}
		return this;
	}

	private void CreateFacebookFreeOfferButton(MetroWidget parent)
	{
		if (!AutoSingleton<PersistenceManager>.Instance.FacebookRewardCollected)
		{
			MetroButton button = MetroButton.Create();
			button.AddSlice9Background(MetroSkin.Slice9Button);
			parent.Add(button);
			MetroMenu.AddKeyNavigationBehaviour(button, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
			MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
			button.Add(metroLayout);
			MetroIcon metroIcon = MetroIcon.Create(MetroSkin.IconBucks);
			metroIcon.SetMass(2f);
			metroLayout.Add(metroIcon);
			MetroWidgetPrice child = MetroWidgetPrice.Create(new Price(5, Currency.bucks));
			metroLayout.Add(child);
			metroLayout.Add(MetroSpacer.Create(0.5f));
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
			metroLayout.Add(metroLayout2);
			metroLayout2.Add(MetroSpacer.Create(0.4f));
			metroLayout2.Add(MetroIcon.Create(MetroSkin.IconFacebook).SetScale(0.36f));
			MetroLabel child2 = MetroLabel.Create("VISIT").SetFont(MetroSkin.SmallFont);
			metroLayout2.Add(child2);
			metroLayout2.Add(MetroSpacer.Create(0.4f));
			button.OnButtonClicked += delegate
			{
				Application.OpenURL(FacebookSetting.FacebookGamePageUrl);
				if (button != null)
				{
					button.Destroy();
					parent.Reflow();
				}
				GiveReward();
			};
		}
	}

	private void GiveReward()
	{
		if (!AutoSingleton<PersistenceManager>.Instance.FacebookRewardCollected)
		{
			AutoSingleton<PersistenceManager>.Instance.FacebookRewardCollected = true;
			List<Reward> list = new List<Reward>();
			Reward reward = new Reward(RewardType.bucks, 5);
			list.Add(reward);
			Action onDismiss = delegate
			{
				reward.Activate();
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
			MetroPopupRewards page = MetroMenuPage.Create<MetroPopupRewards>().Setup("THANKS FOR PASSING BY!", "Here's a gift for you :)", list, onDismiss);
			AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
		}
	}

	protected override void OnMenuUpdate()
	{
		base.OnMenuUpdate();
		if (IsActive() && (GameTrialPayManager.IsSupported() || GameTapjoyManager.IsSupported()))
		{
			ProcessMessageGate.DisplayMessage(this);
		}
	}
}
