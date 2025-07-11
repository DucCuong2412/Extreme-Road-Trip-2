using System;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
	private WheelCollider _wheelCollider;

	private float _radius;

	private float _invCircumference;

	private Transform _pivotTransform;

	private CarController _car;

	private CarConfiguration _carConfig;

	private float _timeLastEmit;

	private bool _hitLastFrame;

	private bool _crashed;

	private bool _freeWheeling;

	private ParticleSystem _dustFX;

	private ParticleSystem _groundChunkFX;

	private TrailRenderer _trailFX;

	private float _targetAcceleration;

	private float _targetBrake;

	public void SetFreeWheeling(bool freeWheeling)
	{
		_freeWheeling = freeWheeling;
	}

	private void SetupWheel()
	{
		GameObject gameObject = new GameObject(base.name + " Collider");
		gameObject.layer = LayerMask.NameToLayer(GameSettings.CarLayer);
		gameObject.transform.parent = base.transform.parent;
		gameObject.transform.localPosition = base.transform.localPosition;
		gameObject.transform.localRotation = base.transform.localRotation;
		WheelCollider wheelCollider = gameObject.AddComponent<WheelCollider>();
		wheelCollider.tag = GameSettings.CarWheelColliderTag;
		wheelCollider.suspensionDistance = _carConfig._suspensionDistance;
		gameObject.transform.position += new Vector3(0f, _carConfig._suspensionDistance, 0f);
		JointSpring suspensionSpring = wheelCollider.suspensionSpring;
		suspensionSpring.spring = _carConfig._suspensionSpring;
		suspensionSpring.damper = _carConfig._suspensionDamper;
		wheelCollider.suspensionSpring = suspensionSpring;
		wheelCollider.radius = GetRadius();
		_wheelCollider = wheelCollider;
	}

	public void Setup(CarController car)
	{
		_car = car;
		_carConfig = _car.Config;
		Vector3 extents = GetComponent<Renderer>().bounds.extents;
		_radius = extents.x;
		Transform transform = new GameObject(base.name + " Anchor").transform;
		transform.position = base.transform.position;
		transform.parent = base.transform.parent;
		_pivotTransform = new GameObject(base.name + " Pivot").transform;
		_pivotTransform.position = transform.position;
		_pivotTransform.parent = transform;
		List<Transform> list = new List<Transform>();
		foreach (Transform item in base.transform)
		{
			list.Add(item);
		}
		foreach (Transform item2 in list)
		{
			item2.parent = _pivotTransform;
		}
		_crashed = false;
		float num = (float)Math.PI * 2f * _radius;
		_invCircumference = 1f / num;
		SetupWheel();
		base.transform.parent = _pivotTransform;
	}

	private ParticleSystem SetupEmitter(Transform prefab, float offsetX, float offsetZ)
	{
		Transform transform = (Transform)UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		if (transform != null)
		{
			transform.parent = _pivotTransform;
			transform.localPosition = new Vector3(offsetX, 0f - _radius, offsetZ);
			return transform.GetComponent<ParticleSystem>();
		}
		return null;
	}

	private TrailRenderer SetupTrail(Transform prefab)
	{
		Transform transform = (Transform)UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		if (transform != null)
		{
			transform.parent = _pivotTransform;
			transform.localPosition = new Vector3(0f, 0f, 1f);
			return transform.GetComponent<TrailRenderer>();
		}
		return null;
	}

	private void Start()
	{
		_dustFX = SetupEmitter(PrefabSingleton<GameSpecialFXManager>.Instance._dustFXPrefab, -0.75f, 2.5f);
		_groundChunkFX = SetupEmitter(PrefabSingleton<GameSpecialFXManager>.Instance._groundChunkFXPrefab, 0f, -1f);
		_trailFX = SetupTrail(PrefabSingleton<GameSpecialFXManager>.Instance._boostTrailPrefab);
	}

	private void PlayDustFX()
	{
		if (_dustFX != null)
		{
			Vector3 a = _car.Velocity;
			a.y = 0f;
			a += new Vector3(-2f, 1f, 0f);
			var main = _dustFX.main;
			main.startSpeed = a.magnitude * 0.9f;
			_dustFX.Emit(1);
		}
	}

	private void PlayGroundChunkFX()
	{
		if (_groundChunkFX != null)
		{
			Vector3 vector = _car.Velocity;
			vector.y = 0f;
			vector += new Vector3(-2f, 4f, 0f);
			var main = _groundChunkFX.main;
			main.startSpeed = vector.magnitude;
			_groundChunkFX.Emit(1);
		}
	}

	public void Accelerate()
	{
		_targetAcceleration = _carConfig._motorTorque * _invCircumference * _car.ResistanceFactor();
		_targetBrake = 0f;
	}

	public void Brake()
	{
		_targetAcceleration = 0f;
		_targetBrake = _carConfig._brakeTorque * _invCircumference;
	}

	public float GetRadius()
	{
		return _radius;
	}

	public bool IsOnGround()
	{
		return _wheelCollider.isGrounded;
	}

	private void FixedUpdate()
	{
		if (!_crashed)
		{
			if (_wheelCollider != null && _wheelCollider.steerAngle != 90f)
			{
				_wheelCollider.steerAngle = 90f;
			}
			float num = _car.Velocity.magnitude * 0.033f;
			float num2 = Mathf.Max(0f, Vector3.Dot(_car.Velocity.normalized, _car.Down));
			float suspensionDistance = _carConfig._suspensionDistance;
			float num3 = Mathf.Max(suspensionDistance, num2 * num);
			_wheelCollider.suspensionDistance = num3;
			float suspensionDamper = _carConfig._suspensionDamper;
			float num4 = suspensionDistance / num3;
			JointSpring suspensionSpring = _wheelCollider.suspensionSpring;
			suspensionSpring.damper = suspensionDamper * num4;
			_wheelCollider.suspensionSpring = suspensionSpring;
			if (_freeWheeling)
			{
				_wheelCollider.motorTorque = 0f;
				_wheelCollider.brakeTorque = 0f;
			}
			else
			{
				_wheelCollider.motorTorque = Mathf.Lerp(_wheelCollider.motorTorque, _targetAcceleration, 3f * Time.deltaTime);
				_wheelCollider.brakeTorque = Mathf.Lerp(_wheelCollider.brakeTorque, _targetBrake, 3f * Time.deltaTime);
			}
		}
		else
		{
			_wheelCollider.motorTorque = 0f;
			_wheelCollider.brakeTorque = 0f;
		}
	}

	private void LateUpdate()
	{
		if (_wheelCollider != null && !_crashed)
		{
			WheelHit hit;
			bool groundHit = _wheelCollider.GetGroundHit(out hit);
			if (groundHit)
			{
				Vector3 localPosition = _pivotTransform.localPosition;
				float y = localPosition.y;
				Vector3 position = _pivotTransform.position;
				Vector3 point = hit.point;
				_pivotTransform.position = new Vector3(point.x, point.y + _radius, position.z);
				Vector3 localPosition2 = _pivotTransform.localPosition;
				localPosition2.x = 0f;
				localPosition2.y = Mathf.Lerp(y, Mathf.Max(localPosition2.y, 0f - _carConfig._suspensionDistance), 20f * Time.deltaTime);
				_pivotTransform.localPosition = localPosition2;
				float time = Time.time;
				if (time - _timeLastEmit > 0.1f)
				{
					Vector3 velocity = _car.Velocity;
					if (velocity.x > 2f)
					{
						PlayDustFX();
						_timeLastEmit = time;
					}
				}
				if (!_hitLastFrame)
				{
					PrefabSingleton<GameSoundManager>.Instance.PlayTireContact();
					PlayGroundChunkFX();
				}
			}
			else
			{
				Transform pivotTransform = _pivotTransform;
				Vector3 localPosition3 = _pivotTransform.localPosition;
				Vector3 localPosition4 = _pivotTransform.localPosition;
				pivotTransform.localPosition = Vector3.Lerp(localPosition3, new Vector3(localPosition4.x, 0f - _carConfig._suspensionDistance, 0f), 3f * Time.deltaTime);
			}
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			float z = eulerAngles.z;
			z -= _wheelCollider.rpm * Time.deltaTime * 6f;
			base.transform.rotation = Quaternion.Euler(0f, 0f, z);
			_hitLastFrame = groundHit;
		}
		if (_trailFX != null)
		{
			if (_car.IsBoosting())
			{
				_trailFX.time = Mathf.Lerp(_trailFX.time, 0.2f, 5f * Time.deltaTime);
			}
			else
			{
				_trailFX.time = Mathf.Lerp(_trailFX.time, 0f, 10f * Time.deltaTime);
			}
		}
	}

	public void OnCrash()
	{
		if (!_crashed)
		{
			_crashed = true;
			base.transform.parent = null;
			base.gameObject.AddComponent<Rigidbody>();
			GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)56;
			GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
			SphereCollider sphereCollider = base.gameObject.AddComponent<SphereCollider>();
			float radius = _radius;
			Vector3 localScale = base.transform.localScale;
			sphereCollider.radius = radius / localScale.x;
			GetComponent<Collider>().material = AutoSingleton<CarPhysics>.Instance.Tire;
			GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(5f, 10f), 0f);
		}
	}
}
