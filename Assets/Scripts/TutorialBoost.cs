using System.Collections;
using UnityEngine;

public class TutorialBoost : Tutorial
{
	public Transform _car;

	public Transform _jumpTarget;

	public Transform _boostTarget;

	public Transform _trailPivot;

	private Vector3 _from;

	private TrailRenderer _trailFX;

	protected override void Reset()
	{
		_car.localPosition = _from;
		_trailFX.enabled = true;
		_trailFX.time = 0f;
	}

	protected override IEnumerator AnimCR()
	{
		Duration delay3 = new Duration(1f);
		Vector3 target2 = _jumpTarget.localPosition;
		while (!delay3.IsDone() && _isAnimating)
		{
			float x2 = Mathf.Lerp(_from.x, target2.x, delay3.Value01());
			_car.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 360f, delay3.Value01()));
			float g = -33f;
			float v0y = 8f;
			float t = delay3.Value01();
			float y2 = _from.y + v0y * t + 0.5f * g * t * t;
			_car.transform.localPosition = new Vector3(x2, y2, 0f);
			yield return null;
		}
		delay3 = new Duration(0.4f);
		_trailFX.time = 1f;
		Vector3 from2 = _car.transform.localPosition;
		target2 = _boostTarget.localPosition;
		while (!delay3.IsDone() && _isAnimating)
		{
			float x = Mathf.Lerp(from2.x, target2.x, delay3.Value01());
			float y = from2.y;
			_car.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 20f, delay3.Value01()));
			_car.transform.localPosition = new Vector3(x, y, 0f);
			yield return null;
		}
		delay3 = new Duration(0.4f);
		while (!delay3.IsDone() && _isAnimating)
		{
			_trailFX.time = 0.8f - delay3.Value01();
			yield return null;
		}
		_trailFX.enabled = false;
		_finished = true;
	}

	protected override void Awake()
	{
		base.Awake();
		_from = _car.localPosition;
		Transform transform = (Transform)Object.Instantiate(PrefabSingleton<GameSpecialFXManager>.Instance._boostTrailPrefab, Vector3.zero, Quaternion.identity);
		transform.gameObject.layer = 8;
		transform.parent = _trailPivot;
		transform.localPosition = Vector3.zero;
		_trailFX = transform.GetComponent<TrailRenderer>();
	}
}
