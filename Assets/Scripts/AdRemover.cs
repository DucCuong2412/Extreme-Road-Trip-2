public class AdRemover : StorePerk
{
	public AdRemover()
		: base(StorePerkType.adRemover)
	{
	}

	public override void Activate()
	{
		AutoSingleton<GameAdProvider>.Instance.DisableInterstitial();
	}

	public override void Deactivate()
	{
		AutoSingleton<GameAdProvider>.Instance.EnableInterstitial();
	}
}
