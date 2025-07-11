using System.Collections;

public class Credentials
{
	public string RoofdogId
	{
		get;
		set;
	}

	public string RoofdogPwd
	{
		get;
		set;
	}

	public string RoofdogName
	{
		get;
		set;
	}

	public string FacebookId
	{
		get;
		set;
	}

	public string FacebookToken
	{
		get;
		set;
	}

	public string FacebookName
	{
		get;
		set;
	}

	public string GamecenterId
	{
		get;
		set;
	}

	public string GamecenterAlias
	{
		get;
		set;
	}

	public string GoogleId
	{
		get;
		set;
	}

	public string GoogleName
	{
		get;
		set;
	}

	public Credentials(string roofdogId, string roofdogPwd, string roofdogName, string facebookId, string facebookToken, string facebookName, string gamecenterId, string gamecenterAlias, string googleId, string googleName)
	{
		if (string.IsNullOrEmpty(roofdogId) || string.IsNullOrEmpty(roofdogPwd))
		{
			roofdogId = null;
			roofdogPwd = null;
			roofdogName = null;
		}
		if (string.IsNullOrEmpty(facebookToken) || string.IsNullOrEmpty(facebookId))
		{
			facebookToken = null;
			facebookId = null;
		}
		RoofdogId = roofdogId;
		RoofdogPwd = roofdogPwd;
		RoofdogName = roofdogName;
		FacebookId = facebookId;
		FacebookToken = facebookToken;
		FacebookName = facebookName;
		GamecenterId = gamecenterId;
		GamecenterAlias = gamecenterAlias;
		GoogleId = googleId;
		GoogleName = googleName;
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["RoofdogId"] = RoofdogId;
		hashtable["RoofdogPwd"] = RoofdogPwd;
		hashtable["RoofdogName"] = RoofdogName;
		hashtable["FacebookId"] = FacebookId;
		hashtable["FacebookToken"] = FacebookToken;
		hashtable["FacebookName"] = FacebookName;
		hashtable["GamecenterId"] = GamecenterId;
		hashtable["GamecenterAlias"] = GamecenterAlias;
		hashtable["GoogleId"] = GoogleId;
		hashtable["GoogleName"] = GoogleName;
		return hashtable;
	}

	public static Credentials FromJsonData(Hashtable data)
	{
		return new Credentials(JsonUtil.ExtractString(data, "RoofdogId", null), JsonUtil.ExtractString(data, "RoofdogPwd", null), JsonUtil.ExtractString(data, "RoofdogName", null), JsonUtil.ExtractString(data, "FacebookId", null), JsonUtil.ExtractString(data, "FacebookToken", null), JsonUtil.ExtractString(data, "FacebookName", null), JsonUtil.ExtractString(data, "GamecenterId", null), JsonUtil.ExtractString(data, "GamecenterAlias", null), JsonUtil.ExtractString(data, "GoogleId", null), JsonUtil.ExtractString(data, "GoogleName", null));
	}

	public bool IsValid()
	{
		return !string.IsNullOrEmpty(RoofdogId) && !string.IsNullOrEmpty(RoofdogPwd);
	}
}
