using System.Collections;
using UnityEngine;

public class LoadConfigScene : LoadConfig
{
	private string _scene;

	public LoadConfigScene(string scene)
	{
		_scene = scene;
	}

	public override IEnumerator Load()
	{
		yield return null;
		if (!AutoSingleton<PersistenceManager>.Instance.SpentMoney)
		{
		}
		yield return null;
		UnityEngine.SceneManagement.SceneManager.LoadScene(_scene);
		yield return null;
	}

	public override IEnumerator Unload()
	{
		yield return null;
	}
}
