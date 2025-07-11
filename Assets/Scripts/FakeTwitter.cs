using System.Collections.Generic;

public class FakeTwitter
{
	public static void init(string consumerKey, string consumerSecret)
	{
	}

	public static bool isLoggedIn()
	{
		return false;
	}

	public static string loggedInUsername()
	{
		return string.Empty;
	}

	public static void login(string username, string password)
	{
	}

	public static void showOauthLoginDialog()
	{
	}

	public static void logout()
	{
	}

	public static void postStatusUpdate(string status)
	{
	}

	public static void postStatusUpdate(string status, string pathToImage)
	{
	}

	public static void getHomeTimeline()
	{
	}

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{
	}

	public static bool isTweetSheetSupported()
	{
		return false;
	}

	public static bool canUserTweet()
	{
		return false;
	}

	public static void showTweetComposer(string status)
	{
		showTweetComposer(status, null, null);
	}

	public static void showTweetComposer(string status, string pathToImage)
	{
		showTweetComposer(status, pathToImage, null);
	}

	public static void showTweetComposer(string status, string pathToImage, string link)
	{
	}
}
