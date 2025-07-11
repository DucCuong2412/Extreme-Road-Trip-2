using UnityEngine;

internal class SlowdownManager : Singleton<SlowdownManager>
{
	private bool _mustSlowdown;

	private float _scheduledResume;

	public void Slowdown(float factor)
	{
		Time.timeScale = factor;
	}

	public void Slowdown(float duration, float factor)
	{
		_mustSlowdown = true;
		_scheduledResume = Time.realtimeSinceStartup + duration;
		Time.timeScale = factor;
	}

	public void Resume()
	{
		_mustSlowdown = false;
		Time.timeScale = 1f;
	}

	public void Update()
	{
		if (_mustSlowdown && !AutoSingleton<PauseManager>.Instance.IsPaused() && Time.realtimeSinceStartup > _scheduledResume)
		{
			Time.timeScale = 1f;
			_mustSlowdown = false;
		}
	}
}
