using UnityEngine;

public class MetroButtonShowroomCarCard : MetroButton
{
	private MetroLayout _layout;

	public static MetroButtonShowroomCarCard Create(Car car, float scale)
	{
		GameObject gameObject = new GameObject(car.Id);
		gameObject.transform.position = Vector3.zero;
		MetroButtonShowroomCarCard metroButtonShowroomCarCard = gameObject.AddComponent<MetroButtonShowroomCarCard>();
		metroButtonShowroomCarCard.Setup(car, scale);
		metroButtonShowroomCarCard.IsKeyNavigatorAccessible = true;
		return metroButtonShowroomCarCard;
	}

	public void SwitchSlice9Background(string slice9)
	{
		_layout.AddSlice9Background(slice9);
	}

	private void Setup(Car car, float scale)
	{
		CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(car);
		_layout = MetroLayout.Create(car.Id + "CardLayout", Direction.vertical);
		string carCardBackground = MetroButtonCarCard.GetCarCardBackground(car.Category);
		if (carCardBackground != string.Empty)
		{
			_layout.AddSlice9Background(carCardBackground);
		}
		Add(_layout);
		_layout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		_layout.Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLabel metroLabel = MetroLabel.Create($"#{car.Rank:000}");
		metroLabel.SetFont(MetroSkin.VerySmallFont);
		metroLabel.SetColor((car.Category != CarCategory.prestige) ? Color.black : Color.white);
		metroLayout.Add(metroLabel);
		MetroLabel metroLabel2 = MetroLabel.Create(car.DisplayName);
		metroLabel2.SetMass(4f);
		metroLabel2.SetFont(MetroSkin.VerySmallFont);
		metroLabel2.SetColor((car.Category != CarCategory.prestige) ? Color.black : Color.white);
		metroLayout.Add(metroLabel2);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(4f);
		_layout.Add(metroLayout2);
		MetroLayout child = MetroLayout.Create(Direction.vertical);
		metroLayout2.Add(child);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		metroLayout3.SetMass(2f);
		metroLayout2.Add(metroLayout3);
		MetroIcon metroIcon = MetroIcon.Create(car);
		metroIcon.SetMass(4f);
		metroIcon.SetScale(scale);
		metroIcon.SetAlignment(MetroAlign.Bottom);
		metroLayout3.Add(metroIcon);
		MetroWidgetCarUpgrade child2 = MetroWidgetCarUpgrade.Create(carProfile.GetUpgradeLevel(), 0.4f);
		metroLayout3.Add(child2);
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		MetroLayout child3 = MetroLayout.Create(Direction.vertical);
		metroLayout2.Add(child3);
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal);
		Add(metroWidget);
		MetroWidget metroWidget2 = MetroLayout.Create(Direction.vertical);
		MetroWidget child4 = MetroSpacer.Create(0.025f);
		MetroWidget child5 = MetroSpacer.Create(0.025f);
		MetroWidget child6 = MetroSpacer.Create(0.05f);
		MetroWidget child7 = MetroSpacer.Create(0.05f);
		metroWidget.Add(child4);
		metroWidget.Add(metroWidget2);
		metroWidget2.Add(child6);
		metroWidget2.Add(_layout);
		metroWidget2.Add(child7);
		metroWidget.Add(child5);
	}
}
