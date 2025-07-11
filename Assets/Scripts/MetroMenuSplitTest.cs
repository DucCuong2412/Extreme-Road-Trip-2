using UnityEngine;

public class MetroMenuSplitTest : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroButton child = MetroButton.Create("a");
		MetroButton child2 = MetroButton.Create("b");
		MetroButton child3 = MetroButton.Create("c");
		MetroButton child4 = MetroButton.Create("d");
		MetroButton metroButton = MetroButton.Create("e");
		metroButton.Mass = 3f;
		MetroButton child5 = MetroButton.Create("f");
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.Mass = 2f;
		metroLayout2.Add(child);
		metroLayout2.Add(child2);
		metroLayout2.Add(child3);
		metroLayout2.Add(child4);
		metroLayout3.Add(metroButton);
		metroLayout3.Add(child5);
		metroLayout.Add(metroLayout2);
		metroLayout.Add(metroLayout3);
		MetroButton[] componentsInChildren = metroLayout.GetComponentsInChildren<MetroButton>();
		foreach (MetroButton metroButton2 in componentsInChildren)
		{
			metroButton2.SetColor(new Color(0.1f, 0.1f, 0.1f));
			metroButton2.GetComponentInChildren<MetroLabel>().SetColor(Color.white);
		}
		Add(metroLayout);
		base.OnStart();
	}
}
