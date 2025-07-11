using System.Collections;

public class ExternalAppPlaformSpecificData
{
	private enum jsonLabel
	{
		AppId,
		StoreURL
	}

	public string AppId;

	public string StoreURL;

	private ExternalAppPlaformSpecificData(string id, string storeUrl)
	{
		AppId = id;
		StoreURL = storeUrl;
	}

	public bool IsValid()
	{
		return !string.IsNullOrEmpty(AppId);
	}

	public static ExternalAppPlaformSpecificData FromJsonData(Hashtable jsonData)
	{
		string id = JsonUtil.ExtractString(jsonData, jsonLabel.AppId.ToString(), string.Empty);
		string storeUrl = JsonUtil.ExtractString(jsonData, jsonLabel.StoreURL.ToString(), string.Empty);
		return new ExternalAppPlaformSpecificData(id, storeUrl);
	}
}
