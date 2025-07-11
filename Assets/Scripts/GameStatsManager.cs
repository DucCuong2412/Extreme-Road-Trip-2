public class GameStatsManager : AutoSingleton<GameStatsManager>
{
	private GameStatsDatabase _database;

	public GameStats CurrentRun
	{
		get;
		private set;
	}

	public GameStats Overall
	{
		get;
		private set;
	}

	protected override void OnAwake()
	{
		_database = new GameStatsDatabase();
		CurrentRun = new GameStats();
		Overall = _database.GetGameStats();
	}

	private void Save()
	{
		_database.Save(Overall);
	}

	public void OnGameStarted()
	{
		CurrentRun.Clear();
		CurrentRun.AddCar(Singleton<GameManager>.Instance.CarRef);
	}

	public void EndGame(Car currentCar)
	{
		Overall.EndGame(CurrentRun, currentCar);
		Save();
	}

	public void RecordCoinsSpent(int amount)
	{
		Overall.RecordCoinsSpent(amount);
		Save();
	}

	public void RecordBucksSpent(int amount)
	{
		Overall.RecordBucksSpent(amount);
		Save();
	}

	public void RecordPrestigeTokensSpent(int amount)
	{
		Overall.RecordPrestigeTokensSpent(amount);
		Save();
	}
}
