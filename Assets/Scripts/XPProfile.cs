using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPProfile
{
	protected const float _defaultXP = 0f;

	protected const int _defaultLevel = 1;

	private static List<XPLevel> _levels;

	public float XP
	{
		get;
		protected set;
	}

	public int Level
	{
		get
		{
			int i;
			for (i = 1; i < _levels.Count; i++)
			{
				float xP = XP;
				XPLevel xPLevel = _levels[i];
				if (!(xP >= xPLevel._xpRequired))
				{
					break;
				}
			}
			return i;
		}
	}

	public XPProfile(float xp = 0f)
	{
		XP = xp;
	}

	public XPProfile(Hashtable data)
	{
		XP = JsonUtil.ExtractFloat(data, "XP", 0f);
	}

	static XPProfile()
	{
		_levels = new List<XPLevel>();
		CSVData cSVData = new CSVData("levels.csv");
		int count = cSVData.Count;
		TextAsset textAsset = Resources.Load("levelsRewards.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			Hashtable hashtable = textAsset.text.hashtableFromJson();
			if (hashtable != null)
			{
				for (int i = 0; i < count; i++)
				{
					int @int = cSVData.GetInt(i, "level");
					int int2 = cSVData.GetInt(i, "xp");
					ArrayList jsonArray = JsonUtil.ExtractArrayList(hashtable, @int.ToString(), new ArrayList());
					_levels.Add(new XPLevel(int2, jsonArray));
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file levelsRewards.json");
		}
	}

	public Hashtable ToJson()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["XP"] = XP;
		return hashtable;
	}

	public float GetProgress01()
	{
		float result = 0f;
		int level = Level;
		if (level < _levels.Count)
		{
			XPLevel xPLevel = _levels[level - 1];
			float xpRequired = xPLevel._xpRequired;
			float num = XP - xpRequired;
			XPLevel xPLevel2 = _levels[level];
			result = num / (xPLevel2._xpRequired - xpRequired);
		}
		return result;
	}

	public virtual void RegisterXP(float xp)
	{
		int level = Level;
		XP += xp;
		int level2 = Level;
		for (int i = level; i < level2; i++)
		{
			XPLevel xPLevel = _levels[i];
			foreach (Reward reward in xPLevel._rewards)
			{
				if (reward.GetRewardType() != RewardType.xp)
				{
					reward.Activate();
				}
				else
				{
					UnityEngine.Debug.LogWarning("A new level reward should not be of type XPReward.");
				}
			}
		}
		GameEvents.OnPlayerXP(level, level2, xp);
		OnXPRegistered(level2);
		AutoSingleton<Player>.Instance.SaveProfile();
	}

	public static List<Reward> GetRewards(int level)
	{
		object result;
		if (level > 0 && level <= _levels.Count)
		{
			XPLevel xPLevel = _levels[level - 1];
			result = xPLevel._rewards;
		}
		else
		{
			result = null;
		}
		return (List<Reward>)result;
	}

	private void OnXPRegistered(int lvl)
	{
		AutoSingleton<AchievementsManager>.Instance.CheckLevelAchievements(lvl);
		AutoSingleton<LeaderboardsManager>.Instance.SubmitScore(LeaderboardType.highestLevel, lvl);
	}
}
