using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageActionFactory
{
	public static Action ProcessPayload(Message message)
	{
		string actionData = message.ActionData;
		switch (message.Action)
		{
		case Message.MessageAction.openUrlAction:
			return delegate
			{
				if (!string.IsNullOrEmpty(actionData))
				{
					Application.OpenURL(actionData);
				}
			};
		case Message.MessageAction.openGameAction:
			if (!string.IsNullOrEmpty(actionData))
			{
				Hashtable jsonData = actionData.hashtableFromJson();
				ExternalAppData appData = ExternalAppData.FromJsonData(jsonData);
				return delegate
				{
					AutoSingleton<ExternalAppLauncher>.Instance.LaunchOrStoreRedirect(appData);
				};
			}
			return delegate
			{
			};
		case Message.MessageAction.redeemRewardAction:
		{
			List<Reward> list = new List<Reward>();
			ArrayList arrayList = actionData.arrayListFromJson();
			if (arrayList != null)
			{
				foreach (Hashtable item in arrayList)
				{
					Reward reward = Reward.FromJsonData(item);
					if (reward != null)
					{
						if (reward.GetRewardType() == RewardType.random)
						{
							List<Reward> randomRewards = AutoSingleton<MissionRewardsManager>.Instance.GetRandomRewards(reward.Amount);
							list.AddRange(randomRewards);
						}
						else
						{
							list.Add(reward);
						}
					}
				}
				if (!message.ActionConsumed)
				{
					foreach (Reward item2 in list)
					{
						item2.Activate();
					}
					message.Consume();
					AutoSingleton<Rooflog>.Instance.OnGenericMessageRewardRedeemed(message.Uid, message.ContextId, list);
				}
				Action onDismiss = delegate
				{
					AutoSingleton<MetroMenuStack>.Instance.Pop();
				};
				MetroPopupRewards popup = MetroMenuPage.Create<MetroPopupRewards>().Setup(message.Title, string.Empty, list, onDismiss, MetroSkin.StarCircle);
				return delegate
				{
					AutoSingleton<MetroMenuStack>.Instance.Push(popup);
				};
			}
			return delegate
			{
			};
		}
		case Message.MessageAction.purchaseProductAction:
			return delegate
			{
				PurchaseManager.Purchase purchase = AutoSingleton<PurchaseManager>.Instance.GetPurchase(message.ActionData);
				if (purchase != null)
				{
					AutoSingleton<PurchaseManager>.Instance.Buy(purchase);
				}
			};
		case Message.MessageAction.emptyAction:
			return delegate
			{
			};
		default:
			UnityEngine.Debug.LogWarning("Action not Handled: " + message.Action.ToString());
			return delegate
			{
			};
		}
	}
}
