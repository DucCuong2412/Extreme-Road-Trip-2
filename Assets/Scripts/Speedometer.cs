using UnityEngine;

public class Speedometer : MonoBehaviour
{
	private const float _range = 145f;

	private Transform _needlePivot;

	private CarController _car;

	private float _angle;

	private float _invVelocityLimit;

	public void Start()
	{
		_needlePivot = base.transform.Find("Needle Pivot");
		_angle = 145f;
		_car = Singleton<GameManager>.Instance.Car;
		_invVelocityLimit = 1f / _car.Car.MaxSpeed;
	}

	public void Update()
	{
		if (_car == null)
		{
			_car = Singleton<GameManager>.Instance.Car;
			return;
		}
		if (_car.IsCrashed())
		{
			_angle = 145f;
		}
		else
		{
			float angle = _angle;
			Vector3 velocity = _car.Velocity;
			_angle = Mathf.Lerp(angle, Mathf.Lerp(145f, -145f, velocity.x * _invVelocityLimit), 3f * Time.deltaTime);
		}
		_needlePivot.rotation = Quaternion.Euler(0f, 0f, _angle);
	}
}
