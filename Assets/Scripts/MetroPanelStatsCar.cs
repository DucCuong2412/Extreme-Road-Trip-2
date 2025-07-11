using UnityEngine;

public class MetroPanelStatsCar : MetroWidget
{
	private MetroWidget _slider;

	public static MetroPanelStatsCar Create(Car selectedCar)
	{
		GameObject gameObject = new GameObject("MetroPanelStatsCar");
		gameObject.transform.position = Vector3.zero;
		MetroPanelStatsCar metroPanelStatsCar = gameObject.AddComponent<MetroPanelStatsCar>();
		metroPanelStatsCar.Setup(selectedCar);
		return metroPanelStatsCar;
	}

	public void Setup(Car selectedCar)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		metroLayout.Add(metroLayout2);
		_slider = MetroSlider.Create(Direction.vertical, 1f);
		metroLayout2.Add(_slider);
		MetroWidget metroWidget = MetroLayout.Create(Direction.vertical).AddSolidBackground().SetColor(MetroSkin.OptionsBackgroundColor);
		_slider.Add(metroWidget);
		MetroWidget metroWidget2 = MetroLayout.Create(Direction.horizontal).AddSolidBackground().SetColor(MetroSkin.OptionsBackgroundColor)
			.SetMass(0.15f);
		metroWidget.Add(metroWidget2);
		MetroLabel child = MetroLabel.Create(selectedCar.DisplayName);
		metroWidget2.Add(child);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroWidget.Add(metroLayout3);
		MetroWidget metroWidget3 = MetroLayout.Create(Direction.vertical).SetColor(Color.grey);
		metroLayout3.Add(MetroSpacer.Create().SetMass(0.05f));
		metroLayout3.Add(metroWidget3);
		metroLayout3.Add(MetroSpacer.Create().SetMass(0.05f));
		AddStats(selectedCar, metroWidget3);
	}

	private void AddStats(Car selectedCar, MetroWidget parent)
	{
		GameStats overall = AutoSingleton<GameStatsManager>.Instance.Overall;
		float floatValue = overall.GetFloatValue(selectedCar, GameStats.CarStats.Type.totalTimePlayed);
		parent.Add(AddStat("Time played:", TimeUtil.Format(floatValue)));
		int value = overall.GetValue(selectedCar, GameStats.CarStats.Type.numberOfRace);
		parent.Add(AddStat("Total distance:", overall.GetValue(selectedCar, GameStats.CarStats.Type.totalDistance).ToString() + "m"));
		parent.Add(AddStat("Longest distance:", overall.GetValue(selectedCar, GameStats.CarStats.Type.maxDistance).ToString() + "m"));
		parent.Add(AddStat("Highest jump:", overall.GetValue(selectedCar, GameStats.CarStats.Type.maxHeight).ToString() + "m"));
		parent.Add(AddStat("Longest jump:", overall.GetValue(selectedCar, GameStats.CarStats.Type.maxJumpLength).ToString() + "m"));
		parent.Add(AddStat("Most flip in a jump:", overall.GetValue(selectedCar, GameStats.CarStats.Type.mostFlipOnJump).ToString()));
		parent.Add(AddStat("Most stunt in a jump:", overall.GetValue(selectedCar, GameStats.CarStats.Type.mostStuntOnJump).ToString()));
		parent.Add(AddStat("Number of races:", value.ToString()));
	}

	private MetroWidget AddStat(string label, string value)
	{
		MetroWidget metroWidget = CreateStatLine();
		MetroWidget metroWidget2 = CreateMetroLabel(label);
		metroWidget2.SetAlignment(MetroAlign.Left);
		metroWidget.Add(metroWidget2);
		MetroWidget metroWidget3 = CreateMetroLabel(value);
		metroWidget3.SetAlignment(MetroAlign.Right);
		metroWidget.Add(metroWidget3);
		return metroWidget;
	}

	private MetroWidget CreateStatLine()
	{
		return MetroLayout.Create(Direction.horizontal);
	}

	private MetroWidget CreateMetroLabel(string content)
	{
		return MetroLabel.Create(content).SetFont(MetroSkin.MediumFont);
	}
}
