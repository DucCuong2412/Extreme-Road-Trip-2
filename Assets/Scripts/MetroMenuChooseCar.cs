using System.Collections.Generic;
using UnityEngine;

public class MetroMenuChooseCar : MetroMenuPager
{
	public const int POCKET_MINE2_CAR_PAGE = 15;

	public const int PRT_CAR_PAGE = 17;

	public const int FISHING_CAR_PAGE = 18;

	public const int POCKET_MINE3_CAR_PAGE = 19;

	private const int NEW_RIDES_MIN_XP_LEVEL = 3;

	private bool _showSpecialOfferPopup;

	private MetroWidget _bottomMenuSpacer;

	private bool _popupShown;

	private void SelectCar(Car car, MetroButtonCarCard card)
	{
		CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(car);
		if (carProfile.IsUnlocked())
		{
			MetroPopupCarSelect metroPopupCarSelect = MetroMenuPage.Create<MetroPopupCarSelect>().Setup(car);
			metroPopupCarSelect.OnPrestigeLeveledUp += card.OnPrestigeLevelChanged;
			AutoSingleton<MetroMenuStack>.Instance.Push(metroPopupCarSelect, MetroAnimation.popup);
		}
		else
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupUnlockCar>().Setup(car), MetroAnimation.popup);
		}
	}

	protected override void OnStart()
	{
		_showSpecialOfferPopup = AutoSingleton<SpecialOfferManager>.Instance.SpecialOfferAttempt();
		if (!_showSpecialOfferPopup)
		{
		}
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		metroLayout.Add(MetroStatusBar.Create());
		MetroSpacer metroSpacer = MetroSpacer.Create();
		metroSpacer.SetMass(6.5f);
		metroLayout.Add(metroSpacer);
		_pager = CreateMetroPager("ChooseCarPager");
		metroSpacer.Add(_pager);
		List<Car> allForSaleCars = AutoSingleton<CarManager>.Instance.GetAllForSaleCars();
		base.ColCount = 3f;
		base.RowCount = 2f;
		int num = Mathf.CeilToInt((float)allForSaleCars.Count / (base.ColCount * base.RowCount));
		int num2 = 0;
		MetroGrid metroGrid = CreateGrid();
		foreach (Car item in allForSaleCars)
		{
			Car carCopy = item;
			MetroButtonCarCard carButton = MetroButtonCarCard.Create(carCopy, (!Device.IsIPad()) ? 1.4f : 1.25f);
			carButton.SetPadding(0.8f);
			int page = num2;
			CarCategory cat = carCopy.Category;
			carButton.OnKeyFocusGained += delegate
			{
				_pager.ChangePage(page);
				carButton.SwitchSlice9Background(MetroButtonCarCard.GetCarCardBackgroundSelected(cat));
				carButton.Berp();
			};
			carButton.OnKeyFocusLost += delegate
			{
				carButton.SwitchSlice9Background(MetroButtonCarCard.GetCarCardBackground(cat));
			};
			carButton.OnButtonClicked += delegate
			{
				SelectCar(carCopy, carButton);
			};
			metroGrid.Add(carButton);
			if (metroGrid.IsFull())
			{
				num2++;
				_pager.Add(CreatePagerChild(metroGrid, num2 == 1, num2 == num));
				metroGrid = CreateGrid();
			}
		}
		if (!metroGrid.IsEmpty())
		{
			num2++;
			_pager.Add(CreatePagerChild(metroGrid, num2 == 1, num2 == num));
		}
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(1.25f);
		metroLayout.Add(metroLayout2);
		metroLayout2.Add(MetroSpacer.Create(0.1f));
		metroLayout2.Add(MetroPagerIndicator.Create(_pager));
		metroLayout2.Add(MetroSpacer.Create(0.1f));
		_bottomMenuSpacer = metroLayout.Add(MetroSpacer.Create(2f));
		SetupMenuButtons();
		PrefabSingleton<GameMusicManager>.Instance.PlayTitleMusic();
		RateGameManager.RateGame();
		base.OnStart();
	}

	private void SetupMenuButtons()
	{
		_bottomMenuSpacer.Clear();
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		_bottomMenuSpacer.Add(metroLayout);
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconBack, "BACK");
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.main));
		};
		metroLayout.Add(metroButton);
		StoreItem storeItem = AutoSingleton<StoreDatabase>.Instance.GetStoreItem(StoreItemType.permanentCoinDoubler);
		if (storeItem != null && !storeItem.IsPurchased())
		{
			metroLayout.Add(MetroButtonCoinsDoublerBundle.Create(storeItem).SetMass(2f));
			storeItem.OnPurchaseSuccess += delegate
			{
				SetupMenuButtons();
				_bottomMenuSpacer.Reflow();
			};
		}
		else if (AutoSingleton<PlatformCapabilities>.Instance.IsIncentivedVideoSupported())
		{
			MetroButton child = AutoSingleton<GameAdProvider>.Instance.CreateFreeCrateButtonMenuChooseCar(0.85f, MetroSkin.Slice9Button, Color.white);
			metroLayout.Add(child);
		}
		MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconStore, "STORE");
		metroLayout.Add(metroButton2);
		metroButton2.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.store));
		};
		if (!AutoSingleton<PersistenceManager>.Instance.HasSeenStore)
		{
			MetroBadge metroBadge = MetroBadge.Create();
			metroButton2.Add(metroBadge);
			metroBadge.UpdateBadge("!", showIcon: true);
		}
		metroLayout.Add(MetroButtonFrenzy.Create());
		MetroButton metroButton3 = MetroSkin.CreateMenuButton(MetroSkin.IconPlay, "RANDOM", MetroSkin.Slice9ButtonRed);
		metroLayout.Add(metroButton3);
		metroButton3.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(AutoSingleton<CarManager>.Instance.GetRandomUnlockedCar()));
		};
	}

	public override void OnFocus()
	{
		base.OnFocus();
		if (_popupShown)
		{
			return;
		}
		if (AutoSingleton<FishingBreakPromoManager>.Instance.AreCarsAvailable() && !AutoSingleton<PersistenceManager>.Instance.HasHeardAboutNewCarV3_14_0 && AutoSingleton<Player>.Instance.Profile.XPProfile.Level >= 3)
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupNewFeature>().Setup("NEW GARAGE ARRIVALS!", "Six new cars are available for completing missions in our new game, Fishing Break! Check 'em out!", delegate
			{
				_pager.ChangePage(18, fastSpeed: true);
				AutoSingleton<MetroMenuStack>.Instance.Pop();
			}, "GO", "FishingPromo"), MetroAnimation.popup);
			AutoSingleton<PersistenceManager>.Instance.HasHeardAboutNewCarV3_14_0 = true;
		}
		else if (AutoSingleton<PocketMine3PromoManager>.Instance.AreCarsAvailable() && !AutoSingleton<PersistenceManager>.Instance.HasHeardAboutNewCarV3_16_0 && AutoSingleton<Player>.Instance.Profile.XPProfile.Level >= 3)
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupNewFeature>().Setup("NEW GARAGE ARRIVALS!", "Six new cars are available for completing missions in our new game, Pocket Mine 3! Check 'em out!", delegate
			{
				_pager.ChangePage(19, fastSpeed: true);
				AutoSingleton<MetroMenuStack>.Instance.Pop();
			}, "GO", "PM3Promo"), MetroAnimation.popup);
			AutoSingleton<PersistenceManager>.Instance.HasHeardAboutNewCarV3_16_0 = true;
		}
		else if (AutoSingleton<Player>.Instance.Profile.XPProfile.Level >= 3 && !AutoSingleton<PersistenceManager>.Instance.HasHeardAboutStore)
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupNewFeature>().Setup("NEW! THE STORE", "The store allows you to stack up on powerups and buy mystery crates. Give it a look!", delegate
			{
				AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.store));
			}, "OPEN", string.Empty), MetroAnimation.popup);
			AutoSingleton<PersistenceManager>.Instance.HasHeardAboutStore = true;
		}
		else if (_showSpecialOfferPopup)
		{
			_showSpecialOfferPopup = false;
			SpecialOffer specialOffer = AutoSingleton<SpecialOfferManager>.Instance.ConsumeSpecialOffer();
			if (specialOffer != null)
			{
				AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupSpecialOffer>().Setup(specialOffer), MetroAnimation.popup);
			}
		}
		_popupShown = true;
	}

	protected override void OnMenuUpdate()
	{
		base.OnMenuUpdate();
		if ((AutoSingleton<PlatformCapabilities>.Instance.IsIncentivedVideoSupported() || GameTrialPayManager.IsSupported()) && IsActive())
		{
			ProcessMessageGate.DisplayMessage(this);
		}
	}
}
