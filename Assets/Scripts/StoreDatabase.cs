using System.Collections.Generic;

public class StoreDatabase : AutoSingleton<StoreDatabase>
{
	private List<StoreItem> _items;

	protected override void OnDestroy()
	{
		foreach (StoreItem item in _items)
		{
			item.Destroy();
		}
		base.OnDestroy();
	}

	protected override void OnAwake()
	{
		_items = new List<StoreItem>();
		CSVData cSVData = new CSVData("store.csv");
		int count = cSVData.Count;
		for (int i = 0; i < count; i++)
		{
			string @string = cSVData.GetString(i, "Type", "powerups");
			int @int = cSVData.GetInt(i, "Count");
			string string2 = cSVData.GetString(i, "Asset", MetroSkin.IconPowerupBoost);
			int int2 = cSVData.GetInt(i, "Price");
			string description = cSVData.GetString(i, "Description", string.Empty).Replace("\\n", "\n");
			Currency currency = Price.ConvertStringToCurrency(cSVData.GetString(i, "Currency", "coins"));
			Price price = null;
			if (currency == Currency.realMoney)
			{
				PurchaseManager.Purchase iAPPurchase = GetIAPPurchase(StoreItem.ConvertStringToStoreItemType(@string));
				if (iAPPurchase == null)
				{
					continue;
				}
				price = new IAPPrice(iAPPurchase);
			}
			else
			{
				price = new Price(int2, currency);
			}
			StoreItem item = new StoreItem(price, @string, @int, string2, description);
			_items.Add(item);
		}
	}

	public List<StoreItem> GetStoreItems()
	{
		return _items;
	}

	public StoreItem GetStoreItem(StoreItemType type)
	{
		return _items.Find((StoreItem i) => i.Type == type);
	}

	public static PurchaseManager.Purchase GetIAPPurchase(StoreItemType type)
	{
		if (type == StoreItemType.permanentCoinDoubler)
		{
			return AutoSingleton<PurchaseManager>.Instance.GetPermanentCoinDoublerPurchase();
		}
		return null;
	}
}
