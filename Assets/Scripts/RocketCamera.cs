using UnityEngine;

public class RocketCamera : Singleton<RocketCamera>
{
	private Transform _target;

	private Vector3 _offset;

	private float _startingY;

	protected override void OnStart()
	{
		Vector3 position = base.transform.position;
		_startingY = position.y;
		base.OnStart();
	}

	public void SetTarget(Transform target)
	{
		_target = target;
	}

	public void LateUpdate()
	{
		Vector3 b = new Vector3(0f, 0f, -20f);
		Vector3 vector = _target.position + b;
		_offset = Vector3.Lerp(_offset, vector, 5f * Time.deltaTime);
		Vector3 position = vector + (_offset - vector);
		position.y = Mathf.Max(position.y, _startingY);
		base.transform.position = position;
		Color color = new Color(0f, 0f, 1f);
		Color a = new Color(0.2f, 0.4f, 1f);
		Color color2 = new Color(0.05f, 0.1f, 0.25f);
		Color color3 = new Color(0.4f, 0.5f, 0.9f);
		Color color4 = new Color(0f, 0f, 0f);
		float num = 100f;
		float num2 = 110f;
		float num3 = 120f;
		float y = position.y;
		color = ((y < num) ? Color.Lerp(a, color2, Mathf.InverseLerp(0f, num, y)) : ((y < num2) ? Color.Lerp(color2, color3, Mathf.InverseLerp(num, num2, y)) : ((!(y < num3)) ? color4 : Color.Lerp(color3, color4, Mathf.InverseLerp(num2, num3, y)))));
		base.GetComponent<Camera>().backgroundColor = color;
	}
}
