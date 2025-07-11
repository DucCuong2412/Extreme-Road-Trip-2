using UnityEngine;

public class MetroButtonCarCard : MetroButton
{
	private Car _car;

	private CarProfile _profile;

	private MetroIcon _carIcon;

	private MetroIcon _lockIcon;

	private MetroLayout _right;

	private MetroWidgetCarUpgrade _upgradeWidget;

	private MetroLabel _lockedLabel;

	private MetroLayout _layout;

	private MetroWidgetPrice _priceWidget;

	private bool _unlocked;

	public Car GetCar()
	{
		return _car;
	}

	public static MetroButtonCarCard Create(Car car, float scale)
	{
		GameObject gameObject = new GameObject(car.Id);
		gameObject.transform.position = Vector3.zero;
		MetroButtonCarCard metroButtonCarCard = gameObject.AddComponent<MetroButtonCarCard>();
		metroButtonCarCard.Setup(car, scale);
		metroButtonCarCard.IsKeyNavigatorAccessible = true;
		return metroButtonCarCard;
	}

	public void SwitchSlice9Background(string slice9)
	{
		_layout.AddSlice9Background(slice9);
	}

	private void RefreshPM2LockLabel(Car car)
	{
		if (car.Id == _car.Id && _lockedLabel != null && !_profile.IsUnlocked() && AutoSingleton<PocketMine2PromoManager>.Instance.IsMissionCompleted(_car))
		{
			_lockedLabel.SetText("CLAIM!");
			_lockedLabel.SetColor(Color.green);
			Reflow();
		}
	}

	private void RefreshPM2PriceLabel(Car car, Price price)
	{
		if (_car.Category == CarCategory.pocketMine && car.Id == _car.Id && _priceWidget != null && !_profile.IsUnlocked())
		{
			if (price.Amount > 0)
			{
				MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(price);
				metroWidgetPrice.SetIconScale(0.3f).SetFont(MetroSkin.SmallFont).SetTextColor(MetroSkin.DarkColor);
				_priceWidget.Replace(metroWidgetPrice).Destroy();
				_priceWidget = metroWidgetPrice;
				_priceWidget.Reflow();
			}
			else
			{
				_priceWidget.Remove().Destroy();
				_priceWidget = null;
			}
		}
	}

	private void RefreshFishingLockLabel(Car car)
	{
		if (car.Id == _car.Id && _lockedLabel != null && !_profile.IsUnlocked() && AutoSingleton<FishingBreakPromoManager>.Instance.IsMissionCompleted(_car))
		{
			_lockedLabel.SetText("CLAIM!");
			_lockedLabel.SetColor(Color.green);
			Reflow();
		}
	}

	private void RefreshFishingPriceLabel(Car car, Price price)
	{
		if (_car.Category == CarCategory.fishingBreak && car.Id == _car.Id && _priceWidget != null && !_profile.IsUnlocked())
		{
			if (price.Amount > 0)
			{
				MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(price);
				metroWidgetPrice.SetIconScale(0.3f).SetFont(MetroSkin.SmallFont).SetTextColor(MetroSkin.DarkColor);
				_priceWidget.Replace(metroWidgetPrice).Destroy();
				_priceWidget = metroWidgetPrice;
				_priceWidget.Reflow();
			}
			else
			{
				_priceWidget.Remove().Destroy();
				_priceWidget = null;
			}
		}
	}

	private void RefreshPocketMine3LockLabel(Car car)
	{
		if (car.Id == _car.Id && _lockedLabel != null && !_profile.IsUnlocked() && AutoSingleton<PocketMine3PromoManager>.Instance.IsMissionCompleted(_car))
		{
			_lockedLabel.SetText("CLAIM!");
			_lockedLabel.SetColor(Color.green);
			Reflow();
		}
	}

	private void RefreshPM3PriceLabel(Car car, Price price)
	{
		if (_car.Category == CarCategory.pocketMine3 && car.Id == _car.Id && _priceWidget != null && !_profile.IsUnlocked())
		{
			if (price.Amount > 0)
			{
				MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(price);
				metroWidgetPrice.SetIconScale(0.3f).SetFont(MetroSkin.SmallFont).SetTextColor(MetroSkin.DarkColor);
				_priceWidget.Replace(metroWidgetPrice).Destroy();
				_priceWidget = metroWidgetPrice;
				_priceWidget.Reflow();
			}
			else
			{
				_priceWidget.Remove().Destroy();
				_priceWidget = null;
			}
		}
	}

	private void RefreshPRTLockLabel(Car car)
	{
		if (car.Id == _car.Id && _lockedLabel != null && !_profile.IsUnlocked() && AutoSingleton<PRTPromoManager>.Instance.IsMissionCompleted(_car))
		{
			_lockedLabel.SetText("CLAIM!");
			_lockedLabel.SetColor(Color.green);
			Reflow();
		}
	}

