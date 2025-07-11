using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimator : MonoBehaviour
{
	private Transform _transform;

	private Transform _backWheel;

	private Transform _frontWheel;

	private List<Transform> _extraWheelList;

	public void Awake()
	{
		_extraWheelList = new List<Transform>();
		_transform = base.transform.Find("Pivot");
		_backWheel = _transform.Find("WheelBack");
		_frontWheel = _transform.Find("WheelFront");
		foreach (Transform item in _transform)
		{
			if (item.name == "WheelExtra")
			{
				_extraWheelList.Add(item);
			}
		}
		StartCoroutine(AnimCR());
	}

	private IEnumerator AnimCR()
	{
		Vector3 localPosition = _transform.localPosition;
		float from = localPosition.y;
		while (true)
		{
			float to = from + UnityEngine.Random.Range(-0.1f, 0.1f);
			Duration delay = new Duration(0.4f);
			while (!delay.IsDone())
			{
				float x = Mathfx.Bounce(delay.Value01());
				Transform transform = _transform;
				Vector3 localPosition2 = _transform.localPosition;
				float x2 = localPosition2.x;
				float y = Mathf.Lerp(from, to, x);
				Vector3 localPosition3 = _transform.localPosition;
				transform.localPosition = new Vector3(x2, y, localPosition3.z);
				yield return null;
			}
		}
	}

	public void Update()
	{
		float angle = Time.fixedDeltaTime * -500f;
		_backWheel.Rotate(Vector3.forward, angle);
		_frontWheel.Rotate(Vector3.forward, angle);
		foreach (Transform extraWheel in _extraWheelList)
		{
			extraWheel.Rotate(Vector3.forward, angle);
		}
	}
}
