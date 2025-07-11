using System.Collections;
using System.Collections.Generic;

public class PlayerProfile
{
	private const int _defaultCoinsValue = 0;

	private const int _defaultBucksValue = 0;

	private const int _defaultPrestigeTokensValue = 0;

	private const int _defaultPrestigePointsValue = 0;

	private const int _defaultGamesUntilEpicPowerupNewPlayers = 10;

	private const int _defaultGamesUntilEpicPowerupCurrentPlayers = 2;

	private const int _defaultEpicPowerupStreak = 0;

	private Dictionary<PowerupType, int> _powerups;

	private List<PowerupType> _newPowerups;

	public XPProfile XPProfile
	{
		get;
		private set;
	}

	public int Coins
	{
		get;
		private set;
	}

	public int Bucks
	{
		get;
		private set;
	}

	public int PrestigeTokens
	{
		get;
		private set;
	}

	public int PrestigePoints
	{
		get;
		private set;
	}

	public ArrayList StorePerkList
	{
		get;
		set;
	}

	public EpicPowerupType NextEpicPowerup
	{
		get;
		private set;
	}

	public int GamesUntilEpicPowerup
	{
		get;
		private set;
	}

	public int EpicPowerupStreak
	{
		get;
		private set;
	}

	public string EpicPowerupServerConfigVersion
	{
		get;
		set;
	}

