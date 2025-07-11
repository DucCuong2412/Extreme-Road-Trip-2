public class BoostPowerup : Powerup
{
	private bool _gameSetupEnded;

	public BoostPowerup(Price price)
		: base(price)
	{
		Singleton<GameManager>.Instance.OnGameSetupEnded += delegate
		{
			_gameSetupEnded = true;
		};
	}

	public override void Enable()
	{
		if (_gameSetupEnded)
		{
			Activate();
		}
		else
		{
			Singleton<GameManager>.Instance.OnGameSetupEnded += Activate;
		}
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
		Singleton<GameManager>.Instance.Car.CarBoost.TriggerMegaBoost();
	}

	protected override void Deactivate()
	{
		Singleton<GameManager>.Instance.OnGameEnded -= Deactivate;
		_gameSetupEnded = false;
	}

	public override PowerupType GetPowerupType()
	{
		return PowerupType.boost;
	}
}
