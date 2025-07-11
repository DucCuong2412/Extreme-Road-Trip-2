using System.Collections.Generic;
using UnityEngine;

public class CrateCarSpecialBehaviour : CarSpecialBehaviour
{
	public const float _slamSpeedLimit = -30f;

	public List<Transform> _crates;

	private CarController _carController;

	private void Start()
	{
		_carController = base.gameObject.GetComponent<CarController>();
		_carController.OnCrash += OnCrash;
		_carController.OnSlammingGround += OnSlammingGround;
	}

	private void OnCrash()
	{
		if (_crates != null)
		{
			foreach (Transform crate in _crates)
			{
				if (crate != null)
				{
					ReleaseCrate(crate);
				}
			}
		}
	}

	private void ReleaseCrate(Transform transform)
	{
		transform.parent = null;
		GameObject gameObject = transform.gameObject;
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.constraints = (RigidbodyConstraints)56;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		rigidbody.drag = 1f;
		gameObject.AddComponent<BoxCollider>().isTrigger = true;
		rigidbody.velocity = new Vector3(Random.Range(-5f, 5f), Random.Range(5f, 10f), 0f);
		gameObject.AddComponent<CrateColliderHandler>();
	}

	private void OnSlammingGround()
	{
		Vector3 velocity = _carController.Velocity;
		if (velocity.y < -30f)
		{
			ShootCrate();
		}
	}

	private void ShootCrate()
	{
		if (_crates.Count > 0)
		{
			int index = Random.Range(0, _crates.Count);
			Transform transform = _crates[index];
			ReleaseCrate(transform);
			Vector3 velocity = _carController.Velocity;
			transform.rigidbody.velocity = new Vector3(velocity.x, (0f - velocity.y) / 3f, velocity.z);
			_crates.RemoveAt(index);
		}
	}
}
