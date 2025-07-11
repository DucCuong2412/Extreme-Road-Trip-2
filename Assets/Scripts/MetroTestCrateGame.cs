public class MetroTestCrateGame : MetroMenuPage
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
		metroSpacer.Add(MetroWidgetCrate.Create(new Reward(RewardType.coins, 50), null));
		MetroSpacer metroSpacer2 = MetroSpacer.Create();
		metroSpacer2.AddSlice9Background(MetroSkin.Slice9PopupBackground);
		metroLayout2.Add(metroSpacer2);
		metroSpacer2.Add(MetroWidgetCrate.Create(new Reward(RewardType.bucks, 1), null));
		metroLayout2.Add(MetroSpacer.Create());
		MetroSpacer metroSpacer3 = MetroSpacer.Create();
		metroSpacer3.AddSlice9Background(MetroSkin.Slice9RedCircle);
		metroLayout2.Add(metroSpacer3);
		metroSpacer3.Add(MetroWidgetCrate.Create(new Reward(RewardType.boost, 5), null));
		MetroSpacer metroSpacer4 = MetroSpacer.Create();
		metroSpacer4.AddSlice9Background(MetroSkin.Slice9PopupBackground);
		metroLayout2.Add(metroSpacer4);
		metroSpacer4.Add(MetroWidgetCrate.Create(new Reward(RewardType.magnet, 5), null));
		metroLayout2.Add(MetroSpacer.Create());
		metroLayout.Add(MetroSpacer.Create());
		base.OnStart();
	}
}
