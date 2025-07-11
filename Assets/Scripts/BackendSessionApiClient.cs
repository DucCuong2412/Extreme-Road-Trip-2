using Prime31;
using Roofdog;
using System;

public static class BackendSessionApiClient
{
	private static Action<SessionStart> _sessionStartListener;

	public static void AddSessionStartListener(Action<SessionStart> listener)
	{
		_sessionStartListener = (Action<SessionStart>)Delegate.Remove(_sessionStartListener, listener);
		_sessionStartListener = (Action<SessionStart>)Delegate.Combine(_sessionStartListener, listener);
	}

	public static void RemoveSessionStartListener(Action<SessionStart> listener)
	{
		_sessionStartListener = (Action<SessionStart>)Delegate.Remove(_sessionStartListener, listener);
	}

	public static void StartSession(Action<SessionStart> onSessionStart, Action<string> onError)
	{
		Credentials credentials = new Credentials(AutoSingleton<BackendSessionManager>.Instance.RoofdogIdentity.Id, AutoSingleton<BackendSessionManager>.Instance.RoofdogIdentity.Pwd, string.Empty, (!AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn()) ? string.Empty : AutoSingleton<GameFacebookManager>.Instance.PlayerId(), (!AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn()) ? string.Empty : Facebook.instance.accessToken, (!AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn()) ? string.Empty : AutoSingleton<GameFacebookManager>.Instance.PlayerAlias(), string.Empty, string.Empty, string.Empty, string.Empty);
		AutoSingleton<BackendApiClient>.Instance.PostJson("/rest/sessions", null, credentials.ToJsonData().toJson(), delegate(HttpResponse response)
		{
			string stringBody = response.GetStringBody();
			SessionStart sessionStart = SessionStart.FromJsonData(stringBody.hashtableFromJson());
			AutoSingleton<BackendSessionManager>.Instance.OnSessionStarted(sessionStart);
			if (_sessionStartListener != null)
			{
				_sessionStartListener(sessionStart);
			}
			if (onSessionStart != null)
			{
				onSessionStart(sessionStart);
			}
		}, onError, new int[1]
		{
			200
		}, HttpRetryOptions.DEFAULT_RETRY);
	}
}
