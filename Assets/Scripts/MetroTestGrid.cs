public class MetroTestGrid : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroGrid metroGrid = MetroGrid.Create(3, 2);
		Add(metroGrid);
		metroGrid.Add(MetroLabel.Create("A"));
		metroGrid.Add(MetroLabel.Create("B"));
		metroGrid.Add(MetroLabel.Create("C"));
		metroGrid.Add(MetroLabel.Create("D"));
		metroGrid.Add(MetroLabel.Create("E"));
		metroGrid.Add(MetroLabel.Create("F"));
		base.OnStart();
	}
}
