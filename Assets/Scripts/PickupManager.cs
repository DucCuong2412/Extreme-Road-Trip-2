using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : AutoSingleton<PickupManager>
{
	private const int _numInstancesPerIntroSequence = 1;

	private const int _numInstancesPerGameSequence = 3;

	private const int _pointsBetweenPickups = 2;

	private GameMode _mode;

	private WeightedList<List<GameObject>> _weightedIntroPickupSequences;

	private WeightedList<List<GameObject>> _weightedGamePickupSequences;

	private Dictionary<GameObject, Queue<Item>> _usedPickups;

	private Dictionary<GameObject, Queue<Item>> _freePickups;

	public void Create(GameMode mode)
	{
		_mode = mode;
		AutoSingleton<PickupDatabase>.Instance.Create(mode);
		List<PickupSequence> introSequences = AutoSingleton<PickupDatabase>.Instance.GetIntroSequences();
		List<PickupSequence> gameSequences = AutoSingleton<PickupDatabase>.Instance.GetGameSequences();
		_weightedIntroPickupSequences = AutoSingleton<PickupDatabase>.Instance.GetWeightedIntroSequences();
		_weightedGamePickupSequences = AutoSingleton<PickupDatabase>.Instance.GetWeightedGameSequences();
		_usedPickups = new Dictionary<GameObject, Queue<Item>>();
		_freePickups = new Dictionary<GameObject, Queue<Item>>();
		FillInstances(introSequences, 1);
		FillInstances(gameSequences, 3);
	}

	public void FreePickups()
	{
		CameraGame instance = PrefabSingleton<CameraGame>.Instance;
		foreach (GameObject key in _usedPickups.Keys)
		{
			while (_usedPickups[key].Count > 0 && instance.IsLeftOfScreen(_usedPickups[key].Peek().GetRightMostPosition()))
			{
				Item item = _usedPickups[key].Dequeue();
				item.Reset();
				_freePickups[key].Enqueue(item);
			}
		}
	}

	public void SpawnPickups(Curve curve, bool intro)
	{
		StartCoroutine(SpawnPickupsCR(curve, intro));
	}

	private void FillInstances(List<PickupSequence> list, int copies)
	{
		foreach (PickupSequence item in list)
		{
			for (int i = 0; i < copies; i++)
			{
				foreach (GameObject template in item._templates)
				{
					if (template != null)
					{
						if (!_usedPickups.ContainsKey(template))
						{
							_usedPickups[template] = new Queue<Item>();
						}
						if (!_freePickups.ContainsKey(template))
						{
							_freePickups[template] = new Queue<Item>();
						}
						GameObject gameObject = Object.Instantiate(template, GameSettings.OutOfWorldVector, Quaternion.identity) as GameObject;
						if (gameObject != null)
						{
							Item component = gameObject.GetComponent<Item>();
							_freePickups[template].Enqueue(component);
						}
					}
				}
			}
		}
	}

	private IEnumerator SpawnPickupsCR(Curve curve, bool intro)
	{
		List<CurvePoint> points = curve.GetPoints();
		float distance = (_mode != 0) ? Random.Range(15f, 35f) : Random.Range(50f, 100f);
		int num;
		if (intro)
		{
			num = 1;
		}
		else
		{
			CurvePoint curvePoint = points[points.Count - 1];
			float x = curvePoint.position.x;
			CurvePoint curvePoint2 = points[0];
			num = (int)((x - curvePoint2.position.x) / distance);
		}
		int numSequences = num;
		int pointsBetweenSequences = (numSequences > 0) ? (points.Count / numSequences) : 0;
		int currentPoint = intro ? Mathf.RoundToInt((float)points.Count * 0.5f) : 0;
		for (int seq = 0; seq < numSequences; seq++)
		{
			List<GameObject> templates = (!intro) ? _weightedGamePickupSequences.Pick() : _weightedIntroPickupSequences.Pick();
			if (templates != null && templates.Count * 2 < points.Count - currentPoint)
			{
				foreach (GameObject go in templates)
				{
					if (go != null)
					{
						Item item = GetFreePickup(go);
						if (item != null)
						{
							Transform t = item.transform;
							Transform transform = t;
							CurvePoint curvePoint3 = points[currentPoint];
							transform.position = curvePoint3.position;
							Transform transform2 = t;
							Vector3 forward = Vector3.forward;
							CurvePoint curvePoint4 = points[currentPoint];
							transform2.rotation = Quaternion.LookRotation(forward, curvePoint4.normal);
							item.Activate();
							_usedPickups[go].Enqueue(item);
						}
					}
					currentPoint += 2;
				}
				int offset = pointsBetweenSequences - templates.Count * 2;
				currentPoint += ((offset <= 0) ? 2 : offset);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private Item GetFreePickup(GameObject go)
	{
		if (_freePickups[go].Count > 0)
		{
			return _freePickups[go].Dequeue();
		}
		GameObject gameObject = Object.Instantiate(go, GameSettings.OutOfWorldVector, Quaternion.identity) as GameObject;
		if (gameObject != null)
		{
			return gameObject.GetComponent<Item>();
		}
		return null;
	}
}
