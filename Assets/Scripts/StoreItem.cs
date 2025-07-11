using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StoreItem
{
	public Price Price
	{
		get;
		private set;
	}

	public StoreItemType Type
	{
		get;
		private set;
	}

	public int Quantity
	{
		get;
		private set;
	}

	public string AssetName
	{
		get;
		private set;
	}

	public string Description
	{
		get;
		private set;
	}

	[method: MethodImpl(32)]
	public event Action OnPurchaseSuccess;

	public StoreItem(Price price, string type, int quantity, string assetName, string description)
	{
		Price = price;
		Type = ConvertStringToStoreItemType(type);
		Quantity = quantity;
		AssetName = assetName;
		Description = description;
		if (Price.IsRealMoney())
		{
			PurchaseManager.OnPurchaseSuccessfull += OnRealMoneyPurchaseSucessfull;
		}
	}

	public void Destroy()
	{
		if (Price.IsRealMoney())
		{
			PurchaseManager.OnPurchaseSuccessfull -= OnRealMoneyPurchaseSucessfull;
		}
	}

	public static StoreItemType ConvertStringToStoreItemType(string storeItem)
	{
		switch (storeItem)
		{
		case "powerups":
			return StoreItemType.powerups;
		case "crates":
			return StoreItemType.crates;
		case "permanentCoinDoubler":
			return StoreItemType.permanentCoinDoubler;
		default:
			UnityEngine.Debug.LogError("Expected powerups or crate as StoreItemType");
			return StoreItemType.powerups;
		}
	}

	public string GetPurchaseString()
	{
		switch (Type)
		{
		case StoreItemType.crates:
			return Quantity.ToString() + " MYSTERY CRATES".Localize();
		case StoreItemType.powerups:
			return Quantity.ToString() + " SETS OF POWERUPS".Localize();
		case StoreItemType.permanentCoinDoubler:
			return "DOUBLE YOUR COIN".Localize();
		default:
			return string.Empty;
		}
	}

	public bool TryPurchase()
	{
		if (Price.Currency == Currency.realMoney)
		{
			if (AutoSingleton<PurchaseManager>.Instance.CanMakePayments())
			{
				PurchaseManager.Purchase iAPPurchase = StoreDatabase.GetIAPPurchase(Type);
				if (iAPPurchase != null)
				{
					AutoSingleton<PurchaseManager>.Instance.Buy(iAPPurchase);
					return true;
				}
				return false;
			}
			return false;
		}
		if (AutoSingleton<CashManager>.Instance.CanBuy(Price))
		{
			AutoSingleton<CashManager>.Instance.Buy(Price);
			if (Quantity > 0)
			{
				if (Type == StoreItemType.powerups)
				{
					AutoSingleton<Player>.Instance.Profile.OnPowerupAdded(PowerupType.boost, Quantity);
					AutoSingleton<Player>.Instance.Profile.OnPowerupAdded(PowerupType.coinDoubler, Quantity);
					AutoSingleton<Player>.Instance.Profile.OnPowerupAdded(PowerupType.magnet, Quantity);
				}
				else if (Type == StoreItemType.crates)
				{
					List<Reward> randomRewards = AutoSingleton<MissionRewardsManager>.Instance.GetRandomRewards(Quantity);
					randomRewards.ForEach(delegate(Reward r)
					{
						r.Activate();
					});
					MetroPopupRewards popup = MetroMenuPage.Create<MetroPopupRewards>().Setup("Surprise!", string.Empty, randomRewards, delegate
					{
						AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
					}, MetroSkin.StarCircle);
					AutoSingleton<MetroMenuStack>.Instance.EnqueuePopup(popup);
				}
			}
			if (this.OnPurchaseSuccess != null)
			{
				this.OnPurchaseSuccess();
			}
			return true;
		}
		AutoSingleton<CashManager>.Instance.HandleNotEnoughCurrency(Price, pauseOnFocus: false);
		return false;
	}

	public bool IsPurchased()
	{
		bool result = false;
		if (Type == StoreItemType.permanentCoinDoubler)
		{
			result = AutoSingleton<StorePerkManager>.Instance.IsStorePerkOwned(StorePerkType.permanentCoinDoubler);
		}
		return result;
	}

	public void OnRealMoneyPurchaseSucessfull(PurchaseManager.Purchase purchase)
	{
		if (purchase == StoreDatabase.GetIAPPurchase(Type) && this.OnPurchaseSuccess != null)
		{
			this.OnPurchaseSuccess();
		}
	}
}
