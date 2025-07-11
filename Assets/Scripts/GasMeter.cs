using UnityEngine;

public class GasMeter : MetroWidget
{
	public enum MeterType
	{
		gas,
		boost,
		megaBoost
	}

	public MeterType _type;

	private bool _wasBoosting;

	private Meter _meter;

	private Vector3 _startingPosition;

	private CarController _car;

	protected override void OnAwake()
	{
		string sliceName = MetroSkin.Slice9MeterRed;
		if (_type == MeterType.boost)
		{
			sliceName = MetroSkin.Slice9MeterBlue;
		}
		else if (_type == MeterType.megaBoost)
		{
			sliceName = MetroSkin.Slice9MeterYellow;
		}
		_transform = base.transform;
		_meter = Meter.Create(sliceName, 0.1f, 0.2f);
		_meter.transform.parent = _transform;
		_meter.transform.localPosition = Vector3.zero;
		base.OnAwake();
	}

	protected override void OnStart()
	{
		_meter.SnapDisplay();
		_transform.position = new Vector3(0f, 0f - PrefabSingleton<CameraGUI>.Instance.HalfScreenHeight + 2.6f, 10f);
		_startingPosition = _transform.position;
		Singleton<GameManager>.Instance.OnCarControllerChanged += OnCarControllerChanged;
		OnCarControllerChanged(Singleton<GameManager>.Instance.Car);
		base.OnStart();
	}

	private void OnCarControllerChanged(CarController car)
	{
		_car = car;
	}

	public void Update()
	{
		if (_type == MeterType.boost || _type == MeterType.megaBoost)
		{
			bool flag = _car.IsBoosting();
			if (flag)
			{
				_meter.SetValue01(_car.CarBoost.GetBoost01());
				if (!_wasBoosting)
				{
					_meter.SnapDisplay();
				}
			}
			_wasBoosting = flag;
		}
		else
		{
			_meter.SetValue01(_car.CarGas.GetGas01());
		}
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		bool flag = _car.IsBoosting();
		bool flag2 = _car.IsCrashed();
		bool flag3 = false;
		if (flag && !flag2)
		{
			if (_car.CarBoost.IsMegaBoosting())
			{
				if (_type == MeterType.megaBoost)
				{
					flag3 = true;
				}
			}
			else if (_type == MeterType.boost)
			{
				flag3 = true;
			}
		}
		if ((!flag || flag2) && _type == MeterType.gas)
		{
			flag3 = true;
		}
		float y = (!flag3) ? (-8f) : 0f;
		_transform.position = Vector3.Lerp(_transform.position, _startingPosition + new Vector3(0f, y, 0f), 8f * Time.deltaTime);
	}
}