	public void OnDestroy()
	{
		FishingBreakPromoManager.CarMissionCompletedEvent -= RefreshFishingLockLabel;
		FishingBreakPromoManager.CarPriceUpdatedEvent -= RefreshFishingPriceLabel;
		PocketMine2PromoManager.CarMissionCompletedEvent -= RefreshPM2LockLabel;
		PocketMine2PromoManager.CarPriceUpdatedEvent -= RefreshPM2PriceLabel;
		PRTPromoManager.OnCarMissionCompleted -= RefreshPRTLockLabel;
		PocketMine3PromoManager.CarMissionCompletedEvent -= RefreshPocketMine3LockLabel;
		PocketMine3PromoManager.CarPriceUpdatedEvent -= RefreshPM3PriceLabel;
	}

	private void RegisterEvent()
	{
		if (_car.Category == CarCategory.pocketMine && !_profile.IsUnlocked())
		{
			PocketMine2PromoManager.CarMissionCompletedEvent += RefreshPM2LockLabel;
			PocketMine2PromoManager.CarPriceUpdatedEvent += RefreshPM2PriceLabel;
		}
		if (_car.Category == CarCategory.fishingBreak && !_profile.IsUnlocked())
		{
			FishingBreakPromoManager.CarMissionCompletedEvent += RefreshFishingLockLabel;
			FishingBreakPromoManager.CarPriceUpdatedEvent += RefreshFishingPriceLabel;
		}
		if (_car.Category == CarCategory.pocketMine3 && !_profile.IsUnlocked())
		{
			PocketMine3PromoManager.CarMissionCompletedEvent += RefreshPocketMine3LockLabel;
			PocketMine3PromoManager.CarPriceUpdatedEvent += RefreshPM3PriceLabel;
		}
		if (_car.Category == CarCategory.prt && !_profile.IsUnlocked())
		{
			PRTPromoManager.OnCarMissionCompleted += RefreshPRTLockLabel;
		}
	}

