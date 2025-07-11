using Roofdog;
using System;
using UnityEngine;

public static class BackendIdentityApiClient
{
	public static void DeleteLinks(string fromRoofdogId, Action onCompletion, Action<string> onError)
	{
		AutoSingleton<BackendApiClient>.Instance.Delete("/rest/identities/" + WWW.EscapeURL(fromRoofdogId) + "/links", null, delegate
		{
			if (onCompletion != null)
			{
				onCompletion();
			}
		}, onError, new int[2]
		{
			200,
			204
		}, HttpRetryOptions.DEFAULT_RETRY);
	}
}
