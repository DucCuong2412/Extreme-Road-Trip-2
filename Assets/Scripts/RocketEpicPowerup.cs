using UnityEngine;

public class RocketEpicPowerup : EpicPowerup
{
	private CarController _car;

	private ParticleSystem _rocketFX;

	public RocketEpicPowerup(Price price)
		: base(price)
	{
	}

	public override EpicPowerupType GetEpicPowerupType()
	{
		return EpicPowerupType.rocket;
	}

	public override string GetIconPath()
	{
		return MetroSkin.IconRocket;
	}

	public override string GetDescription(int streak)
	{
		switch (streak)
		{
		case 1:
			return "The explosion wasn't big enough! Let's do it a second time! How about a discount this time?".Localize();
		case 2:
			return "We're going insane! Let's make it even cheaper this time!".Localize();
		default:
			return "A crazy engineer wants to strap a rocket to your car. What's the worst that could happen?".Localize();
		}
	}

	public override CarController Setup(Car car)
	{
		Singleton<GameManager>.Instance.OnGameSetupEnded += OnGameSetupEnded;
		GameObject gameObject = Object.Instantiate(car.GamePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		_car = gameObject.GetComponent<CarController>();
		GameObject gameObject2 = Object.Instantiate(Resources.Load("Rocket")) as GameObject;
		Transform transform = gameObject.transform.FindChild("RocketAnchor");
		if (transform != null)
		{
			gameObject2.transform.parent = transform;
		}
		else
		{
			UnityEngine.Debug.LogWarning("This car is missing an anchor point for the Rocket.");
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, -1f);
		gameObject2.transform.localRotation = Quaternion.identity;
		_rocketFX = PrefabSingleton<GameSpecialFXManager>.Instance.AddRocketFX(gameObject2.transform);
		return _car;
	}

	private void OnGameSetupEnded()
	{
		_car.CarBoost.TriggerMegaBoost(usingRocket: true);
		if (_rocketFX != null)
		{
			_rocketFX.Play();
		}
	}
}
