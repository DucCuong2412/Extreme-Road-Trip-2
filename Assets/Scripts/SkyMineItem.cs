using System.Collections;
using UnityEngine;

public class SkyMineItem : CollidableItem
{
	private RandomRange _startHeight = new RandomRange(30f, 150f);

	private RandomRange _swayAngle = new RandomRange(15f, 25f);

	private RandomRange _swayDuration = new RandomRange(0.5f, 1.5f);

	private RandomRange _riseOffset = new RandomRange(3f, 7f);

	private RandomRange _riseDuration = new RandomRange(1f, 3f);

	private bool _anim;

	private float _baseY;

	public override void Reset()
	{
		base.Reset();
		_anim = false;
	}

	public override void Activate()
	{
		base.Activate();
		_transform.position += Vector3.up * _startHeight.Pick();
		_transform.rotation = Quaternion.identity;
		Vector3 position = _transform.position;
		_baseY = position.y;
		_anim = true;
		StartCoroutine(FloatCR());
	}

	protected override IEnumerator CollideImpCR(CarController car)
	{
		AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordMineExplosion(1);
		PrefabSingleton<GameSoundManager>.Instance.PlayMineSound();
		if (!car.rigidbody.isKinematic)
		{
			Rigidbody rigidbody = car.rigidbody;
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocity2 = car.Velocity;
			rigidbody.velocity = velocity + new Vector3(0f, (!(velocity2.y >= 0f)) ? 50f : (-50f), 0f);
		}
		AutoSingleton<ExplosionManager>.Instance.Explode(base.Position);
		yield return new WaitForFixedUpdate();
		_transform.position = GameSettings.OutOfWorldVector;
		yield return null;
	}

	private IEnumerator FloatCR()
	{
		float fromAngle = 0f - _swayAngle.Pick();
		float toAngle = 0f - fromAngle;
		float fromY = 0f - _riseOffset.Pick();
		float toY = 0f - fromY;
		Duration riseDelay = new Duration(_riseDuration.Pick());
		Duration swayDelay = new Duration(_swayDuration.Pick());
		while (_anim)
		{
			float y = Mathfx.Lerp(fromY, toY, riseDelay.Value01());
			Transform transform = _transform;
			Vector3 position = _transform.position;
			float x = position.x;
			float y2 = _baseY + y;
			Vector3 position2 = _transform.position;
			transform.position = new Vector3(x, y2, position2.z);
			if (riseDelay.IsDone())
			{
				float tempY = fromY;
				fromY = toY;
				toY = tempY;
				riseDelay = new Duration(_riseDuration.Pick());
			}
			float angle = Mathf.LerpAngle(fromAngle, toAngle, swayDelay.Value01());
			_transform.eulerAngles = new Vector3(0f, 0f, angle);
			if (swayDelay.IsDone())
			{
				float tempAngle = fromAngle;
				fromAngle = toAngle;
				toAngle = tempAngle;
				swayDelay = new Duration(_swayDuration.Pick());
			}
			yield return null;
		}
	}
}
