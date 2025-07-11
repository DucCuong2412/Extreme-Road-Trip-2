using UnityEngine;

public class MetroBadge : MetroWidget
{
	private MetroSpacer _container;

	public static MetroBadge Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroBadge).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroBadge metroBadge = gameObject.AddComponent<MetroBadge>();
		metroBadge.Setup();
		return metroBadge;
	}

	private void Setup()
	{
		_container = MetroSpacer.Create();
		Add(_container);
	}

	public override void Layout(Rect zone)
	{
		zone.x = -0.5f;
		zone.y = -0.5f;
		base.Layout(zone);
	}

	public void UpdateBadge(string badge, bool showIcon, bool useLarge = false)
	{
		_container.Clear();
		if (badge != string.Empty && showIcon)
		{
			MetroIcon metroIcon = MetroIcon.Create((!useLarge) ? MetroSkin.IconBadge : MetroSkin.IconBadgeLarge);
			_container.Add(metroIcon);
			MetroLabel metroLabel = MetroLabel.Create(badge);
			metroLabel.SetFont((!useLarge) ? MetroSkin.SmallFont : MetroSkin.VerySmallFont);
			metroIcon.Add(metroLabel);
		}
		_container.Reflow();
	}
}
