using System.Collections;
using System.Collections.Generic;

public static class MiniJsonExtensions
{
	public static string toJson(this Hashtable obj)
	{
		return MiniJSONRoofdog.jsonEncode(obj);
	}

	public static string toJson(this Dictionary<string, string> obj)
	{
		return MiniJSONRoofdog.jsonEncode(obj);
	}

	public static string toJson(this ArrayList obj)
	{
		return MiniJSONRoofdog.jsonEncode(obj);
	}

	public static ArrayList arrayListFromJson(this string json)
	{
		return MiniJSONRoofdog.jsonDecode(json) as ArrayList;
	}

	public static Hashtable hashtableFromJson(this string json)
	{
		return MiniJSONRoofdog.jsonDecode(json) as Hashtable;
	}
}
