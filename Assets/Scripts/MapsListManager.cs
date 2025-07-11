using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsListManager : AutoSingleton<MapsListManager>
{
	private Hashtable _playlist;

	protected override void OnAwake()
	{
		_playlist = new Hashtable();
		List<Location> allLocations = AutoSingleton<LocationDatabase>.Instance.GetAllLocations();
		foreach (Location item in allLocations)
		{
			_playlist[item.name] = true;
		}
		Load();
		base.OnAwake();
	}

	private void Save()
	{
		ArrayList arrayList = new ArrayList();
		foreach (DictionaryEntry item in _playlist)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Name"] = (string)item.Key;
			hashtable["IsSelected"] = (bool)item.Value;
			arrayList.Add(hashtable);
		}
		Preference.SetString("MapsList", arrayList.toJson());
	}

	private void Load()
	{
		string @string = Preference.GetString("MapsList", string.Empty);
		ArrayList arrayList = @string.arrayListFromJson();
		if (arrayList != null)
		{
			foreach (object item in arrayList)
			{
				Hashtable hashtable = item as Hashtable;
				if (hashtable != null)
				{
					string key = JsonUtil.ExtractString(hashtable, "Name", string.Empty);
					bool flag = JsonUtil.ExtractBool(hashtable, "IsSelected", def: true);
					_playlist[key] = flag;
				}
			}
		}
	}

	public List<string> GetPlaylist()
	{
		List<string> list = new List<string>();
		foreach (DictionaryEntry item2 in _playlist)
		{
			if ((bool)item2.Value)
			{
				string item = (string)item2.Key;
				list.Add(item);
			}
		}
		if (list.Count == 0)
		{
			UnityEngine.Debug.LogWarning("Location playlist should never be empty!");
			list.Add(AutoSingleton<LocationDatabase>.Instance.GetAllLocations()[0].name);
		}
		return list;
	}

	public void SetPlaylist(List<string> newPlaylist)
	{
		if (newPlaylist != null && newPlaylist.Count > 0)
		{
			List<string> list = new List<string>();
			foreach (DictionaryEntry item in _playlist)
			{
				list.Add((string)item.Key);
			}
			foreach (string item2 in list)
			{
				bool flag = newPlaylist.Contains(item2);
				_playlist[item2] = flag;
			}
			Save();
		}
	}
}
