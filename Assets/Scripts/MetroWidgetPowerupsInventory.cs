using UnityEngine;

public class MetroWidgetPowerupsInventory : MetroWidget
{
	public static MetroWidgetPowerupsInventory Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetPowerupsInventory).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetPowerupsInventory metroWidgetPowerupsInventory = gameObject.AddComponent<MetroWidgetPowerupsInventory>();
		metroWidgetPowerupsInventory.Setup();
		return metroWidgetPowerupsInventory;
	}

	private void Setup()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		metroLayout.AddSolidBackground(Color.black);
		metroLayout.SetPadding(0f, 0f);
		metroLayout.Add(PowerupLayout(MetroSkin.IconPowerupBoost, PowerupType.boost));
		metroLayout.Add(PowerupLayout(MetroSkin.IconPowerupCoinDoubler, PowerupType.coinDoubler));
		metroLayout.Add(PowerupLayout(MetroSkin.IconPowerupMagnet, PowerupType.magnet));
		Add(metroLayout);
	}

	private MetroLayout PowerupLayout(string icon, PowerupType type)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		MetroIcon metroIcon = MetroIcon.Create(icon);
		metroIcon.SetScale(0.4f);
		metroIcon.SetAlignment(MetroAlign.Right);
		metroLayout.Add(metroIcon);
		MetroLabel metroLabel = MetroLabel.Create(AutoSingleton<Player>.Instance.Profile.GetNumPowerups(type).ToString());
		metroLabel.SetAlignment(MetroAlign.Left);
		metroLayout.Add(metroLabel);
		return metroLayout;
	}
}
