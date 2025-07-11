using UnityEngine;

public class MetroStatusBar : MetroWidget
{
	private MetroWidget _powerupsInventory;

	public MetroWidgetXPBar XPBar
	{
		get;
		private set;
	}

	public MetroWidgetBucks Bucks
	{
		get;
		private set;
	}

	public MetroWidgetCoins Coins
	{
		get;
		private set;
	}

	public MetroWidgetPrestigeTokens PrestigeTokens
	{
		get;
		private set;
	}

	private void Setup(int bucks, int coins, int prestigeTokens, XPProfile xpProfile, bool showPowerupsInventory)
	{
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).SetPadding(0f).AddSolidBackground()
			.SetColor(MetroSkin.InformationAreaColor);
		Add(metroWidget);
		if (showPowerupsInventory)
		{
			_powerupsInventory = MetroWidgetPowerupsInventory.Create().SetMass(2.5f);
			metroWidget.Add(_powerupsInventory);
		}
		else
		{
			if (xpProfile == null)
			{
				xpProfile = AutoSingleton<Player>.Instance.Profile.XPProfile;
			}
			XPBar = MetroWidgetXPBar.Create(xpProfile);
			XPBar.SetMass(1.5f);
			metroWidget.Add(XPBar);
			metroWidget.Add(MetroSpacer.Create(1f));
		}
		Bucks = MetroWidgetBucks.Create(bucks);
		Bucks.SetAlignment(MetroAlign.Right);
		metroWidget.Add(Bucks);
		Coins = MetroWidgetCoins.Create(coins);
		Coins.SetAlignment(MetroAlign.Right);
		metroWidget.Add(Coins);
		PrestigeTokens = MetroWidgetPrestigeTokens.Create(prestigeTokens);
		PrestigeTokens.SetAlignment(MetroAlign.Right);
		metroWidget.Add(PrestigeTokens);
	}

	public static MetroStatusBar Create(int bucks = -1, int coins = -1, int prestigeTokens = -1, XPProfile xpProfile = null, bool showPowerupsInventory = false)
	{
		GameObject gameObject = new GameObject(typeof(MetroStatusBar).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroStatusBar metroStatusBar = gameObject.AddComponent<MetroStatusBar>();
		metroStatusBar.Setup(bucks, coins, prestigeTokens, xpProfile, showPowerupsInventory);
		return metroStatusBar;
	}

	public void RefreshPowerupsInventory()
	{
		if ((bool)_powerupsInventory)
		{
			MetroWidget metroWidget = MetroWidgetPowerupsInventory.Create().SetMass(2.5f);
			_powerupsInventory.Replace(metroWidget).Destroy();
			_powerupsInventory = metroWidget;
			_powerupsInventory.Parent.Reflow();
		}
	}
}
