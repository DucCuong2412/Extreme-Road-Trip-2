using System.Collections;
using UnityEngine;

public class BalloonItem : PackageItem
{
	public RandomRange _startHeight = new RandomRange(15f, 30f);

	public float _swayAngle = 10f;

	public float _swayDuration = 1f;

	public float _riseOffset = 3f;

	public float _riseDuration = 2f;

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
		StartCoroutine(BalloonCR());
	}

	private IEnumerator BalloonCR()
	{
		float fromAngle = 0f - _swayAngle;
		float toAngle = _swayAngle;
		float fromY = 0f - _riseOffset;
		float toY = _riseOffset;
		Duration riseDelay = new Duration(_riseDuration);
		Duration swayDelay = new Duration(_swayDuration);
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
				riseDelay = new Duration(_riseDuration);
			}
			float angle = Mathf.LerpAngle(fromAngle, toAngle, swayDelay.Value01());
			_transform.eulerAngles = new Vector3(0f, 0f, angle);
			if (swayDelay.IsDone())
			{
				float tempAngle = fromAngle;
				fromAngle = toAngle;
				toAngle = tempAngle;
				swayDelay = new Duration(_swayDuration);
			}
			yield return null;
		}
	}

	protected override void OnBreak(CarController car)
	{
		base.OnBreak(car);
		if (Singleton<GameManager>.IsCreated() && Singleton<GameManager>.Instance.Car != null && Singleton<GameManager>.Instance.Car.Input.Slam)
		{
			AutoSingleton<AchievementsManager>.Instance.CompleteAchievement(AchievementType.hotAirBalloon);
		}
	}
}
