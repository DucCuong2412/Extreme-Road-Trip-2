using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class SubscriptionExpiredEvent : Jsonable
	{
		public string Sku
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
				dictionary.Add("sku", Sku);
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

		public static SubscriptionExpiredEvent CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				SubscriptionExpiredEvent subscriptionExpiredEvent = new SubscriptionExpiredEvent();
				if (jsonMap.ContainsKey("sku"))
				{
					subscriptionExpiredEvent.Sku = (string)jsonMap["sku"];
				}
				return subscriptionExpiredEvent;
				IL_0040:
				SubscriptionExpiredEvent result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_0052:
				SubscriptionExpiredEvent result;
				return result;
			}
		}

		public static SubscriptionExpiredEvent CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				SubscriptionExpiredEvent result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				SubscriptionExpiredEvent result;
				return result;
			}
		}

		public static Dictionary<string, SubscriptionExpiredEvent> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SubscriptionExpiredEvent> dictionary = new Dictionary<string, SubscriptionExpiredEvent>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				SubscriptionExpiredEvent value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<SubscriptionExpiredEvent> ListFromJson(List<object> array)
		{
			List<SubscriptionExpiredEvent> list = new List<SubscriptionExpiredEvent>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
