using System.Collections;
using UnityEngine;

public class LoadConfigShowroom : LoadConfig
{
	public enum NextMenuPage
	{
		showroom,
		chooseShowroomCar,
		chooseShowroom
	}

	private NextMenuPage _nextMenuPage;

	public ShowroomSetup Setup
	{
		get;
		private set;
	}

	public int CarSlotIndex
	{
		get;
		private set;
	}

	public LoadConfigShowroom(ShowroomSetup setup)
	{
		Setup = setup;
		_nextMenuPage = NextMenuPage.showroom;
	}

	public LoadConfigShowroom(int carSlotIndex)
	{
		CarSlotIndex = carSlotIndex;
		_nextMenuPage = NextMenuPage.chooseShowroomCar;
	}

	public LoadConfigShowroom(NextMenuPage nextMenu)
	{
		_nextMenuPage = nextMenu;
	}

	public override IEnumerator Load()
	{
		yield return null;
		NextMenuPage nextMenuPage = _nextMenuPage;
		if (nextMenuPage == NextMenuPage.chooseShowroomCar || nextMenuPage == NextMenuPage.chooseShowroom)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Select");
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Showroom");
		}
		yield return null;
	}

	public override IEnumerator PostLoad()
	{
		switch (_nextMenuPage)
		{
		case NextMenuPage.showroom:
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuShowroom>(), MetroAnimation.none);
			break;
		case NextMenuPage.chooseShowroom:
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuChooseShowroom>(), MetroAnimation.none);
			break;
		default:
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuChooseShowroomCar>(), MetroAnimation.none);
			break;
		}
		AutoSingleton<FadingManager>.Instance.FadeIn();
		yield return new WaitForSeconds(0.3f);
	}

	public override IEnumerator Unload()
	{
		yield return null;
	}
}
