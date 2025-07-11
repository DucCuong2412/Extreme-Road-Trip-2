public class MetroMenuStats : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroWidget metroWidget = MetroWidgetStats.Create();
		metroWidget.SetMass(8f);
		metroLayout.Add(metroWidget);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(2f);
		metroLayout.Add(metroLayout2);
		metroLayout2.Add(MetroSpacer.Create(4f));
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconSkip, "BACK", MetroSkin.Slice9ButtonRed);
		metroLayout2.Add(metroButton);
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Replace(MetroMenuPage.Create<MetroMenuOptions>(), MetroAnimation.slideLeft);
		};
		MetroMenu.AddKeyNavigationBehaviour(metroButton, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
		base.OnStart();
	}
}
