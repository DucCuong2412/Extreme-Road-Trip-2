public class MetroPopupUnlockCar : MetroPopupPage
{
	private Car _car;

	private CarProfile _profile;

	private MetroLabel _description;

	private MetroWidgetPrice _priceWidget;

	private MetroButton _yesButton;

	private MetroButton _refreshPromoDataButton;

	public MetroPopupUnlockCar Setup(Car car)
	{
		_car = car;
		_profile = AutoSingleton<CarManager>.Instance.GetCarProfile(_car);
		return this;
	}

	private void RefreshPM2Label(Car car)
	{
		if (_car.Category == CarCategory.pocketMine && car.Id == _car.Id && _description != null && !_profile.IsUnlocked() && AutoSingleton<PocketMine2PromoManager>.Instance.IsMissionCompleted(_car))
		{
			_description.SetText("Pocket Mine mission completed!");
			_yesButton.SetText("PLAY");
		}
	}

	private void RefreshPM2PriceLabel(Car car, Price price)
	{
		if (_car.Category != CarCategory.pocketMine || !(car.Id == _car.Id) || !(_priceWidget != null) || _profile.IsUnlocked())
		{
			return;
		}
		if (price.Amount > 0)
		{
			MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(price).SetIconScale(0.6f).SetFont(MetroSkin.BigFont);
			_priceWidget.Replace(metroWidgetPrice).Destroy();
			_priceWidget = metroWidgetPrice;
			_priceWidget.Reflow();
			if (!AutoSingleton<PocketMine2PromoManager>.Instance.IsMissionCompleted(_car))
			{
				RefreshLockedPM2Description(price);
			}
		}
		else
		{
			_priceWidget.Remove().Destroy();
			_priceWidget = null;
		}
	}

	private void RefreshFishingLabel(Car car)
	{
		if (_car.Category == CarCategory.fishingBreak && car.Id == _car.Id && _description != null && !_profile.IsUnlocked() && AutoSingleton<FishingBreakPromoManager>.Instance.IsMissionCompleted(_car))
		{
			_description.SetText("Fishing Break! mission completed!");
			_yesButton.SetText("PLAY");
		}
	}

	private void RefreshFishingPriceLabel(Car car, Price price)
	{
		if (_car.Category != CarCategory.fishingBreak || !(car.Id == _car.Id) || !(_priceWidget != null) || _profile.IsUnlocked())
		{
			return;
		}
		if (price.Amount > 0)
		{
			MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(price).SetIconScale(0.6f).SetFont(MetroSkin.BigFont);
			_priceWidget.Replace(metroWidgetPrice).Destroy();
			_priceWidget = metroWidgetPrice;
			_priceWidget.Reflow();
			if (!AutoSingleton<FishingBreakPromoManager>.Instance.IsMissionCompleted(_car))
			{
				RefreshLockedFishingDescription(price);
			}
		}
		else
		{
			_priceWidget.Remove().Destroy();
			_priceWidget = null;
		}
	}

	private void RefreshPM3Label(Car car)
	{
		if (_car.Category == CarCategory.pocketMine3 && car.Id == _car.Id && _description != null && !_profile.IsUnlocked() && AutoSingleton<PocketMine3PromoManager>.Instance.IsMissionCompleted(_car))
		{
			_description.SetText("Pocket Mine 3 mission completed!");
			_yesButton.SetText("PLAY");
			MetroWidget parent = _refreshPromoDataButton.Parent;
			_refreshPromoDataButton.Remove().Destroy();
			_refreshPromoDataButton = null;
			parent.Reflow();
		}
	}

	private void RefreshPM3PriceLabel(Car car, Price price)
	{
		if (_car.Category != CarCategory.pocketMine3 || !(car.Id == _car.Id) || !(_priceWidget != null) || _profile.IsUnlocked())
		{
			return;
		}
		if (price.Amount > 0)
		{
			MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(price).SetIconScale(0.6f).SetFont(MetroSkin.BigFont);
			_priceWidget.Replace(metroWidgetPrice).Destroy();
			_priceWidget = metroWidgetPrice;
			_priceWidget.Reflow();
			if (!AutoSingleton<PocketMine3PromoManager>.Instance.IsMissionCompleted(_car))
			{
				RefreshLockedPM3Description(price);
			}
		}
		else
		{
			_priceWidget.Remove().Destroy();
			_priceWidget = null;
		}
	}

	private void RefreshLockedPM3Description(Price price)
	{
		int progressLeft = AutoSingleton<PocketMine3PromoManager>.Instance.GetProgressLeft(_car);
		price = new Price(progressLeft, price.Currency);
		string format = "Kill {0} more {1} in Pocket Mine 3.".Localize();
		string text = string.Format(format, progressLeft, price.GetCurrencyDisplayName());
		_description.SetText(text);
	}

	private void RefreshLockedPM2Description(Price price)
	{
		int progressLeft = AutoSingleton<PocketMine2PromoManager>.Instance.GetProgressLeft(_car);
		price = new Price(progressLeft, price.Currency);
		string format = "Collect {0} more {1} in Pocket Mine 2.".Localize();
		string text = string.Format(format, progressLeft, price.GetCurrencyDisplayName());
		_description.SetText(text);
	}

	private void RefreshLockedFishingDescription(Price price)
	{
		int progressLeft = AutoSingleton<FishingBreakPromoManager>.Instance.GetProgressLeft(_car);
		price = new Price(progressLeft, price.Currency);
		string format = "Catch {0} more {1} in Fishing Break!".Localize();
		string text = string.Format(format, progressLeft, price.GetCurrencyDisplayName());
		_description.SetText(text);
	}

	private void RefreshPRTLabel(Car car)
	{
		if (_car.Category == CarCategory.prt && car.Id == _car.Id && _description != null && !_profile.IsUnlocked() && AutoSingleton<PRTPromoManager>.Instance.IsMissionCompleted(_car))
		{
			_description.SetText("Pocket Road Trip mission completed!");
			_yesButton.SetText("PLAY");
		}
	}

	public override void OnDestroy()
	{
		PocketMine2PromoManager.CarMissionCompletedEvent -= RefreshPM2Label;
		PocketMine2PromoManager.CarPriceUpdatedEvent -= RefreshPM2PriceLabel;
		FishingBreakPromoManager.CarMissionCompletedEvent -= RefreshFishingLabel;
		FishingBreakPromoManager.CarPriceUpdatedEvent -= RefreshFishingPriceLabel;
		PocketMine3PromoManager.CarMissionCompletedEvent -= RefreshPM3Label;
		PocketMine3PromoManager.CarPriceUpdatedEvent -= RefreshPM3PriceLabel;
		PRTPromoManager.OnCarMissionCompleted -= RefreshPRTLabel;
		base.OnDestroy();
	}

	private void RegisterEvent()
	{
		if (_car.Category == CarCategory.pocketMine && !_profile.IsUnlocked())
		{
			PocketMine2PromoManager.CarMissionCompletedEvent += RefreshPM2Label;
			PocketMine2PromoManager.CarPriceUpdatedEvent += RefreshPM2PriceLabel;
		}
		if (_car.Category == CarCategory.fishingBreak && !_profile.IsUnlocked())
		{
			FishingBreakPromoManager.CarMissionCompletedEvent += RefreshFishingLabel;
			FishingBreakPromoManager.CarPriceUpdatedEvent += RefreshFishingPriceLabel;
		}
		if (_car.Category == CarCategory.pocketMine3 && !_profile.IsUnlocked())
		{
			PocketMine3PromoManager.CarMissionCompletedEvent += RefreshPM3Label;
			PocketMine3PromoManager.CarPriceUpdatedEvent += RefreshPM3PriceLabel;
		}
		if (_car.Category == CarCategory.prt && !_profile.IsUnlocked())
		{
			PRTPromoManager.OnCarMissionCompleted += RefreshPRTLabel;
		}
	}

	protected override void OnStart()
	{
		RegisterEvent();
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel metroLabel = MetroLabel.Create("UNLOCK".Localize() + " " + _car.DisplayName);
		metroLabel.SetFont((!Device.IsIPad()) ? MetroSkin.BigFont : MetroSkin.DefaultFont);
		metroLayout2.Add(metroLabel);
		MetroIcon metroIcon = MetroIcon.Create(_car, asPrefab: true);
		metroIcon.gameObject.AddComponent<CarAnimator>();
		metroIcon.SetScale(2f);
		metroIcon.SetMass(2f);
		metroLayout.Add(metroIcon);
		_description = MetroLabel.Create(_car.Description);
		_description.SetFont(MetroSkin.MediumFont);
		metroLayout.Add(_description);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout3);
		metroLayout3.Add(MetroSpacer.Create());
		Price price = _car.Price;
		float iconScale = 0.6f;
		if (_car.Category == CarCategory.pocketMine)
		{
			int progressLeft = AutoSingleton<PocketMine2PromoManager>.Instance.GetProgressLeft(_car);
			price = new Price(progressLeft, price.Currency);
			if (!AutoSingleton<PocketMine2PromoManager>.Instance.IsMissionCompleted(_car))
			{
				RefreshLockedPM2Description(price);
			}
			else
			{
				_description.SetText("Pocket Mine 2 mission completed!");
			}
		}
		else if (_car.Category == CarCategory.pocketMine3)
		{
			int progressLeft2 = AutoSingleton<PocketMine3PromoManager>.Instance.GetProgressLeft(_car);
			price = new Price(progressLeft2, price.Currency);
			if (!AutoSingleton<PocketMine3PromoManager>.Instance.IsMissionCompleted(_car))
			{
				RefreshLockedPM3Description(price);
			}
			else
			{
				_description.SetText("Pocket Mine 3 mission completed!");
			}
		}
		else if (_car.Category == CarCategory.prt)
		{
			string empty = string.Empty;
			if (!AutoSingleton<PRTPromoManager>.Instance.IsMissionCompleted(_car))
			{
				int progressLeft3 = AutoSingleton<PRTPromoManager>.Instance.GetProgressLeft(_car);
				string pRTCarCategoryFromId = AutoSingleton<PRTPromoManager>.Instance.GetPRTCarCategoryFromId(_car.Id);
				string format = "Travel {0} more meters with a {1} car in Pocket Road Trip.".Localize().Wrap(40);
				empty = string.Format(format, progressLeft3, pRTCarCategoryFromId);
			}
			else
			{
				empty = "Pocket Road Trip mission completed!";
			}
			_description.SetText(empty);
		}
		else if (_car.Category == CarCategory.fishingBreak)
		{
			iconScale = 1f;
			int progressLeft4 = AutoSingleton<FishingBreakPromoManager>.Instance.GetProgressLeft(_car);
			price = new Price(progressLeft4, price.Currency);
			if (!AutoSingleton<FishingBreakPromoManager>.Instance.IsMissionCompleted(_car))
			{
				RefreshLockedFishingDescription(price);
			}
			else
			{
				_description.SetText("Fishing Break! mission completed!");
			}
		}
		if (price.Amount > 0)
		{
			_priceWidget = MetroWidgetPrice.Create(price).SetIconScale(iconScale).SetFont(MetroSkin.BigFont);
			metroLayout3.Add(_priceWidget);
		}
		metroLayout3.Add(MetroSpacer.Create());
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		metroLayout4.Add(MetroSpacer.Create(0.1f));
		string label = "PURCHASE";
		if (_car.Category == CarCategory.pocketMine)
		{
			bool flag = AutoSingleton<PocketMine2PromoManager>.Instance.IsMissionCompleted(_car);
			label = ((!flag) ? "PLAY POCKET MINE 2" : "PLAY");
			if (!flag)
			{
				_refreshPromoDataButton = MetroButton.Create("REFRESH");
				_refreshPromoDataButton.AddSlice9Background(MetroSkin.Slice9ButtonBlue);
				_refreshPromoDataButton.OnButtonClicked += delegate
				{
					AutoSingleton<PocketMine2PromoManager>.Instance.LaunchOrStoreRedirect();
				};
			}
		}
		else if (_car.Category == CarCategory.prt)
		{
			label = ((!AutoSingleton<PRTPromoManager>.Instance.IsMissionCompleted(_car)) ? "PLAY PRT NOW" : "PLAY");
		}
		else if (_car.Category == CarCategory.fishingBreak)
		{
			bool flag2 = AutoSingleton<FishingBreakPromoManager>.Instance.IsMissionCompleted(_car);
			label = ((!flag2) ? "PLAY FISHING BREAK" : "PLAY");
			if (!flag2)
			{
				_refreshPromoDataButton = MetroButton.Create("REFRESH");
				_refreshPromoDataButton.AddSlice9Background(MetroSkin.Slice9ButtonBlue);
				_refreshPromoDataButton.OnButtonClicked += delegate
				{
					AutoSingleton<FishingBreakPromoManager>.Instance.LaunchOrStoreRedirect();
				};
			}
		}
		else if (_car.Category == CarCategory.pocketMine3)
		{
			bool flag3 = AutoSingleton<PocketMine3PromoManager>.Instance.IsMissionCompleted(_car);
			label = ((!flag3) ? "PLAY POCKET MINE 3" : "PLAY");
			if (!flag3)
			{
				_refreshPromoDataButton = MetroButton.Create("REFRESH");
				_refreshPromoDataButton.AddSlice9Background(MetroSkin.Slice9ButtonBlue);
				_refreshPromoDataButton.OnButtonClicked += delegate
				{
					AutoSingleton<PocketMine3PromoManager>.Instance.LaunchOrStoreRedirect();
				};
			}
		}
		_yesButton = MetroButton.Create(label);
		_yesButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		_yesButton.OnButtonClicked += delegate
		{
			if (_car.Category == CarCategory.pocketMine)
			{
				if (AutoSingleton<PocketMine2PromoManager>.Instance.IsMissionCompleted(_car))
				{
					AutoSingleton<PocketMine2PromoManager>.Instance.UnlockPromoCar(_car);
					AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(_car));
				}
				else
				{
					AutoSingleton<ExternalAppLauncher>.Instance.LaunchOrStoreRedirect("pocketMine2");
				}
			}
			else if (_car.Category == CarCategory.prt)
			{
				if (AutoSingleton<PRTPromoManager>.Instance.IsMissionCompleted(_car))
				{
					AutoSingleton<PRTPromoManager>.Instance.UnlockPromoCar(_car);
					AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(_car));
				}
				else
				{
					AutoSingleton<ExternalAppLauncher>.Instance.LaunchOrStoreRedirect("prt");
				}
			}
			else if (_car.Category == CarCategory.fishingBreak)
			{
				if (AutoSingleton<FishingBreakPromoManager>.Instance.IsMissionCompleted(_car))
				{
					AutoSingleton<FishingBreakPromoManager>.Instance.UnlockPromoCar(_car);
					AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(_car));
				}
				else
				{
					AutoSingleton<ExternalAppLauncher>.Instance.LaunchOrStoreRedirect("fishing");
				}
			}
			else if (_car.Category == CarCategory.pocketMine3)
			{
				if (AutoSingleton<PocketMine3PromoManager>.Instance.IsMissionCompleted(_car))
				{
					AutoSingleton<PocketMine3PromoManager>.Instance.UnlockPromoCar(_car);
					AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(_car));
				}
				else
				{
					AutoSingleton<ExternalAppLauncher>.Instance.LaunchOrStoreRedirect("pocketmine3");
				}
			}
			else
			{
				TryBuyCar();
			}
		};
		metroLayout4.Add(_yesButton);
		if (_refreshPromoDataButton != null)
		{
			_yesButton.SetMass(3f);
			metroLayout4.Add(_refreshPromoDataButton);
		}
		metroLayout4.Add(MetroSpacer.Create(0.1f));
		metroLayout.Add(MetroSpacer.Create(0.2f));
		base.OnStart();
	}

	private void TryBuyCar()
	{
		if (!_profile.TryUnlock(_car.Prices))
		{
			AutoSingleton<CashManager>.Instance.HandleNotEnoughCurrency(_car.Prices, pauseOnFocus: false);
		}
		else
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(_car));
		}
		AutoSingleton<CarManager>.Instance.SaveCarProfile(_car, _profile);
	}
}
