using System.Diagnostics;
using UnityEngine;

public static class SilentDebug
{
	public static bool isDebugBuild;

	[Conditional("FALSE")]
	public static void Log(string message, object o = null)
	{
	}

	public static void LogWarning(string message, Object o = null)
	{
		UnityEngine.Debug.LogWarning(message, o);
	}

	public static void LogError(string message, Object o = null)
	{
		UnityEngine.Debug.LogError(message, o);
	}
}
