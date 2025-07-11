using UnityEngine;

internal class WebConsole
{
	public static bool isDebugBuild;

	public static void Log(string message, object o = null)
	{
		Application.ExternalCall("log", message);
	}

	public static void LogWarning(string message, object o = null)
	{
		Application.ExternalCall("log", message);
	}

	public static void LogError(string message, object o = null)
	{
		Application.ExternalCall("log", message);
	}

	public static void LogObject(object o = null)
	{
	}
}
