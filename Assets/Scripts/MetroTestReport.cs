using UnityEngine;

public class MetroTestReport : MetroMenuPage
{
	private Color _lightBlue = ColorUtil.Parse("#6699d3");

	private Color _darkBlue = ColorUtil.Parse("#04346c");

	private Color _orange = ColorUtil.Parse("#ff4c00");

	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.Add(MetroLabel.Create("13"));
		metroLayout2.Add(MetroLabel.Create("Pierre Lambert"));
		metroLayout2.SetColor(_darkBlue);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		metroLayout3.Add(MetroLabel.Create("Date: 29 mars"));
		metroLayout3.Add(MetroLabel.Create("Weight: 220lbs"));
		metroLayout3.Add(MetroLabel.Create("Height: 6'10''"));
		metroLayout2.Add(metroLayout3);
		metroLayout.Add(metroLayout2);
		MetroSlider metroSlider = MetroSlider.Create(Direction.vertical, 1f);
		for (int i = 0; i < 5; i++)
		{
			MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
			metroLayout4.Add(MetroLabel.Create("Test " + i.ToString()));
			for (int j = 0; j < 2; j++)
			{
				MetroLayout metroLayout5 = MetroLayout.Create(Direction.horizontal);
				metroLayout5.SetColor(_lightBlue);
				MetroLabel child = MetroLabel.Create("Exercise " + j.ToString());
				metroLayout5.Add(child);
				MetroSpacer metroSpacer = MetroSpacer.Create();
				metroSpacer.SetColor(_orange);
				metroSpacer.Mass = 4f;
				metroLayout5.Add(metroSpacer);
				MetroLayout metroLayout6 = MetroLayout.Create(Direction.vertical);
				metroLayout6.Add(MetroLabel.Create("Max: 123"));
				metroLayout6.Add(MetroLabel.Create("Avg: 69"));
				metroLayout6.Add(MetroLabel.Create("Min: 42"));
				metroLayout6.Mass = 2f;
				metroLayout5.Add(metroLayout6);
				metroLayout5.Mass = 4f;
				metroLayout4.Add(metroLayout5);
			}
			metroSlider.Add(metroLayout4);
		}
		metroSlider.Mass = 5f;
		metroLayout.Add(metroSlider);
		Add(metroLayout);
		base.OnStart();
	}
}
