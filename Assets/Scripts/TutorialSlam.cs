using System.Collections;
using UnityEngine;

public class TutorialSlam : Tutorial
{
	public Transform _car;

	public Transform _jumpTarget;

	public Transform _slamTarget;

	public Transform _boostTarget;

	public Transform _leftTSControl;

	public Transform _rightTSControl;

	public Transform _leftTSSlamControl;

	public Transform _rightTSSlamControl;

	public Transform _leftKBControl;

	public Transform _rightKBControl;

	private tk2dSprite _leftSprite;

	private tk2dSprite _rightSprite;

	private Vector3 _from;

	private ParticleSystem _slammingFX;

	private ParticleSystem _megaBoostFX;

	protected override void Reset()
	{
		_car.localPosition = _from;
		_leftTSControl.gameObject.SetActive(value: true);
		_rightTSControl.gameObject.SetActive(value: true);
		_leftTSSlamControl.gameObject.SetActive(value: false);
		_rightTSSlamControl.gameObject.SetActive(value: false);
	}

	protected override IEnumerator AnimCR()
	{
		Duration delay3 = new Duration(1f);
		Vector3 target3 = _jumpTarget.localPosition;
		while (!delay3.IsDone() && _isAnimating)
		{
			float x3 = Mathf.Lerp(_from.x, target3.x, delay3.Value01());
			_car.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-20f, 5f, delay3.Value01()));
			float g = -33f;
			float v0y = 18f;
			float t = delay3.Value01();
			float y3 = _from.y + v0y * t + 0.5f * g * t * t;
			_car.transform.localPosition = new Vector3(x3, y3, 0f);
			if (t > 0.8f)
			{
				_leftTSControl.gameObject.SetActive(value: false);
				_rightTSControl.gameObject.SetActive(value: false);
				_leftTSSlamControl.gameObject.SetActive(value: true);
				_rightTSSlamControl.gameObject.SetActive(value: true);
			}
			yield return null;
		}
		delay3 = new Duration(0.25f);
		Vector3 from4 = _car.transform.localPosition;
		target3 = _slamTarget.localPosition;
		_slammingFX.Play();
		_slammingFX.transform.rotation = Quaternion.identity;
		while (!delay3.IsDone() && _isAnimating)
		{
			float x2 = Mathf.Lerp(from4.x, target3.x, delay3.Value01());
			float y2 = Mathf.Lerp(from4.y, target3.y, delay3.Value01());
			_car.transform.localPosition = new Vector3(x2, y2, 0f);
			yield return null;
		}
		_slammingFX.Stop();
		_slammingFX.Clear();
		_megaBoostFX.Play();
		delay3 = new Duration(0.4f);
		Vector3 from3 = _car.transform.localPosition;
		target3 = _boostTarget.localPosition;
		while (!delay3.IsDone() && _isAnimating)
		{
			float x = Mathf.Lerp(from3.x, target3.x, delay3.Value01());
			float y = from3.y;
			_car.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 5f, delay3.Value01()));
			_car.transform.localPosition = new Vector3(x, y, 0f);
			yield return null;
		}
		_megaBoostFX.Stop();
		_megaBoostFX.Clear();
		_finished = true;
	}

	protected override void Awake()
	{
		base.Awake();
		_from = _car.localPosition;
		_leftKBControl.gameObject.SetActive(value: false);
		_rightKBControl.gameObject.SetActive(value: false);
		_leftTSControl.GetComponent<tk2dSprite>().color = new Color(1f, 1f, 1f, 0.2f);
		_rightTSControl.GetComponent<tk2dSprite>().color = new Color(1f, 1f, 1f, 0.2f);
		_slammingFX = PrefabSingleton<GameSpecialFXManager>.Instance.AddCarSlamLinesFX(_car);
		_slammingFX.transform.localPosition += Vector3.up * 2f;
		_slammingFX.gameObject.layer = 8;
		_megaBoostFX = PrefabSingleton<GameSpecialFXManager>.Instance.AddCarMegaBoostFX(_car);
		_megaBoostFX.gameObject.layer = 8;
	}
}
