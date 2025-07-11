using UnityEngine;

public class RealtimeDuration
{
	private float _timeStart;

	private float _timeEnd;

	public RealtimeDuration(float delay)
	{
		_timeStart = Time.realtimeSinceStartup;
		_timeEnd = _timeStart + delay;
	}

	public float Value01()
	{
		return Mathf.InverseLerp(_timeStart, _timeEnd, Time.realtimeSinceStartup);
	}

	public bool IsDone()
	{
		return Time.realtimeSinceStartup > _timeEnd;
	}
}
