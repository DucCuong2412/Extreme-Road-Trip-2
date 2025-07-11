using UnityEngine;

public class MetroPivot : MetroWidget
{
	private Transform _pivot;

	public static MetroPivot Create()
	{
		return Create(Vector3.zero);
	}

	public static MetroPivot Create(Vector3 offset)
	{
		GameObject gameObject = new GameObject("MetroPivot");
		gameObject.transform.localPosition = Vector3.zero;
		Transform transform = new GameObject("Pivot").transform;
		transform.parent = gameObject.transform;
		transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroPivot>().Setup(transform, offset);
	}

	private MetroPivot Setup(Transform pivot, Vector3 offset)
	{
		_pivot = pivot;
		SetOffset(offset);
		return this;
	}

	public MetroPivot SetOffset(Vector3 offset)
	{
		_pivot.localPosition = offset;
		return this;
	}

	public MetroPivot SetScale(Vector3 scale)
	{
		_pivot.localScale = scale;
		return this;
	}

	public MetroPivot SetRotation(Quaternion rotation)
	{
		_pivot.localRotation = rotation;
		return this;
	}

	public override MetroWidget Add(MetroWidget child)
	{
		base.Add(child);
		child.transform.parent = _pivot;
		child.transform.localPosition = Vector3.zero;
		return child;
	}
}
