using UnityEngine;

public class MetroWidgetNotificationMission : MetroWidgetNotification
{
	public static MetroWidgetNotificationMission Create(string text, string icon)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetNotificationMission).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetNotificationMission metroWidgetNotificationMission = gameObject.AddComponent<MetroWidgetNotificationMission>();
		metroWidgetNotificationMission.Setup(0.55f, 0.1f, text, icon, 1f, MetroAlign.Right);
		return metroWidgetNotificationMission;
	}

	protected override void AddCustomComponent()
	{
		MetroIcon metroIcon = MetroIcon.Create(Singleton<GameManager>.Instance.CarRef);
		metroIcon.SetScale(0.5f);
		metroIcon.SetAlignment(MetroAlign.Left);
		AddChild(metroIcon);
	}
}
