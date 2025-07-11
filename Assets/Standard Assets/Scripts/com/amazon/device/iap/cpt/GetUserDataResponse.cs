using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class GetUserDataResponse : Jsonable
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
				dictionary.Add("status", Status);
				return dictionary;
				IL_0056:
				Dictionary<string, object> result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
				IL_0068:
				Dictionary<string, object> result;
				return result;
			}
		}

		public static GetUserDataResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				GetUserDataResponse getUserDataResponse = new GetUserDataResponse();
				if (jsonMap.ContainsKey("requestId"))
				{
					getUserDataResponse.RequestId = (string)jsonMap["requestId"];
				}
				if (jsonMap.ContainsKey("amazonUserData"))
				{
					getUserDataResponse.AmazonUserData = AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
				}
				if (jsonMap.ContainsKey("status"))
				{
					getUserDataResponse.Status = (string)jsonMap["status"];
				}
				return getUserDataResponse;
				IL_0091:
				GetUserDataResponse result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_00a3:
				GetUserDataResponse result;
				return result;
			}
		}

		public static GetUserDataResponse CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				GetUserDataResponse result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				GetUserDataResponse result;
				return result;
			}
		}

		public static Dictionary<string, GetUserDataResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetUserDataResponse> dictionary = new Dictionary<string, GetUserDataResponse>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				GetUserDataResponse value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<GetUserDataResponse> ListFromJson(List<object> array)
		{
			List<GetUserDataResponse> list = new List<GetUserDataResponse>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
