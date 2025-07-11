using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PurchaseManager : AutoSingleton<PurchaseManager>
{
	public enum ReceiptValidationStatus
	{
		ValidProduction,
		ValidSandbox,
		Invalid,
		Unknown
	}

	public abstract class Purchase
	{
		private string _identifier;

		public string Identifier
		{
			get
			{
				return FixCase(_identifier);
			}
			private set
			{
				_identifier = value;
			}
		}

		public bool IsConsumable
		{
			get;
			private set;
		}

		public double DefaultPriceDouble
		{
			get;
			protected set;
		}

		public Purchase(string identifier, bool isConsumable)
		{
			Identifier = identifier;
			IsConsumable = isConsumable;
		}

		public abstract void Redeem();

		public virtual void ServerRedeem()
		{
		}

		public virtual void Revoke()
		{
		}

		public abstract string GetCategory();
	}

	public class PurchasedCurrency : Purchase
	{
		public Currency Currency
		{
			get;
			private set;
		}

		public int Amount
		{
			get;
			private set;
		}

		public int Bonus
		{
			get;
			private set;
		}

		public string Icon
		{
			get;
			private set;
		}

		public PurchasedCurrency(string identifier, Currency currency, int amount, int bonus, string icon, string defaultPrice)
			: base(identifier, isConsumable: true)
		{
			Currency = currency;
			Amount = amount;
			Bonus = bonus;
			Icon = icon;
			double result = 0.0;
			double.TryParse(defaultPrice, out result);
			base.DefaultPriceDouble = result;
		}

		public override void ServerRedeem()
		{
			AutoSingleton<CashManager>.Instance.ForceRefresh();
		}

		public override void Redeem()
		{
			switch (Currency)
			{
			case Currency.coins:
				AutoSingleton<CashManager>.Instance.AddCoins(Amount + Bonus);
				break;
			case Currency.bucks:
				AutoSingleton<CashManager>.Instance.AddBucks(Amount + Bonus);
				break;
			}
		}

		public override string GetCategory()
		{
			return Currency.ToString();
		}
	}

	public class SpecialOfferPurchasedCurrency : Purchase
	{
		public Currency Currency
		{
			get;
			private set;
		}

		public int Amount
		{
			get;
			private set;
		}

		public SpecialOfferPurchasedCurrency(string identifier, Currency currency, int amount, string defaultPrice)
			: base(identifier, isConsumable: true)
		{
			Currency = currency;
			Amount = amount;
			double result = 0.0;
			double.TryParse(defaultPrice, out result);
			base.DefaultPriceDouble = result;
		}

		public override void Redeem()
		{
			switch (Currency)
			{
			case Currency.coins:
				AutoSingleton<CashManager>.Instance.AddCoins(Amount);
				break;
			case Currency.bucks:
				AutoSingleton<CashManager>.Instance.AddBucks(Amount);
				break;
			}
		}

		public override string GetCategory()
		{
			return "SpecialOfferPurchaseCurrency";
		}
	}

	public enum PackType
	{
		starter,
		starter2,
		booster
	}

	public class PurchasedPack : Purchase
	{
		public PackType PackType
		{
			get;
			private set;
		}

		public PurchasedPack(string identifier, PackType packType, double price)
			: base(identifier, isConsumable: true)
		{
			PackType = packType;
			base.DefaultPriceDouble = price;
		}

		public override void Redeem()
		{
			switch (PackType)
			{
			case PackType.starter:
				AutoSingleton<CashManager>.Instance.AddCoins(5000);
				AutoSingleton<CashManager>.Instance.AddBucks(20);
				break;
			case PackType.starter2:
				AutoSingleton<CashManager>.Instance.AddCoins(9000);
				AutoSingleton<CashManager>.Instance.AddBucks(30);
				break;
			case PackType.booster:
			{
				PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
				int amount = 10;
				profile.OnPowerupAdded(PowerupType.boost, amount);
				profile.OnPowerupAdded(PowerupType.coinDoubler, amount);
				profile.OnPowerupAdded(PowerupType.magnet, amount);
				break;
			}
			}
		}

		public override string GetCategory()
		{
			return PackType.ToString();
		}
	}

	public class PurchasedFrenzyRuns : Purchase
	{
		public int Amount
		{
			get;
			private set;
		}

		public PurchasedFrenzyRuns(string identifier, int amount)
			: base(identifier, isConsumable: true)
		{
			Amount = amount;
			base.DefaultPriceDouble = 0.99;
		}

		public override void ServerRedeem()
		{
			AutoSingleton<FrenzyModeManager>.Instance.Refresh();
		}

		public override void Redeem()
		{
			AutoSingleton<FrenzyModeManager>.Instance.AddRuns(Amount);
		}

		public override string GetCategory()
		{
			return "frenzyRuns";
		}
	}

	public class PurchasedAdRemover : Purchase
	{
		public PurchasedAdRemover(string identifier)
			: base(identifier, isConsumable: false)
		{
			base.DefaultPriceDouble = 2.99;
		}

		public override void Redeem()
		{
			AutoSingleton<StorePerkManager>.Instance.ActivateStorePerk(StorePerkType.adRemover);
			AutoSingleton<StorePerkManager>.Instance.ActivateStorePerk(StorePerkType.permanentCoinDoubler);
		}

		public override void Revoke()
		{
			AutoSingleton<StorePerkManager>.Instance.DeactivateStorePerk(StorePerkType.adRemover);
			AutoSingleton<StorePerkManager>.Instance.DeactivateStorePerk(StorePerkType.permanentCoinDoubler);
		}

		public override string GetCategory()
		{
			return "adRemover";
		}
	}

	public class PurchasedPermanentCoinDoubler : Purchase
	{
		public PurchasedPermanentCoinDoubler(string identifier)
			: base(identifier, isConsumable: false)
		{
			base.DefaultPriceDouble = 4.99;
		}

		public override void Redeem()
		{
			AutoSingleton<StorePerkManager>.Instance.ActivateStorePerk(StorePerkType.permanentCoinDoubler);
			AutoSingleton<StorePerkManager>.Instance.ActivateStorePerk(StorePerkType.adRemover);
		}

		public override void Revoke()
		{
			AutoSingleton<StorePerkManager>.Instance.DeactivateStorePerk(StorePerkType.permanentCoinDoubler);
			AutoSingleton<StorePerkManager>.Instance.DeactivateStorePerk(StorePerkType.adRemover);
		}

		public override string GetCategory()
		{
			return "permanentCoinDoubler";
		}
	}

	private const string _faceReceiptTransactionId = "1000000058548257";

	private const string _fakeReceipt = "ewoJInNpZ25hdHVyZSIgPSAiQXNXMnh2UjM1V1NVQ0RmL0RyaUp3VmptT0kyUDRMa2ZmSU85UzJhSXp4anlRTkJrZkZQYm01cnZRRTRsZmJuZXNvNXZzaVdqY0liR1pnRkdLbUZkaXFkQUtHWjBHTXo3N1Nkb0VYaFIzNDRMUVoyOGhhemtkV1dlbTR1SE4wVnJjYmZJRzluNGM3QUoyWkk0ZDdOMUNQZlpSemdXRzRFT2VHVEJmQlBGbElWWkFBQURWekNDQTFNd2dnSTdvQU1DQVFJQ0NHVVVrVTNaV0FTMU1BMEdDU3FHU0liM0RRRUJCUVVBTUg4eEN6QUpCZ05WQkFZVEFsVlRNUk13RVFZRFZRUUtEQXBCY0hCc1pTQkpibU11TVNZd0pBWURWUVFMREIxQmNIQnNaU0JEWlhKMGFXWnBZMkYwYVc5dUlFRjFkR2h2Y21sMGVURXpNREVHQTFVRUF3d3FRWEJ3YkdVZ2FWUjFibVZ6SUZOMGIzSmxJRU5sY25ScFptbGpZWFJwYjI0Z1FYVjBhRzl5YVhSNU1CNFhEVEE1TURZeE5USXlNRFUxTmxvWERURTBNRFl4TkRJeU1EVTFObG93WkRFak1DRUdBMVVFQXd3YVVIVnlZMmhoYzJWU1pXTmxhWEIwUTJWeWRHbG1hV05oZEdVeEd6QVpCZ05WQkFzTUVrRndjR3hsSUdsVWRXNWxjeUJUZEc5eVpURVRNQkVHQTFVRUNnd0tRWEJ3YkdVZ1NXNWpMakVMTUFrR0ExVUVCaE1DVlZNd2daOHdEUVlKS29aSWh2Y05BUUVCQlFBRGdZMEFNSUdKQW9HQkFNclJqRjJjdDRJclNkaVRDaGFJMGc4cHd2L2NtSHM4cC9Sd1YvcnQvOTFYS1ZoTmw0WElCaW1LalFRTmZnSHNEczZ5anUrK0RyS0pFN3VLc3BoTWRkS1lmRkU1ckdYc0FkQkVqQndSSXhleFRldngzSExFRkdBdDFtb0t4NTA5ZGh4dGlJZERnSnYyWWFWczQ5QjB1SnZOZHk2U01xTk5MSHNETHpEUzlvWkhBZ01CQUFHamNqQndNQXdHQTFVZEV3RUIvd1FDTUFBd0h3WURWUjBqQkJnd0ZvQVVOaDNvNHAyQzBnRVl0VEpyRHRkREM1RllRem93RGdZRFZSMFBBUUgvQkFRREFnZUFNQjBHQTFVZERnUVdCQlNwZzRQeUdVakZQaEpYQ0JUTXphTittVjhrOVRBUUJnb3Foa2lHOTJOa0JnVUJCQUlGQURBTkJna3Foa2lHOXcwQkFRVUZBQU9DQVFFQUVhU2JQanRtTjRDL0lCM1FFcEszMlJ4YWNDRFhkVlhBZVZSZVM1RmFaeGMrdDg4cFFQOTNCaUF4dmRXLzNlVFNNR1k1RmJlQVlMM2V0cVA1Z204d3JGb2pYMGlreVZSU3RRKy9BUTBLRWp0cUIwN2tMczlRVWU4Y3pSOFVHZmRNMUV1bVYvVWd2RGQ0TndOWXhMUU1nNFdUUWZna1FRVnk4R1had1ZIZ2JFL1VDNlk3MDUzcEdYQms1MU5QTTN3b3hoZDNnU1JMdlhqK2xvSHNTdGNURXFlOXBCRHBtRzUrc2s0dHcrR0szR01lRU41LytlMVFUOW5wL0tsMW5qK2FCdzdDMHhzeTBiRm5hQWQxY1NTNnhkb3J5L0NVdk02Z3RLc21uT09kcVRlc2JwMGJzOHNuNldxczBDOWRnY3hSSHVPTVoydG04bnBMVW03YXJnT1N6UT09IjsKCSJwdXJjaGFzZS1pbmZvIiA9ICJld29KSW05eWFXZHBibUZzTFhCMWNtTm9ZWE5sTFdSaGRHVXRjSE4wSWlBOUlDSXlNREV5TFRFeExURXpJREU1T2pBMU9qTXhJRUZ0WlhKcFkyRXZURzl6WDBGdVoyVnNaWE1pT3dvSkluQjFjbU5vWVhObExXUmhkR1V0YlhNaUlEMGdJakV6TlRJNE5qSXpNekV3TWpRaU93b0pJblZ1YVhGMVpTMXBaR1Z1ZEdsbWFXVnlJaUE5SUNKaU5HVmtPVFJpTXpBMU1tVmlOekJoTWpreU9EYzRPREJpWTJSa1pUYzFZbUpsWVdObE9UVTFJanNLQ1NKdmNtbG5hVzVoYkMxMGNtRnVjMkZqZEdsdmJpMXBaQ0lnUFNBaU1UQXdNREF3TURBMU9EVTBPREkxTnlJN0Nna2lZblp5Y3lJZ1BTQWlNUzR6SWpzS0NTSmhjSEF0YVhSbGJTMXBaQ0lnUFNBaU5USTJPREUwT0RReUlqc0tDU0owY21GdWMyRmpkR2x2YmkxcFpDSWdQU0FpTVRBd01EQXdNREExT0RVME9ESTFOeUk3Q2draWNYVmhiblJwZEhraUlEMGdJakVpT3dvSkltOXlhV2RwYm1Gc0xYQjFjbU5vWVhObExXUmhkR1V0YlhNaUlEMGdJakV6TlRJNE5qSXpNekV3TWpRaU93b0pJblZ1YVhGMVpTMTJaVzVrYjNJdGFXUmxiblJwWm1sbGNpSWdQU0FpTlRFeE1qSTJNRVV0UWtaRlJDMDBNamhGTFRnNU1qQXROemN5TmpjMFJUSTRRemhESWpzS0NTSnBkR1Z0TFdsa0lpQTlJQ0kxTlRNek5URXlNelVpT3dvSkluWmxjbk5wYjI0dFpYaDBaWEp1WVd3dGFXUmxiblJwWm1sbGNpSWdQU0FpTVRFNE5ERXlOVFVpT3dvSkluQnliMlIxWTNRdGFXUWlJRDBnSW1OaExuSnZiMlprYjJjdWNtOWhaSFJ5YVhBeUxtSjFZMnR6UVNJN0Nna2ljSFZ5WTJoaGMyVXRaR0YwWlNJZ1BTQWlNakF4TWkweE1TMHhOQ0F3TXpvd05Ub3pNU0JGZEdNdlIwMVVJanNLQ1NKdmNtbG5hVzVoYkMxd2RYSmphR0Z6WlMxa1lYUmxJaUE5SUNJeU1ERXlMVEV4TFRFMElEQXpPakExT2pNeElFVjBZeTlIVFZRaU93b0pJbUpwWkNJZ1BTQWlZMkV1Y205dlptUnZaeTV5YjJGa2RISnBjRElpT3dvSkluQjFjbU5vWVhObExXUmhkR1V0Y0hOMElpQTlJQ0l5TURFeUxURXhMVEV6SURFNU9qQTFPak14SUVGdFpYSnBZMkV2VEc5elgwRnVaMlZzWlhNaU93cDkiOwoJImVudmlyb25tZW50IiA9ICJTYW5kYm94IjsKCSJwb2QiID0gIjEwMCI7Cgkic2lnbmluZy1zdGF0dXMiID0gIjAiOwp9==";

	private bool _restore;

	private bool _purchasing;

	private static List<Purchase> _purchases;

	private bool _receivedPurchaseList;

	private bool _requestingProductData;

	[method: MethodImpl(32)]
	public static event Action<Purchase> OnPurchaseSuccessfull;

	static PurchaseManager()
	{
		_purchases = new List<Purchase>();
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.coinsA", Currency.coins, 9000, 0, MetroSkin.IconCoinsPackA, "$2.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.coinsB", Currency.coins, 15000, 1500, MetroSkin.IconCoinsPackB, "$4.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.coinsC", Currency.coins, 30000, 4500, MetroSkin.IconCoinsPackC, "$9.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.coinsD", Currency.coins, 60000, 12000, MetroSkin.IconCoinsPackD, "$19.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.coinsE", Currency.coins, 120000, 30000, MetroSkin.IconCoinsPackE, "$39.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.bucksA", Currency.bucks, 30, 0, MetroSkin.IconBucksPackA, "$2.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.bucksB", Currency.bucks, 50, 5, MetroSkin.IconBucksPackB, "$4.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.bucksC", Currency.bucks, 100, 15, MetroSkin.IconBucksPackC, "$9.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.bucksD", Currency.bucks, 200, 40, MetroSkin.IconBucksPackD, "$19.99"));
		_purchases.Add(new PurchasedCurrency("ca.roofdog.roadtrip2.bucksE", Currency.bucks, 400, 100, MetroSkin.IconBucksPackE, "$39.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.coinsB1", Currency.coins, 18000, "$4.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.coinsC1", Currency.coins, 40000, "$9.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.coinsD1", Currency.coins, 85000, "$19.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.coinsE1", Currency.coins, 181000, "$39.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.coinsF1", Currency.coins, 482000, "$99.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.bucksB1", Currency.bucks, 60, "$4.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.bucksC1", Currency.bucks, 130, "$9.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.bucksD1", Currency.bucks, 280, "$19.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.bucksE1", Currency.bucks, 600, "$39.99"));
		_purchases.Add(new SpecialOfferPurchasedCurrency("ca.roofdog.roadtrip2.bucksF1", Currency.bucks, 1600, "$99.99"));
		_purchases.Add(new PurchasedPack("ca.roofdog.roadtrip2.starterPack3", PackType.starter2, 0.99));
		_purchases.Add(new PurchasedPack("ca.roofdog.roadtrip2.starterPack2", PackType.starter, 2.99));
		_purchases.Add(new PurchasedPack("ca.roofdog.roadtrip2.boosterPack", PackType.booster, 0.99));
		_purchases.Add(new PurchasedFrenzyRuns("ca.roofdog.roadtrip2.frenzyRuns", 7));
		_purchases.Add(new PurchasedAdRemover("ca.roofdog.roadtrip2.adRemover"));
		_purchases.Add(new PurchasedPermanentCoinDoubler("ca.roofdog.roadtrip2.permanentCoinDoubler"));
	}

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		//AutoSingleton<PurchaseServicesGoogle>.Instance.Create();
		base.OnAwake();
	}

	public Purchase GetPurchase(string productIdentifier)
	{
		foreach (Purchase purchase in _purchases)
		{
			if (purchase.Identifier == productIdentifier)
			{
				return purchase;
			}
		}
		return null;
	}

	public PurchasedFrenzyRuns GetFrenzyRunPurchase()
	{
		return _purchases.Find((Purchase p) => p is PurchasedFrenzyRuns) as PurchasedFrenzyRuns;
	}

	public PurchasedPermanentCoinDoubler GetPermanentCoinDoublerPurchase()
	{
		return _purchases.Find((Purchase p) => p is PurchasedPermanentCoinDoubler) as PurchasedPermanentCoinDoubler;
	}

	public List<PurchasedCurrency> GetPurchasesByCurrency(Currency currency)
	{
		List<PurchasedCurrency> list = new List<PurchasedCurrency>();
		foreach (Purchase purchase in _purchases)
		{
			if (purchase is PurchasedCurrency)
			{
				PurchasedCurrency purchasedCurrency = purchase as PurchasedCurrency;
				if (purchasedCurrency.Currency == currency)
				{
					list.Add(purchasedCurrency);
				}
			}
		}
		return list;
	}

	public List<Purchase> GetNotConsumablePurchase()
	{
		List<Purchase> list = new List<Purchase>();
		foreach (Purchase purchase in _purchases)
		{
			if (purchase != null && !purchase.IsConsumable)
			{
				list.Add(purchase);
			}
		}
		return list;
	}

	public void onPurchaseSuccessful(string productIdentifier, string receipt, int quantity, ReceiptValidationStatus receiptStatus, string currencyCode, double productPrice, string formattedPrice, string transactionId, string purchaseData, string dataSignature)
	{
		Purchase purchase = GetPurchase(productIdentifier);
		purchase.Redeem();
		Preference.Save();
		Preference.SaveToDisk();
		AutoSingleton<PersistenceManager>.Instance.SpentMoney = true;
		ShowMessagePopup("Success!", "Thanks for your purchase!");
		bool sandbox = (receiptStatus == ReceiptValidationStatus.ValidSandbox) ? true : false;
		AutoSingleton<Rooflog>.Instance.LogPurchase(productIdentifier, receipt, purchase.DefaultPriceDouble, purchase.GetCategory(), sandbox, _restore, currencyCode, productPrice, formattedPrice, transactionId, purchaseData, dataSignature);
		_purchasing = false;
		if (PurchaseManager.OnPurchaseSuccessfull != null)
		{
			PurchaseManager.OnPurchaseSuccessfull(purchase);
		}
	}

	public void onPurchaseCancelled(string error, string payload = null)
	{
		_purchasing = false;
	}

	public void onPurchaseFailed(string error, bool silentFail = false)
	{
		if (!silentFail)
		{
			ShowMessagePopup("Purchase failed", error);
		}
		_purchasing = false;
	}

	public void OnProductReceived(bool received)
	{
		_requestingProductData = false;
		_receivedPurchaseList = received;
	}

	public string GetPurchasePriceString(Purchase purchase)
	{
		string empty = string.Empty;
		return null;//AutoSingleton<PurchaseServicesGoogle>.Instance.GetPurchasePriceString(purchase.Identifier);
	}

	public bool CanMakePayments()
	{
		return true;
	}

	public void RestorePurchases()
	{
		_restore = true;
	}

	public void RequestProductData()
	{
		if (!_requestingProductData)
		{
			_requestingProductData = true;
			int count = _purchases.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = _purchases[i].Identifier;
			}
			//AutoSingleton<PurchaseServicesGoogle>.Instance.RequestProductData(array);
		}
	}

	public void Buy(Purchase purchase)
	{
		if (!_purchasing)
		{
			_restore = false;
			string identifier = purchase.Identifier;
			_purchasing = true;
			//AutoSingleton<PurchaseServicesGoogle>.Instance.Buy(identifier);
		}
	}

	private static string FixCase(string identifier)
	{
		return identifier.ToLower();
	}

	private void ShowMessagePopup(string title, string message)
	{
		AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupMessage>().Setup(title, message), MetroAnimation.popup);
	}

	public void RefreshProductDataIfNeeded()
	{
		if (!_receivedPurchaseList)
		{
			RequestProductData();
		}
	}

	public bool IsConsumable(string productId)
	{
		return GetPurchase(productId)?.IsConsumable ?? true;
	}

	public void UpdateNotConsumableProductPurchased(List<string> productId, bool revokeFirst = true)
	{
		if (revokeFirst)
		{
			List<Purchase> notConsumablePurchase = GetNotConsumablePurchase();
			notConsumablePurchase.ForEach(delegate(Purchase prod)
			{
				prod.Revoke();
			});
		}
		foreach (string item in productId)
		{
			GetPurchase(item)?.Redeem();
		}
	}

	public void UpdateNotConsumableProductRefunded(List<string> productId)
	{
		foreach (string item in productId)
		{
			GetPurchase(item)?.Revoke();
		}
	}
}
