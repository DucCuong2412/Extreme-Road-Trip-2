public class IAPPrice : Price
{
	private PurchaseManager.Purchase _purchase;

	public IAPPrice(PurchaseManager.Purchase purchase)
		: base(-1, Currency.realMoney)
	{
		_purchase = purchase;
	}

	public override string ToString()
	{
		return AutoSingleton<PurchaseManager>.Instance.GetPurchasePriceString(_purchase);
	}
}
