using com.amazon.device.iap.cpt.json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace com.amazon.device.iap.cpt
{
	public sealed class SkusInput : Jsonable
	{
		public List<string> Skus
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
				dictionary.Add("skus", Skus);
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

		public static SkusInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			try
			{
				if (jsonMap == null)
				{
					return null;
				}
				SkusInput skusInput = new SkusInput();
				if (jsonMap.ContainsKey("skus"))
				{
					skusInput.Skus = (from element in (List<object>)jsonMap["skus"]
						select (string)element).ToList();
				}
				return skusInput;
				IL_0067:
				SkusInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
				IL_0079:
				SkusInput result;
				return result;
			}
		}

		public static SkusInput CreateFromJson(string jsonMessage)
		{
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				return CreateFromDictionary(jsonMap);
				IL_001e:
				SkusInput result;
				return result;
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
				IL_0030:
				SkusInput result;
				return result;
			}
		}

		public static Dictionary<string, SkusInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkusInput> dictionary = new Dictionary<string, SkusInput>();
			foreach (KeyValuePair<string, object> item in jsonMap)
			{
				SkusInput value = CreateFromDictionary(item.Value as Dictionary<string, object>);
				dictionary.Add(item.Key, value);
			}
			return dictionary;
		}

		public static List<SkusInput> ListFromJson(List<object> array)
		{
			List<SkusInput> list = new List<SkusInput>();
			foreach (object item in array)
			{
				list.Add(CreateFromDictionary(item as Dictionary<string, object>));
			}
			return list;
		}
	}
}
