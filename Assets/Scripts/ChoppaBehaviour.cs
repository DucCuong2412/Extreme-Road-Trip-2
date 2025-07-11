using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChoppaBehaviour : MonoBehaviour
{
	private enum AnimState
	{
		takeoff,
		cruiseToDest,
		approach,
		cruiseAway,
		done
	}

	private const float _maxSpeed = 700f;

	private AnimState _currrentState;

	private Transform _transform;

	private float _currentSpeed;

	private float _distanceBonus;

	public bool IsDone => _currrentState == AnimState.done;

	public float CurrentSpeed => _currentSpeed;

	[method: MethodImpl(32)]
	public event Action OnDestinationReached;

	[method: MethodImpl(32)]
	public event Action OnAnimationDone;

	private void Start()
	{
		_transform = base.transform;
		_currentSpeed = 100f;
		_currrentState = AnimState.takeoff;
		Transform transform = base.transform;
		Vector3 position = base.transform.position;
		transform.position = new Vector3(25f, 25f, position.z);
		_distanceBonus = AutoSingleton<EpicPowerupManager>.Instance.GetGameSetupDistance();
	}

	private void UpdateAnim()
	{
		float min = 10f;
		float num = 80f;
		float num2 = 10f;
		float num3 = 0f;
		if (_currrentState == AnimState.takeoff)
		{
			min = 10f;
			num = 100f;
			num2 = 30f;
			Vector3 position = base.transform.position;
			if (position.x >= num)
			{
				_currrentState = AnimState.cruiseToDest;
			}
		}
		if (_currrentState == AnimState.cruiseToDest)
		{
			min = 100f;
			num = _distanceBonus - 300f;
			num2 = 30f;
			num3 = 500f;
			Vector3 position2 = base.transform.position;
			if (position2.x >= num)
			{
				_currrentState = AnimState.approach;
			}
		}
		if (_currrentState == AnimState.approach)
		{
			min = 100f;
			num = _distanceBonus;
			num2 = 30f;
			num3 = -800f;
			Vector3 position3 = base.transform.position;
			if (position3.x >= num)
			{
				_currrentState = AnimState.cruiseAway;
				if (this.OnDestinationReached != null)
				{
					this.OnDestinationReached();
				}
			}
		}
		if (_currrentState == AnimState.cruiseAway)
		{
			min = 100f;
			num = _distanceBonus + 500f;
			num2 = 50f;
			num3 = 50f;
			Vector3 position4 = base.transform.position;
			if (position4.x >= num)
			{
				_currrentState = AnimState.done;
				if (this.OnAnimationDone != null)
				{
					this.OnAnimationDone();
				}
			}
		}
		if (_currrentState != AnimState.done)
		{
			Vector3 position5 = _transform.position;
			float x = position5.x;
			Vector3 position6 = _transform.position;
			float y = position6.y;
			_currentSpeed += num3 * Time.deltaTime;
			_currentSpeed = Mathf.Clamp(_currentSpeed, min, 700f);
			float num4 = _currentSpeed * Time.deltaTime;
			float num5 = x + num4;
			float groundMaxSlope = AutoSingleton<GroundManager>.Instance.GetGroundMaxSlope(num5);
			float to = groundMaxSlope + num2;
			float num6 = Mathf.Lerp(y, to, 4f * Time.deltaTime);
			Transform transform = base.transform;
			float x2 = num5;
			float y2 = num6;
			Vector3 position7 = base.transform.position;
			transform.position = new Vector3(x2, y2, position7.z);
		}
	}

	private void FixedUpdate()
	{
		if (_transform != null && !IsDone)
		{
			UpdateAnim();
		}
	}
}
