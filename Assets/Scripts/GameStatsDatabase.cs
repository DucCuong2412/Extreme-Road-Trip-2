public class GameStatsDatabase
{
	public const string _gameStatsKey = "GameStats";

	public GameStats GetGameStats()
	{
		string @string = Preference.GetString("GameStats", string.Empty);
		return new GameStats(@string.hashtableFromJson(), AutoSingleton<CarDatabase>.Instance.GetAllCars());
	}

	public void Save(GameStats stats)
	{
		string v = stats.ToJson();
		Preference.SetString("GameStats", v);
	}
}
