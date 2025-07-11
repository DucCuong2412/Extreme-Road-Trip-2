using System;
using UnityEngine;

public static class EnumUtil
{
	public static T Parse<T>(string s, T defaultValue) where T : struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			UnityEngine.Debug.LogWarning("Invalid Enum: " + s);
			return defaultValue;
		}
		T result = defaultValue;
		try
		{
			T val = (T)Enum.Parse(typeof(T), s);
			if (!Enum.IsDefined(typeof(T), val))
			{
				return result;
			}
			result = val;
			return result;
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogWarning("Invalid Enum: " + s);
			return result;
		}
	}
}
