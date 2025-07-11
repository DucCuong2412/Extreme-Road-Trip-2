using UnityEngine;

public class MetroWidgetCoins : MetroButton
{
	private MetroLabel _label;

	private int _coins;

	private void Setup(int coins = -1)
	{
		base.IsKeyNavigatorAccessible = false;
		base.OnButtonClicked += delegate
		{
			MetroMenuPage page = MetroMenuPage.Create<MetroMenuMoreCash>().Setup(Currency.coins);
			AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.slideDown);
		};
		MetroGlue metroGlue = MetroGlue.Create("WidgetCoinsGlue", Direction.horizontal);
		Add(metroGlue);
		MetroIcon child = MetroIcon.Create(MetroSkin.IconCoin);
		metroGlue.Add(child);
		_label = MetroLabel.Create("Coins");
		metroGlue.Add(_label);
		if (coins == -1)
		{
			coins = AutoSingleton<Player>.Instance.Profile.Coins;
		}
		SetCoins(coins);
	}

	protected override void OnAwake()
	{
		AutoSingleton<CashManager>.Instance.OnCoinsChanged += SetCoins;
		base.OnAwake();
	}

	public void OnDestroy()
	{
		if (AutoSingleton<CashManager>.IsCreated())
		{
			AutoSingleton<CashManager>.Instance.OnCoinsChanged -= SetCoins;
		}
	}

	public void AddOutline()
	{
		_label.AddOutline();
	}

	public void SetCoins(int coins)
	{
		if (coins < _coins)
		{
			PrefabSingleton<GameSoundManager>.Instance.PlayCashRegister();
		}
		_coins = coins;
		_label.SetText(_coins.ToString());
	}

	public static MetroWidgetCoins Create(int coins = -1)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetCoins).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetCoins metroWidgetCoins = gameObject.AddComponent<MetroWidgetCoins>();
		metroWidgetCoins.Setup(coins);
		return metroWidgetCoins;
	}
}
