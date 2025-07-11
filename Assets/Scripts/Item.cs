using UnityEngine;

public class Item : MonoBehaviour
{
	protected GameObject _go;

	protected Transform _transform;

	public virtual bool IsCollidable => false;

	public virtual bool IsCollectible => false;

	public Vector3 Position => _transform.position;

	public virtual float GetRightMostPosition()
	{
		Vector3 max = RendererBounds.ComputeBounds(_transform).max;
		return max.x;
	}

	public virtual void Awake()
	{
		_go = base.gameObject;
		_transform = base.transform;
	}

	public virtual void Reset()
	{
	}

	public virtual void Activate()
	{
	}
}
