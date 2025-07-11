using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class ResetInput : Jsonable
	{
		public bool Reset
		{
			get;
			set;
		}

		public string ToJson()
		{
			try
			{
				Dictionary<string, object> objectDictionary = GetObjectDictionary();
				return Json.Serialize(objectDictionary);
				IL_0013:
				string result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while Jsoning", inner);
				IL_0025:
				string result;
				return result;
			}
		}

		public override Dictionary<string, object> GetObjectDictionary()
		{
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("reset", Reset);
				return dictionary;
				IL_0023:
				Dictionary<string, object> result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
				IL_0035:
				Dictionary<string, object> result;
				return result;
			}
		}

		public static ResetInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				ResetInput resetInput = new ResetInput();
				if (jsonMap.ContainsKey("reset"))
				{
					resetInput.Reset = (bool)jsonMap["reset"];
				}
				return resetInput;
				IL_0040:
				ResetInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_0052:
				ResetInput result;
				return result;
			}
		}

		public static ResetInput CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				ResetInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				ResetInput result;
				return result;
			}
		}

		public static Dictionary<string, ResetInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ResetInput> dictionary = new Dictionary<string, ResetInput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				ResetInput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<ResetInput> ListFromJson(List<object> array)
		{
			List<ResetInput> list = new List<ResetInput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
