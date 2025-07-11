using UnityEngine;

public class MetroWidgetCredits : MetroWidget
{
	private const float _creditsScrollingSpeed = 2f;

	private MetroSlider _creditsSlider;

	private MetroWidget _sliderContent;

	private MetroWidget _textContainer;

	private float _creditsSliderMaxPos;

	private float _labelsHeight;

	public static MetroWidgetCredits Create(float parentHeight)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetCredits).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetCredits metroWidgetCredits = gameObject.AddComponent<MetroWidgetCredits>();
		metroWidgetCredits.Setup(parentHeight);
		return metroWidgetCredits;
	}

	private void Setup(float parentHeight)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		_textContainer = MetroLayout.Create(Direction.vertical);
		AddTitle("EXTREME ROAD TRIP 2");
		AddTitle("ROOFDOG GAMES");
		AddText("Steve Addison");
		AddText("Philippe Audet");
		AddText("Yanick Belanger");
		AddText("Sharyl Chow");
		AddText("Francois Desilets");
		AddText("Guillaume Germain");
		AddText("Etienne Giroux");
		AddText("Dominic Hamelin-Blais");
		AddText("Sylvain Laporte");
		AddText("Remi Lavoie");
		AddText("Charlotte Niedzviecki");
		AddText("Vincent Paquette");
		AddTitle("MUSIC");
		AddText("Jimmy \"Big Giant Circles\" Hinson");
		AddTitle("SPECIAL THANKS");
		AddText("Nebula Game Studios");
		AddText("François-Xavier Germain");
		AddText("Dorian Denes");
		AddEmptyLine();
		AddText("Copyright 2012 Roofdog Games inc. All Rights Reserved.");
		AddText("Extreme Road Trip 2 is Copyright 2012 Roofdog Games inc.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("Extreme Road Trip 2 is a work of fiction.");
		AddText("Don't repeat anything you've seen in this game with a real car.");
		AddEmptyLine();
		AddText("That would be dangerous, stupid and awesome to watch.");
		AddText("So, if you do, please film it and put it up on Youtube.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("That last paragraph was a joke.");
		AddText("Please ignore it if you're humorless or a lawyer.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("There's not much beyond here, you can go play the game.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("Well, if you insist, I guess I can tell you a story...");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("Wait... nevermind.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("Surprising fact: cars have feelings too.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("Another surprising fact: nobody was expected to make it\nthis far in the credits.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("One more surprising fact: my co-worker will be mad at me for\nhaving to localize this.");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("Still here?  OK, here's a challenge for you:");
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddEmptyLine();
		AddText("DON'T PRESS THE RED BUTTON!");
		AddEmptyLine();
		AddEmptyLine();
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(2f);
		metroLayout2.Add(MetroSpacer.Create(1.3f));
		int redButtonClicks = Preference.GetInt("_redButtonClicks");
		MetroButton metroButton = MetroButton.Create("PRESS ME");
		metroButton.AddSlice9Background(MetroSkin.Slice9RedCircle);
		metroButton.OnButtonClicked += delegate
		{
			Preference.SetInt("_redButtonClicks", redButtonClicks + 1);
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.main));
		};
		metroLayout2.Add(metroButton);
		metroLayout2.Add(MetroSpacer.Create(1.3f));
		_textContainer.Add(metroLayout2);
		AddEmptyLine();
		AddEmptyLine();
		AddText("You've pressed the red button ".Localize() + redButtonClicks + ((redButtonClicks <= 1) ? " time." : " times.").Localize());
		float labelsHeight = _labelsHeight;
		float num = labelsHeight + parentHeight * 2f;
		float onScreenCount = parentHeight / num;
		float mass = parentHeight / labelsHeight;
		_sliderContent = MetroLayout.Create(Direction.vertical);
		_sliderContent.Add(MetroSpacer.Create().SetMass(mass));
		_sliderContent.Add(_textContainer);
		_sliderContent.Add(MetroSpacer.Create().SetMass(mass));
		_creditsSlider = MetroSlider.Create(Direction.vertical, onScreenCount);
		_creditsSlider.Add(_sliderContent);
		metroLayout.Add(_creditsSlider);
		_creditsSliderMaxPos = num / 2f;
	}

	public void Update()
	{
		if (!AutoSingleton<InputManager>.Instance.IsTouching())
		{
			Vector3 position = _creditsSlider.transform.position;
			if (position.y < _creditsSliderMaxPos)
			{
				_creditsSlider.Translate(new Vector3(0f, 2f * Time.deltaTime, 0f));
			}
		}
	}

	private void AddTitle(string title)
	{
		if (_labelsHeight > 0f)
		{
			AddEmptyLine();
		}
		MetroLabel metroLabel = MetroLabel.Create(title);
		metroLabel.SetColor(Color.yellow);
		metroLabel.AddOutline();
		_textContainer.Add(metroLabel);
		_labelsHeight += metroLabel.GetLineHeight();
	}

	private void AddText(string text)
	{
		MetroLabel metroLabel = MetroLabel.Create(text).SetFont(MetroSkin.MediumFont);
		metroLabel.AddOutline();
		metroLabel.SetLineSpacing(0f);
		_textContainer.Add(metroLabel);
		_labelsHeight += metroLabel.GetLineHeight();
	}

	private void AddEmptyLine()
	{
		MetroLabel metroLabel = MetroLabel.Create(string.Empty);
		_textContainer.Add(metroLabel);
		_labelsHeight += metroLabel.GetLineHeight();
	}
}
