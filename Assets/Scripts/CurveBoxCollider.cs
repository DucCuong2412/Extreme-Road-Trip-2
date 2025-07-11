using System.Collections.Generic;
using UnityEngine;

public class CurveBoxCollider : MonoBehaviour
{
	private const float _colliderDepth = 5f;

	private static void GenerateBox(Curve c, Vector3 va, Vector3 vb)
	{
		GameObject gameObject = new GameObject("c");
		Vector3 toDirection = vb - va;
		gameObject.layer = 10;
		gameObject.transform.parent = c.transform;
		gameObject.tag = GameSettings.GroundColliderTag;
		gameObject.transform.localPosition = Vector3.Lerp(va, vb, 0.5f);
		gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.right, toDirection);
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(toDirection.magnitude, 5f, 10f);
		boxCollider.center = new Vector3(0f, -2.5f, 0f);
	}

	public static void Create(Curve curve)
	{
		List<CurvePoint> points = curve.GetPoints();
		IEnumerator<CurvePoint> enumerator = ((IEnumerable<CurvePoint>)points).GetEnumerator();
		enumerator.MoveNext();
		CurvePoint current = enumerator.Current;
		Vector3 vector = current.position;
		enumerator.MoveNext();
		CurvePoint current2 = enumerator.Current;
		Vector3 vector2 = current2.position;
		float num = 0f;
		float num2 = 0f;
		while (enumerator.MoveNext())
		{
			CurvePoint current3 = enumerator.Current;
			Vector3 position = current3.position;
			Vector3 normalized = (vector2 - vector).normalized;
			Vector3 normalized2 = (position - vector2).normalized;
			float num3 = Vector3.Dot(normalized, normalized2);
			num += 1f - num3;
			num2 += (position - vector2).magnitude;
			if (num > 0.002f || num2 > 5f)
			{
				GenerateBox(curve, vector, vector2);
				vector = vector2;
				vector2 = position;
				num = 0f;
				num2 = 0f;
			}
			else
			{
				vector2 = position;
			}
		}
		GenerateBox(curve, vector, vector2);
	}
}
