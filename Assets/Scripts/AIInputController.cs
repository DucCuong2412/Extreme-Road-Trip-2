using UnityEngine;

public class AIInputController : CarInputController
{
	private enum State
	{
		stabilizing,
		evaluatingStunt,
		doingStunt
	}

	private const float _stuntStartDistance1 = 15f;

	private const float _stuntStartSpeed1 = 0f;

	private const float _stuntStartDistance2 = 10f;

	private const float _stuntStartSpeed2 = 5f;

	private const float _stabilizingSafeDeltaAngle = 20f;

	private const float _stabilizingSafeAngSpeed = 0.5f;

	private const float _stabilizingRecklessAngSpeed = 0.6f;

	private const float _stuntEndDistance = 10f;

	private const float _stuntEndSpeed = -1f;

	private const float _slamSafeDeltaAngle = 60f;

	private State _state;

	private Transform _transform;

	private Rigidbody _rigidbody;

	private float _slamStopDistance = 0.5f;

	private void Awake()
	{
		_state = State.stabilizing;
		_transform = base.transform;
		_rigidbody = base.GetComponent<Rigidbody>();
		Vector3 extents = GetComponent<CarController>().GetVisualBounds().extents;
		_slamStopDistance = extents.y;
	}

	private void FixedUpdate()
	{
		if (!base.InputEnabled)
		{
			return;
		}
		Vector3 position = _transform.position;
		float y = position.y;
		GroundManager instance = AutoSingleton<GroundManager>.Instance;
		Vector3 position2 = _transform.position;
		float num = y - instance.GetGroundHeight(position2.x);
		Vector3 velocity = _rigidbody.velocity;
		float y2 = velocity.y;
		Vector3 angularVelocity = _rigidbody.angularVelocity;
		float z = angularVelocity.z;
		Vector3 eulerAngles = _transform.eulerAngles;
		float f = Mathf.DeltaAngle(0f, eulerAngles.z);
		switch (_state)
		{
		case State.stabilizing:
		{
			if (num <= _slamStopDistance)
			{
				_slam = false;
			}
			if ((num >= 15f && y2 > 0f) || (num >= 10f && y2 > 5f))
			{
				_state = State.evaluatingStunt;
				break;
			}
			if (Mathf.Abs(f) < 20f)
			{
				_tilt = ((!(Mathf.Abs(z) < 0.5f)) ? Mathf.Sign(0f - z) : 0f);
				break;
			}
			float num2 = Mathf.Sign(f);
			if (Mathf.Abs(z) >= 0.6f && Mathf.Sign(_tilt) == num2)
			{
				_tilt += Mathf.Sign(z);
			}
			else
			{
				_tilt = 0f - num2;
			}
			break;
		}
		case State.evaluatingStunt:
		{
			bool flag = Random.value >= 0.5f;
			_tilt += ((!flag) ? (-1f) : 1f);
			_state = State.doingStunt;
			break;
		}
		case State.doingStunt:
			if (num <= 10f || y2 < -1f)
			{
				_slam = (Mathf.Abs(f) < 60f);
				_state = State.stabilizing;
			}
			else if (!_slam)
			{
				_tilt = Mathf.Sign(_tilt);
			}
			break;
		}
	}
}
