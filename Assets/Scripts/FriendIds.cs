using System.Collections;
using System.Collections.Generic;

public class FriendIds
{
	public List<string> FacebookIds
	{
		get;
		private set;
	}

	public List<string> GameCenterIds
	{
		get;
		private set;
	}

	public FriendIds()
	{
		FacebookIds = new List<string>();
		GameCenterIds = new List<string>();
	}

	public FriendIds(List<string> facebookIds, List<string> gamecenterIds)
	{
		FacebookIds = facebookIds;
		GameCenterIds = gamecenterIds;
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["FacebookIds"] = new ArrayList(FacebookIds);
		hashtable["GameCenterIds"] = new ArrayList(GameCenterIds);
		return hashtable;
	}
}
