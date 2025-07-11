using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUtil
{
	private static float ToFloat(object o, float def)
	{
		if (o is long)
		{
			return (long)o;
		}
		if (o is int)
		{
			return (int)o;
		}
		if (o is double)
		{
			return (float)(double)o;
		}
		if (o is float)
		{
			return (float)o;
		}
		UnityEngine.Debug.LogWarning("Unable to convert value to float: " + ((o != null) ? o.ToString() : "null"));
		return def;
	}

	private static long ToLong(object o, long def)
	{
		if (o is long)
		{
			return (long)o;
		}
		if (o is int)
		{
			return (int)o;
		}
		if (o is double)
		{
			return (long)(double)o;
		}
		if (o is float)
		{
			return (long)(float)o;
		}
		UnityEngine.Debug.LogWarning("Unable to convert value to long: " + ((o != null) ? o.ToString() : "null"));
		return def;
	}

	public static int ToInt(object o, int def)
	{
		return Mathf.RoundToInt(ToFloat(o, def));
	}

	public static ArrayList ExtractArrayList(Hashtable data, string key, ArrayList def = null)
	{
		if (data != null && data.ContainsKey(key))
		{
			return (data[key] as ArrayList) ?? def;
		}
		return def;
	}

	public static List<object> ExtractList(Hashtable data, string key, List<object> def = null)
	{
		if (data != null && data.ContainsKey(key))
		{
			return (data[key] as List<object>) ?? def;
		}
		return def;
	}

	public static Hashtable ExtractHashtable(Hashtable data, string key, Hashtable def = null)
	{
		if (data != null && data.ContainsKey(key))
		{
			return (data[key] as Hashtable) ?? def;
		}
		return def;
	}

	public static Dictionary<string, object> ExtractDictionary(Hashtable data, string key, Dictionary<string, object> def = null)
	{
		if (data != null && data.ContainsKey(key))
		{
			return (data[key] as Dictionary<string, object>) ?? def;
		}
		return def;
	}

	public static float ExtractFloat(Hashtable data, string key, float def)
	{
		if (data != null && data.ContainsKey(key))
		{
			return ToFloat(data[key], def);
		}
		return def;
	}

	public static long ExtractLong(Hashtable data, string key, long def)
	{
		if (data != null && data.ContainsKey(key))
		{
			return ToLong(data[key], def);
		}
		return def;
	}

	public static int ExtractInt(Hashtable data, string key, int def)
	{
		if (data != null && data.ContainsKey(key))
		{
			return ToInt(data[key], def);
		}
		return def;
	}

	public static bool ExtractBool(Hashtable data, string key, bool def)
	{
		if (data != null && data.ContainsKey(key))
		{
			object obj = data[key];
			if (obj is bool)
			{
				return (bool)obj;
			}
		}
		return def;
	}

	public static string ExtractString(Hashtable data, string key, string def)
	{
		if (data != null && data.ContainsKey(key))
		{
			object obj = data[key];
			if (obj is string)
			{
				return obj as string;
			}
		}
		return def;
	}
}
