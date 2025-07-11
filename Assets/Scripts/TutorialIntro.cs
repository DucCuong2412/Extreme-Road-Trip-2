using System.Collections;
using UnityEngine;

public class TutorialIntro : Tutorial
{
	public Transform _car;

	public Transform _backWheel;

	public Transform _frontWheel;

	public Transform _dustPivot;

	private float _dustTime;

	private ParticleEmitter _dustFX;

	protected override void Reset()
	{
	}

	protected override IEnumerator AnimCR()
	{
		Vector3 localPosition = _car.localPosition;
		float from = localPosition.y;
		float to = from + Random.Range(-0.2f, 0.2f);
		Duration delay = new Duration(0.4f);
		while (!delay.IsDone() && _isAnimating)
		{
			float x = Mathfx.Bounce(delay.Value01());
			Transform car = _car;
			Vector3 localPosition2 = _car.localPosition;
			float x2 = localPosition2.x;
			float y = Mathf.Lerp(from, to, x);
			Vector3 localPosition3 = _car.localPosition;
			car.localPosition = new Vector3(x2, y, localPosition3.z);
			yield return null;
		}
		_finished = true;
	}

	protected override void Awake()
	{
		base.Awake();
		Transform transform = (Transform)Object.Instantiate(PrefabSingleton<GameSpecialFXManager>.Instance._dustFXPrefab, Vector3.zero, Quaternion.identity);
		transform.gameObject.layer = 8;
		transform.parent = _dustPivot;
		transform.localPosition = Vector3.zero;
		_dustFX = transform.particleEmitter;
		_dustFX.worldVelocity = new Vector3(-25f, 1f, 0f);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		_dustTime += Time.fixedDeltaTime;
		if (_dustTime >= 0.1f)
		{
			_dustTime -= 0.1f;
			_dustFX.Emit();
		}
		float angle = Time.fixedDeltaTime * -500f;
		_backWheel.Rotate(Vector3.forward, angle);
		_frontWheel.Rotate(Vector3.forward, angle);
	}
}
