using System.Collections;
using UnityEngine;

public class LoadConfigGame : LoadConfig
{
	public Car Car
	{
		get;
		private set;
	}

	public Location Location
	{
		get;
		private set;
	}

	public GameMode GameMode
	{
		get;
		private set;
	}

	public LoadConfigGame(Car car, GameMode mode = GameMode.normal)
	{
		Car = car;
		GameMode = mode;
	}

	public override IEnumerator Load()
	{
		Location = AutoSingleton<LocationDatabase>.Instance.GetRandomLocation();
		yield return null;
		UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
		yield return null;
	}

	public override IEnumerator Unload()
	{
		yield return null;
	}
}
