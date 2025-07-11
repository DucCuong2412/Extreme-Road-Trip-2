using Prime31;
using System.Collections.Generic;
using UnityEngine;

public class InAppPurchase : AnalyticEvent, IRoofdogEvent, ITapjoyEvent
{
	private RoofdogEventData _rdData;

	private TapjoyIAPEventData _tjData;

	public InAppPurchase(string productId, double gamePrice, string category, bool sandbox, bool restore, string currencyCode, double productPrice, string formattedPrice, string transactionId, string purchaseData, string dataSignature)
	{
		if (!restore)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				["purchase"] = productId
			};
		}
		_rdData = new RoofdogEventData(RoofdogAnalyticsManager.EventCategory.inAppPurchase);
		string value = restore ? ERT2AnalyticEvent.restore.ToString() : ERT2AnalyticEvent.purchase.ToString();
		int value2 = Mathf.RoundToInt((float)(gamePrice * 100.0));
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.eventAction, value);
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.paramStr1, productId);
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.paramStr2, category);
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.paramNum1, value2);
		string skuDetails = SimpleJson.encode(new AndroidSkuDetails(productId, formattedPrice, "inapp", productId, productPrice * 1000000.0, currencyCode, productId));
		_tjData = new TapjoyIAPEventData(productId, currencyCode, productPrice, transactionId, skuDetails, purchaseData, dataSignature);
	}

	public RoofdogEventData ToRoofdogEventData()
	{
		return _rdData;
	}

	public TapjoyIAPEventData ToTapjoyIAPEventData()
	{
		return _tjData;
	}
}
