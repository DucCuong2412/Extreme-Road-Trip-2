using UnityEngine;

public class CoinsPowerup : Powerup
{
	private ParticleSystem _coinsFX;

	public CoinsPowerup(Price price)
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
		GameSettings.AddCoinMultiplier(GetCoinValue);
		CarController car = Singleton<GameManager>.Instance.Car;
		car.OnCrash += OnCrash;
		_coinsFX = PrefabSingleton<GameSpecialFXManager>.Instance.AddCarCoinsFX(car);
	}

	protected override void Deactivate()
	{
		Singleton<GameManager>.Instance.OnGameEnded -= Deactivate;
		GameSettings.RemoveCoinMultiplier(GetCoinValue);
		Singleton<GameManager>.Instance.Car.OnCrash -= OnCrash;
	}

	private void OnCrash()
	{
		if (_coinsFX != null)
		{
			_coinsFX.gameObject.SetActive(value: false);
		}
	}

	private static float GetCoinValue(float originalValue)
	{
		return originalValue * 2f;
	}

	public override PowerupType GetPowerupType()
	{
		return PowerupType.coinDoubler;
	}
}
