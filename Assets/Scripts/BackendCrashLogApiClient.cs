using Roofdog;
using System;

public static class BackendCrashLogApiClient
{
	public static void SendCrashLog(long timestamp, string crashData, Action onSuccess = null, Action<string> onError = null)
	{
		string jsonString = new CrashLogJson(timestamp, crashData).ToJsonData().toJson();
		AutoSingleton<BackendApiClient>.Instance.PostJson("/rest/crashLogs", null, jsonString, delegate
		{
			if (onSuccess != null)
			{
				onSuccess();
			}
		}, onError, new int[1]
		{
			204
		}, HttpRetryOptions.DEFAULT_RETRY);
	}
}
