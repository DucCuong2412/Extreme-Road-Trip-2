using System;
using System.Collections;
using UnityEngine;

public class FrenzyModeManager : AutoSingleton<FrenzyModeManager>
{
	private PersistentInt _frenzyRunInventory;

	private PersistentString _lastFrenzyRunDate;

	private bool _handled;

	private bool _isDestroyed;

	protected override void OnAwake()
	{
		_isDestroyed = false;
		_frenzyRunInventory = new PersistentInt("FrenzyRunInventory", 1);
		_lastFrenzyRunDate = new PersistentString("LastFrenzyRunDate", DateTime.MinValue.ToString());
		StartCoroutine(UpdateCR());
		base.OnAwake();
	}

	public override void Destroy()
	{
		_isDestroyed = true;
		base.Destroy();
	}

	public void AddRuns(int amount)
	{
		if (!_isDestroyed)
		{
			_frenzyRunInventory.Set(_frenzyRunInventory.Get() + amount);
			Preference.Save();
		}
	}

	public bool HasFreeAccess()
	{
		return _frenzyRunInventory.Get() > 0;
	}

	public string GetBadgeCaption()
	{
		if (HasFreeAccess())
		{
			return _frenzyRunInventory.Get().ToString();
		}
		return string.Empty;
	}

	public string GetTimerString()
	{
		if (!HasFreeAccess())
		{
			return TimeUtil.Format((float)(NextRunDate() - DateTime.Now).TotalSeconds);
		}
		return string.Empty;
	}

	public void HandleAccess()
	{
		if (!_handled)
		{
			if (HasFreeAccess())
			{
				_handled = true;
				_frenzyRunInventory.Set(_frenzyRunInventory.Get() - 1);
				_lastFrenzyRunDate.Set(DateTime.Now.ToString());
				Preference.Save();
				AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigGame(AutoSingleton<CarManager>.Instance.GetCar("Frenzymonster"), GameMode.frenzy));
			}
			else
			{
				string title = "No More Frenzy Run Available!";
				string message = "Do you want to purchase 7 Frenzy Runs right now?";
				string yes = "PURCHASE";
				string no = "LATER";
				MetroMenuPopupYesNoLater metroMenuPopupYesNoLater = MetroMenuPage.Create<MetroMenuPopupYesNoLater>().Setup(title, message, yes, no, MetroSkin.Slice9ButtonRed);
				metroMenuPopupYesNoLater.OnButtonYes(delegate
				{
					AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
					MakePurchase();
				});
				metroMenuPopupYesNoLater.OnButtonNo(delegate
				{
					AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
				});
				AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPopupYesNoLater, MetroAnimation.popup);
			}
		}
	}

	private DateTime LastRunDate()
	{
		return DateTime.Parse(_lastFrenzyRunDate.Get());
	}

	private IEnumerator UpdateCR()
	{
		while (true)
		{
			int inv = _frenzyRunInventory.Get();
			if (inv <= 0 && NextRunDate().CompareTo(DateTime.Now) <= 0)
			{
				_frenzyRunInventory.Set(inv + 1);
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	public DateTime NextRunDate()
	{
		DateTime dateTime = LastRunDate();
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1.0).AddHours(8.0);
	}

	private void MakePurchase()
	{
		PurchaseManager instance = AutoSingleton<PurchaseManager>.Instance;
		PurchaseManager.PurchasedFrenzyRuns frenzyRunPurchase = instance.GetFrenzyRunPurchase();
		if (frenzyRunPurchase != null)
		{
			AutoSingleton<PurchaseManager>.Instance.Buy(frenzyRunPurchase);
		}
	}

	public void Refresh()
	{
		_frenzyRunInventory = new PersistentInt("FrenzyRunInventory", 1);
	}
}
