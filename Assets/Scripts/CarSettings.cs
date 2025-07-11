using UnityEngine;

public class CarSettings
{
	public static CarConfiguration GetCarConfiguration(Car car)
	{
		CarConfiguration carConfiguration = new CarConfiguration();
		int num = 20;
		CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(car);
		int upgradeLevel = carProfile.GetUpgradeLevel();
		int num2 = car.GasLevel + upgradeLevel;
		float num3 = Mathf.Clamp01((float)num2 / (float)num) * 2f;
		carConfiguration._gasFullAmount *= 1f + 0.5f * num3;
		int num4 = car.SpeedLevel + upgradeLevel;
		float num5 = Mathf.Clamp01((float)num4 / (float)num) * 2f;
		carConfiguration._maxSpeed *= 1f + 0.25f * num5;
		carConfiguration._motorTorque *= 1f + 0.5f * num5;
		int num6 = car.FlipLevel + upgradeLevel;
		float num7 = Mathf.Clamp01((float)num6 / (float)num) * 2f;
		carConfiguration._tiltVelocity *= 1f + 0.25f * num7;
		carConfiguration._tiltOppositeBoost *= 1f + 0.2f * num7;
		carConfiguration._tiltVelocityLimit *= 1f + 0.25f * num7;
		int num8 = car.BoostLevel + upgradeLevel;
		float num9 = Mathf.Clamp01((float)num8 / (float)num) * 2f;
		carConfiguration._boostAcceleration *= 1f + 0.2f * num9;
		carConfiguration._boostExtraFactor *= 1f + 0.2f * num9;
		int slamLevel = car.SlamLevel;
		float num10 = Mathf.Clamp01((float)slamLevel / (float)num) * 2f;
		carConfiguration._slamForce *= 1f + 0.65f * num10;
		carConfiguration._mass = car.Mass;
		carConfiguration._suspensionDistance = car.SuspensionDistance;
		carConfiguration._suspensionSpring = car.SuspensionSpring;
		carConfiguration._suspensionDamper = car.SuspensionDamper;
		return carConfiguration;
	}
}
