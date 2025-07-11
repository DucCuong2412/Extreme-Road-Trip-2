using UnityEngine;

public class MetroButtonMoreGames : MetroWidget
{
	public static MetroButtonMoreGames Create()
	{
		return CreateButton().Setup();
	}

	public static MetroButtonMoreGames CreateBanner()
	{
		return CreateButton().SetupBanner();
	}

	private static MetroButtonMoreGames CreateButton()
	{
		GameObject gameObject = new GameObject(typeof(MetroButtonMoreGames).ToString());
		gameObject.transform.position = Vector3.zero;
		return gameObject.AddComponent<MetroButtonMoreGames>();
	}

	private MetroButtonMoreGames Setup()
	{
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconMoreGames, "MORE GAMES");
		metroButton.OnButtonClicked += OnButtonClicked;
		Add(metroButton);
		return this;
	}

	private MetroButtonMoreGames SetupBanner()
	{
		MetroButton metroButton = MetroButton.Create();
		metroButton.OnButtonClicked += OnButtonClicked;
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		metroButton.Add(metroLayout);
		MetroIcon metroIcon = MetroIcon.Create(Object.Instantiate(Resources.Load("MoreGamesButton")) as GameObject);
		metroLayout.Add(metroIcon);
		MetroLabel metroLabel = MetroLabel.Create("MORE GAMES");
		metroLabel.SetOutlineColor(Color.black);
		metroLabel.SetColor(Color.black);
		metroIcon.Add(metroLabel);
		Add(metroButton);
		return this;
	}

	private void OnButtonClicked()
	{
		string text = "http://roofdog.ca";
		switch (Device.GetDeviceType())
		{
		case "ios":
			text = "https://itunes.apple.com/ca/developer/roofdog-games/id445060906";
			break;
		case "android":
			text = "https://play.google.com/store/apps/developer?id=Roofdog+Games";
			break;
		case "amazon":
			text = "https://www.amazon.com/s/?field-brandtextbin=Roofdog%20Games%20inc.&node=2350149011";
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			Application.OpenURL(text);
		}
	}
}
