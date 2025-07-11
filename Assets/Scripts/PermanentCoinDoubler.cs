public class PermanentCoinDoubler : StorePerk
{
	public PermanentCoinDoubler()
		: base(StorePerkType.permanentCoinDoubler)
	{
	}

	public override void Activate()
	{
		GameSettings.RemoveCoinMultiplier(GetCoin);
		GameSettings.AddCoinMultiplier(GetCoin);
	}

	public override void Deactivate()
	{
		GameSettings.RemoveCoinMultiplier(GetCoin);
	}

	private static float GetCoin(float coin)
	{
		return coin * 2f;
	}
}
