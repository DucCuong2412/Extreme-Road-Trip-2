using TapjoyUnity;
using UnityEngine;

public class TapjoyAnalyticsEventHandler
{
	public static void OnEvent(AnalyticEvent evt)
	{
		ITapjoyEvent tapjoyEvent = evt as ITapjoyEvent;
		if (tapjoyEvent != null && Application.platform != 0 && Application.platform != RuntimePlatform.WindowsEditor)
		{
			TapjoyIAPEventData tapjoyIAPEventData = tapjoyEvent.ToTapjoyIAPEventData();
			string text = tapjoyIAPEventData.PurchaseData;
			string text2 = tapjoyIAPEventData.DataSignature;
			if (string.IsNullOrEmpty(text))
			{
				text = "DATA_WAS_EMPTY";
			}
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "SIGNATURE_WAS_EMPTY";
			}
			//Tapjoy.TrackPurchaseInGooglePlayStore(tapjoyIAPEventData.SkuDetails, text, text2);
		}
	}
}
