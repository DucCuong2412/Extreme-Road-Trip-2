using System.Collections.Generic;
using UnityEngine;

public class CurveMeshCollider : MonoBehaviour
{
	protected MeshCollider _meshCollider;

	private Curve _curve;

	private int _curveCount;

	private Mesh _mesh;

	private void InitMesh()
	{
		_meshCollider = base.gameObject.AddComponent<MeshCollider>();
		_mesh = new Mesh();
		_mesh.Clear();
	}

	public static void Create(Curve curve)
	{
		GameObject gameObject = new GameObject("Collision Mesh");
		gameObject.tag = GameSettings.GroundColliderTag;
		gameObject.transform.parent = curve.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.AddComponent<CurveMeshCollider>().Setup(curve);
	}

	private void OnDestroy()
	{
		if (_mesh != null)
		{
			UnityEngine.Object.Destroy(_mesh);
			_mesh = null;
		}
	}

	private void Setup(Curve curve)
	{
		_curve = curve;
		InitMesh();
	}

	private void Update()
	{
		List<CurvePoint> points = _curve.GetPoints();
		int count = points.Count;
		if (_curveCount != count && count != 0)
		{
			_curveCount = count;
			Vector3[] array = new Vector3[count * 2];
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				CurvePoint curvePoint = points[i];
				Vector3 position = curvePoint.position;
				Vector3 vector = position;
				Vector3 vector2 = position;
				vector.z = 5f;
				vector2.z = -5f;
				array[num] = vector;
				array[num + 1] = vector2;
				num += 2;
			}
			_mesh.vertices = array;
			int[] array2 = new int[6 * (count - 1)];
			for (int j = 0; j < count - 1; j++)
			{
				int num2 = j * 6;
				int num3 = j * 2;
				array2[num2] = num3 + 2;
				array2[num2 + 1] = num3 + 1;
				array2[num2 + 2] = num3;
				array2[num2 + 3] = num3 + 1;
				array2[num2 + 4] = num3 + 2;
				array2[num2 + 5] = num3 + 3;
			}
			_mesh.triangles = array2;
			_meshCollider.sharedMesh = _mesh;
		}
	}
}
