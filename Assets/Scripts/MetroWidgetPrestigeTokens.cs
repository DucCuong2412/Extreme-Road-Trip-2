using System;
using UnityEngine;

public class MetroWidgetPrestigeTokens : MetroButton
{
	private MetroLabel _label;

	private int _prestigeTokens;

	private void Setup(int prestigeTokens = -1)
	{
		base.IsKeyNavigatorAccessible = false;
		if (prestigeTokens == -1)
		{
			prestigeTokens = AutoSingleton<Player>.Instance.Profile.PrestigeTokens;
		}
		base.OnButtonClicked += delegate
		{
			string titleString = "Prestige Tokens".Localize();
			string messageString = string.Format("You have {0} token(s). Complete 100% of the missions for any car to earn Prestige Tokens.".Localize(), _prestigeTokens);
			string buttonString = "OK".Localize();
			string slice9ButtonRed = MetroSkin.Slice9ButtonRed;
			Action buttonAction = delegate
			{
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
			MetroPopupMessage page = MetroMenuPage.Create<MetroPopupMessage>().Setup(titleString, messageString, buttonString, slice9ButtonRed, buttonAction);
			AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
		};
		MetroGlue metroGlue = MetroGlue.Create("WidgetPrestigeGlue", Direction.horizontal);
		Add(metroGlue);
		MetroIcon child = MetroIcon.Create(MetroSkin.IconPrestigeToken);
		metroGlue.Add(child);
		_label = MetroLabel.Create("Prestige Tokens");
		metroGlue.Add(_label);
		SetPrestigeTokens(prestigeTokens);
	}

	protected override void OnAwake()
	{
		AutoSingleton<CashManager>.Instance.OnPrestigeTokensChanged += SetPrestigeTokens;
		base.OnAwake();
	}

	public void OnDestroy()
	{
		if (AutoSingleton<CashManager>.IsCreated())
		{
			AutoSingleton<CashManager>.Instance.OnPrestigeTokensChanged -= SetPrestigeTokens;
		}
	}

	public void AddOutline()
	{
		_label.AddOutline();
	}

	public void SetPrestigeTokens(int prestigeTokens)
	{
		_prestigeTokens = prestigeTokens;
		_label.SetText(_prestigeTokens.ToString());
	}

	public static MetroWidgetPrestigeTokens Create(int bucks = -1)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetPrestigeTokens).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetPrestigeTokens metroWidgetPrestigeTokens = gameObject.AddComponent<MetroWidgetPrestigeTokens>();
		metroWidgetPrestigeTokens.Setup(bucks);
		return metroWidgetPrestigeTokens;
	}
}
