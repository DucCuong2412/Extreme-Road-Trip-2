public static class RateGameManager
{
	public static void RateGame()
	{
		string title = "Having fun?".Localize();
		string message = "Please rate the game to help spread the word!".Localize();
		EtceteraAndroid.askForReview(3, 48, 24, title, message);
	}
}
