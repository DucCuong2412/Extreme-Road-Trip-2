using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPropsManager : AutoSingleton<ActionPropsManager>
{
	private const int _numInstancesPerProp = 5;

	private WeightedList<Transform> _weightedProps;

	private Dictionary<Transform, Queue<Item>> _usedProps;

	private Dictionary<Transform, Queue<Item>> _freeProps;

	public void FreeProps()
	{
		CameraGame instance = PrefabSingleton<CameraGame>.Instance;
		foreach (Transform key in _usedProps.Keys)
		{
			while (_usedProps[key].Count > 0 && instance.IsLeftOfScreen(_usedProps[key].Peek().GetRightMostPosition()))
			{
				Item item = _usedProps[key].Dequeue();
				item.Reset();
				_freeProps[key].Enqueue(item);
			}
		}
	}

	public void SpawnProps(Curve curve)
	{
		StartCoroutine(SpawnPropsCR(curve));
	}

	protected override void OnAwake()
	{
		List<PickupElement> elements = AutoSingleton<ActionPropDatabase>.Instance.GetElements();
		_weightedProps = AutoSingleton<ActionPropDatabase>.Instance.GetWeightedElements();
		_usedProps = new Dictionary<Transform, Queue<Item>>();
		_freeProps = new Dictionary<Transform, Queue<Item>>();
		foreach (PickupElement item in elements)
		{
			for (int i = 0; i < 5; i++)
			{
				if (item._prefab != null)
				{
					if (!_usedProps.ContainsKey(item._prefab))
					{
						_usedProps[item._prefab] = new Queue<Item>();
					}
					if (!_freeProps.ContainsKey(item._prefab))
					{
						_freeProps[item._prefab] = new Queue<Item>();
					}
					Transform transform = Object.Instantiate(item._prefab, GameSettings.OutOfWorldVector, Quaternion.identity) as Transform;
					if (transform != null)
					{
						Item component = transform.GetComponent<Item>();
						_freeProps[item._prefab].Enqueue(component);
					}
				}
			}
		}
		base.OnAwake();
	}

	private IEnumerator SpawnPropsCR(Curve curve)
	{
		List<CurvePoint> points = curve.GetPoints();
		int len = points.Count;
		for (int i = 5; i < len - 5; i++)
		{
			if (Random.Range(0f, 1f) < 0.005f)
			{
				yield return StartCoroutine(SpawnPropCR(curve, points[i]));
			}
		}
	}

	private IEnumerator SpawnPropCR(Curve curve, CurvePoint point)
	{
		Transform template = _weightedProps.Pick();
		Item item = GetFreeProp(template);
		if (item != null)
		{
			Transform t = item.transform;
			t.position = point.position;
			t.rotation = Quaternion.LookRotation(Vector3.forward, point.normal);
			item.Activate();
			_usedProps[template].Enqueue(item);
		}
		yield return new WaitForFixedUpdate();
	}

	private Item GetFreeProp(Transform t)
	{
		if (_freeProps[t].Count > 0)
		{
			return _freeProps[t].Dequeue();
		}
		Transform transform = Object.Instantiate(t, GameSettings.OutOfWorldVector, Quaternion.identity) as Transform;
		if (transform != null)
		{
			return transform.GetComponent<Item>();
		}
		return null;
	}
}
