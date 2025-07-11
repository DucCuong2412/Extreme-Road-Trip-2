using System.Collections;

internal class CrashLogJson
{
	public long Timestamp
	{
		get;
		private set;
	}

	public string CrashData
	{
		get;
		private set;
	}

	public CrashLogJson(long timestamp, string crashData)
	{
		Timestamp = timestamp;
		CrashData = crashData;
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Timestamp"] = Timestamp;
		hashtable["CrashData"] = CrashData;
		return hashtable;
	}
}
