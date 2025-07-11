using UnityEngine;

public class MagnetPowerup : Powerup
{
	private ParticleSystem _magnetFX;

	public MagnetPowerup(Price price)
		: base(price)
	{
	}

	public override void Enable()
	{
		Activate();
		base.Enable();
	}

	public override void Disable()
	{
		Deactivate();
		base.Disable();
	}

	protected override void Activate()
	{
		Singleton<GameManager>.Instance.OnGameEnded += Deactivate;
		CarController car = Singleton<GameManager>.Instance.Car;
		car.SetMagnetReach(car.Config._magnetReach * 3f);
		car.OnCrash += OnCrash;
		_magnetFX = PrefabSingleton<GameSpecialFXManager>.Instance.AddCarMagnetFX(car);
	}

	protected override void Deactivate()
	{
		Singleton<GameManager>.Instance.OnGameEnded -= Deactivate;
		CarController car = Singleton<GameManager>.Instance.Car;
		car.OnCrash -= OnCrash;
	}

	private void OnCrash()
	{
		if (_magnetFX != null)
		{
			_magnetFX.gameObject.SetActive(value: false);
		}
	}

	public override PowerupType GetPowerupType()
	{
		return PowerupType.magnet;
	}
}
