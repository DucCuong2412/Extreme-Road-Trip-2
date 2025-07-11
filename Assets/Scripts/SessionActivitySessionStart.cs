public class SessionActivitySessionStart : AnalyticEvent, IRoofdogEvent
{
	public SessionActivitySessionStart(bool resume)
	{
	}

	public RoofdogEventData ToRoofdogEventData()
	{
		return RoofdogAnalyticsManager.ToData(RoofdogAnalyticsManager.EventCategory.sessionActivity, ERT2AnalyticEvent.sessionStart.ToString());
	}
}
