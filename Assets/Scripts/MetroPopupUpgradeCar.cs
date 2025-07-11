using System;
using System.Collections;

public class MetroPopupUpgradeCar : MetroPopupPage
{
	private Car _car;

	private CarProfile _profile;

	private MetroBadge _badge;

	private MetroButton _prestige;

	private MetroLayout _bottom;

	private MetroSpacer _middleSpacer;

	private MetroWidgetCarUpgrade _upgradeWidget;

	private MetroWidgetPrice _priceWidget;

	private bool _animating;

	public MetroPopupUpgradeCar Setup(Car car, Action popAction)
	{
		_onClose = popAction;
		_car = car;
		_profile = AutoSingleton<CarManager>.Instance.GetCarProfile(_car);
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel metroLabel = MetroLabel.Create("UPGRADE YOUR CAR");
		metroLabel.SetFont(MetroSkin.BigFont);
		metroLayout2.Add(metroLabel);
		MetroWidget metroWidget = MetroLayout.Create(Direction.vertical).SetMass(1.5f);
		metroLayout.Add(metroWidget);
		MetroIcon carIcon = MetroIcon.Create(_car, asPrefab: true);
		carIcon.SetScale(2f);
		carIcon.SetAlignment(MetroAlign.Bottom);
		this.YieldAndExecute(delegate
		{
			carIcon.gameObject.AddComponent<CarAnimator>();
		});
		metroWidget.Add(carIcon);
		metroWidget.Add(MetroSpacer.Create(0.2f));
		_upgradeWidget = MetroWidgetCarUpgrade.Create(_profile.GetUpgradeLevel(), 1f);
		_upgradeWidget.SetMass(0.2f);
		metroWidget.Add(_upgradeWidget);
		metroWidget.Add(MetroSpacer.Create(0.2f));
		_bottom = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(_bottom);
		SetupBottom();
		metroLayout.Add(MetroSpacer.Create(0.2f));
		return this;
	}

	private MetroButton CreateCloseButton()
	{
		MetroButton metroButton = MetroButton.Create("CLOSE");
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += _onClose;
		return metroButton;
	}

	private void SetupBottom()
	{
		_bottom.Add(MetroSpacer.Create(0.5f));
		if (_profile.CanUpgrade())
		{
			MetroButton upgrade = MetroButton.Create();
			upgrade.AddSlice9Background(MetroSkin.Slice9Button);
			upgrade.OnKeyFocusGained += delegate
			{
				upgrade.AddSlice9Background(MetroSkin.Slice9ButtonRed);
				upgrade.Berp();
			};
			upgrade.OnKeyFocusLost += delegate
			{
				upgrade.AddSlice9Background(MetroSkin.Slice9Button);
			};
			MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
			upgrade.Add(metroLayout);
			metroLayout.Add(MetroSpacer.Create());
			metroLayout.Add(MetroLabel.Create("UPGRADE"));
			metroLayout.Add(MetroSpacer.Create());
			_priceWidget = MetroWidgetPrice.Create(_profile.GetUpgradePrice(_car));
			metroLayout.Add(_priceWidget);
			metroLayout.Add(MetroSpacer.Create());
			_bottom.Add(upgrade);
			_badge = MetroBadge.Create();
			upgrade.Add(_badge);
			UpdateBadge();
			upgrade.OnButtonClicked += delegate
			{
				if (_profile.CanUpgrade() && !_animating)
				{
					if (!_profile.TryUpgrade(_car))
					{
						AutoSingleton<CashManager>.Instance.HandleNotEnoughCurrency(_profile.GetUpgradePrice(_car), pauseOnFocus: false);
					}
					else
					{
						AutoSingleton<CarManager>.Instance.SaveCarProfile(_car, _profile);
						StartCoroutine(UpgradeCR(upgrade, _bottom));
					}
				}
			};
		}
		bool flag = AutoSingleton<PrestigeManager>.Instance.IsCarPrestigeAvailable(_car);
		if (flag)
		{
			_prestige = MetroButton.Create();
			_prestige.AddSlice9Background(MetroSkin.Slice9Button);
			_prestige.OnKeyFocusGained += delegate
			{
				_prestige.AddSlice9Background(MetroSkin.Slice9ButtonRed);
				_prestige.Berp();
			};
			_prestige.OnKeyFocusLost += delegate
			{
				_prestige.AddSlice9Background(MetroSkin.Slice9Button);
			};
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
			_prestige.Add(metroLayout2);
			metroLayout2.Add(MetroSpacer.Create());
			metroLayout2.Add(MetroLabel.Create("PRESTIGE"));
			metroLayout2.Add(MetroSpacer.Create());
			if (_profile.CanUpgrade())
			{
				_middleSpacer = MetroSpacer.Create(0.2f);
				_bottom.Add(_middleSpacer);
			}
			_bottom.Add(_prestige);
			_prestige.OnButtonClicked += delegate
			{
				AutoSingleton<PrestigeManager>.Instance.ShowPrestigeTokenPopupSequence(_car, OnPrestigePopupDismissed, GetType().ToString());
			};
		}
		if (!_profile.CanUpgrade() && !flag)
		{
			MetroButton child = CreateCloseButton();
			_bottom.Add(child);
		}
		_bottom.Add(MetroSpacer.Create(0.5f));
	}

	private void OnPrestigePopupDismissed()
	{
		AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		_bottom.Clear();
		SetupBottom();
		_bottom.Reflow();
	}

	protected override void OnStart()
	{
		base.OnStart();
	}

	public override void OnFocus()
	{
		base.OnFocus();
		UpdateBadge();
	}

	private IEnumerator UpgradeCR(MetroButton button, MetroLayout layout)
	{
		_animating = true;
		yield return null;
		PrefabSingleton<GameSoundManager>.Instance.PlayUpgradeSound();
		if (!_profile.CanUpgrade())
		{
			if (AutoSingleton<PrestigeManager>.Instance.IsCarPrestigeAvailable(_car))
			{
				button.Destroy();
				if (_middleSpacer != null)
				{
					_middleSpacer.Destroy();
				}
			}
			else
			{
				MetroButton close = CreateCloseButton();
				button.Replace(close).Destroy();
				button = null;
			}
		}
		else
		{
			MetroWidgetPrice newPrice = MetroWidgetPrice.Create(_profile.GetUpgradePrice(_car));
			_priceWidget.Replace(newPrice).Destroy();
			_priceWidget = newPrice;
			UpdateBadge();
		}
		layout.Reflow();
		_upgradeWidget.StartUpgradeAnim(_profile.GetUpgradeLevel());
		while (_upgradeWidget.IsAnimating)
		{
			yield return null;
		}
		_animating = false;
	}

	private void UpdateBadge()
	{
		if (!(_badge != null))
		{
			return;
		}
		int num = 0;
		PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
		int num2 = profile.Coins;
		int num3 = profile.Bucks;
		for (int i = _profile.GetUpgradeLevel() + 1; i <= 5; i++)
		{
			Price upgradePrice = _profile.GetUpgradePrice(_car, i);
			if ((upgradePrice.IsCoins() && upgradePrice.Amount <= num2) || (upgradePrice.IsBucks() && upgradePrice.Amount <= num3))
			{
				num2 -= (upgradePrice.IsCoins() ? upgradePrice.Amount : 0);
				num3 -= (upgradePrice.IsBucks() ? upgradePrice.Amount : 0);
				num++;
				continue;
			}
			break;
		}
		_badge.UpdateBadge(num.ToString(), num > 0);
	}
}
