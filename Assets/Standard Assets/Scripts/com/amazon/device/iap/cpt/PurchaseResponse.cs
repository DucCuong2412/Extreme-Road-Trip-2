using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class PurchaseResponse : Jsonable
	{
		public string RequestId
		{
			get;
			set;
		}

		public AmazonUserData AmazonUserData
		{
			get;
			set;
		}

		public PurchaseReceipt PurchaseReceipt
		{
			get;
			set;
		}

		public string Status
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
				dictionary.Add("amazonUserData", (AmazonUserData == null) ? null : AmazonUserData.GetObjectDictionary());
				dictionary.Add("purchaseReceipt", (PurchaseReceipt == null) ? null : PurchaseReceipt.GetObjectDictionary());
				dictionary.Add("status", Status);
				return dictionary;
				IL_007d:
				Dictionary<string, object> result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
				IL_008f:
				Dictionary<string, object> result;
				return result;
			}
		}

		public static PurchaseResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				PurchaseResponse purchaseResponse = new PurchaseResponse();
				if (jsonMap.ContainsKey("requestId"))
				{
					purchaseResponse.RequestId = (string)jsonMap["requestId"];
				}
				if (jsonMap.ContainsKey("amazonUserData"))
				{
					purchaseResponse.AmazonUserData = AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("purchaseReceipt"))
				{
					purchaseResponse.PurchaseReceipt = PurchaseReceipt.CreateFromDictionary(jsonMap["purchaseReceipt"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("status"))
				{
					purchaseResponse.Status = (string)jsonMap["status"];
				}
				return purchaseResponse;
				IL_00bc:
				PurchaseResponse result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_00ce:
				PurchaseResponse result;
				return result;
			}
		}

		public static PurchaseResponse CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				PurchaseResponse result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				PurchaseResponse result;
				return result;
			}
		}

		public static Dictionary<string, PurchaseResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, PurchaseResponse> dictionary = new Dictionary<string, PurchaseResponse>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				PurchaseResponse value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<PurchaseResponse> ListFromJson(List<object> array)
		{
			List<PurchaseResponse> list = new List<PurchaseResponse>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
