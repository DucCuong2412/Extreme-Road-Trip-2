using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSettings
{
	public const float Skybox_AdvanceFactor = 0.0005f;

	public const float Background_AdvanceFactor = 0.03f;

	public const float RoadSign_AdvanceFactor = 0.06f;

	public const float Ground_z_Distance = 2f;

	public const float Setup_Distance = 80f;

	public const float Setup_VelocityLimit = 20f;

	public const float SuddenDeath_Distance = 10000f;

	public const float DustEmitInterval = 0.1f;

	public const float Camera_PaddingBottom = 0.1f;

	public const float Camera_PaddingTop = 0.1f;

	public const float Camera_RegionTopZoomOut = 0.2f;

	public const float Camera_MinScreenHeight = 16f;

	public const float Camera_VelocityZoomFactor = 1.25f;

	public const float Camera_BoostLag = -2f;

	public const float Camera_MegaBoostLag = -6f;

	public const float Camera_BoostLagDuration = 1f;

	public const float Camera_MegaBoostLagDuration = 1.5f;

	public const float Camera_MegaBoostShake = 5f;

	public const float Stunt_PerfectLandingDelay = 0.02f;

	public const float Stunt_GreatLandingDelay = 0.06f;

	public const float Stunt_GoodLandingDelay = 0.09f;

	public const float Stunt_WheelieTime = 1f;

	public const float Stunt_BigAirHeight = 20f;

	public const float Stunt_LongJumpDistance = 100f;

	public const float Stunt_HangTimeDuration = 2f;

	public const float Stunt_CloseCallHeight = 6f;

	public const float Stunt_CloseCallMinAngle = 100f;

	public const float Stunt_CloseCallMaxAngle = 260f;

	public const float Stunt_CloseCallResetDistance = 20f;

	public const float Stunt_CloseCallMaxYVelocity = 5f;

	public const float Stunt_LongSlamDistance = 25f;

	public const float Stunt_HandsFreeLandingDelay = 0.5f;

	public const float Stunt_DelayBeforeHideMessage = 1f;

	public const float Stunt_UpsideDownMinAngle = 135f;

	public const float Stunt_UpsideDownMaxAngle = 225f;

	public const float Fade_Duration = 0.3f;

	public const int Hud_StuntTextCharPerLine = 30;

	public const int PopupMessage_TextCharPerLine = 40;

	public const int PopupPromoMessage_TextCharPerLine = 50;

	public const int PrestigePopupMessage_TextCharPerLine = 50;

	public const float Props_Probability = 0.04f;

	public const float ActionProps_Probability = 0.005f;

	public const float Pickups_MinDistancePerSequence = 50f;

	public const float Pickups_MaxDistancePerSequence = 100f;

	public const float Pickups_FrenzyMinDistancePerSequence = 15f;

	public const float Pickups_FrenzyMaxDistancePerSequence = 35f;

	public const int Package_MinCollectibles = 5;

	public const int Package_MaxCollectibles = 10;

	public const float BumpableProp_zOffset = 1f;

	public const float Coin_Value = 1f;

	public const float Buck_Value = 1f;

	public const float Gas_Value = 0.5f;

	public const float Rider_VelocityLimit = 50f;

	public const float Boost_FullAmount = 10f;

	public const float MegaBoost_FullAmount = 5f;

	public const float Boost_HandsFreeLanding = 1.25f;

	public const float Boost_PerfectSlamLanding = 1.25f;

	public const float Boost_PerfectLanding = 1f;

	public const float Boost_WheelieLanding = 1f;

	public const float Boost_GreatLanding = 0.75f;

	public const float Boost_GoodLanding = 0.5f;

	public const float Boost_SloppyLanding = 0.25f;

	public const int MissionsProgressNumSteps = 5;

	public const int MissionsProgressNumRewards = 2;

	public const float XP_Stunt = 5f;

	public const float XP_1M = 0.1f;

	public const int MaxUpgradeLevel = 5;

	public const int FacebookBucksReward = 5;

	public const int FacebookCoinsPerInvite = 50;

	public const float FrenzyRunTotalTime = 35f;

	public static string CarBumperColliderTag = "CarBumperCollider";

	public static string CarWheelColliderTag = "CarWheelCollider";

	public static string CarMagnetColliderTag = "CarMagnetCollider";

	public static string CarLayer = "Car";

	public static Vector3 OutOfWorldVector = new Vector3(-1000f, -1000f, -1000f);

	public static string GroundColliderTag = "GroundCollider";

	public static string BumpableOnLayer = "BumpableOn";

	public static string BumpableOffLayer = "BumpableOff";

	public static Vector3 Rider_Force_On_Explosion = new Vector3(500f, 3000f, 0f);

	private static HashSet<Func<float, float>> _coinValueMultiplierFuncList;

	[method: MethodImpl(32)]
	public static event Func<float, float> GetGasValue;

	public static float GetLandEventBoostFactor(StuntEvent landEvent)
	{
		switch (landEvent)
		{
		case StuntEvent.perfectLanding:
			return 1f;
		case StuntEvent.wheelieLanding:
			return 1f;
		case StuntEvent.greatLanding:
			return 0.75f;
		case StuntEvent.goodLanding:
			return 0.5f;
		case StuntEvent.perfectSlamLanding:
			return 1.25f;
		case StuntEvent.handsFreeLanding:
			return 1.25f;
		default:
			return 0.25f;
		}
	}

	public static float GetCollectibleValue(CollectibleType type)
	{
		switch (type)
		{
		case CollectibleType.pinataBuck:
			return 1f;
		case CollectibleType.coin:
		case CollectibleType.pinataCoin:
			return CallCoinMultipliers(1f);
		case CollectibleType.gas:
		case CollectibleType.pinataGas:
			if (GameSettings.GetGasValue != null)
			{
				return GameSettings.GetGasValue(0.5f);
			}
			return 0.5f;
		default:
			return 0f;
		}
	}

	private static float GetCoinDefaultValue(float amount)
	{
		return amount;
	}

	public static void AddCoinMultiplier(Func<float, float> fc)
	{
		if (_coinValueMultiplierFuncList == null)
		{
			_coinValueMultiplierFuncList = new HashSet<Func<float, float>>();
		}
		_coinValueMultiplierFuncList.Add(fc);
	}

	public static void RemoveCoinMultiplier(Func<float, float> fc)
	{
		if (_coinValueMultiplierFuncList != null)
		{
			_coinValueMultiplierFuncList.Remove(fc);
		}
	}

	private static float CallCoinMultipliers(float amount)
	{
		if (_coinValueMultiplierFuncList == null)
		{
			_coinValueMultiplierFuncList = new HashSet<Func<float, float>>();
		}
		float num = amount;
		foreach (Func<float, float> coinValueMultiplierFunc in _coinValueMultiplierFuncList)
		{
			num = coinValueMultiplierFunc(num);
		}
		return num;
	}
}
