using System;
using UnityEngine;

public static class ColorUtil
{
	public static Color Parse(string c)
	{
		if (c.Length != 7)
		{
			UnityEngine.Debug.LogError("Expected color string of format \"#aabbcc\"");
			return Color.black;
		}
		float r = (float)(double)Convert.ToUInt32(c.Substring(1, 2), 16) / 256f;
		float g = (float)(double)Convert.ToUInt32(c.Substring(3, 2), 16) / 256f;
		float b = (float)(double)Convert.ToUInt32(c.Substring(5, 2), 16) / 256f;
		return new Color(r, g, b);
	}
}
