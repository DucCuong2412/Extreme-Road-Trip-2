using System.Collections.Generic;
using UnityEngine;

public class PackageItem : BreakableItem
{
	public PickupElement[] _collectiblePrefabs;

	public int _minCollectibles = 5;

	public int _maxCollectibles = 10;

	private WeightedList<Transform> _weightedTemplates;

	private Dictionary<Transform, Queue<CollidableItem>> _instances;

	public override void Awake()
	{
		base.Awake();
		_weightedTemplates = PickupElement.ArrayToWeightedList(_collectiblePrefabs);
		_instances = new Dictionary<Transform, Queue<CollidableItem>>();
		PickupElement[] collectiblePrefabs = _collectiblePrefabs;
		foreach (PickupElement pickupElement in collectiblePrefabs)
		{
			if (_instances.ContainsKey(pickupElement._prefab))
			{
				continue;
			}
			_instances[pickupElement._prefab] = new Queue<CollidableItem>();
			for (int j = 0; j < _maxCollectibles; j++)
			{
				Transform transform = Object.Instantiate(pickupElement._prefab, GameSettings.OutOfWorldVector, Quaternion.identity) as Transform;
				if (transform != null)
				{
					_instances[pickupElement._prefab].Enqueue(transform.GetComponent<CollidableItem>());
				}
			}
		}
	}

	protected override void OnBreak(CarController car)
	{
		base.OnBreak(car);
		int num = Random.Range(_minCollectibles, _maxCollectibles);
		for (int i = 0; i < num; i++)
		{
			Transform key = _weightedTemplates.Pick();
			if (_instances[key].Count > 0)
			{
				Vector3 vector = new Vector3(Random.Range(-10f, 40f), Random.Range(-6f, 35f), 0f);
				CollidableItem collidableItem = _instances[key].Dequeue();
				collidableItem.transform.position = _transform.position + vector * Time.fixedDeltaTime;
				Vector3 velocity = vector;
				float x = velocity.x;
				Vector3 velocity2 = car.Velocity;
				velocity.x = x + velocity2.x;
				Rigidbody rigidbody = collidableItem.GetRigidbody();
				if (rigidbody != null)
				{
					rigidbody.velocity = velocity;
				}
				collidableItem.Reset();
				collidableItem.Activate();
				collidableItem.Collide(car);
				_instances[key].Enqueue(collidableItem);
			}
		}
	}
}
