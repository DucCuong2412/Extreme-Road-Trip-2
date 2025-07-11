using System.Diagnostics;
using UnityEngine;

public class DebugUtil
{
	[Conditional("UNITY_EDITOR")]
	public static void Assert(bool condition, string description)
	{
		if (!condition)
		{
			UnityEngine.Debug.LogError(description);
		}
	}

	[Conditional("UNITY_EDITOR")]
	public static void Assert(bool condition)
	{
	}
}
