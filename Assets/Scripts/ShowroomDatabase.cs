using System.Collections.Generic;
using UnityEngine;

public class ShowroomDatabase : AutoSingleton<ShowroomDatabase>
{
	private List<Showroom> _allShowroom;

	private Dictionary<Showroom, ShowroomProfile> _showroomProfileCache;

	protected override void OnAwake()
	{
		_allShowroom = new List<Showroom>();
		_showroomProfileCache = new Dictionary<Showroom, ShowroomProfile>();
		CSVData cSVData = new CSVData("showrooms.csv");
		int count = cSVData.Count;
		for (int i = 0; i < count; i++)
		{
			string @string = cSVData.GetString(i, "prefab");
			if (@string == null || !(@string != string.Empty))
			{
				continue;
			}
			GameObject gameObject = Resources.Load("Prefabs/" + @string) as GameObject;
			if (!(gameObject != null))
			{
				continue;
			}
			Showroom component = gameObject.GetComponent<Showroom>();
			if (component != null)
			{
				component.Description = cSVData.GetString(i, "description", string.Empty);
				component.DisplayName = cSVData.GetString(i, "displayName", "SHOWROOM");
				int @int = cSVData.GetInt(i, "coins", 1000);
				int int2 = cSVData.GetInt(i, "bucks", 1000);
				if (int2 != 0)
				{
					component.Price = new Price(int2, Currency.bucks);
				}
				else
				{
					component.Price = new Price(@int, Currency.coins);
				}
				if (component.Price.IsFree())
				{
					GetShowroomProfile(component).UnlockForFree();
				}
				_allShowroom.Add(component);
			}
		}
		AutoSingleton<AchievementsManager>.Instance.ShowroomUnlock();
	}

	public List<Showroom> GetAllShowroom()
	{
		return _allShowroom;
	}

	public Showroom GetShowroom(string showroomName)
	{
		Showroom showroom = _allShowroom.Find((Showroom s) => s.name == showroomName);
		if (showroom == null)
		{
			UnityEngine.Debug.LogWarning("Can't find showroom: " + showroomName);
			showroom = _allShowroom[0];
		}
		return showroom;
	}

	public int GetUnlockedShowroomCount()
	{
		int num = 0;
		foreach (Showroom item in GetAllShowroom())
		{
			ShowroomProfile showroomProfile = GetShowroomProfile(item);
			if (showroomProfile.IsUnlocked())
			{
				num++;
			}
		}
		return num;
	}

	private string ShowroomProfilePrefId(Showroom showroom)
	{
		return showroom.name + "Profile";
	}

	public ShowroomProfile GetShowroomProfile(Showroom showroom)
	{
		if (!_showroomProfileCache.ContainsKey(showroom))
		{
			string @string = Preference.GetString(ShowroomProfilePrefId(showroom), string.Empty);
			if (@string == string.Empty)
			{
				_showroomProfileCache[showroom] = new ShowroomProfile();
			}
			else
			{
				_showroomProfileCache[showroom] = new ShowroomProfile(@string);
			}
		}
		return _showroomProfileCache[showroom];
	}

	public void SaveShowroomProfile(Showroom showroom, ShowroomProfile profile)
	{
		string v = profile.ToJson();
		Preference.SetString(ShowroomProfilePrefId(showroom), v);
		Preference.Save();
	}
}
