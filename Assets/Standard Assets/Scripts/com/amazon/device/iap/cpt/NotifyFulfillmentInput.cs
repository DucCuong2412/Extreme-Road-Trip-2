using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class NotifyFulfillmentInput : Jsonable
	{
		public string ReceiptId
		{
			get;
			set;
		}

		public string FulfillmentResult
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
				dictionary.Add("receiptId", ReceiptId);
				dictionary.Add("fulfillmentResult", FulfillmentResult);
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

		public static NotifyFulfillmentInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput();
				if (jsonMap.ContainsKey("receiptId"))
				{
					notifyFulfillmentInput.ReceiptId = (string)jsonMap["receiptId"];
				}
				if (jsonMap.ContainsKey("fulfillmentResult"))
				{
					notifyFulfillmentInput.FulfillmentResult = (string)jsonMap["fulfillmentResult"];
				}
				return notifyFulfillmentInput;
				IL_0066:
				NotifyFulfillmentInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_0078:
				NotifyFulfillmentInput result;
				return result;
			}
		}

		public static NotifyFulfillmentInput CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				NotifyFulfillmentInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				NotifyFulfillmentInput result;
				return result;
			}
		}

		public static Dictionary<string, NotifyFulfillmentInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, NotifyFulfillmentInput> dictionary = new Dictionary<string, NotifyFulfillmentInput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				NotifyFulfillmentInput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<NotifyFulfillmentInput> ListFromJson(List<object> array)
		{
			List<NotifyFulfillmentInput> list = new List<NotifyFulfillmentInput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
