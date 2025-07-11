using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public Rigidbody _rigidbody;

	public Transform _frontWheel;

	public Transform[] _extraWheels;

	public Transform _backWheel;

	public Transform[] _visuals;

	public Transform[] _colliders;

	private CarWheel[] _carWheels;

	private bool _crashed;

	private bool _inSetup;

	protected Transform _transform;

	private ParticleSystem _slammingFX;

	private CarInputController _carInputController;

	private SphereCollider _magnetCollider;

	private CarConfiguration _config;

	public Car Car
	{
		get;
		private set;
	}

	public CarBoost CarBoost
	{
		get;
		private set;
	}

	public CarGas CarGas
	{
		get;
		private set;
	}

	public bool Slamming
	{
		get;
		private set;
	}

	public bool IsInSetup => _inSetup;

	public CarInputController Input
	{
		get
		{
			if (_carInputController == null)
			{
				_carInputController = (GetComponent<CarInputController>() ?? base.gameObject.AddComponent<PlayerInputController>());
			}
			return _carInputController;
		}
	}

	public CarConfiguration Config
	{
		get
		{
			if (_config == null)
			{
				_config = CarSettings.GetCarConfiguration(Car);
			}
			return _config;
		}
	}

	public Vector3 Position => _transform.position;

	public Vector3 Velocity => _rigidbody.velocity;

	public Vector3 Down => _transform.rotation * Vector3.down;

	[method: MethodImpl(32)]
	public event Action OnCrash;

	[method: MethodImpl(32)]
	public event Action OnSlammingGround;

	public void Setup(Car car)
	{
		Car = car;
		_carWheels = new CarWheel[2 + _extraWheels.Length];
		_carWheels[1] = _backWheel.GetComponent<CarWheel>();
		_carWheels[0] = _frontWheel.GetComponent<CarWheel>();
		for (int i = 0; i < _extraWheels.Length; i++)
		{
			_carWheels[i + 2] = _extraWheels[i].GetComponent<CarWheel>();
		}
		for (int j = 0; j < _carWheels.Length; j++)
		{
			_carWheels[j].SetFreeWheeling(j != 1);
			_carWheels[j].Setup(this);
		}
	}

	private void Awake()
	{
		_transform = base.transform;
		_rigidbody = GetComponent<Rigidbody>();
		CarBoost = base.gameObject.AddComponent<CarBoost>();
		CarGas = base.gameObject.AddComponent<CarGas>();
		base.gameObject.AddComponent<CarAnalyzer>();
		base.gameObject.AddComponent<CarStunt>();
		base.gameObject.AddComponent<CarEngineSound>();
	}

	private void Start()
	{
		int layer = LayerMask.NameToLayer(GameSettings.CarLayer);
		Transform[] colliders = _colliders;
		foreach (Transform transform in colliders)
		{
			transform.tag = GameSettings.CarBumperColliderTag;
			transform.gameObject.layer = layer;
			transform.collider.material = AutoSingleton<CarPhysics>.Instance.Bumper;
		}
		_rigidbody.mass = Config._mass;
		_rigidbody.angularDrag = Config._angularDrag;
		if (_rigidbody.isKinematic)
		{
			_rigidbody.interpolation = RigidbodyInterpolation.None;
		}
		else
		{
			_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		}
		_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		_rigidbody.centerOfMass = Vector3.zero;
		_slammingFX = PrefabSingleton<GameSpecialFXManager>.Instance.AddCarSlamLinesFX(_transform);
		PrefabSingleton<GameSpecialFXManager>.Instance.AddCarShadow(this);
		AutoSingleton<ExplosionManager>.Instance.Create();
		_magnetCollider = base.gameObject.AddComponent<SphereCollider>();
		_magnetCollider.isTrigger = true;
		_magnetCollider.tag = GameSettings.CarMagnetColliderTag;
		_magnetCollider.radius = Config._magnetReach;
		Input.InputEnabled = true;
	}

	public bool AllWheelsOnGround()
	{
		CarWheel[] carWheels = _carWheels;
		foreach (CarWheel carWheel in carWheels)
		{
			if (!carWheel.IsOnGround())
			{
				return false;
			}
		}
		return true;
	}

	public bool IsTouchingGround()
	{
		CarWheel[] carWheels = _carWheels;
		foreach (CarWheel carWheel in carWheels)
		{
			if (carWheel.IsOnGround())
			{
				return true;
			}
		}
		return false;
	}

	public float ResistanceFactor()
	{
		float num = 1f;
		if (CarBoost.IsMegaBoosting())
		{
			num = Config._megaBoostFactor;
		}
		if (_inSetup)
		{
			num = 20f / Config._maxSpeed;
		}
		float to = Config._maxSpeed * num;
		Vector3 velocity = Velocity;
		float num2 = Mathf.InverseLerp(0f, to, velocity.x);
		return 1f - num2 * num2;
	}

	public void OnGameSetupStarted()
	{
		_inSetup = true;
		if (!_rigidbody.isKinematic)
		{
			_rigidbody.velocity = new Vector3(20f, 0f, 0f);
		}
	}

	public void OnGameSetupEnded()
	{
		_inSetup = false;
		Input.HasTiltedRight = false;
	}

	public void SetMagnetReach(float reach)
	{
		_magnetCollider.radius = reach;
	}

	private CarWheel SetupWheel(Transform t)
	{
		return t.GetComponent<CarWheel>();
	}

	private void FixedUpdate()
	{
		if (!_crashed && !_rigidbody.isKinematic)
		{
			CarWheel[] carWheels = _carWheels;
			foreach (CarWheel carWheel in carWheels)
			{
				if (HasGas() || IsBoosting())
				{
					carWheel.Accelerate();
				}
				else
				{
					carWheel.Brake();
				}
			}
			float num = Input.Tilt;
			if (_inSetup)
			{
				Vector3 eulerAngles = _rigidbody.rotation.eulerAngles;
				float num2 = Mathf.DeltaAngle(0f, eulerAngles.z);
				if (Mathf.Abs(num2) >= 30f && num2 * num >= 0f)
				{
					num = 0f;
					_rigidbody.angularVelocity *= Mathf.Lerp(0.9f, 0.6f, Mathf.InverseLerp(30f, 50f, Mathf.Abs(num2)));
				}
			}
			if (num != 0f)
			{
				Vector3 angularVelocity = _rigidbody.angularVelocity;
				float z = angularVelocity.z;
				float num3 = 1f;
				if (num * z < 0f)
				{
					num3 = Config._tiltOppositeBoost;
				}
				_rigidbody.angularVelocity += new Vector3(0f, 0f, Config._tiltVelocity * num * num3 * Time.deltaTime);
			}
			if (Input.Slam && !IsTouchingGround())
			{
				if (!Slamming)
				{
					Slamming = true;
					PrefabSingleton<GameSoundManager>.Instance.PlaySlamming();
					if (_slammingFX != null)
					{
						_slammingFX.Play();
					}
				}
				if (_slammingFX != null)
				{
					_slammingFX.transform.rotation = Quaternion.identity;
				}
				_rigidbody.velocity += new Vector3(0f, Config._slamForce, 0f);
			}
			else if (Slamming)
			{
				AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordSlam(Car, 1);
				Slamming = false;
				PrefabSingleton<GameSoundManager>.Instance.StopSlamming();
				if (this.OnSlammingGround != null && IsTouchingGround())
				{
					this.OnSlammingGround();
				}
				if (_slammingFX != null)
				{
					_slammingFX.Stop();
				}
			}
		}
		ClampVelocity();
		if (CarBoost.IsBoosting() && !_rigidbody.isKinematic)
		{
			float num4 = (!CarBoost.IsMegaBoosting()) ? 1f : Config._megaBoostFactor;
			_rigidbody.AddForce(new Vector3(Config._boostAcceleration * num4 * Config._mass * ResistanceFactor(), 0f, 0f));
			return;
		}
		Vector3 velocity = Velocity;
		if (velocity.x < 0f && !HasGas())
		{
			Crash();
		}
	}

	private void ClampVelocity()
	{
		if (!_crashed && !_rigidbody.isKinematic)
		{
			Vector3 velocity = _rigidbody.velocity;
			float maxSpeed = Car.MaxSpeed;
			velocity.x = Mathf.Clamp(velocity.x, 0f - maxSpeed, maxSpeed);
			_rigidbody.velocity = velocity;
			Vector3 angularVelocity = _rigidbody.angularVelocity;
			float z = angularVelocity.z;
			float tiltVelocityLimit = Config._tiltVelocityLimit;
			z = Mathf.Clamp(z, 0f - tiltVelocityLimit, tiltVelocityLimit);
			_rigidbody.angularVelocity = new Vector3(0f, 0f, z);
		}
	}

	public void OnCollisionEnter(Collision collisionInfo)
	{
		ContactPoint[] contacts = collisionInfo.contacts;
		ContactPoint[] array = contacts;
		int num = 0;
		while (true)
		{
			if (num >= array.Length)
			{
				return;
			}
			ContactPoint cp = array[num];
			if (cp.thisCollider.CompareTag(GameSettings.CarBumperColliderTag) && cp.otherCollider.CompareTag(GameSettings.GroundColliderTag))
			{
				UnityEngine.Debug.DrawLine(cp.point, cp.point + cp.normal, Color.red, 5f);
				PrefabSingleton<GameSpecialFXManager>.Instance.PlaySparksFX(cp.point, collisionInfo.relativeVelocity);
				if (CheckForCrash(cp))
				{
					break;
				}
			}
			num++;
		}
		Crash();
	}

	public void OnCollisionStay(Collision collisionInfo)
	{
		ContactPoint[] contacts = collisionInfo.contacts;
		ContactPoint[] array = contacts;
		int num = 0;
		ContactPoint contactPoint;
		while (true)
		{
			if (num < array.Length)
			{
				contactPoint = array[num];
				if (contactPoint.thisCollider.CompareTag(GameSettings.CarBumperColliderTag) && contactPoint.otherCollider.CompareTag(GameSettings.GroundColliderTag))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		UnityEngine.Debug.DrawLine(contactPoint.point, contactPoint.point + contactPoint.normal, Color.blue, 5f);
		PrefabSingleton<GameSpecialFXManager>.Instance.PlaySparksFX(contactPoint.point, collisionInfo.relativeVelocity);
	}

	private bool CheckForCrash(ContactPoint cp)
	{
		return Vector3.Dot(_transform.rotation * Vector3.up, cp.normal) < 0f;
	}

	public Bounds GetVisualBounds()
	{
		return _visuals[0].renderer.bounds;
	}

	public bool IsBoosting()
	{
		return CarBoost.IsBoosting();
	}

	public bool HasGas()
	{
		return CarGas.HasGas();
	}

	public void Crash()
	{
		if (!_crashed)
		{
			_crashed = true;
			Transform[] visuals = _visuals;
			foreach (Transform transform in visuals)
			{
				transform.renderer.enabled = false;
			}
			Transform[] colliders = _colliders;
			foreach (Transform transform2 in colliders)
			{
				transform2.collider.isTrigger = true;
			}
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().detectCollisions = false;
			CarWheel[] carWheels = _carWheels;
			foreach (CarWheel carWheel in carWheels)
			{
				carWheel.OnCrash();
				PrefabSingleton<CameraGame>.Instance.SetTarget(carWheel.transform);
			}
			AutoSingleton<ExplosionManager>.Instance.Explode(Position);
			if (this.OnCrash != null)
			{
				this.OnCrash();
			}
		}
	}

	public bool IsCrashed()
	{
		return _crashed;
	}
}
