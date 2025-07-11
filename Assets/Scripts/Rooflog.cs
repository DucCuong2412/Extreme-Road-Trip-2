using System;
using System.Collections.Generic;
using UnityEngine;

public class Rooflog : AutoSingleton<Rooflog>
{
	private DateTime _pauseTime;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		AutoSingleton<RoofdogAnalyticData>.Instance.Create();
		RoofdogAnalyticsManager.RegisterExternalDataHeader(AddExternalData);
		OnSessionStart();
	}

	private static void AddExternalData(RoofdogEventData evt)
	{
		if (AutoSingleton<Rooflog>.IsCreated() && evt != null && evt.Data != null)
		{
			PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
			evt.Data["friendCount"] = AutoSingleton<BackendManager>.Instance.GetAllFriendCount().ToString();
			evt.Data["playerXp"] = Mathf.RoundToInt(profile.XPProfile.XP).ToString();
			evt.Data["playerLevel"] = profile.XPProfile.Level.ToString();
			evt.Data["buck"] = profile.Bucks.ToString();
			evt.Data["coin"] = profile.Coins.ToString();
		}
	}

	private void OnSessionStart(bool resume = false)
	{
		if (AutoSingleton<RoofdogAnalyticData>.IsCreated())
		{
			AutoSingleton<RoofdogAnalyticData>.Instance.OnSessionStarted();
		}
		AnalyticsEventDispatcher.OnEvent(new SessionActivitySessionStart(resume));
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			_pauseTime = DateTime.Now;
		}
		else if ((DateTime.Now - _pauseTime).TotalSeconds > 60.0)
		{
			OnSessionStart(resume: true);
		}
	}

	public void LogPurchase(string productId, string receipt, double gamePrice, string category, bool sandbox, bool restore, string currencyCode, double productPrice, string formattedPrice, string transactionId, string purchaseData, string dataSignature)
	{
		if (!sandbox)
		{
			if (!restore)
			{
				AutoSingleton<RoofdogAnalyticData>.Instance.OnMoneySpend(gamePrice);
				AnalyticsEventDispatcher.OnEvent(new InAppPurchase(productId, gamePrice, category, sandbox, restore, currencyCode, productPrice, formattedPrice, transactionId, purchaseData, dataSignature));
			}
		}
		else
		{
			SilentDebug.LogWarning("Sandbox receipt, dont log");
		}
	}

	public void LogEndRun(GameMode mode, int coins, int bucks, int distance, string carId)
	{
		AnalyticsEventDispatcher.OnEvent(new IncomeEndRun(mode, coins, bucks, distance, carId));
	}

	public void OnTapjoyReward(Reward r)
	{
		AutoSingleton<RoofdogAnalyticData>.Instance.OnTapjoyReward(r.Amount);
	}

	public void OnVideoViewed()
	{
		AutoSingleton<RoofdogAnalyticData>.Instance.OnAdReward();
	}

	public void OnGenericMessageActionTaken(string uid, string contextId, GenericMessageActionTaken action)
	{
		switch (action)
		{
		case GenericMessageActionTaken.taken:
			AnalyticsEventDispatcher.OnEvent(new MessageActionTaken(contextId));
			break;
		case GenericMessageActionTaken.dismissed:
			AnalyticsEventDispatcher.OnEvent(new MessageActionDismissed(contextId));
			break;
		default:
			SilentDebug.LogWarning("Unkown action");
			break;
		}
	}

	public void OnGenericMessageRead(string uid, string contextId)
	{
		AnalyticsEventDispatcher.OnEvent(new MessageViewed(contextId));
	}

	public void OnGenericMessageRewardRedeemed(string uid, string contextId, List<Reward> rewards)
	{
		if (rewards != null)
		{
			foreach (Reward reward in rewards)
			{
				if (reward != null)
				{
					AnalyticsEventDispatcher.OnEvent(new InGameIncomeMessage(contextId, reward));
				}
			}
		}
	}
}
