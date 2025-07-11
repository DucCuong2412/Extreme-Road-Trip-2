using UnityEngine;

public class BaseJumpCamera : Singleton<BaseJumpCamera>
{
	private Transform _target;

	private Vector3 _offset;

	public void SetTarget(Transform target)
	{
		_target = target;
	}

	public void LateUpdate()
	{
		Vector3 b = new Vector3(0f, 0f, -20f);
		Vector3 vector = _target.position + b;
		_offset = Vector3.Lerp(_offset, vector, 5f * Time.deltaTime);
		base.transform.position = vector - (_offset - vector);
	}
}
