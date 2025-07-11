using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsManager : AutoSingleton<PropsManager>
{
	private const int _numInstancesPerProp = 5;

	private Transform[] _templates;

	private List<Item> _usedProps;

	private List<Item> _freeProps;

	public void FreeProps()
	{
		CameraGame cam = PrefabSingleton<CameraGame>.Instance;
		_usedProps.RemoveAll(delegate(Item item)
		{
			if (cam.IsLeftOfScreen(item.GetRightMostPosition()))
			{
				item.Reset();
				_freeProps.Add(item);
				return true;
			}
			return false;
		});
	}

	public void SpawnProps(Curve curve)
	{
		StartCoroutine(SpawnPropsCR(curve));
	}

	protected override void OnAwake()
	{
		_usedProps = new List<Item>();
		_freeProps = new List<Item>();
		_templates = AutoSingleton<LocationManager>.Instance.GetPropsPrefabs();
		Transform[] templates = _templates;
		foreach (Transform original in templates)
		{
			for (int j = 0; j < 5; j++)
			{
				Transform transform = Object.Instantiate(original, GameSettings.OutOfWorldVector, Quaternion.identity) as Transform;
				if (transform != null)
				{
					Item component = transform.GetComponent<Item>();
					_freeProps.Add(component);
				}
			}
		}
		base.OnAwake();
	}

	private IEnumerator SpawnPropsCR(Curve c)
	{
		List<CurvePoint> points = c.GetPoints();
		int len = points.Count;
		for (int i = 5; i < len - 5; i++)
		{
			if (Random.Range(0f, 1f) < 0.04f)
			{
				yield return StartCoroutine(SpawnPropCR(c, points[i]));
			}
		}
	}

	private IEnumerator SpawnPropCR(Curve curve, CurvePoint point)
	{
		Item item = GetRandomFreeProp();
		if (item != null)
		{
			_usedProps.Add(item);
			Transform t = item.transform;
			t.position = point.position;
			if (item.IsCollidable)
			{
				t.rotation = Quaternion.LookRotation(Vector3.forward, point.normal);
				item.Activate();
			}
			else
			{
				t.rotation = Quaternion.identity;
				Renderer r = t.GetComponentInChildren<Renderer>();
				Bounds b = r.bounds;
				Vector3 min = b.min;
				float minx = min.x;
				Vector3 max = b.max;
				float maxx = max.x;
				float miny3 = curve.GetRealCurveHeight(minx);
				float miny2 = curve.GetRealCurveHeight(maxx);
				Vector3 position = point.position;
				position.y = Mathf.Min(position.y, miny3, miny2);
				t.position = position;
			}
		}
		yield return new WaitForFixedUpdate();
	}

	private Item GetRandomFreeProp()
	{
		if (_freeProps.Count > 0)
		{
			int index = Random.Range(0, _freeProps.Count);
			Item result = _freeProps[index];
			_freeProps.RemoveAt(index);
			return result;
		}
		if (_templates.Length > 0)
		{
			int num = Random.Range(0, _templates.Length);
			Transform transform = Object.Instantiate(_templates[num], GameSettings.OutOfWorldVector, Quaternion.identity) as Transform;
			if (transform != null)
			{
				return transform.GetComponent<Item>();
			}
			return null;
		}
		return null;
	}
}
