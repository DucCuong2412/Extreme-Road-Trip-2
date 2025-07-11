using System.Collections;

public class RoofdogIdentity
{
	public string Id
	{
		get;
		private set;
	}

	public string Pwd
	{
		get;
		private set;
	}

	public string PublicId
	{
		get;
		private set;
	}

	public static RoofdogIdentity FromJsonData(Hashtable data)
	{
		if (data == null)
		{
			return null;
		}
		RoofdogIdentity roofdogIdentity = new RoofdogIdentity();
		roofdogIdentity.Id = JsonUtil.ExtractString(data, "Id", string.Empty);
		roofdogIdentity.Pwd = JsonUtil.ExtractString(data, "Pwd", string.Empty);
		roofdogIdentity.PublicId = JsonUtil.ExtractString(data, "PublicId", string.Empty);
		return roofdogIdentity;
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Id"] = Id;
		hashtable["Pwd"] = Pwd;
		hashtable["PublicId"] = PublicId;
		return hashtable;
	}

	public override string ToString()
	{
		return $"[RoofdogIdentity: Id={Id}, Pwd={Pwd}, PublicId={PublicId}]";
	}
}
