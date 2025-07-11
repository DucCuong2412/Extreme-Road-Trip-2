using System.Collections;
using UnityEngine;

public abstract class StorePerk
{
	public StorePerkType StorePerkType
	{
		get;
		private set;
	}

	public StorePerk(StorePerkType type)
	{
		StorePerkType = type;
	}

	public abstract void Activate();

	public abstract void Deactivate();

	public static StorePerk FromJsonData(Hashtable data)
	{
		string text = JsonUtil.ExtractString(data, "PerkType", string.Empty);
		StorePerk storePerk = null;
		switch (text)
		{
		case "adRemover":
			return new AdRemover();
		case "permanentCoinDoubler":
			return new PermanentCoinDoubler();
		default:
			UnityEngine.Debug.Log("Cant read store perk");
			return null;
		}
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["PerkType"] = StorePerkType.ToString();
		return hashtable;
	}

	public static StorePerk Create(StorePerkType type)
	{
		switch (type)
		{
		case StorePerkType.adRemover:
			return new AdRemover();
		case StorePerkType.permanentCoinDoubler:
			return new PermanentCoinDoubler();
		default:
			UnityEngine.Debug.Log("Cant create store perk");
			return null;
		}
	}
}
