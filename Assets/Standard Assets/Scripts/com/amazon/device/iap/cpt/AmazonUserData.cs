using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class AmazonUserData : Jsonable
	{
		public string UserId
		{
			get;
			set;
		}

		public string Marketplace
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
				dictionary.Add("userId", UserId);
				dictionary.Add("marketplace", Marketplace);
				return dictionary;
				IL_002f:
				Dictionary<string, object> result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
				IL_0041:
				Dictionary<string, object> result;
				return result;
			}
		}

		public static AmazonUserData CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				AmazonUserData amazonUserData = new AmazonUserData();
				if (jsonMap.ContainsKey("userId"))
				{
					amazonUserData.UserId = (string)jsonMap["userId"];
				}
				if (jsonMap.ContainsKey("marketplace"))
				{
					amazonUserData.Marketplace = (string)jsonMap["marketplace"];
				}
				return amazonUserData;
				IL_0066:
				AmazonUserData result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_0078:
				AmazonUserData result;
				return result;
			}
		}

		public static AmazonUserData CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				AmazonUserData result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				AmazonUserData result;
				return result;
			}
		}

		public static Dictionary<string, AmazonUserData> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AmazonUserData> dictionary = new Dictionary<string, AmazonUserData>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				AmazonUserData value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<AmazonUserData> ListFromJson(List<object> array)
		{
			List<AmazonUserData> list = new List<AmazonUserData>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
