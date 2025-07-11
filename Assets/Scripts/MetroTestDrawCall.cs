public class MetroTestDrawCall : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		for (int i = 0; i < 4; i++)
		{
			metroLayout.Add(MetroButtonCar.Create(AutoSingleton<CarDatabase>.Instance.GetAllCars()[i], 0.5f));
		}
		base.OnStart();
	}
}
