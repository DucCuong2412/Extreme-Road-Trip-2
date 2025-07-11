using System.Collections;
using UnityEngine;

public class WidgetLoaderContainer : MetroWidget
{
	private IWidgetLoader _widgetLoading;

	private bool _killCoroutine;

	public static WidgetLoaderContainer Create(IWidgetLoader streamedWidget, MetroWidget loading)
	{
		GameObject gameObject = new GameObject(typeof(WidgetLoaderContainer).ToString());
		return gameObject.AddComponent<WidgetLoaderContainer>().Setup(streamedWidget, loading);
	}

	private WidgetLoaderContainer Setup(IWidgetLoader streamedWidget, MetroWidget loading)
	{
		if (loading != null)
		{
			Add(loading);
		}
		_widgetLoading = streamedWidget;
		StartCoroutine(LoadCR());
		return this;
	}

	protected override void Cleanup()
	{
		base.Cleanup();
		_killCoroutine = true;
	}

	private IEnumerator LoadCR()
	{
		yield return null;
		while (!_widgetLoading.IsLoaded() && !_killCoroutine)
		{
			yield return null;
		}
		if (!_killCoroutine)
		{
			Clear();
			MetroWidget widget = _widgetLoading.GetLoadedWidget();
			Add(widget);
			Reflow();
			_widgetLoading = null;
		}
	}
}
