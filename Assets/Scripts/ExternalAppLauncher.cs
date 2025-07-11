using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ExternalAppLauncher : AutoSingleton<ExternalAppLauncher>
{
	private Dictionary<string, ExternalAppData> _extData;

	protected override void OnAwake()
	{
		base.OnAwake();
		Object.DontDestroyOnLoad(base.gameObject);
		_extData = new Dictionary<string, ExternalAppData>();
		LoadConfig();
	}

	public void LoadConfig()
	{
		TextAsset textAsset = Resources.Load("externalApp.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			Hashtable data = textAsset.text.hashtableFromJson();
			Dictionary<string, ExternalAppData> extData = ExternalAppDataFactory.FromHashtable(data);
			SetExtData(extData);
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file externalApp.json.txt");
		}
	}

	public void SetExtData(Dictionary<string, ExternalAppData> data)
	{
		if (data != null)
		{
			if (data.Count <= 0)
			{
				UnityEngine.Debug.LogWarning("ExternalAppLauncher: Configuration is empty");
			}
			_extData = data;
		}
		else
		{
			UnityEngine.Debug.LogWarning("ExternalAppLauncher: Configuration is null");
		}
	}

	public void LoadData(Dictionary<string, ExternalAppData> extData)
	{
		if (extData != null && extData.Count > 0)
		{
			_extData = extData;
		}
	}

	public bool IsGameInstalled(string externalRef)
	{
		string externalRefId = GetExternalRefId(externalRef);
		return ExternalAppLaunch.IsGameInstalled(externalRefId);
	}

	public bool LaunchGame(string externalRef, string param = null)
	{
		string externalRefId = GetExternalRefId(externalRef);
		return ExternalAppLaunch.LaunchExternalApp(externalRefId, param);
	}

	public string GetDisplayName(string externalRef)
	{
		if (_extData.ContainsKey(externalRef))
		{
			ExternalAppData externalAppData = _extData[externalRef];
			return externalAppData.DisplayName;
		}
		return string.Empty;
	}

	private string GetExternalRefId(string externalRef)
	{
		if (!string.IsNullOrEmpty(externalRef) && _extData.ContainsKey(externalRef))
		{
			ExternalAppData data = _extData[externalRef];
			return GetExternalRefId(data);
		}
		UnityEngine.Debug.LogWarning("Invalid external Ref or no config");
		return string.Empty;
	}

	private string GetExternalRefId(ExternalAppData data)
	{
		if (data != null && data.IsValid() && data.PlatformData != null && data.PlatformData.ContainsKey(GetCurrentPlatformId()))
		{
			ExternalAppPlaformSpecificData externalAppPlaformSpecificData = data.PlatformData[GetCurrentPlatformId()];
			return externalAppPlaformSpecificData.AppId;
		}
		if (GetCurrentPlatformId() != "EDITOR")
		{
			UnityEngine.Debug.LogWarning("Doesn't have a config for this platform: " + GetCurrentPlatformId());
		}
		return string.Empty;
	}

	public string GetExternalStoreURL(string externalRef)
	{
		if (_extData.ContainsKey(externalRef))
		{
			ExternalAppData data = _extData[externalRef];
			return GetExternalStoreURL(data);
		}
		return string.Empty;
	}

	private string GetExternalStoreURL(ExternalAppData data)
	{
		if (data != null && data.IsValid() && data.PlatformData != null && data.PlatformData.ContainsKey(GetCurrentPlatformId()))
		{
			ExternalAppPlaformSpecificData externalAppPlaformSpecificData = data.PlatformData[GetCurrentPlatformId()];
			return externalAppPlaformSpecificData.StoreURL;
		}
		return string.Empty;
	}

	private string GetCurrentPlatformId()
	{
		return Device.GetDeviceType().ToUpper();
	}

	[Conditional("DEBUG_LOG")]
	private static void Log(string log)
	{
		UnityEngine.Debug.Log("ExternalAppLauncher: " + log);
	}

	public void LaunchOrStoreRedirect(string externalRef, string param = null)
	{
		string externalRefId = GetExternalRefId(externalRef);
		string externalStoreURL = GetExternalStoreURL(externalRef);
		ExternalAppLaunch.LaunchOrRedirect(externalRefId, param, externalStoreURL);
	}

	public void LaunchOrStoreRedirect(ExternalAppData data, string param = null)
	{
		if (data != null && data.IsValid())
		{
			string externalRefId = GetExternalRefId(data);
			string externalStoreURL = GetExternalStoreURL(data);
			ExternalAppLaunch.LaunchOrRedirect(externalRefId, param, externalStoreURL);
		}
	}
}
