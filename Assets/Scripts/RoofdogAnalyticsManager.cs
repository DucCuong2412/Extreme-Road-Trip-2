using System;
using System.Collections;
using UnityEngine;

public class RoofdogAnalyticsManager : AutoSingleton<RoofdogAnalyticsManager>
{
	public enum EventCategory
	{
		sessionActivity,
		inGameExpense,
		inGameIncome,
		gameplay,
		inAppPurchase,
		metaGame,
		social,
		messaging,
		specialOffer
	}

	public enum Header
	{
		osVersion,
		deviceModel,
		timeStamp,
		totalMoneySpent,
		totalTimeInApp,
		timeInSession,
		sessionCount,
		totalAdsWatched,
		totalOffersCompleted
	}

	public enum Parameter
	{
		eventCategory,
		eventAction,
		paramStr1,
		paramStr2,
		paramStr3,
		paramNum1,
		paramNum2,
		paramNum3
	}

	public delegate void AddExternalData(RoofdogEventData data);

	private static AddExternalData _externalAddData;

	public void LogEvent(RoofdogEventData evt)
	{
		if (evt != null && evt.Data != null)
		{
			Hashtable data = evt.Data;
			data = AddGlobalData(data);
			if (_externalAddData != null)
			{
				_externalAddData(evt);
			}
		}
	}

	private static Hashtable AddGlobalData(Hashtable data)
	{
		data[Header.osVersion.ToString()] = Device.GetDeviceAPIVersionInt();
		data[Header.deviceModel.ToString()] = Device.GetDeviceForm();
		data[Header.timeStamp.ToString()] = Math.Truncate((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
		data[Header.totalMoneySpent.ToString()] = AutoSingleton<RoofdogAnalyticData>.Instance.GetTotalMoneySpendCent();
		data[Header.totalTimeInApp.ToString()] = AutoSingleton<RoofdogAnalyticData>.Instance.GetTotalSecondInApp();
		data[Header.timeInSession.ToString()] = AutoSingleton<RoofdogAnalyticData>.Instance.GetTimeSecondInSession();
		data[Header.sessionCount.ToString()] = AutoSingleton<RoofdogAnalyticData>.Instance.GetSessionCount();
		data[Header.totalAdsWatched.ToString()] = AutoSingleton<RoofdogAnalyticData>.Instance.GetTotalAdReward();
		data[Header.totalOffersCompleted.ToString()] = AutoSingleton<RoofdogAnalyticData>.Instance.GetTapjoyRewardCount();
		return data;
	}

	public static void RegisterExternalDataHeader(AddExternalData fct)
	{
		_externalAddData = fct;
	}

	public static RoofdogEventData ToData(EventCategory cat, string action, string paramStr1 = null, string paramStr2 = null, string paramStr3 = null, int paramInt1 = int.MaxValue, int paramInt2 = int.MaxValue, int paramInt3 = int.MaxValue)
	{
		if (paramInt1 < 0 || paramInt2 < 0 || paramInt3 < 0)
		{
			UnityEngine.Debug.LogWarning("Roofdog Analytics doesn't accept negative number");
			return null;
		}
		RoofdogEventData roofdogEventData = new RoofdogEventData(cat);
		Hashtable hashtable = new Hashtable();
		hashtable[Parameter.eventCategory.ToString()] = cat.ToString();
		hashtable[Parameter.eventAction.ToString()] = action;
		hashtable = AddIfNotNull(hashtable, Parameter.paramStr1.ToString(), paramStr1);
		hashtable = AddIfNotNull(hashtable, Parameter.paramStr2.ToString(), paramStr2);
		hashtable = AddIfNotNull(hashtable, Parameter.paramStr3.ToString(), paramStr3);
		hashtable = AddIfNotDefault(hashtable, Parameter.paramNum1.ToString(), paramInt1);
		hashtable = AddIfNotDefault(hashtable, Parameter.paramNum2.ToString(), paramInt2);
		hashtable = (roofdogEventData.Data = AddIfNotDefault(hashtable, Parameter.paramNum3.ToString(), paramInt3));
		return roofdogEventData;
	}

	private static Hashtable AddIfNotNull(Hashtable ht, string key, string value)
	{
		if (value != null)
		{
			ht[key] = value;
		}
		return ht;
	}

	private static Hashtable AddIfNotDefault(Hashtable ht, string key, int value)
	{
		if (value != int.MaxValue)
		{
			ht[key] = value;
		}
		return ht;
	}
}
