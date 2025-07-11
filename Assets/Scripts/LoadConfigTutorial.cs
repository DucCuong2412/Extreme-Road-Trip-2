using System.Collections;
using UnityEngine;

public class LoadConfigTutorial : LoadConfig
{
	public LoadConfigMenu.NextMenuPage NextMenuPage
	{
		get;
		private set;
	}

	public LoadConfigTutorial(LoadConfigMenu.NextMenuPage nextMenuPage)
	{
		NextMenuPage = nextMenuPage;
	}

	public override IEnumerator Load()
	{
		yield return null;
		UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
		yield return null;
	}
}
