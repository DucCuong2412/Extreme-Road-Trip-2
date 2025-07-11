using System.Collections;

public class MetroWidgetNotificationManager : AutoSingleton<MetroWidgetNotificationManager>
{
	private bool _isAnimationRunning;

	protected override void OnAwake()
	{
		_isAnimationRunning = false;
		base.OnAwake();
	}

	public void ShowMessage(string text)
	{
		ShowAchievement(text, MetroSkin.IconAchievements, 0.5f);
	}

	public void ShowMission(string text)
	{
		MetroMenuPage metroMenuPage = AutoSingleton<MetroMenuStack>.Instance.Peek();
		if (!(metroMenuPage != null) || metroMenuPage.GetType() != typeof(MetroMenuEndRun))
		{
			MetroWidgetNotification widget = MetroWidgetNotificationMission.Create(text, MetroSkin.StarWithGlow);
			Show(widget);
		}
	}

	public void ShowAchievement(string text, string icon, float iconScale = 0.8f)
	{
		MetroWidgetNotification widget = MetroWidgetNotification.Create(text, icon, iconScale);
		Show(widget);
	}

	public void ShowMoney()
	{
		MetroWidgetNotificationMoney widget = MetroWidgetNotificationMoney.Create();
		Show(widget);
	}

	private void Show(MetroWidgetNotification widget)
	{
		StartCoroutine(Animate(widget));
	}

	private IEnumerator Animate(MetroWidgetNotification widget)
	{
		while (_isAnimationRunning)
		{
			yield return null;
		}
		_isAnimationRunning = true;
		yield return StartCoroutine(widget.Animate());
		_isAnimationRunning = false;
		widget.Destroy();
	}
}
