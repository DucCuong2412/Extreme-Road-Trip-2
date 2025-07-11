using System.Collections.Generic;
using UnityEngine;

public class IncomeEndRun : AnalyticEvent, IRoofdogEvent
{
	private RoofdogEventData _roofdogData;

	public IncomeEndRun(GameMode mode, int coinsAmount, int bucksAmount, int distance, string carId)
	{
		_roofdogData = new RoofdogEventData(RoofdogAnalyticsManager.EventCategory.inGameIncome);
		_roofdogData.AddData(RoofdogAnalyticsManager.Parameter.eventAction, ERT2AnalyticEvent.endRun.ToString());
		_roofdogData.AddData(RoofdogAnalyticsManager.Parameter.paramNum1, coinsAmount);
		_roofdogData.AddData(RoofdogAnalyticsManager.Parameter.paramNum2, bucksAmount);
		_roofdogData.AddData(RoofdogAnalyticsManager.Parameter.paramNum3, distance);
		_roofdogData.AddData(RoofdogAnalyticsManager.Parameter.paramStr1, mode.ToString());
		string value = string.IsNullOrEmpty(carId) ? string.Empty : carId;
		_roofdogData.AddData(RoofdogAnalyticsManager.Parameter.paramStr2, value);
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			["coins"] = coinsAmount.ToString()
		};
		switch (mode)
		{
		case GameMode.normal:
			break;
		case GameMode.frenzy:
			dictionary["bucks"] = bucksAmount.ToString();
			break;
		default:
			UnityEngine.Debug.Log("LogEndRun() - Unknown game mode: " + mode.ToString());
			break;
		}
	}

	public RoofdogEventData ToRoofdogEventData()
	{
		return _roofdogData;
	}
}
