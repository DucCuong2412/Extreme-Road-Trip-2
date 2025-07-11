using System.Collections;
using UnityEngine;

public class MetroWidgetNotification : MetroWidget
{
	private Vector3 _initialPos;

	private Vector3 _finalPos;

	private MetroLabel _label;

	public static MetroWidgetNotification Create(string text, string icon, float iconScale = 1f)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetNotification).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetNotification metroWidgetNotification = gameObject.AddComponent<MetroWidgetNotification>();
		metroWidgetNotification.Setup(0.55f, 0.1f, text, icon, iconScale, MetroAlign.Left);
		return metroWidgetNotification;
	}

	protected virtual void AddCustomComponent()
	{
	}

	public void Setup(float wRatio = 0.55f, float hRatio = 0.1f, string text = "", string icon = null, float iconScale = 0f, MetroAlign iconAlign = MetroAlign.Center)
	{
		_transform = base.transform;
		_transform.parent = PrefabSingleton<CameraGUI>.Instance.transform;
		AddSlice9Background(MetroSkin.Slice9RoundedSemiTransparent);
		_label = MetroLabel.Create(string.Empty);
		Add(_label);
		_label.SetText(text);
		_label.SetFont((!Device.IsIPad()) ? MetroSkin.SmallFont : MetroSkin.VerySmallFont);
		if (icon != null)
		{
			MetroIcon metroIcon = MetroIcon.Create(icon);
			metroIcon.SetAlignment(iconAlign);
			metroIcon.SetScale(iconScale);
			_label.Add(metroIcon);
		}
		AddCustomComponent();
		CameraGUI instance = PrefabSingleton<CameraGUI>.Instance;
		float num = instance.ScreenWidth * wRatio;
		float num2 = instance.ScreenHeight * hRatio;
		float left = 0f - num / 2f;
		float top = instance.HalfScreenHeight + num2;
		Rect zone = new Rect(left, top, num, num2);
		Layout(zone);
		float y = instance.HalfScreenHeight - num2 / 2f;
		Vector3 localPosition = _transform.localPosition;
		_initialPos = new Vector3(localPosition.x, localPosition.y, localPosition.z);
		_finalPos = new Vector3(localPosition.x, y, localPosition.z);
	}

	protected void AddChild(MetroWidget childWidget)
	{
		_label.Add(childWidget);
	}

	private IEnumerator LerpFromTo(Vector3 from, Vector3 to)
	{
		Duration delay = new Duration(0.7f);
		_transform.localPosition = from;
		while (!delay.IsDone())
		{
			float x = Mathfx.Hermite(0f, 1f, delay.Value01());
			_transform.localPosition = Vector3.Lerp(from, to, x);
			yield return null;
		}
		_transform.localPosition = to;
	}

	public IEnumerator Animate()
	{
		float widgetPosZ = GetPosZ();
		_initialPos = new Vector3(_initialPos.x, _initialPos.y, widgetPosZ);
		_finalPos = new Vector3(_finalPos.x, _finalPos.y, widgetPosZ);
		yield return StartCoroutine(LerpFromTo(_initialPos, _finalPos));
		yield return new WaitForSeconds(3f);
		yield return StartCoroutine(LerpFromTo(_finalPos, _initialPos));
	}

	private float GetPosZ()
	{
		Vector3 localPosition = _transform.localPosition;
		float result = localPosition.z;
		MetroMenuPage metroMenuPage = AutoSingleton<MetroMenuStack>.Instance.Peek();
		if (metroMenuPage != null && metroMenuPage.GetType() != typeof(MetroMenuPause))
		{
			result = metroMenuPage.GetZ() - 1f;
		}
		return result;
	}
}
