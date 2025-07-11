using System.Collections;
using UnityEngine;

public class WidgetLoadingWheel : MetroIcon
{
	public static WidgetLoadingWheel Create(float scale = 1f)
	{
		MetroIcon metroIcon = MetroIcon.Create(MetroSkin.Spinner);
		GameObject gameObject = new GameObject("WidgetLoadingWheel");
		metroIcon.transform.parent = gameObject.transform;
		metroIcon.transform.localPosition = Vector3.zero;
		metroIcon.name = "Pivot";
		WidgetLoadingWheel widgetLoadingWheel = gameObject.AddComponent<WidgetLoadingWheel>();
		widgetLoadingWheel._pivot = metroIcon.transform;
		widgetLoadingWheel._renderer = metroIcon.GetComponentInChildren<Renderer>();
		widgetLoadingWheel.SetScale(scale);
		return widgetLoadingWheel;
	}

	protected override void OnStart()
	{
		if (_pivot != null)
		{
			StartCoroutine(AnimLoadingCR());
		}
	}

	private IEnumerator AnimLoadingCR()
	{
		while (true)
		{
			_pivot.Rotate(0f, 0f, -5f);
			yield return null;
		}
	}

	public override MetroIcon SetStretch(MetroStretch stretch)
	{
		return (stretch == MetroStretch.none) ? base.SetStretch(stretch) : base.SetStretch(MetroStretch.fullRatio);
	}
}
