using UnityEngine;

public class CarGas : MonoBehaviour
{
	private float _currentGas;

	private float _maxGas;

	private float _invMaxGas;

	private CarController _car;

	private void Start()
	{
		_car = GetComponent<CarController>();
		_maxGas = _car.Config._gasFullAmount;
		_invMaxGas = 1f / _maxGas;
		_currentGas = _maxGas;
	}

	public bool HasGas()
	{
		return _currentGas > 0f;
	}

	public float GetGas()
	{
		return Mathf.Max(0f, _currentGas);
	}

	public float GetGas01()
	{
		return Mathf.Clamp(_currentGas * _invMaxGas, 0f, 1f);
	}

	public void AddGas(float amount)
	{
		_currentGas += amount;
	}

	public void Update()
	{
		if (!_car.IsBoosting() && !_car.IsCrashed() && _car.Input.InputEnabled && !_car.IsInSetup)
		{
			_currentGas -= Time.deltaTime;
		}
		_currentGas = Mathf.Clamp(_currentGas, 0f, _maxGas);
	}
}
