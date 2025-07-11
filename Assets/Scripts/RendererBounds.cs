using UnityEngine;

public class RendererBounds : MonoBehaviour
{
	private Bounds _bounds;

	private Bounds Bounds => _bounds;

	public void Start()
	{
		Refresh();
	}

	public void Refresh()
	{
		_bounds = ComputeBounds(base.transform);
	}

	public static Bounds ComputeBounds(Transform t)
	{
		Bounds result = new Bounds(t.position, Vector3.zero);
		MeshRenderer[] componentsInChildren = t.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			result.Encapsulate(meshRenderer.bounds);
		}
		return result;
	}
}
