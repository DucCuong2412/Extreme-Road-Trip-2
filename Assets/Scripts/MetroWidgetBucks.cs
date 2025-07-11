using UnityEngine;

public class MetroWidgetBucks : MetroButton
{
	private MetroLabel _label;

	private int _bucks;

	private void Setup(int bucks = -1)
	{
		base.IsKeyNavigatorAccessible = false;
		base.OnButtonClicked += delegate
		{
			MetroMenuPage page = MetroMenuPage.Create<MetroMenuMoreCash>().Setup(Currency.bucks);
			AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.slideDown);
		};
		MetroGlue metroGlue = MetroGlue.Create("WidgetBucksGlue", Direction.horizontal);
		Add(metroGlue);
		MetroIcon child = MetroIcon.Create(MetroSkin.IconBucks);
		metroGlue.Add(child);
		_label = MetroLabel.Create("Bucks");
		metroGlue.Add(_label);
		if (bucks == -1)
		{
			bucks = AutoSingleton<Player>.Instance.Profile.Bucks;
		}
		SetBucks(bucks);
	}

	protected override void OnAwake()
	{
		AutoSingleton<CashManager>.Instance.OnBucksChanged += SetBucks;
		base.OnAwake();
	}

	public void OnDestroy()
	{
		if (AutoSingleton<CashManager>.IsCreated())
		{
			AutoSingleton<CashManager>.Instance.OnBucksChanged -= SetBucks;
		}
	}

	public void AddOutline()
	{
		_label.AddOutline();
	}

	public void SetBucks(int bucks)
	{
		if (bucks < _bucks)
		{
			PrefabSingleton<GameSoundManager>.Instance.PlayCashRegister();
		}
		_bucks = bucks;
		_label.SetText(_bucks.ToString());
	}

	public static MetroWidgetBucks Create(int bucks = -1)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetBucks).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetBucks metroWidgetBucks = gameObject.AddComponent<MetroWidgetBucks>();
		metroWidgetBucks.Setup(bucks);
		return metroWidgetBucks;
	}
}
