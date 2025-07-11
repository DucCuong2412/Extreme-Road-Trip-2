using System.Collections;
using UnityEngine;

public class TutorialControls : Tutorial
{
	public Transform _car;

	public Transform _leftTSControl;

	public Transform _rightTSControl;

	public Transform _leftKBControl;

	public Transform _rightKBControl;

	private tk2dSprite _leftSprite;

	private tk2dSprite _rightSprite;

	private float _tilt = 1f;

	protected override void Reset()
	{
	}

	protected override IEnumerator AnimCR()
	{
		yield return null;
	}

	protected override void FixedUpdate()
	{
		Vector3 eulerAngles = _car.localRotation.eulerAngles;
		float num = Mathf.DeltaAngle(0f, eulerAngles.z);
		Vector3 angularVelocity = _car.GetComponent<Rigidbody>().angularVelocity;
		float z = angularVelocity.z;
		float num2 = 1f;
		if (_tilt * z < 0f)
		{
			num2 = 1.3f;
		}
		_car.GetComponent<Rigidbody>().angularVelocity += new Vector3(0f, 0f, 7f * _tilt * num2 * Time.deltaTime);
		Vector3 angularVelocity2 = _car.GetComponent<Rigidbody>().angularVelocity;
		float z2 = angularVelocity2.z;
		float num3 = 2f;
		z2 = Mathf.Clamp(z2, 0f - num3, num3);
		_car.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, z2);
		if (num > 30f)
		{
			_tilt = -1f;
			_leftSprite.color = new Color(1f, 1f, 1f, 0.2f);
			_rightSprite.color = new Color(1f, 1f, 1f, 1f);
		}
		if (num < -30f)
		{
			_tilt = 1f;
			_leftSprite.color = new Color(1f, 1f, 1f, 1f);
			_rightSprite.color = new Color(1f, 1f, 1f, 0.2f);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_leftKBControl.gameObject.SetActive(value: false);
		_rightKBControl.gameObject.SetActive(value: false);
		_leftSprite = _leftTSControl.GetComponent<tk2dSprite>();
		_rightSprite = _rightTSControl.GetComponent<tk2dSprite>();
	}
}
