using System;
using System.Collections.Generic;

public static class ServerRewardUIHandler
{
	public static bool ProcessServerReward()
	{
		ServerReward pendingReward = AutoSingleton<MissionRewardsManager>.Instance.GetPendingReward();
		if (pendingReward != null)
		{
			List<Reward> rewardList = pendingReward.GetRewardList();
			rewardList.ForEach(delegate(Reward r)
			{
				r.Activate();
			});
			Action onDismiss = delegate
			{
				AutoSingleton<MetroMenuStack>.Instance.Pop();
			};
			MetroPopupRewards page = MetroMenuPage.Create<MetroPopupRewards>().Setup(pendingReward.Title, pendingReward.Subtitle, rewardList, onDismiss, MetroSkin.StarCircle);
			AutoSingleton<MetroMenuStack>.Instance.Push(page);
			return true;
		}
		return false;
	}
}
