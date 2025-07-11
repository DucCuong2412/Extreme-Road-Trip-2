public class MetroTestPopup : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel child = MetroLabel.Create("DO YOU WANT FREE MONEY?");
		metroLayout2.Add(child);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout3);
		MetroButton metroButton = MetroButton.Create("YES");
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<CashManager>.Instance.AddCoins(10000);
			AutoSingleton<MetroMenuStack>.Instance.Pop();
		};
		metroLayout3.Add(metroButton);
		MetroButton metroButton2 = MetroButton.Create("NOPE");
		metroButton2.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop();
		};
		metroLayout3.Add(metroButton2);
		base.OnStart();
	}
}