	public PlayerProfile(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			Load();
		}
		else
		{
			Load(json);
		}
	}

	public void OnCoinsChanged(int newCoinsAmount)
	{
		if (newCoinsAmount != Coins)
		{
			Coins = newCoinsAmount;
			AutoSingleton<Player>.Instance.SaveProfile();
		}
	}

	public void OnBucksChanged(int newAmount)
	{
		if (newAmount != Bucks)
		{
			Bucks = newAmount;
			AutoSingleton<Player>.Instance.SaveProfile();
		}
	}

	public void OnPrestigeTokensChanged(int newTokensAmount)
	{
		PrestigeTokens = newTokensAmount;
		AutoSingleton<Player>.Instance.SaveProfile();
	}

	public void OnPrestigePointsChanged(int newPointsAmount)
	{
		PrestigePoints = newPointsAmount;
		AutoSingleton<Player>.Instance.SaveProfile();
	}

	public void OnPowerupAdded(PowerupType p, int amount)
	{
		if (_powerups.ContainsKey(p))
		{
			Dictionary<PowerupType, int> powerups;
			Dictionary<PowerupType, int> dictionary = powerups = _powerups;
			PowerupType key;
			PowerupType key2 = key = p;
			int num = powerups[key];
			dictionary[key2] = num + amount;
		}
		else
		{
			_powerups[p] = amount;
			_newPowerups.Add(p);
		}
		AutoSingleton<Player>.Instance.SaveProfile();
	}

	public void OnPowerupRemoved(PowerupType p, int amount)
	{
		if (_powerups.ContainsKey(p))
		{
			Dictionary<PowerupType, int> powerups;
			Dictionary<PowerupType, int> dictionary = powerups = _powerups;
			PowerupType key;
			PowerupType key2 = key = p;
			int num = powerups[key];
			dictionary[key2] = num - amount;
			_newPowerups.Remove(p);
			AutoSingleton<Player>.Instance.SaveProfile();
		}
	}

	public bool HasPowerup(PowerupType p)
	{
		return _powerups.ContainsKey(p);
	}

	public int GetNumPowerups(PowerupType p)
	{
		return HasPowerup(p) ? _powerups[p] : 0;
	}

	public bool IsNewPowerup(PowerupType p)
	{
		return _newPowerups.Contains(p);
	}

	public bool CanUsePowerup(PowerupType p)
	{
		return HasPowerup(p) && (p != 0 || NextEpicPowerup != EpicPowerupType.rocket);
	}

	public void SetNextEpicPowerup(EpicPowerupType type)
	{
		NextEpicPowerup = type;
		AutoSingleton<Player>.Instance.SaveProfile();
	}

	public void SetGamesUntilEpicPowerup(int games)
	{
		GamesUntilEpicPowerup = games;
		AutoSingleton<Player>.Instance.SaveProfile();
	}

	public void SetEpicPowerupStreak(int streak)
	{
		EpicPowerupStreak = streak;
		AutoSingleton<Player>.Instance.SaveProfile();
	}

	private void Load()
	{
		XPProfile = new XPProfile();
		Coins = 0;
		Bucks = 0;
		PrestigeTokens = 0;
		PrestigePoints = 0;
		_powerups = new Dictionary<PowerupType, int>();
		_newPowerups = new List<PowerupType>();
		NextEpicPowerup = EpicPowerupType.none;
		GamesUntilEpicPowerup = 10;
		EpicPowerupStreak = 0;
		StorePerkList = new ArrayList();
	}

	public void Load(string json)
	{
		Hashtable data = json.hashtableFromJson();
		XPProfile = new XPProfile(JsonUtil.ExtractHashtable(data, "XP"));
		Coins = JsonUtil.ExtractInt(data, "Coins", 0);
		Bucks = JsonUtil.ExtractInt(data, "Bucks", 0);
		PrestigeTokens = JsonUtil.ExtractInt(data, "PrestigeTokens", 0);
		PrestigePoints = JsonUtil.ExtractInt(data, "PrestigePoints", 0);
		_powerups = new Dictionary<PowerupType, int>();
		ArrayList arrayList = JsonUtil.ExtractArrayList(data, "Powerups", new ArrayList());
		foreach (Hashtable item in arrayList)
		{
			PowerupType typeFromString = Powerup.GetTypeFromString(JsonUtil.ExtractString(item, "PowerupType", string.Empty));
			_powerups[typeFromString] = JsonUtil.ExtractInt(item, "Amount", 0);
		}
		_newPowerups = new List<PowerupType>();
		ArrayList arrayList2 = JsonUtil.ExtractArrayList(data, "NewPowerups", new ArrayList());
		foreach (string item2 in arrayList2)
		{
			_newPowerups.Add(Powerup.GetTypeFromString(item2));
		}
		NextEpicPowerup = EpicPowerup.GetTypeFromString(JsonUtil.ExtractString(data, "EP", string.Empty));
		GamesUntilEpicPowerup = JsonUtil.ExtractInt(data, "DaysUntilEP", 2);
		EpicPowerupStreak = JsonUtil.ExtractInt(data, "EPStreak", 0);
		StorePerkList = JsonUtil.ExtractArrayList(data, "StorePerk", new ArrayList());
		EpicPowerupServerConfigVersion = JsonUtil.ExtractString(data, "EpicPowerupServerVersion", string.Empty);
	}

	public string ToJson()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Type"] = GetType().ToString();
		hashtable["XP"] = XPProfile.ToJson();
		hashtable["Coins"] = Coins;
		hashtable["Bucks"] = Bucks;
		hashtable["PrestigeTokens"] = PrestigeTokens;
		hashtable["PrestigePoints"] = PrestigePoints;
		ArrayList arrayList = new ArrayList();
		foreach (PowerupType key in _powerups.Keys)
		{
			Hashtable hashtable2 = new Hashtable();
			hashtable2["PowerupType"] = key.ToString();
			hashtable2["Amount"] = _powerups[key];
			arrayList.Add(hashtable2);
		}
		hashtable["Powerups"] = arrayList;
		ArrayList arrayList2 = new ArrayList();
		foreach (PowerupType newPowerup in _newPowerups)
		{
			arrayList2.Add(newPowerup.ToString());
		}
		hashtable["NewPowerups"] = arrayList2;
		hashtable["EP"] = NextEpicPowerup.ToString();
		hashtable["DaysUntilEP"] = GamesUntilEpicPowerup;
		hashtable["EPStreak"] = EpicPowerupStreak;
		hashtable["StorePerk"] = StorePerkList;
		hashtable["EpicPowerupServerVersion"] = EpicPowerupServerConfigVersion;
		return hashtable.toJson();
	}

	public List<int> GetPowerupsInventory()
	{
		List<int> list = new List<int>();
		list.Add(GetNumPowerups(PowerupType.boost));
		list.Add(GetNumPowerups(PowerupType.coinDoubler));
		list.Add(GetNumPowerups(PowerupType.magnet));
		return list;
	}
}