	private void Setup(Car car, float scale)
	{
		_car = car;
		_profile = AutoSingleton<CarManager>.Instance.GetCarProfile(_car);
		RegisterEvent();
		int value = AutoSingleton<GameStatsManager>.Instance.Overall.GetValue(_car, GameStats.CarStats.Type.maxDistance);
		_layout = MetroLayout.Create(_car.Id + "CardLayout", Direction.vertical);
		string carCardBackground = GetCarCardBackground(car.Category);
		if (carCardBackground != string.Empty)
		{
			_layout.AddSlice9Background(carCardBackground);
		}
		Add(_layout);
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		_layout.Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLabel metroLabel = MetroLabel.Create($"#{_car.Rank:000}");
		metroLabel.SetFont(MetroSkin.SmallFont);
		metroLabel.SetColor((car.Category != CarCategory.prestige) ? Color.black : Color.white);
		metroLayout.Add(metroLabel);
		MetroLabel metroLabel2 = MetroLabel.Create(_car.DisplayName);
		metroLabel2.SetMass(4f);
		metroLabel2.SetFont(MetroSkin.SmallFont);
		metroLabel2.SetColor((car.Category != CarCategory.prestige) ? Color.black : Color.white);
		metroLayout.Add(metroLabel2);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(4f);
		_layout.Add(metroLayout2);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		if (_profile.IsUnlocked())
		{
			metroLayout3.Add(MetroIcon.Create((value < 10000) ? MetroSkin.SpriteCardMedalLocked : MetroSkin.SpriteCardMedalGold));
			metroLayout3.Add(MetroIcon.Create((value < 5000) ? MetroSkin.SpriteCardMedalLocked : MetroSkin.SpriteCardMedalSilver));
			metroLayout3.Add(MetroIcon.Create((value < 2000) ? MetroSkin.SpriteCardMedalLocked : MetroSkin.SpriteCardMedalBronze));
			metroLayout3.Add(MetroSpacer.Create());
		}
		metroLayout2.Add(metroLayout3);
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
		metroLayout4.SetMass(2f);
		metroLayout2.Add(metroLayout4);
		_carIcon = MetroIcon.Create(_car);
		_carIcon.SetMass(4f);
		_carIcon.SetScale(scale);
		_carIcon.SetAlignment(MetroAlign.Bottom);
		SetCarIconColor(Color.black);
		metroLayout4.Add(_carIcon);
		float iconScale = 0.3f;
		Price price = _car.Price;
		if (_profile.IsUnlocked())
		{
			MetroLabel metroLabel3 = MetroLabel.Create("· " + value + "m ·");
			metroLabel3.SetFont(MetroSkin.SmallFont);
			metroLabel3.SetColor(Color.black);
			metroLayout4.Add(metroLabel3);
			_upgradeWidget = MetroWidgetCarUpgrade.Create(_profile.GetUpgradeLevel(), 0.55f);
			metroLayout4.Add(_upgradeWidget);
		}
		else
		{
			string content = "UNLOCK";
			Color color = Color.red;
			if (car.Category == CarCategory.pocketMine)
			{
				int progressLeft = AutoSingleton<PocketMine2PromoManager>.Instance.GetProgressLeft(_car);
				price = new Price(progressLeft, price.Currency);
				if (AutoSingleton<PocketMine2PromoManager>.Instance.IsMissionCompleted(_car))
				{
					content = "CLAIM!";
					color = Color.green;
				}
			}
			else if (car.Category == CarCategory.pocketMine3)
			{
				int progressLeft2 = AutoSingleton<PocketMine3PromoManager>.Instance.GetProgressLeft(_car);
				price = new Price(progressLeft2, price.Currency);
				if (AutoSingleton<PocketMine3PromoManager>.Instance.IsMissionCompleted(_car))
				{
					content = "CLAIM!";
					color = Color.green;
				}
			}
			else if (car.Category == CarCategory.prt)
			{
				if (AutoSingleton<PRTPromoManager>.Instance.IsMissionCompleted(_car))
				{
					content = "CLAIM!";
					color = Color.green;
				}
			}
			else if (car.Category == CarCategory.fishingBreak)
			{
				int progressLeft3 = AutoSingleton<FishingBreakPromoManager>.Instance.GetProgressLeft(_car);
				price = new Price(progressLeft3, price.Currency);
				iconScale = 0.45f;
				if (AutoSingleton<FishingBreakPromoManager>.Instance.IsMissionCompleted(_car))
				{
					content = "CLAIM!";
					color = Color.green;
				}
			}
			_lockedLabel = MetroLabel.Create(content);
			_lockedLabel.SetFont(MetroSkin.SmallFont);
			_lockedLabel.SetColor(color);
			_lockedLabel.AddOutline();
			metroLayout4.Add(_lockedLabel);
			MetroLayout metroLayout5 = MetroLayout.Create(Direction.horizontal);
			metroLayout4.Add(metroLayout5);
			if (price.Amount > 0)
			{
				_priceWidget = MetroWidgetPrice.Create(price);
				_priceWidget.SetIconScale(iconScale).SetFont(MetroSkin.SmallFont).SetTextColor(MetroSkin.DarkColor);
				metroLayout5.Add(_priceWidget);
			}
		}
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		_right = MetroLayout.Create(Direction.vertical);
		metroLayout2.Add(_right);
		UpdatePrestigeBadges();
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal);
		Add(metroWidget);
		MetroWidget metroWidget2 = MetroLayout.Create(Direction.vertical);
		MetroWidget child = MetroSpacer.Create(0.025f);
		MetroWidget child2 = MetroSpacer.Create(0.025f);
		metroWidget.Add(child);
		metroWidget.Add(metroWidget2);
		metroWidget2.Add(_layout);
		metroWidget.Add(child2);
		PollUnlocked();
	}

	private void AddPrestigeBadge(MetroLayout right, string badge, bool showCurrentMissionPercentage)
	{
		MetroIcon metroIcon = MetroIcon.Create(badge);
		metroIcon.SetMass(1.5f);
		right.Add(metroIcon);
		if (showCurrentMissionPercentage)
		{
			float num = (float)AutoSingleton<MissionManager>.Instance.GetCompletedMissionCount(_car) / (float)AutoSingleton<MissionManager>.Instance.GetMissionCount(_car);
			MetroPie metroPie = metroIcon.GetComponentInChildren<MetroPie>().Setup(num);
			MetroFont verySmallFont = MetroSkin.VerySmallFont;
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load(verySmallFont.FontName), Vector3.zero, Quaternion.identity);
			gameObject.layer = 8;
			gameObject.transform.parent = metroPie.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, -0.1f);
			TextMesh component = gameObject.GetComponent<TextMesh>();
			component.anchor = TextAnchor.MiddleCenter;
			component.GetComponent<Renderer>().material.color = Color.black;
			component.text = Mathf.RoundToInt(100f * num).ToString() + "%";
			if (num == 1f)
			{
				gameObject.transform.localScale *= 0.7f;
			}
			else
			{
				gameObject.transform.localScale *= 0.8f;
			}
		}
		else
		{
			MetroPie componentInChildren = metroIcon.GetComponentInChildren<MetroPie>();
			if (componentInChildren != null)
			{
				componentInChildren.Setup(0f);
			}
		}
	}

	public void OnPrestigeLevelChanged()
	{
		UpdatePrestigeBadges();
	}

	private void UpdatePrestigeBadges()
	{
		_right.Clear();
		if (_profile.IsUnlocked())
		{
			MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
			metroLayout.SetMass(4f);
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
			metroLayout2.SetMass(0.5f);
			MetroLabel metroLabel = MetroLabel.Create("Missions");
			metroLabel.SetMass(0.3f);
			metroLabel.SetFont(MetroSkin.VerySmallFont);
			metroLabel.SetColor(Color.black);
			metroLabel.SetAlignment(MetroAlign.Center);
			metroLabel.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
			metroLayout.Add(metroLabel);
			PrestigeManager instance = AutoSingleton<PrestigeManager>.Instance;
			int carPrestigeLevel = instance.GetCarPrestigeLevel(_car);
			for (int num = instance.GetPrestigeMaximumLevel(); num > 0; num--)
			{
				string badge = (carPrestigeLevel != num - 1) ? ((carPrestigeLevel >= num - 1) ? instance.GetPrestigeLevelBadge(num) : MetroSkin.SpriteCardPrestigeBadgeLocked) : MetroSkin.SpriteCardPrestigeBadgeUnlocked;
				AddPrestigeBadge(metroLayout2, badge, carPrestigeLevel == num - 1);
			}
			metroLayout.Add(metroLayout2);
			metroLayout.Add(MetroSpacer.Create(0.2f));
			_right.Add(metroLayout);
			_right.Add(MetroSpacer.Create(2f));
		}
		Reflow();
	}

	public override void OnFocus()
	{
		base.OnFocus();
		PollUnlocked();
		PollUpgrade();
	}

	private void PollUnlocked()
	{
		if (!_unlocked && _profile.IsUnlocked())
		{
			_unlocked = true;
			ShowUnlocked();
		}
	}

	private void PollUpgrade()
	{
		if (_profile.IsUnlocked() && _upgradeWidget != null)
		{
			_upgradeWidget.ShowLevel(_profile.GetUpgradeLevel());
		}
	}

	private void ShowUnlocked()
	{
		SetCarIconColor(Color.white);
	}

	private void SetCarIconColor(Color color)
	{
		tk2dSprite[] componentsInChildren = _carIcon.GetComponentsInChildren<tk2dSprite>();
		foreach (tk2dSprite tk2dSprite in componentsInChildren)
		{
			tk2dSprite.color = color;
		}
	}

	public static string GetCarCardBackground(CarCategory category)
	{
		string result = string.Empty;
		switch (category)
		{
		case CarCategory.soldForBucks:
			result = MetroSkin.Slice9CardBaseBucks;
			break;
		case CarCategory.soldForCoins:
			result = MetroSkin.Slice9CardBaseCoins;
			break;
		case CarCategory.super:
			result = MetroSkin.Slice9CardSuper;
			break;
		case CarCategory.prestige:
			result = MetroSkin.Slice9CardBasePrestige;
			break;
		case CarCategory.pocketMine:
			result = MetroSkin.Slice9CardBasePocketMine;
			break;
		case CarCategory.pocketMine3:
			result = MetroSkin.Slice9CardBasePocketMine;
			break;
		case CarCategory.fishingBreak:
			result = MetroSkin.Slice9CardBasePocketMine;
			break;
		case CarCategory.prt:
			result = MetroSkin.Slice9CardBasePRT;
			break;
		default:
			UnityEngine.Debug.LogWarning("GetCarCardBackground() haven't found a slice9 for car category: " + category.ToString());
			break;
		}
		return result;
	}

	public static string GetCarCardBackgroundSelected(CarCategory category)
	{
		string result = string.Empty;
		switch (category)
		{
		case CarCategory.soldForBucks:
			result = MetroSkin.Slice9CardBaseBucksSelected;
			break;
		case CarCategory.soldForCoins:
			result = MetroSkin.Slice9CardBaseCoinsSelected;
			break;
		case CarCategory.super:
			result = MetroSkin.Slice9CardSuperSelected;
			break;
		case CarCategory.prestige:
			result = MetroSkin.Slice9CardBasePrestigeSelected;
			break;
		case CarCategory.pocketMine:
			result = MetroSkin.Slice9CardBasePocketmineSelected;
			break;
		case CarCategory.pocketMine3:
			result = MetroSkin.Slice9CardBasePocketmineSelected;
			break;
		case CarCategory.fishingBreak:
			result = MetroSkin.Slice9CardBasePocketmineSelected;
			break;
		case CarCategory.prt:
			result = MetroSkin.Slice9CardBasePRTSelected;
			break;
		}
		return result;
	}
}
