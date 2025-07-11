using UnityEngine;

public class CrashManager : AutoSingleton<CrashManager>
{
	private bool _isDone;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		LoadAndSendLastReport();
		base.OnAwake();
	}

	public bool IsDone()
	{
		return _isDone;
	}

	private void LoadAndSendLastReport()
	{
		_isDone = true;
	}

	private void OnCrashReportSuccess()
	{
		UnityEngine.Debug.Log("Crash report sent sucessfully.");
		_isDone = true;
	}

	private void OnCrashReportFailed(string error)
	{
		_isDone = true;
		UnityEngine.Debug.LogWarning("Could not send crash report: " + error);
	}
}
