using UnityEngine;

public class MetroWidgetReward : MetroGlue
{
	private MetroLabel _label;

	private int _coins;

	private MetroIcon GetRewardIcon(Reward reward)
	{
		switch (reward.GetRewardType())
		{
		case RewardType.coins:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconCoin), 0.8f);
		case RewardType.bucks:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconBucks), 0.8f);
		case RewardType.boost:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconPowerupBoost), 0.8f);
		case RewardType.magnet:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconPowerupMagnet), 0.8f);
		case RewardType.coinDoubler:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconPowerupCoinDoubler), 0.8f);
		case RewardType.prestigeTokens:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconPrestigeToken), 0.8f);
		case RewardType.prestigeBadge1:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.SpriteCardPrestigeBadge1), 0.8f);
		case RewardType.prestigeBadge2:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.SpriteCardPrestigeBadge2), 0.8f);
		case RewardType.car:
		{
			Car carByRank = AutoSingleton<CarDatabase>.Instance.GetCarByRank(reward.Amount);
			if (carByRank != null)
			{
				return CreateRewardIcon(MetroIcon.Create(carByRank), 3f);
			}
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconClose), 0.8f);
		}
		default:
			return CreateRewardIcon(MetroIcon.Create(MetroSkin.IconClose), 0.8f);
		}
	}

	private MetroIcon CreateRewardIcon(MetroIcon icon, float scale)
	{
		icon.SetScale(scale);
		return icon;
	}

	private void Setup(Reward reward)
	{
		MetroIcon rewardIcon = GetRewardIcon(reward);
		Add(rewardIcon);
		if (reward.GetRewardType() != RewardType.car)
		{
			_label = MetroLabel.Create("x" + reward.Amount.ToString());
			_label.AddOutline();
			Add(_label);
		}
		SetFlow(Direction.horizontal);
	}

	public static MetroWidgetReward Create(Reward reward)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetReward).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetReward metroWidgetReward = gameObject.AddComponent<MetroWidgetReward>();
		metroWidgetReward.Setup(reward);
		return metroWidgetReward;
	}
}
