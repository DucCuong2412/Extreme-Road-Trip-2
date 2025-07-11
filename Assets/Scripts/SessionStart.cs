using System.Collections;

public class SessionStart
{
	public string SessionToken
	{
		get;
		private set;
	}

	public RoofdogIdentity CreatedIdentity
	{
		get;
		private set;
	}

	public static SessionStart FromJsonData(Hashtable data)
	{
		SessionStart sessionStart = new SessionStart();
		sessionStart.SessionToken = JsonUtil.ExtractString(data, "SessionToken", null);
		sessionStart.CreatedIdentity = RoofdogIdentity.FromJsonData(JsonUtil.ExtractHashtable(data, "CreatedIdentity"));
		return sessionStart;
	}

	public override string ToString()
	{
		return $"[SessionStart: SessionToken={SessionToken}, RoofdogIdentity={CreatedIdentity}]";
	}
}
