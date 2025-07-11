using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDatabase : AutoSingleton<LocationDatabase>
{
	private List<Location> _allLocations;

	protected override void OnAwake()
	{
		_allLocations = new List<Location>();
		TextAsset textAsset = Resources.Load("locations.json", typeof(TextAsset)) as TextAsset;
		foreach (object item in textAsset.text.arrayListFromJson())
		{
			Hashtable hashtable = item as Hashtable;
			if (hashtable.ContainsKey("prefab"))
			{
				GameObject gameObject = Resources.Load("Locations/" + hashtable["prefab"]) as GameObject;
				if (gameObject != null)
				{
					Location component = gameObject.GetComponent<Location>();
					if (component != null)
					{
						component.DisplayName = JsonUtil.ExtractString(hashtable, "name", "SOMEWHERE");
						_allLocations.Add(component);
					}
				}
			}
		}
	}

	public Location GetRandomLocation()
	{
		List<string> playlist = AutoSingleton<MapsListManager>.Instance.GetPlaylist();
		if (playlist.Count > 0)
		{
			string locationName = playlist[Random.Range(0, playlist.Count)];
			return GetLocation(locationName);
		}
		UnityEngine.Debug.LogWarning("Location playlist was empty!!");
		return _allLocations[Random.Range(0, _allLocations.Count)];
	}

	public List<Location> GetAllLocations()
	{
		return _allLocations;
	}

	public Location GetLocation(string locationName)
	{
		return _allLocations.Find((Location l) => l.name == locationName);
	}
}
