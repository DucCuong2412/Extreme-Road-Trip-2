using System.Collections;
using UnityEngine;

public class Preference : AutoSingleton<Preference>
{
	private Hashtable _data;

	private bool _useNotifications;

	private void Load()
	{
		LoadLocal();
	}

	public static void Save()
	{
		SaveLocal();
	}

	public static void SaveToDisk()
	{
		PlayerPrefs.Save();
	}

	private static void SaveLocal()
	{
		string text = AutoSingleton<Preference>.Instance._data.toJson();
		PlayerPrefs.SetString("_data", text);
		string value = Security.ComputeHash(text);
		PlayerPrefs.SetString("_hash", value);
	}

	private int CompareProgression(IDictionary cloudData, Hashtable localData)
	{
		int num = 0;
		string text = cloudData["GameStats"] as string;
		if (!string.IsNullOrEmpty(text))
		{
			string json = JsonUtil.ExtractString(localData, "GameStats", string.Empty);
			GameStats gameStats = new GameStats(text.hashtableFromJson(), AutoSingleton<CarDatabase>.Instance.GetAllCars());
			GameStats gameStats2 = new GameStats(json.hashtableFromJson(), AutoSingleton<CarDatabase>.Instance.GetAllCars());
			int total = gameStats.GetTotal(GameStats.CarStats.Type.totalDistance);
			int total2 = gameStats2.GetTotal(GameStats.CarStats.Type.totalDistance);
			num = total.CompareTo(total2);
			if (num == 0)
			{
				string text2 = cloudData["DefaultPlayerProfile"] as string;
				if (!string.IsNullOrEmpty(text2))
				{
					string json2 = JsonUtil.ExtractString(localData, "DefaultPlayerProfile", string.Empty);
					PlayerProfile playerProfile = new PlayerProfile(text2);
					PlayerProfile playerProfile2 = new PlayerProfile(json2);
					num = playerProfile.Bucks.CompareTo(playerProfile2.Bucks);
				}
				else
				{
					SilentDebug.LogWarning("Can't find PlayerProfile in cloud saved data");
				}
			}
		}
		else
		{
			SilentDebug.LogWarning("Can't find GameStats in cloud saved data");
		}
		return num;
	}

	private void LoadLocal()
	{
		string @string = PlayerPrefs.GetString("_data", "{}");
		OverwriteGameData(@string.hashtableFromJson());
	}

	private void OnApplicationQuit()
	{
		Save();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			Save();
			SaveToDisk();
		}
	}

	public static void SetUseNotifications(bool use)
	{
		AutoSingleton<Preference>.Instance._useNotifications = use;
		PlayerPrefs.SetInt("_useNotifications", use ? 1 : 0);
	}

	public static bool UseNotifications()
	{
		return AutoSingleton<Preference>.Instance._useNotifications;
	}

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_useNotifications = ((PlayerPrefs.GetInt("_useNotifications", 1) == 1) ? true : false);
		Load();
		base.OnAwake();
	}

	public static int GetInt(string key, int def = 0)
	{
		return JsonUtil.ExtractInt(AutoSingleton<Preference>.Instance._data, key, def);
	}

	public static void SetInt(string key, int v)
	{
		AutoSingleton<Preference>.Instance._data[key] = v;
	}

	public static float GetFloat(string key, float def = 0f)
	{
		return JsonUtil.ExtractFloat(AutoSingleton<Preference>.Instance._data, key, def);
	}

	public static void SetFloat(string key, float v)
	{
		AutoSingleton<Preference>.Instance._data[key] = v;
	}

	public static bool GetBool(string key, bool def = false)
	{
		return JsonUtil.ExtractBool(AutoSingleton<Preference>.Instance._data, key, def);
	}

	public static void SetBool(string key, bool v)
	{
		AutoSingleton<Preference>.Instance._data[key] = v;
	}

	public static string GetString(string key, string def = "")
	{
		return JsonUtil.ExtractString(AutoSingleton<Preference>.Instance._data, key, def);
	}

	public static void SetString(string key, string v)
	{
		AutoSingleton<Preference>.Instance._data[key] = v;
	}

	public static bool HasKey(string key)
	{
		return AutoSingleton<Preference>.Instance._data.ContainsKey(key);
	}

	private void OverwriteGameData(Hashtable data)
	{
		_data = data;
	}

	public Hashtable GetData()
	{
		return _data;
	}
}
