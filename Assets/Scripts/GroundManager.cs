using System;
using UnityEngine;

public class GroundManager : AutoSingleton<GroundManager>
{
	private delegate float SlopeFunction(float r);

	private Curve _prev;

	private Curve _curr;

	private Curve _next;

	private int _segmentCount;

	private Vector3 _startPoint = new Vector3(-50f, -1f, 0f);

	public Material GroundMaterial
	{
		get;
		private set;
	}

	public Material SurfaceMaterial
	{
		get;
		private set;
	}

	public void Create(GameMode mode)
	{
		AutoSingleton<PropsManager>.Instance.Create();
		AutoSingleton<PickupManager>.Instance.Create(mode);
		AutoSingleton<ActionPropsManager>.Instance.Create();
		AutoSingleton<FriendsRoadSignManager>.Instance.Create();
	}

	protected override void OnStart()
	{
		GroundMaterial = AutoSingleton<LocationManager>.Instance.GetGroundMaterial();
		SurfaceMaterial = AutoSingleton<LocationManager>.Instance.GetSurfaceMaterial();
		_prev = null;
		_curr = CreateFirstCurve(_startPoint);
		_next = null;
		base.OnStart();
	}

	private SlopeFunction MakeSin(float a, float p)
	{
		return (float r) => a * Mathf.Sin(r * p * (float)Math.PI * 2f);
	}

	private SlopeFunction MakeSlope(float h)
	{
		return (float r) => r * h;
	}

	private SlopeFunction MakeHermite(float h)
	{
		return (float r) => Mathfx.Hermite(0f, h, r);
	}

	private SlopeFunction MakeHermiteBump(float h, float from, float over)
	{
		return delegate(float r)
		{
			if (r < from)
			{
				return 0f;
			}
			return (r > from + over) ? h : Mathfx.Hermite(0f, h, (r - from) / over);
		};
	}

	private SlopeFunction MakeBounceBump(float a, float from, float over)
	{
		return (float r) => (r < from || r > from + over) ? 0f : (a * Mathfx.Bounce(1f - Mathf.Clamp01((r - from) / over)));
	}

	private SlopeFunction MakeBerpBump(float h, float from, float over)
	{
		return delegate(float r)
		{
			if (r < from)
			{
				return 0f;
			}
			return (r > from + over) ? h : (h - Mathfx.Berp(h, 0f, Mathf.Clamp01((r - from) / over)));
		};
	}

	private Curve CreateCurveUsingFunction(string name, Vector3 start, float width, int segCount, SlopeFunction SF)
	{
		name = Mathf.RoundToInt(start.x).ToString() + "_" + name;
		Curve curve = new GameObject(name).AddComponent<Curve>();
		float[] array = new float[segCount];
		for (int i = 0; i < segCount; i++)
		{
			float r = (float)i / (float)segCount;
			array[i] = SF(r);
		}
		curve.Create(start, width, array, 0f);
		return curve;
	}

	private Curve CreateFirstCurve(Vector3 pointStart)
	{
		Curve curve = CreateCurveUsingFunction("Hermite Bump Start", pointStart, 180f, 250, MakeHermiteBump(-12f, 0.92f, 0.08f));
		curve.SetCurveMin(-5f);
		curve.SetCurveMax(0f);
		AddPropsOnCurve(curve);
		AddPickupsOnCurve(curve, intro: true);
		return curve;
	}

	private Curve CreateCurveWithRandomFunction(Vector3 pointStart)
	{
		switch (UnityEngine.Random.Range(0, 9))
		{
		case 0:
			return CreateCurveUsingFunction("Sin", pointStart, 80f, 200, MakeSin(UnityEngine.Random.Range(1f, 3f), UnityEngine.Random.Range(3, 6)));
		case 1:
			return CreateCurveUsingFunction("Random upslope", pointStart, UnityEngine.Random.Range(30f, 50f), 100, MakeSlope(UnityEngine.Random.Range(4f, 8f)));
		case 2:
			return CreateCurveUsingFunction("Random downslope", pointStart, UnityEngine.Random.Range(30f, 50f), 100, MakeSlope(UnityEngine.Random.Range(-4f, -18f)));
		case 3:
			return CreateCurveUsingFunction("Simple Hermite", pointStart, 80f, 50, MakeHermite(10f));
		case 4:
			return CreateCurveUsingFunction("Hermite up bump", pointStart, 80f, 100, MakeHermiteBump(UnityEngine.Random.Range(2f, 6f), 0.4f, 0.2f));
		case 5:
			return CreateCurveUsingFunction("Hermite down bump", pointStart, 100f, 100, MakeHermiteBump(UnityEngine.Random.Range(-6f, -12f), 0.6f, 0.1f));
		case 6:
			return CreateCurveUsingFunction("Bounce bump", pointStart, 100f, 100, MakeBounceBump(3.5f, 0.2f, 0.6f));
		case 7:
			return CreateCurveUsingFunction("Berp bump", pointStart, 100f, 100, MakeBerpBump(3f, 0.4f, 0.3f));
		default:
			return CreateCurveUsingFunction("Flat ground", pointStart, UnityEngine.Random.Range(20f, 30f), 20, MakeSlope(UnityEngine.Random.Range(-1f, 1f)));
		}
	}

