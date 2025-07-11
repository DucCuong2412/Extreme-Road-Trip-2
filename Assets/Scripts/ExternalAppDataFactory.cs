using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExternalAppDataFactory
{
	public static Dictionary<string, ExternalAppData> FromHashtable(Hashtable data)
	{
		if (data != null)
		{
			Dictionary<string, ExternalAppData> dictionary = new Dictionary<string, ExternalAppData>();
			{
				foreach (DictionaryEntry datum in data)
				{
					string text = datum.Key as string;
					Hashtable hashtable = datum.Value as Hashtable;
					if (!string.IsNullOrEmpty(text) && hashtable != null && hashtable.Count > 0)
					{
						ExternalAppData externalAppData = ExternalAppData.FromJsonData(hashtable);
						if (externalAppData != null && externalAppData.IsValid())
						{
							dictionary[text] = externalAppData;
						}
					}
				}
				return dictionary;
			}
		}
		UnityEngine.Debug.LogError("data Shouldn't be null");
		return null;
	}
}
