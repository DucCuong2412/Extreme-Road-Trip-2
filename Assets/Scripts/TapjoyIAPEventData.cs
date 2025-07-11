public class TapjoyIAPEventData
{
	public string ProductId;

	public string CurrencyCode;

	public double ProductPrice;

	public string TransactionId;

	public string SkuDetails;

	public string PurchaseData;

	public string DataSignature;

	public TapjoyIAPEventData(string productId, string currencyCode, double productPrice, string transactionId, string skuDetails, string purchaseData, string dataSignature)
	{
		ProductId = productId;
		CurrencyCode = currencyCode;
		ProductPrice = productPrice;
		TransactionId = transactionId;
		SkuDetails = skuDetails;
		PurchaseData = purchaseData;
		DataSignature = dataSignature;
	}
}
