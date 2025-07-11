using System.Collections;
using UnityEngine;

public abstract class CollidableItem : Item
{
	protected Collider _collider;

	protected Rigidbody _rigidbody;

	protected bool _collided;

	public override bool IsCollidable => true;

	public Rigidbody GetRigidbody()
	{
		return _rigidbody;
	}

	public override void Awake()
	{
		_collider = base.GetComponent<Collider>();
		_rigidbody = base.GetComponent<Rigidbody>();
		base.Awake();
	}

	public override void Activate()
	{
		_collided = false;
		base.Activate();
	}

	public void Collide(CarController car)
	{
		if (!_collided)
		{
			_collided = true;
			StartCoroutine(CollideImpCR(car));
		}
	}

	protected abstract IEnumerator CollideImpCR(CarController car);

	private void OnTriggerEnter(Collider other)
	{
		if (_collided)
		{
			return;
		}
		if (IsCollectible)
		{
			if (other.CompareTag(GameSettings.CarMagnetColliderTag))
			{
				Collide(other.transform.root.GetComponentInChildren<CarController>());
			}
		}
		else if (other.CompareTag(GameSettings.CarBumperColliderTag))
		{
			Collide(other.transform.root.GetComponentInChildren<CarController>());
		}
	}
}
