using System.Collections.Generic;

public class MetroViewportTest : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroButton child = MetroButton.Create("Top Left");
		metroLayout2.Add(child);
		MetroButton child2 = MetroButton.Create("Top Right");
		metroLayout2.Add(child2);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout3);
		MetroWidget child3 = MetroButton.Create("Left").SetFont(MetroSkin.SmallFont).SetMass(0.2f);
		metroLayout3.Add(child3);
		MetroViewport metroViewport = MetroViewport.Create(MetroSkin.ClippedGUILayer1);
		metroLayout3.Add(metroViewport);
		MetroSlider metroSlider = MetroSlider.Create(Direction.horizontal, 2.2f);
		metroSlider.SetMass(0.5f);
		metroViewport.Add(metroSlider);
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		foreach (Car item in allCars)
		{
			MetroButtonCar metroButtonCar = MetroButtonCar.Create(item, 0.5f);
			metroButtonCar.SetPadding(0f, 0.1f);
			metroSlider.Add(metroButtonCar);
		}
		metroViewport.Add(metroSlider);
		MetroWidget child4 = MetroButton.Create("Right").SetFont(MetroSkin.SmallFont).SetMass(0.2f);
		metroLayout3.Add(child4);
		MetroWidget child5 = MetroButton.Create("Bottom").SetMass(0.35f);
		metroLayout.Add(child5);
		base.OnStart();
	}
}
