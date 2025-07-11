public static class AnalyticsEventDispatcher
{
	private static class RoofdogAnalyticsHandler
	{
		public static void OnEvent(AnalyticEvent evt)
		{
			IRoofdogEvent roofdogEvent = evt as IRoofdogEvent;
			if (roofdogEvent != null)
			{
				RoofdogEventData roofdogEventData = roofdogEvent.ToRoofdogEventData();
				if (roofdogEventData != null)
				{
					AutoSingleton<RoofdogAnalyticsManager>.Instance.LogEvent(roofdogEventData);
				}
			}
		}
	}

	public static void OnEvent(AnalyticEvent evt)
	{
		TapjoyAnalyticsEventHandler.OnEvent(evt);
	}
}
