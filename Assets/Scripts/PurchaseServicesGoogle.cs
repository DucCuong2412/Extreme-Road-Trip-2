using System.Collections.Generic;
using UnityEngine;

public class PurchaseServicesGoogle : AutoSingleton<PurchaseServicesGoogle>
{
	private Dictionary<string, GoogleSkuInfo> _skus;

	private string _pendingPurchaseId = string.Empty;

	private Maybe<bool> _billingSupported = new Maybe<bool>();

	private List<string> _purchasedSkus = new List<string>();

	private List<string> _productQuery = new List<string>();

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_skus = new Dictionary<string, GoogleSkuInfo>();
		GoogleIAB.unbindService();
		GoogleIAB.setAutoVerifySignatures(shouldVerify: true);
		GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkH/mEBd+Pi/05zhNczazkmWeMr4XgNmAvGWs/b2Ve/lVPhlIT79Mrm0Gx25Ii+5AvuQDpyq6Af/VbsKZ5q+Vi2XzrsaSBeEej5BTT9VHTsfoYiNDL8kt/IzA2lRINoDHcKgXJ94Sjdr2OpecrPsjDRUhCfVKbVr9hDn6Qt1YEibu54SagtA5cfIg3TsdlC75dZBUj8rU6552j+R01w5+IjzqOtL2eJ+Pyq9Yn9c10GLWRVWMxXfOf5SauhsYBLrDHJBXijIQUIrgswbwy4AXKymfMwBzbTmNBbXS+JdJ+sVroJ3YzZ9BpRGoTXz9ClmWIt2epjgrgXhCJjMeUpxsSwIDAQAB");
		GoogleIABManager.billingSupportedEvent += onBillingSupported;
		GoogleIABManager.billingNotSupportedEvent += onBillingNotSupported;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseSucceededEvent += onPurchaseSuccessful;
		GoogleIABManager.purchaseFailedEvent += onPurchaseFailed;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += OnpurchaseCompleteAwaitingVerification;
		base.OnAwake();
	}

	private void OnpurchaseCompleteAwaitingVerification(string purchaseData, string signature)
	{
	}

	private void OnApplicationQuit()
	{
		GoogleIAB.unbindService();
	}

	public string GetPurchasePriceString(string id)
	{
		if (_skus.ContainsKey(id))
		{
			return StringUtil.DecodeHtmlChars(_skus[id].price);
		}
		return string.Empty;
	}

	public void Buy(string id)
	{
		if (_purchasedSkus.Contains(id))
		{
			_pendingPurchaseId = id;
			ConsumeAllPurchases();
		}
		else if (_skus.ContainsKey(id))
		{
			if (IsBillingSupported())
			{
				GoogleIAB.purchaseProduct(id);
			}
		}
		else if (_skus.Count == 0)
		{
			AutoSingleton<PurchaseManager>.Instance.RequestProductData();
		}
	}

	public void RequestProductData(string[] array)
	{
		if (IsBillingSupported())
		{
			_productQuery.Clear();
			_productQuery.AddRange(array);
			QueryProductInventory();
		}
	}

	private void QueryProductInventory()
	{
		int num = Mathf.Min(15, _productQuery.Count);
		if (num > 0)
		{
			string[] skus = _productQuery.GetRange(0, num).ToArray();
			_productQuery.RemoveRange(0, num);
			GoogleIAB.queryInventory(skus);
		}
	}

	private bool IsBillingSupported()
	{
		return _billingSupported.IsSet() && _billingSupported.Get();
	}

	private void ConsumeAllPurchases()
	{
		if (IsBillingSupported())
		{
			List<string> list = new List<string>();
			foreach (string purchasedSku in _purchasedSkus)
			{
				if (AutoSingleton<PurchaseManager>.Instance.IsConsumable(purchasedSku))
				{
					list.Add(purchasedSku);
				}
			}
			if (list.Count > 0)
			{
				GoogleIAB.consumeProducts(list.ToArray());
			}
		}
	}

	private void onBillingSupported()
	{
		_billingSupported.Set(data: true);
		AutoSingleton<PurchaseManager>.Instance.RequestProductData();
	}

	private void onBillingNotSupported(string error)
	{
		_billingSupported.Set(data: false);
	}

	private void onPurchaseSuccessful(GooglePurchase purchase)
	{
		_pendingPurchaseId = string.Empty;
		_purchasedSkus.Add(purchase.productId);
		if (!AutoSingleton<PurchaseManager>.Instance.IsConsumable(purchase.productId))
		{
			OnPurchaseSuccessful(purchase);
		}
		else
		{
			ConsumeAllPurchases();
		}
	}

	private void onPurchaseFailed(string error)
	{
		_pendingPurchaseId = string.Empty;
		if (error != null && (error.Contains("User canceled") || error.Contains("User cancelled")))
		{
			AutoSingleton<PurchaseManager>.Instance.onPurchaseCancelled(error);
		}
		else
		{
			AutoSingleton<PurchaseManager>.Instance.onPurchaseFailed(error);
		}
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		if (_purchasedSkus.Contains(purchase.productId))
		{
			_purchasedSkus.Remove(purchase.productId);
		}
		else
		{
			SilentDebug.LogWarning("We consumed an item that wasn't tracked as purchased: " + purchase.productId);
		}
		OnPurchaseSuccessful(purchase);
		if (_pendingPurchaseId != string.Empty)
		{
			_pendingPurchaseId = string.Empty;
			Buy(_pendingPurchaseId);
		}
	}

	private void OnPurchaseSuccessful(GooglePurchase purchase)
	{
		string currencyCode = string.Empty;
		double productPrice = 0.0;
		string orderId = purchase.orderId;
		string originalJson = purchase.originalJson;
		string signature = purchase.signature;
		string formattedPrice = productPrice.ToString();
		if (_skus.ContainsKey(purchase.productId))
		{
			GoogleSkuInfo googleSkuInfo = _skus[purchase.productId];
			currencyCode = googleSkuInfo.priceCurrencyCode;
			productPrice = (double)googleSkuInfo.priceAmountMicros / 1000.0 / 1000.0;
			formattedPrice = googleSkuInfo.price;
		}
		AutoSingleton<PurchaseManager>.Instance.onPurchaseSuccessful(purchase.productId, purchase.purchaseToken, 1, PurchaseManager.ReceiptValidationStatus.ValidProduction, currencyCode, productPrice, formattedPrice, orderId, originalJson, signature);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		if (_pendingPurchaseId != string.Empty && IsBillingSupported())
		{
			GoogleIAB.purchaseProduct(_pendingPurchaseId);
			_pendingPurchaseId = string.Empty;
		}
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		foreach (GoogleSkuInfo sku in skus)
		{
			_skus[sku.productId] = sku;
		}
		if (_productQuery.Count <= 0)
		{
			bool received = skus.Count > 0;
			AutoSingleton<PurchaseManager>.Instance.OnProductReceived(received);
			UpdatePurchasedProduct(purchases);
		}
		else
		{
			QueryProductInventory();
		}
	}

	private void UpdatePurchasedProduct(List<GooglePurchase> purchases)
	{
		_purchasedSkus.Clear();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (GooglePurchase purchase in purchases)
		{
			if (AutoSingleton<PurchaseManager>.Instance.IsConsumable(purchase.productId))
			{
				_purchasedSkus.Add(purchase.productId);
			}
			else if (purchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
			{
				list.Add(purchase.productId);
			}
			else if (purchase.purchaseState == GooglePurchase.GooglePurchaseState.Canceled || purchase.purchaseState == GooglePurchase.GooglePurchaseState.Refunded)
			{
				list2.Add(purchase.productId);
			}
		}
		AutoSingleton<PurchaseManager>.Instance.UpdateNotConsumableProductPurchased(list);
		AutoSingleton<PurchaseManager>.Instance.UpdateNotConsumableProductRefunded(list2);
		ConsumeAllPurchases();
	}

	private void queryInventoryFailedEvent(string error)
	{
		_productQuery.Clear();
		bool received = false;
		AutoSingleton<PurchaseManager>.Instance.OnProductReceived(received);
	}
}
