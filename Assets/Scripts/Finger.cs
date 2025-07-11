using UnityEngine;

public class Finger
{
	private float _timeStart;

	private float _travel;

	public FakeTouch Touch
	{
		get;
		private set;
	}

	public float Duration => Time.time - _timeStart;

	public float Travel => _travel;

	public float Speed => _travel / Mathf.Max(0.001f, Duration);

	public Vector2 Position
	{
		get
		{
			FakeTouch touch = Touch;
			return touch.position;
		}
	}

	public Finger(FakeTouch touch)
	{
		Touch = touch;
		_timeStart = Time.time;
		_travel = 0f;
	}

	public bool IsFinished()
	{
		FakeTouch touch = Touch;
		int result;
		if (touch.phase != TouchPhase.Ended)
		{
			FakeTouch touch2 = Touch;
			result = ((touch2.phase == TouchPhase.Canceled) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void UpdateFinger(FakeTouch touch)
	{
		if (touch.deltaTime > 0f)
		{
			touch.deltaPosition *= Time.deltaTime / touch.deltaTime;
		}
		Touch = touch;
		FakeTouch touch2 = Touch;
		float magnitude = touch2.deltaPosition.magnitude;
		float num = magnitude / (float)Screen.height * 16f;
		_travel += num;
	}
}
