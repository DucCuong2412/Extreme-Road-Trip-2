using UnityEngine;

public struct FakeTouch
{
	public int fingerId;

	public Vector2 position;

	public Vector2 deltaPosition;

	public float deltaTime;

	public int tapCount;

	public TouchPhase phase;

	public FakeTouch(Touch t)
	{
		fingerId = t.fingerId;
		position = t.position;
		deltaPosition = t.deltaPosition;
		deltaTime = t.deltaTime;
		tapCount = t.tapCount;
		phase = t.phase;
	}
}
