using UnityEngine;

public class MetroPanelCarInfo : MetroWidget
{
	private Car _car;

	private MetroIcon _carIcon;

	public void Setup(Car car)
	{
		_car = car;
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroWidget metroWidget = MetroSpacer.Create().AddSolidBackground().SetColor(MetroSkin.DarkColor);
		metroLayout.Add(metroWidget);
		MetroLabel child = MetroLabel.Create(_car.DisplayName);
		metroWidget.Add(child);
		_carIcon = MetroIcon.Create(_car).SetScale(1.5f);
		_carIcon.SetMass(3f);
		metroLayout.Add(_carIcon);
	}

	public static MetroPanelCarInfo Create(Car car)
	{
		GameObject gameObject = new GameObject(typeof(MetroPanelCarInfo).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		MetroPanelCarInfo metroPanelCarInfo = gameObject.AddComponent<MetroPanelCarInfo>();
		metroPanelCarInfo.Setup(car);
		return metroPanelCarInfo;
	}
}
