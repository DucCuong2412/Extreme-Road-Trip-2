public class PlayerDatabase : AutoSingleton<PlayerDatabase>
{
	public const string _playerProfilePrefId = "DefaultPlayerProfile";

	private PlayerProfile _playerProfileCache;

	public PlayerProfile GetPlayerProfile()
	{
		if (_playerProfileCache == null)
		{
			string @string = Preference.GetString("DefaultPlayerProfile", string.Empty);
			_playerProfileCache = new PlayerProfile(@string);
		}
		return _playerProfileCache;
	}

	public void Refresh()
	{
		if (_playerProfileCache != null)
		{
			string @string = Preference.GetString("DefaultPlayerProfile", string.Empty);
			_playerProfileCache.Load(@string);
		}
	}

	public void SavePlayerProfile(PlayerProfile playerProfile)
	{
		string v = playerProfile.ToJson();
		Preference.SetString("DefaultPlayerProfile", v);
		Preference.Save();
	}
}
