using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
	private List<CurvePoint> _points;

	private Maybe<float> _forcedMin = new Maybe<float>();

	private Maybe<float> _forcedMax = new Maybe<float>();

	private Material _groundMaterial;

	private Material _surfaceMaterial;

	private float _ymin = float.PositiveInfinity;

	private float _ymax = float.NegativeInfinity;

	private float _slope;

	private float _xmin;

	private float _xmax;

	private static Vector3 _lastPositionAdded = Vector3.zero;

	private void Awake()
	{
		_points = new List<CurvePoint>(400);
	}

	public List<CurvePoint> GetPoints()
	{
		return _points;
	}

	public void Create(Vector3 firstPoint, float width, float[] data, float slope)
	{
		_groundMaterial = AutoSingleton<GroundManager>.Instance.GroundMaterial;
		_surfaceMaterial = AutoSingleton<GroundManager>.Instance.SurfaceMaterial;
		_slope = slope;
		_xmin = firstPoint.x;
		_xmax = _xmin + width;
		int num = data.Length;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)i / (float)num;
			Vector3 position = firstPoint + new Vector3(num2 * width, data[i], 0f);
			_ymin = Mathf.Min(_ymin, position.y - slope * num2);
			_ymax = Mathf.Max(_ymax, position.y - slope * num2);
			AddPosition(position);
		}
		if (_groundMaterial != null)
		{
			CurveRenderer.Create(this, isGround: true, _groundMaterial, 0.0625f, 30f);
		}
		if (_surfaceMaterial != null)
		{
			CurveRenderer.Create(this, isGround: false, _surfaceMaterial, 1f, 1f);
		}
		CurveMeshCollider.Create(this);
	}

	public CurvePoint GetEndPoint()
	{
		List<CurvePoint> points = GetPoints();
		return points[points.Count - 1];
	}

	public float GetXRatio(float x)
	{
		return Mathf.InverseLerp(_xmin, _xmax, x);
	}

	public void GetPointsAround(List<CurvePoint> points, float x, out int a, out int b)
	{
		a = 0;
		b = points.Count - 1;
		while (b - a > 1)
		{
			int num = a + (b - a) / 2;
			CurvePoint curvePoint = points[num];
			if (x < curvePoint.position.x)
			{
				b = num;
			}
			else
			{
				a = num;
			}
		}
	}

	public float GetRealCurveHeight(float x)
	{
		List<CurvePoint> points = GetPoints();
		GetPointsAround(points, x, out int a, out int b);
		CurvePoint curvePoint = points[a];
		float y = curvePoint.position.y;
		CurvePoint curvePoint2 = points[b];
		float y2 = curvePoint2.position.y;
		CurvePoint curvePoint3 = points[a];
		float x2 = curvePoint3.position.x;
		CurvePoint curvePoint4 = points[b];
		return Mathf.Lerp(y, y2, Mathf.InverseLerp(x2, curvePoint4.position.x, x));
	}

	public float GetCurveMin(float x)
	{
		if (_forcedMin.IsSet())
		{
			return _forcedMin.Get();
		}
		if (_ymin == float.PositiveInfinity)
		{
			return 0f;
		}
		return _ymin + _slope * GetXRatio(x);
	}

	public void SetCurveMin(float min)
	{
		_forcedMin.Set(min);
	}

	public void SetCurveMax(float max)
	{
		_forcedMax.Set(max);
	}

	public float GetCurveMax(float x)
	{
		if (_forcedMax.IsSet())
		{
			return _forcedMax.Get();
		}
		if (_ymax == float.NegativeInfinity)
		{
			return 0f;
		}
		return _ymax + _slope * GetXRatio(x);
	}

	private void AddPosition(Vector3 position)
	{
		Vector3 vector = position - _lastPositionAdded;
		Vector3 normalized = new Vector3(0f - vector.y, vector.x, 0f).normalized;
		_points.Add(new CurvePoint(position, normalized));
		_lastPositionAdded = position;
	}

	private void ResetMin()
	{
		_ymin = float.PositiveInfinity;
	}
}
