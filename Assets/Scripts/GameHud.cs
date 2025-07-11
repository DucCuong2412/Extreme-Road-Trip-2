using UnityEngine;

public class GameHud : MetroMenuPage
{
	private MetroLabel _distance;

	private MetroLabel _bestValue;

	private MetroLabel _coins;

	private MetroLabel _bucks;

	private MetroLabel _frenzyTimer;

	public GameObject _roundArrowLeft;

	public GameObject _roundArrowRight;

	private int _cachedBest;

	private void OnApplicationPause(bool pause)
	{
		if (pause && AutoSingleton<MetroMenuStack>.Instance.Peek() == this)
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroMenuPause>(), MetroAnimation.none);
		}
	}

	protected override void OnAwake()
	{
		base.OnAwake();
		if (AutoSingleton<PersistenceManager>.Instance.UseInvertedControl)
		{
			Anchor component = _roundArrowLeft.GetComponent<Anchor>();
			Anchor component2 = _roundArrowRight.GetComponent<Anchor>();
			component._horizontalAlignement = HorizontalAlignement.right;
			component2._horizontalAlignement = HorizontalAlignement.left;
		}
	}

	protected override void OnStart()
	{
		GameMode gameMode = GameMode.normal;
		LoadConfigGame loadConfigGame = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigGame;
		if (loadConfigGame != null)
		{
			gameMode = loadConfigGame.GameMode;
		}
		float alignmentOffset = -0.4f;
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		metroLayout.Add(metroLayout2);
		MetroButton metroButton = MetroButton.Create();
		metroButton.AddSpecialKey(KeyCode.Escape);
		metroButton.IsKeyNavigatorAccessible = false;
		MetroIcon metroIcon = MetroIcon.Create(MetroSkin.IconPause);
		metroIcon.SetScale(0.75f);
		metroIcon.SetAlignment(MetroAlign.Left);
		metroButton.Add(metroIcon);
		metroButton.OnButtonClicked += delegate
		{
			MetroMenuStack instance = AutoSingleton<MetroMenuStack>.Instance;
			if (instance != null && instance.Peek().GetType() != typeof(MetroMenuPause))
			{
				instance.Push(MetroMenuPage.Create<MetroMenuPause>(), MetroAnimation.none);
			}
		};
		metroLayout2.Add(metroButton);
		metroLayout2.Add(MetroSpacer.Create(9f));
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		metroLayout3.Mass = 6f;
		metroLayout.Add(metroLayout3);
		if (gameMode == GameMode.frenzy)
		{
			_frenzyTimer = MetroLabel.Create(TimeUtil.Format(35f));
			_frenzyTimer.SetFont(MetroSkin.BigFont);
			_frenzyTimer.AddOutline();
			metroLayout3.Add(_frenzyTimer);
		}
		metroLayout3.Add(MetroPowerupPurchase.Create().SetMass((!(_frenzyTimer == null)) ? 2f : 3f));
		metroLayout3.Add(MetroSpacer.Create(2f));
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
		metroLayout.Add(metroLayout4);
		_distance = MetroLabel.Create("Distance");
		_distance.SetAlignment(MetroAlign.Right);
		_distance.AlignmentOffset = alignmentOffset;
		_distance.SetMass(1.2f);
		_distance.SetFont(MetroSkin.BigFont);
		_distance.AddOutline();
		metroLayout4.Add(_distance);
		_bestValue = MetroLabel.Create("Best");
		_bestValue.SetFont(MetroSkin.SmallFont);
		_bestValue.AddOutline();
		_bestValue.SetAlignment(MetroAlign.Right);
		_bestValue.AlignmentOffset = alignmentOffset;
		metroLayout4.Add(_bestValue);
		MetroGlue metroGlue = MetroGlue.Create(Direction.horizontal);
		metroGlue.SetAlignment(MetroAlign.Right);
		metroGlue.AlignmentOffset = alignmentOffset;
		metroLayout4.Add(metroGlue);
		MetroIcon child = MetroIcon.Create(MetroSkin.IconCoin);
		metroGlue.Add(child);
		_coins = MetroLabel.Create("0");
		_coins.AddOutline();
		metroGlue.Add(_coins);
		if (gameMode == GameMode.frenzy)
		{
			MetroGlue metroGlue2 = MetroGlue.Create(Direction.horizontal);
			metroGlue2.SetAlignment(MetroAlign.Right);
			metroGlue2.AlignmentOffset = alignmentOffset;
			metroLayout4.Add(metroGlue2);
			MetroIcon child2 = MetroIcon.Create(MetroSkin.IconBucks);
			metroGlue2.Add(child2);
			_bucks = MetroLabel.Create("0");
			_bucks.AddOutline();
			metroGlue2.Add(_bucks);
		}
		metroLayout4.Add(MetroSpacer.Create((!(_bucks == null)) ? 12f : 13f));
		_cachedBest = AutoSingleton<GameStatsManager>.Instance.Overall.GetMaxDistance(Singleton<GameManager>.Instance.CarRef);
		ShowBest(_cachedBest);
		new GameObject("Stunt Hud").AddComponent<StuntHud>().Setup();
		base.OnStart();
	}

	private void ShowBest(int score)
	{
		_bestValue.SetColor(MetroSkin.TextNormalColor);
		_bestValue.SetText("BEST".Localize() + ": " + score.ToString() + "m");
	}

	protected override void OnMenuUpdate()
	{
		int num = Mathf.RoundToInt(AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetMaxDistance(Singleton<GameManager>.Instance.CarRef));
		_distance.SetText(num.ToString() + "m");
		if (num > _cachedBest)
		{
			_cachedBest = num;
			ShowBest(num);
		}
		_coins.SetText(AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetValue(Singleton<GameManager>.Instance.CarRef, GameStats.CarStats.Type.coinsPickedUp).ToString());
		if (_bucks != null)
		{
			_bucks.SetText(AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetValue(Singleton<GameManager>.Instance.CarRef, GameStats.CarStats.Type.bucksPickedUp).ToString());
		}
		if (_frenzyTimer != null)
		{
			float num2 = Mathf.Max(0f, 35f - Singleton<GameManager>.Instance.GetGameTimer());
			_frenzyTimer.SetText(TimeUtil.Format(num2));
			if (num2 <= 5f)
			{
				_frenzyTimer.SetColor(Color.red);
			}
			else if (num2 <= 15f)
			{
				_frenzyTimer.SetColor(Color.Lerp(Color.yellow, Color.red, Mathf.InverseLerp(15f, 5f, num2)));
			}
		}
	}

	protected override void ShowMask()
	{
		_mask = MetroQuad.Create(base.transform);
		_mask.transform.localPosition = new Vector3(0f, 0f, -5f);
		_mask.SetSize(_zone.width, _zone.height);
		_mask.SetColor(MetroSkin.DarkMask);
	}

	protected override void HideMask()
	{
		if (_mask != null)
		{
			UnityEngine.Object.Destroy(_mask.gameObject);
			_mask = null;
		}
	}
}
