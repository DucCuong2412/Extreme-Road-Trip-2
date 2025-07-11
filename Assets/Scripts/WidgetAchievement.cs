using System.Collections;
using UnityEngine;

public class WidgetAchievement : PrefabSingleton<WidgetAchievement>
{
	private float _screenHeight;

	private float _widgetHeight;

	private MetroLabel _label;

	private bool _animating;

	protected override void OnStart()
	{
		base.OnStart();
		_screenHeight = PrefabSingleton<CameraGUI>.Instance.camera.orthographicSize;
		Vector3 extents = RendererBounds.ComputeBounds(base.transform).extents;
		_widgetHeight = extents.y;
		base.transform.parent = PrefabSingleton<CameraGUI>.Instance.transform;
		SetPosition(_screenHeight + _widgetHeight);
		_label = GetComponentInChildren<MetroLabel>();
	}

	public void Show(string text)
	{
		StartCoroutine(DoYourThing(text));
	}

	private void SetPosition(float offset)
	{
		base.transform.localPosition = new Vector3(0f, offset, 1f);
	}

	private IEnumerator DoYourThing(string text)
	{
		yield return null;
		while (_animating)
		{
			yield return null;
		}
		_animating = true;
		_label.SetText(text);
		float from = _screenHeight + _widgetHeight + 0.5f;
		float to = _screenHeight - _widgetHeight;
		Duration delay2 = new Duration(0.4f);
		while (!delay2.IsDone())
		{
			SetPosition(Mathfx.Hermite(from, to, delay2.Value01()));
			yield return null;
		}
		SetPosition(to);
		yield return new WaitForSeconds(1f);
		delay2 = new Duration(0.4f);
		while (!delay2.IsDone())
		{
			SetPosition(Mathfx.Hermite(to, from, delay2.Value01()));
			yield return null;
		}
		SetPosition(from);
		_animating = false;
	}
}
