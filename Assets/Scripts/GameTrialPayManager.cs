public class GameTrialPayManager : AutoSingleton<GameTrialPayManager>
{
	public static bool IsAvailable()
	{
		return false;
	}

	public static bool IsSupported()
	{
		return false;
	}

	public static MetroWidget GetOfferWallButton()
	{
		return null;
	}
}
