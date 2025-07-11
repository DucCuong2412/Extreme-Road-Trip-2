public class InGameIncomeMessage : AnalyticEvent, IRoofdogEvent
{
	private RoofdogEventData _rdData;

	public InGameIncomeMessage(string contextId, Reward reward)
	{
		string value = string.IsNullOrEmpty(contextId) ? string.Empty : contextId;
		_rdData = new RoofdogEventData(RoofdogAnalyticsManager.EventCategory.inGameIncome);
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.eventAction, ERT2AnalyticEvent.messageReward.ToString());
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.paramStr1, reward.GetRewardType().ToString());
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.paramNum1, reward.Amount);
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.paramStr2, value);
	}

	public RoofdogEventData ToRoofdogEventData()
	{
		return _rdData;
	}
}