	private Curve CreateCurveClassical(Vector3 pointStart)
	{
		Curve curve = new GameObject("Ground").AddComponent<Curve>();
		float num = UnityEngine.Random.Range(0.1f, 0.3f);
		float num2 = 200f;
		float num3 = num2 / 20f;
		float[] array = new float[20];
		for (int i = 0; i < 20; i++)
		{
			float num4 = pointStart.x + (float)i * num3;
			float num5 = 7f;
			float num6 = Mathf.Sin(num * num3 * (float)i);
			float num7 = Mathf.Sin((1f - num) * num3 * (float)i);
			float a = 1f + num5 * (num6 + num7);
			a = Mathf.Max(a, 0f);
			a = Mathf.Min(a, i);
			a = (array[i] = Mathf.Min(a, (num4 - 100f) * 0.04f));
		}
		curve.Create(pointStart, num2, array, 0f);
		return curve;
	}

	private Curve CreateCurveRandomWalk(Vector3 pointStart)
	{
		Curve curve = new GameObject("Ground").AddComponent<Curve>();
		float width = 500f;
		float[] array = new float[50];
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < 50; i++)
		{
			num2 = ((!(Mathf.Abs(num2) < 4f)) ? (num2 + UnityEngine.Random.Range(0f, 5f) * Mathf.Sign(0f - num2)) : (num2 + UnityEngine.Random.Range(-3f, 3f)));
			num = (array[i] = num + num2);
		}
		curve.Create(pointStart, width, array, 0f);
		return curve;
	}

	private Curve CreateCurveSimplexNoise(Vector3 pointStart)
	{
		Curve curve = new GameObject("Ground").AddComponent<Curve>();
		float num = 400f;
		int num2 = (int)num;
		float num3 = num / (float)num2;
		float[] array = new float[num2];
		float num4 = 0.02f + Simplex.Noise(pointStart.x, 50f) * 0.01f;
		float num5 = 0.03f + Simplex.Noise(pointStart.x, 100f) * 0.01f;
		float num6 = Simplex.Noise(pointStart.x, 150f) * 50f;
		for (int i = 0; i < num2; i++)
		{
			float num7 = pointStart.x + (float)i * num3;
			float num8 = 5.5f;
			float num9 = 2.5f;
			float num10 = num8 * Simplex.Noise(num7 * num4, 0f);
			float num11 = num9 * Simplex.Noise(num7 * num5, 0f);
			float num12 = num10 + num11;
			if (num12 < 0f)
			{
				float num13 = Mathf.Pow(1f - (0f - num12) / (num8 + num9), 1.5f);
				num12 *= num13;
			}
			num12 *= Mathf.Min(0.0025f * num7, 1f);
			array[i] = num12 + num6 * ((float)i / (float)num2);
		}
		RecenterData(array);
		curve.Create(pointStart, num, array, num6);
		return curve;
	}

	private void RecenterData(float[] data)
	{
		float num = data[0];
		float num2 = data.Length;
		for (int i = 0; (float)i < num2; i++)
		{
			data[i] -= num;
		}
	}

	private Curve CreateCurve(Vector3 pointStart)
	{
		return CreateCurveSimplexNoise(pointStart);
	}

	public float GetGroundHeightForCamera(float x)
	{
		return _curr.GetCurveMin(x);
	}

	public float GetGroundMaxSlope(float x)
	{
		return _curr.GetCurveMax(x);
	}

	public float GetGroundHeight(float x)
	{
		if (_prev != null)
		{
			float xRatio = _prev.GetXRatio(x);
			if (xRatio < 1f)
			{
				return _prev.GetRealCurveHeight(x);
			}
		}
		if (_curr != null)
		{
			return _curr.GetRealCurveHeight(x);
		}
		return 0f;
	}

	public void FixedUpdate()
	{
		Vector3 cameraPosition = AutoSingleton<WorldManager>.Instance.GetCameraPosition();
		float x = cameraPosition.x;
		float xRatio = _curr.GetXRatio(x);
		if (_prev != null && xRatio > 0.25f)
		{
			AutoSingleton<PropsManager>.Instance.FreeProps();
			AutoSingleton<PickupManager>.Instance.FreePickups();
			AutoSingleton<ActionPropsManager>.Instance.FreeProps();
			AutoSingleton<FriendsRoadSignManager>.Instance.FreeItems();
			UnityEngine.Object.Destroy(_prev.gameObject);
			_prev = null;
		}
		if (_next == null && xRatio > 0.5f)
		{
			_segmentCount++;
			CurvePoint endPoint = _curr.GetEndPoint();
			_next = CreateCurve(endPoint.position);
			AddPropsOnCurve(_next);
			AddPickupsOnCurve(_next);
			AddFriendsRoadSign(_next);
		}
		if (_next != null && xRatio > 0.999f)
		{
			_prev = _curr;
			_curr = _next;
			_next = null;
		}
	}

	private void AddPropsOnCurve(Curve c)
	{
		AutoSingleton<PropsManager>.Instance.SpawnProps(c);
		AutoSingleton<ActionPropsManager>.Instance.SpawnProps(c);
	}

	private void AddPickupsOnCurve(Curve c, bool intro = false)
	{
		AutoSingleton<PickupManager>.Instance.SpawnPickups(c, intro);
	}

	private void AddFriendsRoadSign(Curve c)
	{
		AutoSingleton<FriendsRoadSignManager>.Instance.SpawnRoadSign(c);
	}
}
