using UnityEngine;

public class MetroWidgetNotificationMoney : MetroWidgetNotification
{
	public static MetroWidgetNotificationMoney Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetNotificationMoney).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetNotificationMoney metroWidgetNotificationMoney = gameObject.AddComponent<MetroWidgetNotificationMoney>();
		metroWidgetNotificationMoney.Setup(0.4f, 0.1f, string.Empty);
		return metroWidgetNotificationMoney;
	}

	protected override void AddCustomComponent()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		MetroWidgetBucks metroWidgetBucks = MetroWidgetBucks.Create();
		metroWidgetBucks.SetActive(active: false);
		metroWidgetBucks.SetAlignment(MetroAlign.Center);
		metroWidgetBucks.AddOutline();
		metroLayout.Add(metroWidgetBucks);
		MetroWidgetCoins metroWidgetCoins = MetroWidgetCoins.Create();
		metroWidgetCoins.SetActive(active: false);
		metroWidgetCoins.SetAlignment(MetroAlign.Center);
		metroWidgetCoins.AddOutline();
		metroLayout.Add(metroWidgetCoins);
		AddChild(metroLayout);
	}
}
