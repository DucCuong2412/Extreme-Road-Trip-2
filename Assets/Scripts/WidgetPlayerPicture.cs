using UnityEngine;

public class WidgetPlayerPicture : MetroWidget
{
	private float _padding;

	public static WidgetPlayerPicture Create(string id, float padding = 0f)
	{
		GameObject gameObject = new GameObject("PlayerPicture");
		gameObject.transform.position = Vector3.zero;
		WidgetPlayerPicture widgetPlayerPicture = gameObject.AddComponent<WidgetPlayerPicture>();
		widgetPlayerPicture.Setup(id, padding);
		return widgetPlayerPicture;
	}

	private WidgetPlayerPicture Setup(string name, float padding)
	{
		MetroPng metroPng = PictureManager.CreateMetroPicture(name);
		metroPng.SetStretch(MetroStretch.fullRatio);
		_padding = padding;
		Add(metroPng);
		return this;
	}

	public override void Layout(Rect zone)
	{
		Vector2 center = zone.center;
		float x = center.x;
		Vector2 center2 = zone.center;
		Vector3 localPosition = new Vector3(x, center2.y, -0.1f);
		base.transform.localPosition = localPosition;
		_zone = zone;
		float num = zone.width;
		float num2 = zone.height;
		if (num < num2)
		{
			num2 = num;
		}
		else
		{
			num = num2;
		}
		if (_padding > 0f && _padding < 1f)
		{
			num -= num * _padding;
			num2 = num;
		}
		float num3 = (0f - num) * 0.5f;
		float top = num3;
		_childsZone = new Rect(num3, top, num, num2);
		LayoutBackground();
		LayoutChilds();
	}
}
