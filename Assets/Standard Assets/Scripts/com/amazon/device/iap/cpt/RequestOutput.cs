using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class RequestOutput : Jsonable
	{
		public string RequestId
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
				dictionary.Add("requestId", RequestId);
				return dictionary;
				IL_001e:
				Dictionary<string, object> result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
				IL_0030:
				Dictionary<string, object> result;
				return result;
			}
		}

		public static RequestOutput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				RequestOutput requestOutput = new RequestOutput();
				if (jsonMap.ContainsKey("requestId"))
				{
					requestOutput.RequestId = (string)jsonMap["requestId"];
				}
				return requestOutput;
				IL_0040:
				RequestOutput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_0052:
				RequestOutput result;
				return result;
			}
		}

		public static RequestOutput CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				RequestOutput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				RequestOutput result;
				return result;
			}
		}

		public static Dictionary<string, RequestOutput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, RequestOutput> dictionary = new Dictionary<string, RequestOutput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				RequestOutput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<RequestOutput> ListFromJson(List<object> array)
		{
			List<RequestOutput> list = new List<RequestOutput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
