using Prime31;
using System;
using System.Collections;
using UnityEngine;

public class GameTwitterManager : AutoSingleton<GameTwitterManager>
{
	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Init();
		base.OnAwake();
	}

	private void Init()
	{
		TwitterAndroid.init("5FRKVWIB7z9DKVRUufhQ", "l0eGGmnUwWMhpXKJtwgbyRovp5nnwPQRJgJkwNhGiw");
	}

	public bool IsLoggedIn()
	{
		return TwitterAndroid.isLoggedIn();
	}

	public bool IsAvailable()
	{
		return true;
	}

	public void Tweet(string tweetText)
	{
		if (IsAvailable())
		{
			StartCoroutine(LoginAndExecute(delegate
			{
				ShowTweetConfirmationPopup(tweetText);
			}));
		}
	}

	public void Tweet(string tweetText, byte[] image)
	{
		if (IsAvailable())
		{
			StartCoroutine(LoginAndExecute(delegate
			{
				ShowTweetConfirmationPopup(tweetText, image);
			}));
		}
	}

	private void ShowTweetConfirmationPopup(string tweetText, byte[] image = null)
	{
		string titleString = "Twitter";
		string messageString = tweetText;
		string buttonString = "POST";
		Action buttonAction = delegate
		{
			if (image != null)
			{
				TwitterAndroid.postStatusUpdate(tweetText, image);
			}
			else
			{
				TwitterAndroid.postStatusUpdate(tweetText);
			}
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		};
		AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupMessage>().Setup(titleString, messageString, buttonString, MetroSkin.Slice9ButtonBlue, buttonAction), MetroAnimation.popup);
	}

	private void Login()
	{
		TwitterAndroid.showLoginDialog();
	}

	private IEnumerator LoginAndExecute(Action onLoginDone)
	{
		if (!IsLoggedIn())
		{
			Login();
			yield return StartCoroutine(WaitForLogin());
		}
		onLoginDone?.Invoke();
	}

	private IEnumerator WaitForLogin()
	{
		while (!IsLoggedIn())
		{
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void Logout()
	{
		TwitterAndroid.logout();
		Destroy();
	}
}
