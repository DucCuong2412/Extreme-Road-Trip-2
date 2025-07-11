using System.Collections;
using UnityEngine;

public class LoadConfigMenu : LoadConfig
{
	public enum NextMenuPage
	{
		main,
		options,
		chooseCar,
		store
	}

	private NextMenuPage _nextMenuPage;

	public LoadConfigMenu(NextMenuPage nextMenuPage)
	{
		_nextMenuPage = nextMenuPage;
	}

	public override IEnumerator Load()
	{
		yield return null;
		yield return null;
		switch (_nextMenuPage)
		{
		case NextMenuPage.chooseCar:
			UnityEngine.SceneManagement.SceneManager.LoadScene("Select");
			break;
		case NextMenuPage.store:
			UnityEngine.SceneManagement.SceneManager.LoadScene("Store");
			break;
		default:
			UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
			break;
		}
		yield return null;
	}

	public override IEnumerator PostLoad()
	{
		switch (_nextMenuPage)
		{
		case NextMenuPage.options:
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuOptions>(), MetroAnimation.none);
			break;
		case NextMenuPage.chooseCar:
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuChooseCar>(), MetroAnimation.none);
			break;
		case NextMenuPage.store:
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuStore>(), MetroAnimation.none);
			break;
		default:
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuMain>(), MetroAnimation.none);
			break;
		}
		AutoSingleton<FadingManager>.Instance.FadeIn();
		yield return new WaitForSeconds(0.3f);
	}
}
