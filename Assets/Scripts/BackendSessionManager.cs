using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackendSessionManager : AutoSingleton<BackendSessionManager>
{
	private const string ROOFDOG_IDENTITY_PREF_KEY = "RoofdogIdentity";

	public string SessionToken
	{
		get;
		private set;
	}

	public RoofdogIdentity RoofdogIdentity
	{
		get;
		private set;
	}

	[method: MethodImpl(32)]
	public static event Action OnSessionStartedEvent;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		LoadIdentity();
		base.OnAwake();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public bool IsSessionValid()
	{
		return !string.IsNullOrEmpty(SessionToken);
	}

	public string GetPublicId()
	{
		if (IsSessionValid())
		{
			RoofdogIdentity roofdogIdentity = RoofdogIdentity;
			if (roofdogIdentity != null && !string.IsNullOrEmpty(roofdogIdentity.PublicId))
			{
				return roofdogIdentity.PublicId;
			}
		}
		return string.Empty;
	}

	public void OnSessionStarted(SessionStart sessionStart)
	{
		SessionToken = sessionStart.SessionToken;
		if (sessionStart.CreatedIdentity != null)
		{
			RoofdogIdentity = sessionStart.CreatedIdentity;
			SaveIdentity();
		}
		if (BackendSessionManager.OnSessionStartedEvent != null)
		{
			BackendSessionManager.OnSessionStartedEvent();
		}
	}

	private void LoadIdentity()
	{
		string @string = Preference.GetString("RoofdogIdentity", "{}");
		RoofdogIdentity = RoofdogIdentity.FromJsonData(@string.hashtableFromJson());
	}

	private void SaveIdentity()
	{
		if (RoofdogIdentity != null)
		{
			Preference.SetString("RoofdogIdentity", RoofdogIdentity.ToJsonData().toJson());
			Preference.Save();
		}
	}
}
