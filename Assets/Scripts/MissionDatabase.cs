public class MissionDatabase
{
	private const string _missionsKey = "Missions";

	public string Load()
	{
		return Preference.GetString("Missions", string.Empty);
	}

	public void Save(string json)
	{
		Preference.SetString("Missions", json);
	}
}
