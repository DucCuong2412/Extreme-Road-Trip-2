using UnityEngine;

public class PlayerInputController : CarInputController
{
	private bool _tiltLeft;

	private bool _tiltRight;

	private bool _invertKey;

	private void Awake()
	{
		_invertKey = (AutoSingleton<PersistenceManager>.Instance.UseInvertedControl && AutoSingleton<PlatformCapabilities>.Instance.AreInvertedControlsSupported());
	}

	private void CheckTilt()
	{
		_tiltLeft = false;
		_tiltRight = false;
		FakeTouch[] touches = AutoSingleton<InputManager>.Instance.GetTouches();
		for (int i = 0; i < touches.Length; i++)
		{
			FakeTouch fakeTouch = touches[i];
			if (fakeTouch.phase != TouchPhase.Canceled && fakeTouch.phase != TouchPhase.Ended)
			{
				Vector2 position = fakeTouch.position;
				if (position.x < (float)Screen.width * 0.5f)
				{
					_tiltLeft = true;
				}
				else
				{
					_tiltRight = true;
				}
			}
		}
		_tiltLeft |= (UnityEngine.Input.GetKey(KeyCode.LeftArrow) || UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.Space));
		_tiltRight |= (UnityEngine.Input.GetKey(KeyCode.RightArrow) || UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightShift) || UnityEngine.Input.GetKey(KeyCode.Space));
		if (_invertKey)
		{
			bool tiltLeft = _tiltLeft;
			_tiltLeft = _tiltRight;
			_tiltRight = tiltLeft;
		}
		base.HasTiltedRight |= _tiltRight;
	}

	private void Update()
	{
		if (base.InputEnabled)
		{
			CheckTilt();
			_slam = (_tiltLeft && _tiltRight);
			_tilt = 0f;
			if (_tiltLeft)
			{
				_tilt += 1f;
			}
			if (_tiltRight)
			{
				_tilt -= 1f;
			}
		}
	}
}
