public class MessageActionDismissed : AnalyticEvent, IRoofdogEvent
{
	private RoofdogEventData _rdData;

	public MessageActionDismissed(string contextId)
	{
		string value = string.IsNullOrEmpty(contextId) ? string.Empty : contextId;
		_rdData = new RoofdogEventData(RoofdogAnalyticsManager.EventCategory.messaging);
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.eventAction, ERT2AnalyticEvent.messageActionDismissed.ToString());
		_rdData.AddData(RoofdogAnalyticsManager.Parameter.paramStr1, value);
	}

	public RoofdogEventData ToRoofdogEventData()
	{
		return _rdData;
	}
}
