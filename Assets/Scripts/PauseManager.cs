using UnityEngine;

public class PauseManager : AutoSingleton<PauseManager>
{
	private bool _paused;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		Time.timeScale = 1f;
	}

	public bool IsPaused()
	{
		return _paused;
	}

	public void Pause()
	{
		_paused = true;
		Time.timeScale = 0f;
	}

	public void Resume()
	{
		_paused = false;
		Time.timeScale = 1f;
	}
}
