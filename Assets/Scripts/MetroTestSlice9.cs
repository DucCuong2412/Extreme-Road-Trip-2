public class MetroTestSlice9 : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		Add(metroLayout);
		MetroLabel child = MetroLabel.Create("SLICE 9 TEST");
		metroLayout.Add(child);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		metroLayout2.Add(MetroSpacer.Create());
		MetroSpacer metroSpacer = MetroSpacer.Create();
		metroSpacer.AddSlice9Background(MetroSkin.Slice9RedCircle);
		metroLayout2.Add(metroSpacer);
		MetroSpacer metroSpacer2 = MetroSpacer.Create();
		metroSpacer2.AddSlice9Background(MetroSkin.Slice9PopupBackground);
		metroLayout2.Add(metroSpacer2);
		metroLayout2.Add(MetroSpacer.Create());
		metroLayout.Add(MetroSpacer.Create());
		base.OnStart();
	}
}
