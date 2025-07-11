public class MetroGlueTest : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroIcon child = MetroIcon.Create(MetroSkin.IconCoin);
		metroLayout2.Add(child);
		MetroLabel child2 = MetroLabel.Create("123");
		metroLayout2.Add(child2);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout3);
		MetroGlue metroGlue = MetroGlue.Create(Direction.horizontal);
		metroLayout3.Add(metroGlue);
		MetroIcon child3 = MetroIcon.Create(MetroSkin.IconCoin);
		metroGlue.Add(child3);
		MetroLabel child4 = MetroLabel.Create("222");
		metroGlue.Add(child4);
		metroGlue.Add(MetroIcon.Create(MetroSkin.IconCoin));
		MetroGlue metroGlue2 = MetroGlue.Create(Direction.vertical);
		metroLayout3.Add(metroGlue2);
		MetroIcon child5 = MetroIcon.Create(MetroSkin.IconCoin);
		metroGlue2.Add(child5);
		metroGlue2.Add(MetroIcon.Create(MetroSkin.IconCoin));
		metroGlue2.Add(MetroIcon.Create(MetroSkin.IconCoin));
		MetroLabel child6 = MetroLabel.Create("333");
		metroGlue2.Add(child6);
		Add(metroLayout);
		base.OnStart();
	}
}
