public class PrestigeDatabase
{
	private const string _prestigeKey = "Prestige";

	public string Load()
	{
		return Preference.GetString("Prestige", string.Empty);
	}

	public void Save(string json)
	{
		Preference.SetString("Prestige", json);
	}
}
