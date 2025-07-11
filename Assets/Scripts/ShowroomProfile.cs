using System.Collections;
using UnityEngine;

public class ShowroomProfile
{
	private bool _unlocked;

	public ShowroomProfile()
	{
		_unlocked = false;
	}

	public ShowroomProfile(string json)
	{
		Hashtable data = json.hashtableFromJson();
		_unlocked = JsonUtil.ExtractBool(data, "Unlocked", def: false);
	}

	public string ToJson()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Unlocked"] = _unlocked;
		return hashtable.toJson();
	}

	public bool IsUnlocked()
	{
		return _unlocked;
	}

	public void UnlockForFree()
	{
		UnlockShowroom();
	}

	public bool TryUnlock(Price price)
	{
		if (_unlocked)
		{
			UnityEngine.Debug.Log("Already unlocked");
			return false;
		}
		if (AutoSingleton<CashManager>.Instance.CanBuy(price))
		{
			AutoSingleton<CashManager>.Instance.Buy(price);
			UnlockShowroom();
			return true;
		}
		UnityEngine.Debug.Log("Not enough cash");
		return false;
	}

	private void UnlockShowroom()
	{
		_unlocked = true;
		AutoSingleton<AchievementsManager>.Instance.ShowroomUnlock();
	}
}
