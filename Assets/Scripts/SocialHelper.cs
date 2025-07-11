using UnityEngine;

public class SocialHelper
{
	public static void Logout(SocialNetwork network)
	{
		switch (network)
		{
		case SocialNetwork.twitter:
			AutoSingleton<GameTwitterManager>.Instance.Logout();
			break;
		case SocialNetwork.facebook:
			AutoSingleton<GameFacebookManager>.Instance.Logout();
			break;
		}
	}

	public static string GetSharingShowroomString()
	{
		return "Look at my showroom from Extreme Road Trip 2!";
	}

	public static void ShareShowroom(SocialNetwork network, byte[] image)
	{
		PostPicture(network, GetSharingShowroomString(), image);
	}

	public static void PostPicture(SocialNetwork network, string message, byte[] image)
	{
		switch (network)
		{
		case SocialNetwork.twitter:
			AutoSingleton<GameTwitterManager>.Instance.Tweet(message + " #xroadtrip2", image);
			break;
		case SocialNetwork.facebook:
			AutoSingleton<GameFacebookManager>.Instance.PublishImageToStream(image);
			break;
		}
	}

	public static string GetBestRunDescription(Car car, int bucks, int coins, GameMode gameMode)
	{
		switch (gameMode)
		{
		case GameMode.normal:
		{
			int maxDistance = AutoSingleton<GameStatsManager>.Instance.Overall.GetMaxDistance(car);
			return "I've reached".Localize() + " " + maxDistance.ToString() + "m in Extreme Road Trip 2 with".Localize() + " " + car.DisplayName.Localize();
		}
		case GameMode.frenzy:
			return $"I just racked up {bucks} bucks and {coins} coins in my daily Frenzy Run! Did you play yours today?";
		default:
			UnityEngine.Debug.LogWarning("Social sharing - Unknown game mode: " + gameMode.ToString());
			return string.Empty;
		}
	}

	public static void ShareBestRunScore(SocialNetwork network, Car car, int bucks, int coins, GameMode gameMode)
	{
		if (car != null)
		{
			string title = "My best score!".Localize();
			string bestRunDescription = GetBestRunDescription(car, bucks, coins, gameMode);
			Publish(network, title, bestRunDescription);
		}
	}

	private static void Publish(SocialNetwork network, string title, string message)
	{
		switch (network)
		{
		case SocialNetwork.twitter:
		{
			string text = title + " " + message;
			string text2 = " http://extremeroadtrip2.com";
			string text3 = " #xroadtrip2";
			if (text.Length + text2.Length < 140)
			{
				text += text2;
			}
			if (text.Length + text3.Length < 140)
			{
				text += text3;
			}
			AutoSingleton<GameTwitterManager>.Instance.Tweet(text);
			break;
		}
		case SocialNetwork.facebook:
			AutoSingleton<GameFacebookManager>.Instance.PublishToStream(title, message);
			break;
		}
	}
}
