using System.Collections;
using System.Collections.Generic;

public class ServerReward
{
	public Reward Reward
	{
		get;
		private set;
	}

	public string Title
	{
		get;
		private set;
	}

	public string Subtitle
	{
		get;
		private set;
	}

	public ServerReward(Reward reward, string title, string subtitle)
	{
		Reward = reward;
		Title = title;
		Subtitle = subtitle;
	}

	public List<Reward> GetRewardList()
	{
		if (Reward.GetRewardType() == RewardType.random)
		{
			return AutoSingleton<MissionRewardsManager>.Instance.GetRandomRewards(Reward.Amount);
		}
		List<Reward> list = new List<Reward>();
		list.Add(Reward);
		return list;
	}

	public static ServerReward FromJsonData(Hashtable data)
	{
		Reward reward = Reward.FromJsonData(JsonUtil.ExtractHashtable(data, "Reward"));
		if (reward == null)
		{
			return null;
		}
		string title = JsonUtil.ExtractString(data, "Title", string.Empty);
		string subtitle = JsonUtil.ExtractString(data, "Subtitle", string.Empty);
		return new ServerReward(reward, title, subtitle);
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Reward"] = Reward.ToJsonData();
		hashtable["Title"] = Title;
		hashtable["Subtitle"] = Subtitle;
		return hashtable;
	}
}
