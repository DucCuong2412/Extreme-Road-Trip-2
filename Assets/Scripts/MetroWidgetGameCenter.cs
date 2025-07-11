using UnityEngine;

public class MetroWidgetGameCenter : MetroWidget
{
	private void Setup()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		Add(metroLayout);
		MetroIcon metroIcon = MetroIcon.Create(MetroSkin.IconGameCenter);
		metroIcon.SetScale(0.8f);
		metroLayout.Add(metroIcon);
		string content = AutoSingleton<BackendManager>.Instance.PlayerAlias();
		MetroLabel metroLabel = MetroLabel.Create(content);
		metroLabel.SetAlignment(MetroAlign.Left);
		metroLabel.SetMass(4f);
		metroLayout.Add(metroLabel);
	}

	public static MetroWidgetGameCenter Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetGameCenter).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetGameCenter metroWidgetGameCenter = gameObject.AddComponent<MetroWidgetGameCenter>();
		metroWidgetGameCenter.Setup();
		return metroWidgetGameCenter;
	}
}
