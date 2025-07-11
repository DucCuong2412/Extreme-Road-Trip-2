using System.Collections.Generic;
using UnityEngine;

public class CurveRenderer : MonoBehaviour
{
	protected MeshFilter _meshFilter;

	protected MeshRenderer _meshRenderer;

	private Material _material;

	private bool _isGround;

	private float _uvSpread;

	private float _width;

	private Curve _curve;

	public static void Create(Curve curve, bool isGround, Material material, float uvSpread, float width)
	{
		GameObject gameObject = new GameObject((!isGround) ? "Surface" : "Ground");
		gameObject.transform.parent = curve.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, (!isGround) ? 1.99f : 2f);
		gameObject.AddComponent<CurveRenderer>().Setup(curve, isGround, material, uvSpread, width);
	}

	private void OnDestroy()
	{
		if (_meshFilter.mesh != null)
		{
			UnityEngine.Object.Destroy(_meshFilter.mesh);
			_meshFilter.mesh = null;
		}
	}

	private void Setup(Curve curve, bool isGround, Material material, float uvSpread, float width)
	{
		_curve = curve;
		_isGround = isGround;
		_material = material;
		_uvSpread = uvSpread;
		_width = width;
		InitMesh();
		if (_isGround)
		{
			base.gameObject.layer = 10;
		}
	}

	private void InitMesh()
	{
		_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		_meshRenderer.sharedMaterial = _material;
		_meshFilter = base.gameObject.AddComponent<MeshFilter>();
		_meshFilter.mesh = new Mesh();
		_meshFilter.mesh.Clear();
		List<CurvePoint> points = _curve.GetPoints();
		int count = points.Count;
		if (count == 0)
		{
			return;
		}
		Vector3[] array = new Vector3[count * 2];
		Color[] array2 = new Color[count * 2];
		Vector3[] array3 = new Vector3[count * 2];
		Vector2[] array4 = new Vector2[count * 2];
		int num = 0;
		int num2 = 0;
		Vector3 b = Vector3.up;
		for (int i = 0; i < count; i++)
		{
			CurvePoint curvePoint = points[i];
			Vector3 position = curvePoint.position;
			Vector3 vector = position;
			Vector3 vector2 = position;
			if (_isGround)
			{
				vector2.y -= _width;
			}
			else if (i == 0 || i == count - 1)
			{
				vector.y += _width * 0.1f;
				vector2.y -= _width * 0.9f;
			}
			else
			{
				Vector3 vector3 = position - b;
				Vector3 normalized = new Vector3(0f - vector3.y, vector3.x, 0f).normalized;
				vector += normalized * _width * 0.1f;
				vector2 -= normalized * _width * 0.9f;
			}
			float y = 1f;
			float y2 = 0f;
			if (_isGround)
			{
				y = (vector.y + _width) * _uvSpread;
				y2 = vector2.y * _uvSpread;
			}
			array[num] = vector;
			array2[num] = Color.white;
			array3[num] = Vector3.back;
			array4[num] = new Vector2(vector.x * _uvSpread, y);
			array[num + 1] = vector2;
			array2[num + 1] = ((!_isGround) ? Color.white : Color.gray);
			array3[num + 1] = Vector3.back;
			array4[num + 1] = new Vector2(vector2.x * _uvSpread, y2);
			num += 2;
			num2++;
			b = position;
		}
		_meshFilter.mesh.vertices = array;
		_meshFilter.mesh.colors = array2;
		_meshFilter.mesh.normals = array3;
		_meshFilter.mesh.uv = array4;
		int[] array5 = new int[6 * (count - 1)];
		for (int j = 0; j < count - 1; j++)
		{
			int num3 = j * 6;
			int num4 = j * 2;
			array5[num3] = num4 + 2;
			array5[num3 + 1] = num4 + 1;
			array5[num3 + 2] = num4;
			array5[num3 + 3] = num4 + 1;
			array5[num3 + 4] = num4 + 2;
			array5[num3 + 5] = num4 + 3;
		}
		_meshFilter.mesh.triangles = array5;
	}
}
