using UnityEngine;

public class MetroPanelStatsGlobal : MetroWidget
{
	private MetroWidget _slider;

	public static MetroPanelStatsGlobal Create()
	{
		GameObject gameObject = new GameObject("MetroPanelStatsGlobal");
		gameObject.transform.position = Vector3.zero;
		MetroPanelStatsGlobal metroPanelStatsGlobal = gameObject.AddComponent<MetroPanelStatsGlobal>();
		metroPanelStatsGlobal.Setup();
		return metroPanelStatsGlobal;
	}

	public void Setup()
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
		MetroLabel child = MetroLabel.Create("OVERALL");
		metroWidget2.Add(child);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroWidget.Add(metroLayout3);
		GameStats overall = AutoSingleton<GameStatsManager>.Instance.Overall;
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
		metroLayout3.Add(MetroSpacer.Create().SetMass(0.05f));
		metroLayout3.Add(metroLayout4);
		metroLayout3.Add(MetroSpacer.Create().SetMass(0.05f));
		int total = overall.GetTotal(GameStats.CarStats.Type.totalTimePlayed);
		metroLayout4.Add(AddStat("Time played:", TimeUtil.Format(total)));
		metroLayout4.Add(AddStat("Total distance:", overall.GetTotal(GameStats.CarStats.Type.totalDistance).ToString() + "m"));
		metroLayout4.Add(AddStat("Longest distance:", overall.GetMax(GameStats.CarStats.Type.maxDistance).ToString() + "m"));
		metroLayout4.Add(AddStat("Highest jump:", overall.GetMax(GameStats.CarStats.Type.maxHeight).ToString() + "m"));
		metroLayout4.Add(AddStat("Longest jump:", overall.GetMax(GameStats.CarStats.Type.maxJumpLength).ToString() + "m"));
		metroLayout4.Add(AddStat("Most flip in a jump:", overall.GetMax(GameStats.CarStats.Type.mostFlipOnJump).ToString()));
		metroLayout4.Add(AddStat("Most stunt in a jump:", overall.GetMax(GameStats.CarStats.Type.mostStuntOnJump).ToString()));
		metroLayout4.Add(AddStat("Number of races:", overall.GetTotal(GameStats.CarStats.Type.numberOfRace).ToString()));
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
