using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward
{
	private RewardType _type;

	private static Dictionary<string, RewardType> _fromString;

	public int Amount
	{
		get;
		set;
	}

	public Reward(RewardType type, int amount)
	{
		_type = type;
		Amount = amount;
	}

	static Reward()
	{
		_fromString = new Dictionary<string, RewardType>();
		foreach (int value in Enum.GetValues(typeof(RewardType)))
		{
			_fromString[((RewardType)value).ToString()] = (RewardType)value;
		}
	}

	public RewardType GetRewardType()
	{
		return _type;
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["type"] = _type.ToString();
		hashtable["amount"] = Amount;
		return hashtable;
	}

	public static Reward FromJsonData(Hashtable ht)
	{
		string text = JsonUtil.ExtractString(ht, "type", null);
		if (text != null && _fromString.ContainsKey(text))
		{
			RewardType type = _fromString[text];
			int amount = JsonUtil.ExtractInt(ht, "amount", 0);
			return new Reward(type, amount);
		}
		UnityEngine.Debug.LogWarning("JSON WARNING: Could not extract reward.");
		return null;
	}

	public static List<Reward> FromJsonData(ArrayList jsonArray)
	{
		List<Reward> list = new List<Reward>();
		foreach (Hashtable item in jsonArray)
		{
			Reward reward = FromJsonData(item);
			if (reward != null)
			{
				list.Add(reward);
			}
		}
		return list;
	}

	public void Activate()
	{
		switch (_type)
		{
		case RewardType.prestigeBadge1:
		case RewardType.prestigeBadge2:
		case RewardType.prestigeBadge3:
			break;
		case RewardType.coins:
			AutoSingleton<CashManager>.Instance.AddCoins(Amount);
			break;
		case RewardType.bucks:
			AutoSingleton<CashManager>.Instance.AddBucks(Amount);
			break;
		case RewardType.xp:
			AutoSingleton<Player>.Instance.Profile.XPProfile.RegisterXP(Amount);
			break;
		case RewardType.boost:
		{
			PlayerProfile profile3 = AutoSingleton<Player>.Instance.Profile;
			profile3.OnPowerupAdded(PowerupType.boost, Amount);
			break;
		}
		case RewardType.magnet:
		{
			PlayerProfile profile2 = AutoSingleton<Player>.Instance.Profile;
			profile2.OnPowerupAdded(PowerupType.magnet, Amount);
			break;
		}
		case RewardType.coinDoubler:
		{
			PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
			profile.OnPowerupAdded(PowerupType.coinDoubler, Amount);
			break;
		}
		case RewardType.prestigeTokens:
			AutoSingleton<CashManager>.Instance.AddPrestigeTokens(Amount);
			break;
		case RewardType.car:
			AutoSingleton<CarManager>.Instance.UnlockCarForFree(Amount);
			break;
		default:
			UnityEngine.Debug.LogError("Invalid powerup activated");
			break;
		}
	}
}
