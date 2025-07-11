using System.Collections;
using System.Collections.Generic;

public class ExternalAppData
{
	private enum jsonLabel
	{
		DisplayName,
		PlatformAppData
	}

	public string DisplayName;

	public Dictionary<string, ExternalAppPlaformSpecificData> PlatformData;

	private ExternalAppData(string displayName, Dictionary<string, ExternalAppPlaformSpecificData> platformData = null)
	{
		DisplayName = displayName;
		if (platformData != null)
		{
			PlatformData = platformData;
		}
		else
		{
			PlatformData = new Dictionary<string, ExternalAppPlaformSpecificData>();
		}
	}

	public bool IsValid()
	{
		return PlatformData != null && PlatformData.Count > 0;
	}

	public void AddPlatform(string platformId, ExternalAppPlaformSpecificData data)
	{
		PlatformData[platformId] = data;
	}

	public static ExternalAppData FromJsonData(Hashtable jsonData)
	{
		string displayName = JsonUtil.ExtractString(jsonData, jsonLabel.DisplayName.ToString(), string.Empty);
		Hashtable hashtable = JsonUtil.ExtractHashtable(jsonData, jsonLabel.PlatformAppData.ToString(), new Hashtable());
		Dictionary<string, ExternalAppPlaformSpecificData> dictionary = new Dictionary<string, ExternalAppPlaformSpecificData>();
		foreach (DictionaryEntry item in hashtable)
		{
			Hashtable hashtable2 = item.Value as Hashtable;
			string text = item.Key as string;
			if (!string.IsNullOrEmpty(text) && hashtable2 != null && hashtable2.Count > 0)
			{
				ExternalAppPlaformSpecificData externalAppPlaformSpecificData = ExternalAppPlaformSpecificData.FromJsonData(hashtable2);
				if (externalAppPlaformSpecificData != null && externalAppPlaformSpecificData.IsValid())
				{
					dictionary[text] = externalAppPlaformSpecificData;
				}
			}
		}
		if (dictionary.Count > 0)
		{
			return new ExternalAppData(displayName, dictionary);
		}
		return null;
	}
}
