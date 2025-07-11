using System;

public class MetroPopupSpecialOffer : MetroPopupPage
{
	private SpecialOffer _offer;

	private Car _car;

	private bool _hasEnoughMoney;

	public MetroPopupSpecialOffer Setup(SpecialOffer offer)
	{
		_offer = offer;
		_car = AutoSingleton<CarManager>.Instance.GetCar(_offer.Car.Id);
		_hasEnoughMoney = AutoSingleton<CashManager>.Instance.CanBuy(_offer.Price);
		return this;
	}

	protected override void OnStart()
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel metroLabel = MetroLabel.Create(_offer.Title);
		metroLabel.SetFont((!Device.IsIPad()) ? MetroSkin.BigFont : MetroSkin.DefaultFont);
		metroLayout2.Add(metroLabel);
		MetroIcon metroIcon = MetroIcon.Create(_car, asPrefab: true);
		metroIcon.gameObject.AddComponent<CarAnimator>();
		metroIcon.SetScale(2f);
		metroIcon.SetMass(2f);
		metroLayout.Add(metroIcon);
		MetroLabel child = MetroLabel.Create(_offer.Pitch);
		metroLayout.Add(child);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout3);
		MetroWidget child2 = MetroLabel.Create("Regular Price: ").SetFont(MetroSkin.MediumFont).SetAlignment(MetroAlign.Right)
			.SetMass(2f);
		metroLayout3.Add(child2);
		MetroWidget child3 = MetroWidgetPrice.Create(_car.Prices).SetIconScale(0.3f).SetFont(MetroSkin.MediumFont)
			.SetAlignment(MetroAlign.Left);
		metroLayout3.Add(child3);
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		MetroWidget child4 = MetroLabel.Create("Special Price: ").SetFont(MetroSkin.BigFont).SetAlignment(MetroAlign.Right)
			.SetMass(2f);
		metroLayout4.Add(child4);
		MetroWidget child5 = MetroWidgetPrice.Create(_offer.Price).SetIconScale(0.6f).SetFont(MetroSkin.BigFont)
			.SetAlignment(MetroAlign.Left);
		metroLayout4.Add(child5);
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		MetroLayout metroLayout5 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout5);
		metroLayout5.Add(MetroSpacer.Create(0.4f));
		MetroButton metroButton = MetroButton.Create("PURCHASE");
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			TryBuyCar();
		};
		metroLayout5.Add(metroButton);
		metroLayout5.Add(MetroSpacer.Create(0.4f));
		metroLayout.Add(MetroSpacer.Create(0.2f));
		_onClose = (Action)Delegate.Combine(_onClose, (Action)delegate
		{
			AutoSingleton<SpecialOfferManager>.Instance.ReportResolution(_offer, (!_hasEnoughMoney) ? SpecialOfferResolutionType.nomoney : SpecialOfferResolutionType.rejected);
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		});
		base.OnStart();
	}

	private void TryBuyCar()
	{
		CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(_car);
		if (!carProfile.TryUnlock(_offer.Price))
		{
			AutoSingleton<CashManager>.Instance.HandleNotEnoughCurrency(_offer.Price, pauseOnFocus: false);
		}
		else
		{
			AutoSingleton<SpecialOfferManager>.Instance.ReportResolution(_offer, (!_hasEnoughMoney) ? SpecialOfferResolutionType.converted : SpecialOfferResolutionType.redeemed);
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(_car));
		}
		AutoSingleton<CarManager>.Instance.SaveCarProfile(_car, carProfile);
	}
}
