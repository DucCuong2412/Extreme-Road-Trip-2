using Roofdog;
using System;

public static class BackendAnalyticsApiClient
{
	public static void PostAnalyticsEvent(string jsonData, Action onSuccess = null, Action<string> onError = null)
	{
		PostEvent("/rest/events/analytics", jsonData, onSuccess, onError);
	}

	private static void PostEvent(string path, string jsonData, Action onSuccess, Action<string> onError)
	{
		AutoSingleton<BackendApiClient>.Instance.PostJson(path, null, jsonData, delegate
		{
			if (onSuccess != null)
			{
				onSuccess();
			}
		}, onError, new int[1]
		{
			204
		}, HttpRetryOptions.NO_RETRY);
	}
}
