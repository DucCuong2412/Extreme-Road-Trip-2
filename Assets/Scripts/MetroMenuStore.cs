using System.Collections.Generic;
using UnityEngine;

public class MetroMenuStore : MetroMenuPage
{
	protected override void OnStart()
	{
		AutoSingleton<PersistenceManager>.Instance.HasSeenStore = true;
		AutoSingleton<PersistenceManager>.Instance.HasHeardAboutStore = true;
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroStatusBar statusBar = MetroStatusBar.Create(-1, -1, -1, null, showPowerupsInventory: true);
		metroLayout.Add(statusBar);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(7f);
		metroLayout.Add(metroLayout2);
		//MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		//metroLayout2.Add(metroLayout3);
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
		metroLayout2.Add(metroLayout4);
		List<StoreItem> storeItems = AutoSingleton<StoreDatabase>.Instance.GetStoreItems();
		//StoreItem storeItem = storeItems.Find((StoreItem i) => i.Type == StoreItemType.permanentCoinDoubler);
		//if (storeItem != null)
		//{
		//	MetroButton coinsDoublerButton = CreateStoreItemButton(storeItem, (!storeItem.IsPurchased()) ? MetroSkin.Slice9ButtonGreen : MetroSkin.Slice9StoreSquare, Color.white);
		//	storeItem.OnPurchaseSuccess += delegate
		//	{
		//		coinsDoublerButton.Reflow();
		//	};
		//	metroLayout3.Add(coinsDoublerButton);
		//}
        //đây là chỗ bỏ nút Video Ad:	
        //if (AutoSingleton<PlatformCapabilities>.Instance.IsIncentivedVideoSupported())
        //{
        //	MetroButton child = AutoSingleton<GameAdProvider>.Instance.CreateFreeCrateButtonMenuStore(1.2f);
        //	metroLayout3.Add(child);
        //}
        //else
        //{
        //	metroLayout3.Add(MetroSpacer.Create());
        //}
        MetroGrid metroGrid = MetroGrid.Create(3, 2);
		metroLayout4.Add(metroGrid);
		MetroLayout metroLayout5 = MetroLayout.Create(Direction.horizontal);
		metroLayout5.SetMass(2f);
		metroLayout.Add(metroLayout5);
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconBack, "BACK", MetroSkin.Slice9ButtonRed);
		metroLayout5.Add(metroButton);
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.chooseCar));
		};
		if (AutoSingleton<PlatformCapabilities>.Instance.IsCurrencyPurchaseSupported())
		{
			MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconBucksPackC, "BUCKS");
			metroButton2.OnButtonClicked += delegate
			{
				MetroMenuPage page2 = MetroMenuPage.Create<MetroMenuMoreCash>().Setup(Currency.bucks);
				AutoSingleton<MetroMenuStack>.Instance.Push(page2, MetroAnimation.slideDown);
			};
			metroLayout5.Add(metroButton2);
			MetroButton metroButton3 = MetroSkin.CreateMenuButton(MetroSkin.IconCoinsPackC, "COINS");
			metroButton3.OnButtonClicked += delegate
			{
				MetroMenuPage page = MetroMenuPage.Create<MetroMenuMoreCash>().Setup(Currency.coins);
				AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.slideDown);
			};
			metroLayout5.Add(metroButton3);
		}
		foreach (StoreItem item in storeItems)
		{
			if (item.Type == StoreItemType.crates || item.Type == StoreItemType.powerups)
			{
				item.OnPurchaseSuccess += delegate
				{
					statusBar.RefreshPowerupsInventory();
				};
				metroGrid.Add(CreateStoreItemButton(item, MetroSkin.Slice9StoreSquare, Color.black));
			}
		}
		base.OnStart();
	}

	private MetroButton CreateStoreItemButton(StoreItem item, string bgSlice9, Color textColor)
	{
		MetroButton b = MetroButton.Create();
		b.AddSlice9Background(bgSlice9);
		b.Add(MetroWidgetStoreItem.Create(item, textColor));
		b.OnKeyFocusGained += delegate
		{
			b.AddSlice9Background(MetroSkin.Slice9StoreSquareSelected);
			b.Berp();
		};
		b.OnKeyFocusLost += delegate
		{
			b.AddSlice9Background(MetroSkin.Slice9StoreSquare);
		};
		b.OnButtonClicked += delegate
		{
			if (item.IsPurchased())
			{
				MetroPopupMessage page = MetroMenuPage.Create<MetroPopupMessage>().Setup("STORE", "You have already purchased this item.");
				AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
			}
			else if (!item.Price.IsRealMoney())
			{
				MetroPopupStorePurchase popup = MetroMenuPage.Create<MetroPopupStorePurchase>().Setup(item);
				AutoSingleton<MetroMenuStack>.Instance.EnqueuePopup(popup);
			}
			else
			{
				item.TryPurchase();
			}
		};
		return b;
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
