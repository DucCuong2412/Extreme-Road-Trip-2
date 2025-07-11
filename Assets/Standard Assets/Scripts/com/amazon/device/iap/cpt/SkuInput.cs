using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkuInput : Jsonable
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

		public static SkuInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				SkuInput skuInput = new SkuInput();
				if (jsonMap.ContainsKey("sku"))
				{
					skuInput.Sku = (string)jsonMap["sku"];
				}
				return skuInput;
				IL_0040:
				SkuInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_0052:
				SkuInput result;
				return result;
			}
		}

		public static SkuInput CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				SkuInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				SkuInput result;
				return result;
			}
		}

		public static Dictionary<string, SkuInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkuInput> dictionary = new Dictionary<string, SkuInput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				SkuInput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<SkuInput> ListFromJson(List<object> array)
		{
			List<SkuInput> list = new List<SkuInput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
