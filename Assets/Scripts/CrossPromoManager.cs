using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPromoManager<T> : AutoSingleton<T> where T : MonoBehaviour
{
	private Hashtable SharedData = new Hashtable();

	private string _prefKey;

	private string _deepUrl;

	protected virtual void OnProgressLoaded()
	{
	}

	protected virtual void OnDeepLinkParsed()
	{
	}

	protected void Setup(string prefKey, string deepUrl)
	{
		_prefKey = prefKey;
		_deepUrl = deepUrl;
		LoadProgress();
		//ReadProgressFromDeepLink();
	}

	protected void SaveProgress()
	{
		Preference.SetString(_prefKey, SharedData.toJson());
		Preference.Save();
	}

	protected void LoadProgress()
	{
		string @string = Preference.GetString(_prefKey, "{}");
		Hashtable hashtable = @string.hashtableFromJson();
		if (hashtable != null)
		{
			SharedData = @string.hashtableFromJson();
			OnProgressLoaded();
		}
	}

	//protected void ReadProgressFromDeepLink()
	//{
	//	string appLaunchUrl = AutoSingleton<GameFacebookManager>.Instance.GetAppLaunchUrl();
	//	if (appLaunchUrl.Contains(_deepUrl))
	//	{
	//		int num = appLaunchUrl.IndexOf('?');
	//		if (num != -1)
	//		{
	//			Dictionary<string, object> dictionary = ParseUri(appLaunchUrl.Substring(num + 1));
	//			foreach (KeyValuePair<string, object> item in dictionary)
	//			{
	//				SetProgress(item.Key, item.Value);
	//			}
	//			SaveProgress();
	//			OnDeepLinkParsed();
	//		}
	//	}
	//}

	private Dictionary<string, object> ParseUri(string uri)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		string[] array = uri.Split('&');
		Array.ForEach(array, delegate(string param)
		{
			string[] array2 = param.Split('=');
			if (array2.Length == 2)
			{
				parameters[array2[0]] = array2[1];
			}
		});
		return parameters;
	}

	protected virtual void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			//ReadProgressFromDeepLink();
		}
	}

	protected object GetSharedValue(string key)
	{
		if (SharedData.Contains(key))
		{
			return SharedData[key];
		}
		return null;
	}

	private void SetProgress(string key, object value)
	{
		SharedData[key] = value;
	}

	protected bool IsValidData(string key)
	{
		return SharedData.Contains(key);
	}
}
