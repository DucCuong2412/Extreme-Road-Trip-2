public class Player : AutoSingleton<Player>
{
	public PlayerProfile Profile
	{
		get;
		private set;
	}

	protected override void OnAwake()
	{
		Profile = AutoSingleton<PlayerDatabase>.Instance.GetPlayerProfile();
		base.OnAwake();
	}

	public void SaveProfile()
	{
		AutoSingleton<PlayerDatabase>.Instance.SavePlayerProfile(Profile);
	}
}
