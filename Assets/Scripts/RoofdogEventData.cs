using System.Collections;

public class RoofdogEventData
{
	public Hashtable Data
	{
		get;
		set;
	}

	public RoofdogEventData(RoofdogAnalyticsManager.EventCategory category)
	{
		Data = new Hashtable();
		AddData(RoofdogAnalyticsManager.Parameter.eventCategory, category.ToString());
	}

	public override string ToString()
	{
		string result = string.Empty;
		if (Data != null)
		{
			result = Data.toJson();
		}
		return result;
	}

	public void AddData(RoofdogAnalyticsManager.Parameter param, int value)
	{
		Data[param.ToString()] = value;
	}

	public void AddData(RoofdogAnalyticsManager.Parameter param, string value)
	{
		Data[param.ToString()] = value;
	}

	public void AddData(RoofdogAnalyticsManager.Parameter param, float value)
	{
		Data[param.ToString()] = value;
	}
}
