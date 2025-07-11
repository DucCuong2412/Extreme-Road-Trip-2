using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionRewardsManager : AutoSingleton<MissionRewardsManager>
{
	private WeightedList<Reward> _rewards;

	private List<ServerReward> _rewardList;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_rewards = new WeightedList<Reward>();
		TextAsset textAsset = Resources.Load("missionRewards.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			ArrayList arrayList = textAsset.text.arrayListFromJson();
			if (arrayList != null)
			{
				foreach (object item in arrayList)
				{
					Hashtable data = item as Hashtable;
					Hashtable ht = JsonUtil.ExtractHashtable(data, "reward");
					int weight = JsonUtil.ExtractInt(data, "frequency", 1);
					Reward reward = Reward.FromJsonData(ht);
					if (reward != null)
					{
						_rewards.Add(reward, weight);
					}
					else
					{
						UnityEngine.Debug.LogWarning("Warning: could not extract reward from missionRewards.json");
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file missionRewards.json");
		}
		InitServerReward();
		base.OnAwake();
	}

	public List<Reward> GetReward(int numTotalMissionsCompleted, int numMissionsCompleted)
	{
		if (numTotalMissionsCompleted % 5 - numMissionsCompleted < 0)
		{
			List<Reward> list = new List<Reward>();
			for (int i = 0; i < 2; i++)
			{
				Reward reward = _rewards.Pick();
				if (reward != null)
				{
					list.Add(reward);
				}
			}
			return list;
		}
		return null;
	}

	public List<Reward> GetRandomRewards(int count)
	{
		List<Reward> list = new List<Reward>();
		for (int i = 0; i < count; i++)
		{
			Reward reward = _rewards.Pick();
			if (reward != null)
			{
				list.Add(reward);
			}
		}
		return list;
	}

	public void OnServerRewardReceived(Hashtable data)
	{
		Hashtable ht = JsonUtil.ExtractHashtable(data, "Reward", new Hashtable());
		Reward reward = Reward.FromJsonData(ht);
		if (reward != null)
		{
			string title = JsonUtil.ExtractString(data, "Title", "Reward Received");
			string subtitle = JsonUtil.ExtractString(data, "Subtitle", "Here's a gift!");
			ServerReward serReward = new ServerReward(reward, title, subtitle);
			_rewardList.AddRange(SplitReward(serReward));
			SaveRewardList();
		}
	}

	public void OnServerRewardReceived(List<ServerReward> serverRewards)
	{
		foreach (ServerReward serverReward in serverRewards)
		{
			_rewardList.AddRange(SplitReward(serverReward));
			SaveRewardList();
		}
	}

	public ServerReward GetPendingReward()
	{
		ServerReward result = null;
		if (_rewardList.Count > 0)
		{
			result = _rewardList[0];
			_rewardList.RemoveAt(0);
			SaveRewardList();
		}
		return result;
	}

	public bool HasPendingServerReward()
	{
		return _rewardList.Count > 0;
	}

	public void InitServerReward()
	{
		_rewardList = new List<ServerReward>();
		string @string = Preference.GetString("ServerRewardsCache", "[]");
		Hashtable data = @string.hashtableFromJson();
		ArrayList arrayList = JsonUtil.ExtractArrayList(data, "ServerRewardsList", new ArrayList());
		foreach (object item2 in arrayList)
		{
			Hashtable data2 = item2 as Hashtable;
			ServerReward item = ServerReward.FromJsonData(data2);
			_rewardList.Add(item);
		}
	}

	private void SaveRewardList()
	{
		Hashtable hashtable = new Hashtable();
		ArrayList arrayList = new ArrayList();
		foreach (ServerReward reward in _rewardList)
		{
			arrayList.Add(reward.ToJsonData());
		}
		hashtable["ServerRewardsList"] = arrayList;
		Preference.SetString("ServerRewardsCache", hashtable.toJson());
	}

	private List<ServerReward> SplitReward(ServerReward serReward, int maxRewardPerLot = 4)
	{
		List<ServerReward> list = new List<ServerReward>();
		Reward reward = serReward.Reward;
		if (reward.GetRewardType() == RewardType.random && reward.Amount > maxRewardPerLot)
		{
			while (reward.Amount - maxRewardPerLot > 0)
			{
				ServerReward item = new ServerReward(new Reward(reward.GetRewardType(), maxRewardPerLot), serReward.Title, serReward.Subtitle);
				list.Add(item);
				serReward.Reward.Amount -= maxRewardPerLot;
			}
			list.Add(serReward);
		}
		else
		{
			list.Add(serReward);
		}
		return list;
	}
}
