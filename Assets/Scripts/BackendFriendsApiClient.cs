using Roofdog;
using System;

public static class BackendFriendsApiClient
{
	public static void UpdateFriends(FriendIds friendIds, Action onSuccess, Action<string> onError)
	{
		if (string.IsNullOrEmpty(AutoSingleton<BackendSessionManager>.Instance.SessionToken))
		{
			onError?.Invoke("No Session Token");
		}
		else
		{
			AutoSingleton<BackendApiClient>.Instance.PostJson("/rest/friends/updates", null, friendIds.ToJsonData().toJson(), delegate
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
}
