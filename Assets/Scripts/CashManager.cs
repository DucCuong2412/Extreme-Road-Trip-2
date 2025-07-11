using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CashManager : AutoSingleton<CashManager>
{
	private PurchaseManager.PurchasedCurrency _currentPurchase;

	[method: MethodImpl(32)]
	public event Action<int> OnCoinsChanged;

	[method: MethodImpl(32)]
	public event Action<int> OnBucksChanged;

	[method: MethodImpl(32)]
	public event Action<int> OnPrestigeTokensChanged;

	protected override void OnAwake()
	{
		this.OnCoinsChanged = (Action<int>)Delegate.Combine(this.OnCoinsChanged, new Action<int>(AutoSingleton<Player>.Instance.Profile.OnCoinsChanged));
		this.OnBucksChanged = (Action<int>)Delegate.Combine(this.OnBucksChanged, new Action<int>(AutoSingleton<Player>.Instance.Profile.OnBucksChanged));
		this.OnPrestigeTokensChanged = (Action<int>)Delegate.Combine(this.OnPrestigeTokensChanged, new Action<int>(AutoSingleton<Player>.Instance.Profile.OnPrestigeTokensChanged));
		_currentPurchase = null;
		base.OnAwake();
	}

	public int Coins()
	{
		return AutoSingleton<Player>.Instance.Profile.Coins;
	}

	public int Bucks()
	{
		return AutoSingleton<Player>.Instance.Profile.Bucks;
	}

	public int PrestigeTokens()
	{
		return AutoSingleton<Player>.Instance.Profile.PrestigeTokens;
	}

	public void AddCoins(int amountToAdd)
	{
		int obj = AutoSingleton<Player>.Instance.Profile.Coins + amountToAdd;
		if (this.OnCoinsChanged != null)
		{
			this.OnCoinsChanged(obj);
		}
	}

	public void AddBucks(int amountToAdd)
	{
		int obj = AutoSingleton<Player>.Instance.Profile.Bucks + amountToAdd;
		if (this.OnBucksChanged != null)
		{
			this.OnBucksChanged(obj);
		}
	}

	public void AddPrestigeTokens(int amountToAdd)
	{
		int obj = AutoSingleton<Player>.Instance.Profile.PrestigeTokens + amountToAdd;
		if (this.OnPrestigeTokensChanged != null)
		{
			this.OnPrestigeTokensChanged(obj);
		}
	}

	public void RemoveCoins(int amountToRemove)
	{
		int obj = AutoSingleton<Player>.Instance.Profile.Coins - amountToRemove;
		if (this.OnCoinsChanged != null)
		{
			this.OnCoinsChanged(obj);
		}
		AutoSingleton<GameStatsManager>.Instance.RecordCoinsSpent(amountToRemove);
	}

	public void RemoveBucks(int amountToRemove)
	{
		int obj = AutoSingleton<Player>.Instance.Profile.Bucks - amountToRemove;
		if (this.OnBucksChanged != null)
		{
			this.OnBucksChanged(obj);
		}
		AutoSingleton<GameStatsManager>.Instance.RecordBucksSpent(amountToRemove);
	}

	public void RemovePrestigeTokens(int amountToRemove)
	{
		int obj = AutoSingleton<Player>.Instance.Profile.PrestigeTokens - amountToRemove;
		if (this.OnPrestigeTokensChanged != null)
		{
			this.OnPrestigeTokensChanged(obj);
		}
		AutoSingleton<GameStatsManager>.Instance.RecordPrestigeTokensSpent(amountToRemove);
	}

	public bool CanBuy(Price price)
	{
		if (price.IsCoins())
		{
			return Coins() >= price.Amount;
		}
		if (price.IsBucks())
		{
			return Bucks() >= price.Amount;
		}
		return PrestigeTokens() >= price.Amount;
	}

	public bool CanBuy(List<Price> prices)
	{
		bool flag = true;
		foreach (Price price in prices)
		{
			flag &= CanBuy(price);
		}
		return flag;
	}

	public void Buy(Price price)
	{
		if (CanBuy(price))
		{
			if (price.IsCoins())
			{
				RemoveCoins(price.Amount);
			}
			else if (price.IsBucks())
			{
				RemoveBucks(price.Amount);
			}
			else
			{
				RemovePrestigeTokens(price.Amount);
			}
		}
	}

	public void Buy(List<Price> prices)
	{
		if (CanBuy(prices))
		{
			foreach (Price price in prices)
			{
				Buy(price);
			}
		}
	}

	public void HandleNotEnoughCurrency(Price price, bool pauseOnFocus)
	{
		List<Price> list = new List<Price>();
		list.Add(price);
		HandleNotEnoughCurrency(list, pauseOnFocus);
	}

	public void HandleNotEnoughCurrency(List<Price> prices, bool pauseOnFocus)
	{
		using (List<Price>.Enumerator enumerator = prices.GetEnumerator())
		{
			Price price;
			while (enumerator.MoveNext())
			{
				price = enumerator.Current;
				if (!CanBuy(price))
				{
					PurchaseManager instance = AutoSingleton<PurchaseManager>.Instance;
					List<PurchaseManager.PurchasedCurrency> purchasesByCurrency = instance.GetPurchasesByCurrency(price.Currency);
					if (purchasesByCurrency != null && purchasesByCurrency.Count > 0)
					{
						int num = 0;
						string s = price.Currency.ToString();
						num = ((!price.IsCoins()) ? (price.Amount - Bucks()) : (price.Amount - Coins()));
						purchasesByCurrency.Sort((PurchaseManager.PurchasedCurrency p1, PurchaseManager.PurchasedCurrency p2) => p1.Amount - p2.Amount);
						int num2 = purchasesByCurrency.FindIndex((PurchaseManager.PurchasedCurrency p) => p.Amount >= price.Amount);
						_currentPurchase = ((num2 < 0) ? purchasesByCurrency[purchasesByCurrency.Count - 1] : purchasesByCurrency[num2]);
						string message = num.ToString() + " " + s.Localize() + " " + "needed to complete purchase.".Localize() + " " + "Get".Localize() + " " + _currentPurchase.Amount.ToString() + " " + "for".Localize() + " " + instance.GetPurchasePriceString(_currentPurchase) + "?";
						string title = "Not enough".Localize() + " " + s.Localize() + "!";
						string yes = "Buy".Localize();
						string no = "Cancel".Localize();
						MetroMenuPopupYesNoLater metroMenuPopupYesNoLater = MetroMenuPage.Create<MetroMenuPopupYesNoLater>().Setup(title, message, yes, no, MetroSkin.Slice9ButtonRed);
						if (pauseOnFocus)
						{
							metroMenuPopupYesNoLater.OnFocusGained += delegate
							{
								AutoSingleton<PauseManager>.Instance.Pause();
							};
						}
						metroMenuPopupYesNoLater.OnButtonYes(delegate
						{
							AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
							AlertButtonClicked(yes);
						});
						metroMenuPopupYesNoLater.OnButtonNo(delegate
						{
							AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
							AlertButtonClicked(no);
						});
						AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPopupYesNoLater);
					}
					else if (price.IsPrestigeTokens())
					{
						int num3 = price.Amount - PrestigeTokens();
						string titleString = "Not enough".Localize() + " " + "Prestige Tokens".Localize() + "!";
						string messageString = num3.ToString() + " " + "Prestige Tokens".Localize() + " " + "needed to complete purchase.".Localize() + " " + "You can obtain Prestige Tokens by completing 100% of the missions for any car.".Localize();
						MetroPopupMessage metroPopupMessage = MetroMenuPage.Create<MetroPopupMessage>().Setup(titleString, messageString, "OK", MetroSkin.Slice9ButtonRed, DismissedPopup);
						if (pauseOnFocus)
						{
							metroPopupMessage.OnFocusGained += delegate
							{
								AutoSingleton<PauseManager>.Instance.Pause();
							};
						}
						AutoSingleton<MetroMenuStack>.Instance.Push(metroPopupMessage);
					}
					else
					{
						UnityEngine.Debug.LogError("No purchases available!");
					}
				}
				else
				{
					UnityEngine.Debug.LogWarning("User has enough money to make the purchase! This should not be shown.");
				}
			}
		}
	}

	private void DismissedPopup()
	{
		AutoSingleton<PauseManager>.Instance.Resume();
		_currentPurchase = null;
		AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
	}

	private void AlertButtonClicked(string button)
	{
		if (_currentPurchase != null && button == "Buy".Localize())
		{
			AutoSingleton<PurchaseManager>.Instance.Buy(_currentPurchase);
		}
		AutoSingleton<PauseManager>.Instance.Resume();
		_currentPurchase = null;
	}

	public void ForceRefresh()
	{
		AddCoins(0);
		AddBucks(0);
	}
}
