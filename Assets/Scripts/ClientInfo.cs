using System;
using System.Collections;
using System.Globalization;

public class ClientInfo
{
	public string Version
	{
		get;
		private set;
	}

	public string Platform
	{
		get;
		private set;
	}

	public string DeviceId
	{
		get;
		private set;
	}

	public string Language
	{
		get;
		private set;
	}

	public string PushToken
	{
		get;
		private set;
	}

	public DateTime LocalTime
	{
		get;
		private set;
	}

	public ClientInfo(string version, string platform, string deviceId, string language, string pushToken, DateTime localTime)
	{
		Version = version;
		Platform = platform;
		DeviceId = deviceId;
		Language = language;
		PushToken = pushToken;
		LocalTime = localTime;
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Version"] = Version;
		hashtable["Platform"] = Platform;
		hashtable["DeviceId"] = DeviceId;
		hashtable["Language"] = Language;
		hashtable["PushToken"] = PushToken;
		hashtable["LocalTime"] = LocalTime.ToString("s", CultureInfo.InvariantCulture);
		return hashtable;
	}
}
