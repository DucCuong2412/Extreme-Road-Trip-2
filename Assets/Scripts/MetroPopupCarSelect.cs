using System;
using System.Collections;
using System.Runtime.CompilerServices;

public class MetroPopupCarSelect : MetroPopupPage
{
	private Car _car;

	private CarProfile _profile;

	private MetroBadge _badge;

	private MetroButton _prestigeButton;

	private MetroLayout _bottom;

	private MetroWidget _textLayout;

	private MetroWidgetCarUpgrade _upgradeWidget;

	private MetroWidgetPrice _priceWidget;

	private bool _animating;

	[method: MethodImpl(32)]
	public event Action OnPrestigeLeveledUp;

	public MetroPopupCarSelect Setup(Car car)
	{
		_car = car;
		_profile = AutoSingleton<CarManager>.Instance.GetCarProfile(_car);
		return this;
	}

	protected override void OnStart()
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel metroLabel = MetroLabel.Create(_car.DisplayName);
		metroLabel.SetFont(MetroSkin.BigFont);
		metroLayout2.Add(metroLabel);
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).SetMass(2f);
		metroLayout.Add(metroWidget);
		MetroWidget metroWidget2 = MetroLayout.Create(Direction.vertical).SetPadding(2f).SetMass(1.5f);
		metroWidget.Add(metroWidget2);
		MetroIcon carIcon = MetroIcon.Create(_car, asPrefab: true);
		carIcon.SetScale(2f);
		carIcon.SetAlignment(MetroAlign.Bottom);
		this.YieldAndExecute(delegate
		{
			carIcon.gameObject.AddComponent<CarAnimator>();
		});
		metroWidget2.Add(carIcon);
		_upgradeWidget = MetroWidgetCarUpgrade.Create(_profile.GetUpgradeLevel(), 1f);
		_upgradeWidget.SetMass(0.2f);
		metroWidget2.Add(_upgradeWidget);
		MetroWidget metroWidget3 = MetroLayout.Create(Direction.horizontal).AddSlice9Background(MetroSkin.Slice9Button);
		metroWidget.Add(metroWidget3);
		metroWidget.Add(MetroSpacer.Create(0.1f));
		metroWidget3.Add(MetroSpacer.Create(0.1f));
		_textLayout = MetroLayout.Create(Direction.vertical).SetPadding(4f);
		metroWidget3.Add(_textLayout);
		metroWidget3.Add(MetroSpacer.Create(0.1f));
		AddStatsText();
		MetroLabel metroLabel2 = MetroLabel.Create(_car.Description);
		metroLabel2.SetFont(MetroSkin.MediumFont);
		metroLayout.Add(metroLabel2);
		_bottom = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(_bottom);
		_bottom.Add(MetroSpacer.Create(0.1f));
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
			MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
			upgrade.Add(metroLayout3);
			metroLayout3.Add(MetroSpacer.Create());
			metroLayout3.Add(MetroLabel.Create("UPGRADE"));
			metroLayout3.Add(MetroSpacer.Create());
			_priceWidget = MetroWidgetPrice.Create(_profile.GetUpgradePrice(_car));
			metroLayout3.Add(_priceWidget);
			metroLayout3.Add(MetroSpacer.Create());
			_bottom.Add(upgrade);
			MetroSpacer buttonsSpacer = MetroSpacer.Create(0.1f);
			_bottom.Add(buttonsSpacer);
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
						StartCoroutine(UpgradeCR(upgrade, _bottom, buttonsSpacer));
					}
				}
			};
		}
		if (AutoSingleton<PrestigeManager>.Instance.IsCarPrestigeAvailable(_car))
		{
			_prestigeButton = MetroButton.Create();
			_prestigeButton.AddSlice9Background(MetroSkin.Slice9Button);
			_prestigeButton.OnKeyFocusGained += delegate
			{
				_prestigeButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
				_prestigeButton.Berp();
			};
			_prestigeButton.OnKeyFocusLost += delegate
			{
				_prestigeButton.AddSlice9Background(MetroSkin.Slice9Button);
			};
			MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
			_prestigeButton.Add(metroLayout4);
			metroLayout4.Add(MetroSpacer.Create());
			metroLayout4.Add(MetroLabel.Create("PRESTIGE"));
			metroLayout4.Add(MetroSpacer.Create());
			_bottom.Add(_prestigeButton);
			_bottom.Add(MetroSpacer.Create(0.1f));
			_prestigeButton.OnButtonClicked += delegate
			{
				AutoSingleton<PrestigeManager>.Instance.ShowPrestigeTokenPopupSequence(_car, OnPrestigePopupDismissed, GetType().ToString());
			};
		}
		MetroButton play = MetroButton.Create("PLAY");
		play.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		play.OnKeyFocusGained += delegate
		{
			play.AddSlice9Background(MetroSkin.Slice9ButtonRed);
			play.Berp();
		};
		play.OnKeyFocusLost += delegate
		{
			play.AddSlice9Background(MetroSkin.Slice9Button);
		};
		play.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(_car));
		};
		_bottom.Add(play);
		_bottom.Add(MetroSpacer.Create(0.1f));
		metroLayout.Add(MetroSpacer.Create(0.2f));
		base.OnStart();
	}

	public override void OnFocus()
	{
		base.OnFocus();
		UpdateBadge();
	}

	private void OnPrestigePopupDismissed()
	{
		AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		_textLayout.Clear();
		AddStatsText();
		_textLayout.Reflow();
		_prestigeButton.Destroy();
		_bottom.Remove(_prestigeButton);
		_bottom.Reflow();
		if (this.OnPrestigeLeveledUp != null)
		{
			this.OnPrestigeLeveledUp();
		}
	}

	private void AddStatsText()
	{
		_textLayout.Add(MetroSpacer.Create(0.5f));
		GameStats overall = AutoSingleton<GameStatsManager>.Instance.Overall;
		int value = overall.GetValue(_car, GameStats.CarStats.Type.maxDistance);
		_textLayout.Add(AddStat("Best", value.ToString() + "m"));
		float floatValue = overall.GetFloatValue(_car, GameStats.CarStats.Type.best2kTime);
		_textLayout.Add(AddStat("Best 2K", (floatValue != 0f) ? TimeUtil.Format(floatValue) : "-   "));
		floatValue = overall.GetFloatValue(_car, GameStats.CarStats.Type.best5kTime);
		_textLayout.Add(AddStat("Best 5K", (floatValue != 0f) ? TimeUtil.Format(floatValue) : "-   "));
		floatValue = overall.GetFloatValue(_car, GameStats.CarStats.Type.best10kTime);
		_textLayout.Add(AddStat("Best 10K", (floatValue != 0f) ? TimeUtil.Format(floatValue) : "-   "));
		string text = AutoSingleton<MissionManager>.Instance.GetCompletedMissionCount(_car).ToString() + "/" + AutoSingleton<MissionManager>.Instance.GetMissionCount(_car);
		_textLayout.Add(AddStat("Missions", text));
		string text2 = AutoSingleton<PrestigeManager>.Instance.GetCarPrestigeLevel(_car).ToString();
		_textLayout.Add(AddStat("Prestige Level", text2));
		_textLayout.Add(MetroSpacer.Create(0.5f));
	}

	private MetroWidget AddStat(string label, string text)
	{
		MetroWidget metroWidget = CreateStatLine();
		MetroWidget metroWidget2 = CreateMetroLabel(label);
		metroWidget2.SetAlignment(MetroAlign.Left);
		metroWidget.Add(metroWidget2);
		MetroWidget metroWidget3 = CreateMetroLabel(text);
		metroWidget3.SetAlignment(MetroAlign.Right);
		metroWidget.Add(metroWidget3);
		return metroWidget;
	}

	private MetroWidget CreateStatLine()
	{
		return MetroLayout.Create(Direction.horizontal);
	}

	private MetroWidget CreateMetroLabel(string content)
	{
		return MetroLabel.Create(content).SetFont(MetroSkin.SmallFont);
	}

	private IEnumerator UpgradeCR(MetroButton button, MetroLayout layout, MetroSpacer spacer)
	{
		_animating = true;
		yield return null;
		PrefabSingleton<GameSoundManager>.Instance.PlayUpgradeSound();
		button.SetActive(active: false);
		if (!_profile.CanUpgrade())
		{
			button.Remove().Destroy();
			spacer.Remove().Destroy();
			button = null;
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
		if (button != null)
		{
			button.SetActive(active: true);
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
