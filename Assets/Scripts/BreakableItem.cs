using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItem : TangibleItem
{
	public Transform[] _destroyOnBreak;

	public EmitterController _breakFX;

	private Transform _pivot;

	private Dictionary<Transform, Vector3> _initDOBPos = new Dictionary<Transform, Vector3>();

	private Dictionary<Transform, Vector3> _initPos = new Dictionary<Transform, Vector3>();

	private Dictionary<Transform, Quaternion> _initRot = new Dictionary<Transform, Quaternion>();

	public float ZOffset
	{
		get;
		set;
	}

	public override float GetRightMostPosition()
	{
		Vector3 position = _transform.position;
		float num = position.x;
		Transform[] destroyOnBreak = _destroyOnBreak;
		foreach (Transform t in destroyOnBreak)
		{
			float val = num;
			Vector3 max = RendererBounds.ComputeBounds(t).max;
			num = Math.Max(val, max.x);
		}
		foreach (Transform key in _initPos.Keys)
		{
			Vector3 max2 = RendererBounds.ComputeBounds(key).max;
			if (!PrefabSingleton<CameraGame>.Instance.IsBelowScreen(max2.y))
			{
				num = Math.Max(num, max2.x);
			}
		}
		return num;
	}

	public override void Awake()
	{
		base.Awake();
		_pivot = _transform.Find("Pivot");
		Transform[] destroyOnBreak = _destroyOnBreak;
		foreach (Transform transform in destroyOnBreak)
		{
			_initDOBPos.Add(transform, transform.localPosition);
		}
		foreach (Transform item in _pivot)
		{
			_initPos.Add(item, item.localPosition);
			_initRot.Add(item, item.localRotation);
		}
		ZOffset = 6f;
	}

	public override void Reset()
	{
		base.Reset();
		foreach (Transform key in _initPos.Keys)
		{
			Rigidbody rigidbody = key.GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				UnityEngine.Object.Destroy(rigidbody);
			}
			key.parent = _pivot;
		}
	}

	public override void Activate()
	{
		base.Activate();
		Transform[] destroyOnBreak = _destroyOnBreak;
		foreach (Transform transform in destroyOnBreak)
		{
			transform.localPosition = _initDOBPos[transform];
		}
		foreach (Transform item in _pivot)
		{
			item.localPosition = _initPos[item];
			item.localRotation = _initRot[item];
		}
	}

	protected override IEnumerator CollideImpCR(CarController car)
	{
		OnBreak(car);
		PrefabSingleton<GameSoundManager>.Instance.PlayTangibleItemSound(_sound);
		Transform[] destroyOnBreak = _destroyOnBreak;
		foreach (Transform t in destroyOnBreak)
		{
			t.localPosition = GameSettings.OutOfWorldVector;
		}
		if (!car._rigidbody.isKinematic)
		{
			Vector3 breakerVelocity = car.Velocity;
			foreach (Transform t2 in _initPos.Keys)
			{
				t2.localPosition += Vector3.back * ZOffset;
				ZOffset += 0.1f;
				AddRigidbody(t2.gameObject, car);
				t2.parent = null;
			}
			if (_breakFX != null)
			{
				Vector3 velocity = breakerVelocity;
				velocity.y = Mathf.Max(velocity.y, 5f);
				_breakFX.EmitWithVelocity(_transform.position, velocity);
			}
		}
		yield return new WaitForFixedUpdate();
	}

	protected virtual void OnBreak(CarController car)
	{
	}
}
