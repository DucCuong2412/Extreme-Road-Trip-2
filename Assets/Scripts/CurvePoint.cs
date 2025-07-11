using UnityEngine;

public struct CurvePoint
{
	public Vector3 position;

	public Vector3 normal;

	public CurvePoint(Vector3 position, Vector3 normal)
	{
		this.position = position;
		this.normal = normal;
	}
}
