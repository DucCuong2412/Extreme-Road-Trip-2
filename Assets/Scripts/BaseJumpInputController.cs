using UnityEngine;

public class BaseJumpInputController : MonoBehaviour
{
	protected float _tilt;

	private bool _slam;

	public float Tilt => _tilt;

	public bool Slam => _slam;

	private bool TiltLeft()
	{
		FakeTouch[] touches = AutoSingleton<InputManager>.Instance.GetTouches();
		for (int i = 0; i < touches.Length; i++)
		{
			FakeTouch fakeTouch = touches[i];
			if (fakeTouch.phase != TouchPhase.Canceled && fakeTouch.phase != TouchPhase.Ended)
			{
				Vector2 position = fakeTouch.position;
				if (position.x < (float)Screen.width * 0.5f)
				{
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKey(KeyCode.LeftArrow) || UnityEngine.Input.GetKey(KeyCode.LeftShift);
	}

	private bool TiltRight()
	{
		FakeTouch[] touches = AutoSingleton<InputManager>.Instance.GetTouches();
		for (int i = 0; i < touches.Length; i++)
		{
			FakeTouch fakeTouch = touches[i];
			if (fakeTouch.phase != TouchPhase.Canceled && fakeTouch.phase != TouchPhase.Ended)
			{
				Vector2 position = fakeTouch.position;
				if (position.x > (float)Screen.width * 0.5f)
				{
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKey(KeyCode.RightArrow) || UnityEngine.Input.GetKey(KeyCode.RightShift);
	}

	private void Update()
	{
		_slam = (TiltLeft() && TiltRight());
		_tilt = 0f;
		if (TiltLeft())
		{
			_tilt += 1f;
		}
		if (TiltRight())
		{
			_tilt -= 1f;
		}
	}
}
