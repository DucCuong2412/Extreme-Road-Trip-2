using Missions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroMenuEndRun : MetroMenuPage
{
	private const int MAX_POPUP_SEQUENCE_COUNT = 3;

	public Car _car;

	public int _distance;

	public int _best;

	public int _bucks;

	public int _coins;

	public int _stunts;

	public float _previousXP;

	public int _previousBucks;

	public int _previousCoins;

	public int _numTotalMissionsCompleted;

	public int _numMissionsCompleted;

	public List<Reward> _missionsRewards;

	public List<Mission> _previousMissions;

	public List<Mission> _nextMissions;

	private List<Mission> _newMissions;

	private EpicPowerup _epicPowerup;

	private bool _debugging = true;

	private bool _skipping;

	private bool _waitingForPopup;

	private bool _changingCar;

	private bool _prestigeTriggered;

	private int _nbPopupShown;

	private MetroButton _changeCarButton;

	private MetroButton _nextButton;

	private MetroIcon _nextButtonIcon;

	private MetroLabel _mainInfoCounter;

	private MetroLabel _bestCounter;

	private MetroLabel _coinsCounter;

	private MetroLabel _stuntsCounter;

	private MetroPanelCurrentMissions _missionsPanel;

	private MetroBadge _carUpgradeBadge;

	private VisualXPProfile _visualXP;

	private MetroStatusBar _statusBar;

	private bool _facebookInvitationChecked;

	private GameMode _gameMode;

	private string _infoSuffix;

	private bool _checkForBikeTripPromo = true;

	public void Setup(Car car, int distance, int best, int bucks, int coins, int stunts, float previousXP, int previousBucks, int previousCoins, int numTotalMissionsCompleted, int numMissionsCompleted, List<Reward> missionsRewards, List<Mission> previousMissions, List<Mission> nextMissions, EpicPowerup epicPowerup)
	{
		_car = car;
		_distance = distance;
		_best = best;
		_bucks = bucks;
		_coins = coins;
		_stunts = stunts;
		_previousXP = previousXP;
		_previousBucks = previousBucks;
		_previousCoins = previousCoins;
		_numTotalMissionsCompleted = numTotalMissionsCompleted;
		_numMissionsCompleted = numMissionsCompleted;
		_missionsRewards = missionsRewards;
		_previousMissions = previousMissions;
		_nextMissions = nextMissions;
		_epicPowerup = epicPowerup;
		_debugging = false;
		_skipping = false;
		_gameMode = ((AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigGame)?.GameMode ?? GameMode.normal);
		AutoSingleton<CashManager>.Instance.OnPrestigeTokensChanged += OnPrestigeChange;
		AutoSingleton<Rooflog>.Instance.LogEndRun(_gameMode, _coins, _bucks, _distance, car.Id);
	}

	protected override void OnStart()
	{
		_infoSuffix = ((_gameMode != 0) ? (" " + "Bucks".Localize()) : "m");
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		metroLayout.SetPadding(-10f);
		metroLayout.AddSolidBackground().SetColor(MetroSkin.SemiTransparent);
		Add(metroLayout);
		_visualXP = new VisualXPProfile(_previousXP);
		_statusBar = MetroStatusBar.Create(_previousBucks, _previousCoins, AutoSingleton<Player>.Instance.Profile.PrestigeTokens, _visualXP);
		_statusBar.XPBar.OnLevelUp += OnLevelUp;
		metroLayout.Add(_statusBar);
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).SetMass(5f);
		metroLayout.Add(metroWidget);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		metroWidget.Add(metroLayout2);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		metroWidget.Add(metroLayout3);
		metroLayout2.Add(MetroPanelLeaderboards.Create());
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
		metroLayout4.AddSlice9Background(MetroSkin.Slice9RoundedSemiTransparent);
		metroLayout3.Add(metroLayout4);
		MetroLayout metroLayout5 = MetroLayout.Create(Direction.horizontal);
		metroLayout4.Add(metroLayout5);
		MetroLayout metroLayout6 = MetroLayout.Create(Direction.vertical);
		metroLayout5.Add(metroLayout6);
		if (_gameMode == GameMode.frenzy)
		{
			MetroIcon metroIcon = MetroIcon.Create((AutoSingleton<LocalizationManager>.Instance.Language != 0) ? MetroSkin.IconFrenzyRunFrench : MetroSkin.IconFrenzyRunEnglish);
			metroIcon.SetScale(0.7f);
			metroLayout6.Add(metroIcon);
		}
		_mainInfoCounter = MetroLabel.Create(string.Empty);
		metroLayout6.Add(_mainInfoCounter);
		MetroLayout metroLayout7 = MetroLayout.Create(Direction.horizontal);
		metroLayout7.SetMass(0.5f);
		metroLayout6.Add(metroLayout7);
		metroLayout7.Add(MetroSpacer.Create());
		MetroLabel metroLabel = MetroLabel.Create("Best");
		metroLabel.SetFont(MetroSkin.SmallFont);
		metroLabel.SetColor(Color.yellow);
		metroLabel.AddOutline();
		metroLayout7.Add(metroLabel);
		metroLayout7.Add(MetroSpacer.Create(0.5f));
		_bestCounter = MetroLabel.Create(_best.ToString() + _infoSuffix);
		_bestCounter.SetFont(MetroSkin.SmallFont);
		metroLayout7.Add(_bestCounter);
		metroLayout7.Add(MetroSpacer.Create());
		MetroLayout metroLayout8 = MetroLayout.Create(Direction.vertical);
		metroLayout5.Add(metroLayout8);
		if (AutoSingleton<PlatformCapabilities>.Instance.UseSocialSharing())
		{
			metroLayout8.Add(CreateSocialSharingButton());
		}
		MetroLayout metroLayout9 = MetroLayout.Create(Direction.horizontal);
		metroLayout8.Add(metroLayout9);
		if (AutoSingleton<PlatformCapabilities>.Instance.UseGameCenterChallenge())
		{
			MetroButton metroButton = CreateChallengeButton();
			metroButton.SetMass(3f);
			metroLayout9.Add(metroButton);
		}
		if (_gameMode == GameMode.normal && AutoSingleton<PlatformCapabilities>.Instance.IsGameCenterSupported())
		{
			MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconGameCenter, null);
			metroLayout9.Add(metroButton2);
			metroButton2.OnButtonClicked += delegate
			{
				if (AutoSingleton<PlatformCapabilities>.Instance.IsShowFullGameCenterScreenSupported())
				{
					AutoSingleton<AchievementsManager>.Instance.ShowAchievements();
				}
				else
				{
					AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupGameCenter>().Setup(), MetroAnimation.popup);
				}
			};
		}
		MetroLayout metroLayout10 = MetroLayout.Create(Direction.horizontal);
		metroLayout10.SetMass(0.25f);
		metroLayout4.Add(metroLayout10);
		metroLayout10.Add(MetroSpacer.Create());
		_coinsCounter = MetroLabel.Create(string.Empty);
		_coinsCounter.SetFont(MetroSkin.SmallFont);
		metroLayout10.Add(_coinsCounter);
		metroLayout10.Add(MetroSpacer.Create(0.5f));
		MetroLabel metroLabel2 = MetroLabel.Create("Coins");
		metroLabel2.SetFont(MetroSkin.SmallFont);
		metroLabel2.SetColor(Color.yellow);
		metroLabel2.AddOutline();
		metroLayout10.Add(metroLabel2);
		metroLayout10.Add(MetroSpacer.Create());
		_stuntsCounter = MetroLabel.Create(string.Empty);
		_stuntsCounter.SetFont(MetroSkin.SmallFont);
		metroLayout10.Add(_stuntsCounter);
		metroLayout10.Add(MetroSpacer.Create(0.5f));
		MetroLabel metroLabel3 = MetroLabel.Create("Stunts");
		metroLabel3.SetFont(MetroSkin.SmallFont);
		metroLabel3.SetColor(Color.yellow);
		metroLabel3.AddOutline();
		metroLayout10.Add(metroLabel3);
		metroLayout10.Add(MetroSpacer.Create());
		if (_gameMode == GameMode.normal)
		{
			MetroLayout metroLayout11 = MetroLayout.Create(Direction.vertical);
			metroLayout11.AddSlice9Background(MetroSkin.Slice9RoundedSemiTransparent);
			metroLayout3.Add(metroLayout11);
			HandleDebugMissions();
			if (_nextMissions != null)
			{
				_newMissions = _nextMissions.FindAll((Mission m) => !_previousMissions.Contains(m));
			}
			_missionsPanel = MetroPanelCurrentMissions.Create(_car, _previousMissions, _numTotalMissionsCompleted - _numMissionsCompleted, showtimeMode: true);
			_missionsPanel.OnProgressFull += OnMissionsProgressFull;
			_missionsPanel.OnAnimationEnd += OnMissionsAnimationEnd;
			_missionsPanel.SetMass(3f);
			metroLayout11.Add(_missionsPanel);
		}
		MetroLayout metroLayout12 = MetroLayout.Create(Direction.horizontal);
		metroLayout12.SetMass(2f);
		metroLayout.Add(metroLayout12);
		MetroButtonMoreGames child = MetroButtonMoreGames.Create();
		metroLayout12.Add(child);
		_changeCarButton = ((_gameMode != 0) ? MetroSkin.CreateMenuButton(MetroSkin.IconBack, "BACK") : MetroSkin.CreateMenuButton(MetroSkin.IconChangeCar, "CHANGE CAR"));
		_changeCarButton.OnButtonClicked += ChangingCarFunc;
		metroLayout12.Add(_changeCarButton);
		if (_gameMode == GameMode.normal)
		{
			MetroButton metroButton3 = MetroSkin.CreateMenuButton(MetroSkin.IconUpgradeCar, "UPGRADE CAR", null, 0.8f);
			metroButton3.OnButtonClicked += delegate
			{
				EnqueuePopup(MetroMenuPage.Create<MetroPopupUpgradeCar>().Setup(_car, OnDismissedPrestigeRewardPopup));
			};
			_carUpgradeBadge = MetroBadge.Create();
			metroButton3.Add(_carUpgradeBadge);
			UpdateCarUpgradeBadge();
			metroLayout12.Add(metroButton3);
		}
		MetroButton endRunMultiplayerButton = BikeTripPromoUIManager.GetEndRunMultiplayerButton();
		if (endRunMultiplayerButton != null)
		{
			metroLayout12.Add(endRunMultiplayerButton);
		}
		_nextButton = MetroSkin.CreateMenuButton(MetroSkin.IconSkip, "SKIP", MetroSkin.Slice9ButtonRed);
		_nextButtonIcon = _nextButton.GetComponentInChildren<MetroIcon>();
		_nextButton.OnButtonClicked += SkipFunc;
		metroLayout12.Add(_nextButton);
		if (_epicPowerup == null && _debugging)
		{
			_epicPowerup = new TransportEpicPowerup(new Price(10000, Currency.coins));
		}
		StartCoroutine(ShowDataCR());
		base.OnStart();
	}

	private MetroButton CreateSocialSharingButton()
	{
		MetroButton metroButton = MetroButton.Create();
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonBlue);
		metroButton.OnButtonClicked += delegate
		{
			bool flag = AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported();
			bool flag2 = AutoSingleton<PlatformCapabilities>.Instance.IsTwitterSupported();
			Action action = delegate
			{
				SocialHelper.ShareBestRunScore(SocialNetwork.facebook, _car, _bucks, _coins, _gameMode);
			};
			Action action2 = delegate
			{
				SocialHelper.ShareBestRunScore(SocialNetwork.twitter, _car, _bucks, _coins, _gameMode);
			};
			if (flag && flag2)
			{
				AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupSocialSharing>().Setup(action, action2), MetroAnimation.popup);
			}
			else if (flag)
			{
				action();
			}
			else if (flag2)
			{
				action2();
			}
		};
		MetroMenu.AddKeyNavigationBehaviour(metroButton, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9ButtonBlue);
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		metroButton.Add(metroLayout);
		MetroIcon metroIcon = MetroIcon.Create(MetroSkin.IconShare);
		metroIcon.SetMass(0.5f);
		metroIcon.SetScale(0.8f);
		metroIcon.SetAlignment(MetroAlign.Right);
		metroLayout.Add(metroIcon);
		MetroLabel metroLabel = MetroLabel.Create("SHARE");
		metroLabel.SetFont(MetroSkin.SmallFont);
		metroLabel.SetColor(Color.white);
		metroLayout.Add(metroLabel);
		return metroButton;
	}

	private MetroButton CreateChallengeButton()
	{
		return MetroButton.Create();
	}

	private void HandleDebugMissions()
	{
		if (_debugging)
		{
			_previousMissions = new List<Mission>();
			TravelMission travelMission = new TravelMission();
			travelMission.Completed = false;
			travelMission.Id = "1";
			travelMission.Objective = 1f;
			travelMission.Description = "Test mission 1";
			_previousMissions.Add(travelMission);
			TravelMission travelMission2 = new TravelMission();
			travelMission2.Completed = true;
			travelMission2.Id = "2";
			travelMission2.Objective = 1f;
			travelMission2.Description = "Test mission 2";
			_previousMissions.Add(travelMission2);
			TravelMission travelMission3 = new TravelMission();
			travelMission3.Completed = true;
			travelMission3.Id = "3";
			travelMission3.Objective = 1f;
			travelMission3.Description = "Test mission 3";
			_previousMissions.Add(travelMission3);
			_nextMissions = new List<Mission>();
			_nextMissions.Add(travelMission);
			TravelMission travelMission4 = new TravelMission();
			travelMission4.Completed = false;
			travelMission4.Id = "4";
			travelMission4.Objective = 1f;
			travelMission4.Description = "NEW Test mission 4";
			_nextMissions.Add(travelMission4);
			TravelMission travelMission5 = new TravelMission();
			travelMission5.Completed = false;
			travelMission5.Id = "5";
			travelMission5.Objective = 1f;
			travelMission5.Description = "NEW Test mission 5";
			_nextMissions.Add(travelMission5);
			_missionsRewards = new List<Reward>();
			_missionsRewards = AutoSingleton<MissionRewardsManager>.Instance.GetReward(_numTotalMissionsCompleted, _numMissionsCompleted);
		}
	}

	private void SkipFunc()
	{
		_skipping = true;
	}

	private void RetryFunc()
	{
		if (_gameMode == GameMode.frenzy)
		{
			AutoSingleton<FrenzyModeManager>.Instance.HandleAccess();
		}
		else
		{
			AutoSingleton<LoadingManager>.Instance.Reload();
		}
	}

	private void ChangingCarFunc()
	{
		_skipping = true;
		_changingCar = true;
	}

	private void LeavingFunc()
	{
		CleanupLoopingStuff();
		AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.chooseCar));
	}

	private void OnLevelUp(int level)
	{
		_waitingForPopup = true;
		PrefabSingleton<GameMusicManager>.Instance.PlayLevelUpMusic();
		MetroPopupRewards popup = MetroMenuPage.Create<MetroPopupRewards>().Setup("LEVEL UP!", string.Format("You are now level {0}".Localize(), level), XPProfile.GetRewards(level), OnLevelUpPopupDismissed, MetroSkin.IconFlag);
		EnqueuePopup(popup);
		_nbPopupShown++;
	}

	private void OnLevelUpPopupDismissed()
	{
		DequeuePopup();
		PrefabSingleton<GameMusicManager>.Instance.PlayGameMusic();
		List<Reward> rewards = XPProfile.GetRewards(_visualXP.Level);
		foreach (Reward item in rewards)
		{
			if (item.GetRewardType() == RewardType.bucks)
			{
				_previousBucks += item.Amount;
			}
			if (item.GetRewardType() == RewardType.coins)
			{
				_previousCoins += item.Amount;
			}
		}
		_statusBar.Bucks.SetBucks(_previousBucks);
		_statusBar.Coins.SetCoins(_previousCoins);
		_statusBar.XPBar.ResumeAnim();
		_waitingForPopup = false;
	}

	private void OnMissionsProgressFull()
	{
		_waitingForPopup = true;
		MetroPopupRewards popup = MetroMenuPage.Create<MetroPopupRewards>().Setup(5.ToString() + " " + "STARS!".Localize(), string.Empty, _missionsRewards, OnMissionsProgressFullPopupDismissed, MetroSkin.IconMissionStar);
		EnqueuePopup(popup);
		_nbPopupShown++;
	}

	private void OnMissionsProgressFullPopupDismissed()
	{
		DequeuePopup();
		if (_missionsRewards != null)
		{
			foreach (Reward missionsReward in _missionsRewards)
			{
				if (missionsReward.GetRewardType() == RewardType.bucks)
				{
					_previousBucks += missionsReward.Amount;
				}
				if (missionsReward.GetRewardType() == RewardType.coins)
				{
					_previousCoins += missionsReward.Amount;
				}
			}
		}
		_statusBar.Bucks.SetBucks(_previousBucks);
		_statusBar.Coins.SetCoins(_previousCoins);
		_missionsPanel.ResumeAnim();
		_waitingForPopup = false;
	}

	private IEnumerator ShowMissionsAnimation(List<Mission> newMissions)
	{
		if (_missionsPanel != null)
		{
			_missionsPanel.StartAnim(_car, newMissions);
			while (_missionsPanel.IsAnimating && !_skipping)
			{
				yield return null;
			}
			_missionsPanel.StopAnim();
		}
	}

	private void OnMissionsAnimationEnd()
	{
		if (AutoSingleton<PrestigeManager>.Instance.IsCarPrestigeAvailable(_car))
		{
			string str = "You've completed 100% of the missions for this car. You can now perform a Prestige to earn an exclusive badge and prestige tokens. ".Localize();
			str = ((!AutoSingleton<PrestigeManager>.Instance.IsNextPrestigeLevelMax(_car)) ? (str + "Performing a Prestige will reset your missions completion for this car to 0%.".Localize() + " " + "Complete them over again to earn more exclusive items.".Localize()) : (str + "This is the last Prestige level for this car. Your missions completion percentage will remain at 100%.".Localize()));
			MetroMenuPopupYesNoLater metroMenuPopupYesNoLater = MetroMenuPage.Create<MetroMenuPopupYesNoLater>().Setup("ALL MISSIONS COMPLETED", str, "PRESTIGE", "CANCEL", MetroSkin.Slice9ButtonRed, MetroSkin.SmallFont, null, 50);
			metroMenuPopupYesNoLater.OnButtonYes(delegate
			{
				DequeuePopup();
				AutoSingleton<PrestigeManager>.Instance.LogPrestige(PrestigeManager.LogPrestigeEventName.popupPreConfPrestige, accept: true, GetType().ToString(), _car);
				AutoSingleton<PrestigeManager>.Instance.ShowPrestigeTokenPopupSequence(_car, OnDismissedPrestigeRewardPopup, GetType().ToString());
			});
			metroMenuPopupYesNoLater.OnButtonNo(delegate
			{
				AutoSingleton<PrestigeManager>.Instance.LogPrestige(PrestigeManager.LogPrestigeEventName.popupPreConfPrestige, accept: false, GetType().ToString(), _car);
				DequeuePopup();
			});
			metroMenuPopupYesNoLater.OnClose(delegate
			{
				AutoSingleton<PrestigeManager>.Instance.LogPrestige(PrestigeManager.LogPrestigeEventName.popupPreConfPrestige, accept: false, GetType().ToString(), _car);
				DequeuePopup();
			});
			EnqueuePopup(metroMenuPopupYesNoLater);
			_nbPopupShown++;
		}
	}

	private void OnPrestigeChange(int amount)
	{
		_prestigeTriggered = true;
	}

	private void OnDismissedPrestigeRewardPopup()
	{
		DequeuePopup();
		if (_prestigeTriggered)
		{
			_prestigeTriggered = false;
			_newMissions.Clear();
			AutoSingleton<MissionManager>.Instance.GetMissions(_car).ForEach(delegate(Mission m)
			{
				_newMissions.Add(m);
			});
			_missionsPanel.OnPrestigeActivated(_car, _newMissions.Count);
			StartCoroutine(ShowMissionsAnimation(_newMissions));
		}
	}

	private IEnumerator ShowDataCR()
	{
		yield return new WaitForSeconds(MetroSkin.MenuPageBerpDuration);
		int mainInfo = (_gameMode != 0) ? _bucks : _distance;
		Duration delay = new Duration((mainInfo <= _best) ? 0.5f : 1f);
		if (!_skipping)
		{
			PrefabSingleton<GameSoundManager>.Instance.StartLoopingSound(GameSoundManager.LoopingSound.Beep);
		}
		bool once = false;
		while (!delay.IsDone() && !_skipping)
		{
			int v3 = Mathf.RoundToInt(Mathf.Lerp(0f, mainInfo, delay.Value01()));
			_mainInfoCounter.SetText(v3.ToString() + _infoSuffix);
			if (v3 > _best)
			{
				if (!once)
				{
					once = true;
					PrefabSingleton<GameSoundManager>.Instance.PlayCrowdSound();
					StartCoroutine(NewRecordStampCR());
				}
				PrefabSingleton<GameSpecialFXManager>.Instance.PlayGUISparksFX(_bestCounter.transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-0.5f, 0.5f), -1f));
				_bestCounter.SetText(v3.ToString() + _infoSuffix);
				_bestCounter.SetColor(MetroSkin.TextAlertColor);
				_mainInfoCounter.SetColor(MetroSkin.TextAlertColor);
			}
			yield return null;
		}
		_mainInfoCounter.SetText(mainInfo + _infoSuffix);
		_bestCounter.SetText(Mathf.Max(mainInfo, _best) + _infoSuffix);
		PrefabSingleton<GameSoundManager>.Instance.StopLoopingSound();
		Duration delay3 = new Duration(0.5f);
		if (!_skipping && _coins > 0)
		{
			PrefabSingleton<GameSoundManager>.Instance.StartLoopingSound(GameSoundManager.LoopingSound.Coin);
			PrefabSingleton<GameSpecialFXManager>.Instance.StartGUICoinShowerFX(_coinsCounter.transform.position + Vector3.back);
		}
		while (!delay3.IsDone() && !_skipping && _coins > 0)
		{
			int v2 = Mathf.RoundToInt(Mathf.Lerp(0f, _coins, delay3.Value01()));
			_coinsCounter.SetText(v2.ToString());
			yield return null;
		}
		_coinsCounter.SetText(_coins.ToString());
		PrefabSingleton<GameSpecialFXManager>.Instance.StopGUICoinShowerFX();
		PrefabSingleton<GameSoundManager>.Instance.StopLoopingSound();
		if (!_skipping && _coins > 0)
		{
			yield return new WaitForSeconds(0.2f);
		}
		Duration delay2 = new Duration(0.5f);
		if (!_skipping && _stunts > 0)
		{
			PrefabSingleton<GameSoundManager>.Instance.StartLoopingSound(GameSoundManager.LoopingSound.Flash);
		}
		while (!delay2.IsDone() && !_skipping && _stunts > 0)
		{
			int v = Mathf.RoundToInt(Mathf.Lerp(0f, _stunts, delay2.Value01()));
			_stuntsCounter.SetText(v.ToString());
			float angle = UnityEngine.Random.Range(0f, 360f);
			float offset = UnityEngine.Random.Range(0.5f, 1f);
			float x = Mathf.Cos(angle * ((float)Math.PI / 180f)) * offset;
			float y = Mathf.Sin(angle * ((float)Math.PI / 180f)) * offset;
			PrefabSingleton<GameSpecialFXManager>.Instance.PlayGUISparksFX(_stuntsCounter.transform.position + new Vector3(x, y, -1f));
			yield return null;
		}
		_stuntsCounter.SetText(_stunts.ToString());
		PrefabSingleton<GameSoundManager>.Instance.StopLoopingSound();
		yield return StartCoroutine(XPProgressCR((float)_distance * 0.1f + (float)_stunts * 5f));
		yield return StartCoroutine(ShowMissionsAnimation(_newMissions));
		while (AutoSingleton<MetroMenuStack>.Instance.HasPendingPopup())
		{
			yield return null;
		}
		if (_epicPowerup != null)
		{
			MetroPopupEpicPowerup popup = MetroMenuPage.Create<MetroPopupEpicPowerup>().Setup(_epicPowerup, DequeuePopup, RetryFunc);
			EnqueuePopup(popup);
			_nbPopupShown++;
		}
		while (AutoSingleton<MetroMenuStack>.Instance.HasPendingPopup())
		{
			yield return null;
		}
		AutoSingleton<GameAdProvider>.Instance.MaybeShowInterstitialEndRun();
		_nextButton.SetText("RETRY");
		MetroIcon icon = MetroIcon.Create(MetroSkin.IconPlay);
		icon.SetScale(0.7f);
		_nextButtonIcon.Replace(icon).Destroy();
		_nextButtonIcon = icon;
		icon.Parent.Reflow();
		_nextButton.OnButtonClicked -= SkipFunc;
		_nextButton.OnButtonClicked += RetryFunc;
		UpdateNextFrenzyButtonText();
		while (_waitingForPopup || AutoSingleton<MetroMenuStack>.Instance.HasPendingPopup())
		{
			yield return null;
		}
		_changeCarButton.OnButtonClicked -= ChangingCarFunc;
		_changeCarButton.OnButtonClicked += LeavingFunc;
		if (_changingCar)
		{
			LeavingFunc();
		}
	}

	private IEnumerator XPProgressCR(float xp)
	{
		int startLevel = _visualXP.Level;
		float startProg = _visualXP.GetProgress01();
		_visualXP.RegisterXP(xp);
		int endLevel = _visualXP.Level;
		float endProg = _visualXP.GetProgress01();
		MetroWidgetProgressBar.ProgressBarColorPalette colors = new MetroWidgetProgressBar.ProgressBarColorPalette(MetroSkin.XPWidgetBackColor, MetroSkin.XPWidgetBarGrindColor1, MetroSkin.XPWidgetBarGrindColor2);
		_statusBar.XPBar.StartAnim(string.Empty, startLevel, startProg, endLevel, endProg, colors);
		while (_statusBar.XPBar.IsAnimating && !_skipping)
		{
			yield return null;
		}
		_statusBar.XPBar.StopAnim();
	}

	private IEnumerator NewRecordStampCR()
	{
		if (_distance <= _best)
		{
			yield break;
		}
		if (!_skipping)
		{
			yield return new WaitForSeconds(0.2f);
		}
		Vector3 position = _mainInfoCounter.transform.position;
		Transform pivot = new GameObject("New Record Pivot").transform;
		MetroLabel record = MetroLabel.Create("New\nRecord!");
		record.transform.parent = pivot;
		record.transform.localPosition = Vector3.zero;
		pivot.position = position + Vector3.back;
		pivot.rotation = Quaternion.Euler(0f, 0f, 10f);
		pivot.parent = base.transform;
		float fromScale = 2f;
		float toScale = 1f;
		float time = 0.2f;
		Duration wait = new Duration(time);
		bool stomped = false;
		while (!wait.IsDone() && !_skipping)
		{
			float scale = Mathfx.Berp(fromScale, toScale, wait.Value01());
			pivot.localScale = Vector3.one * scale;
			if (!stomped && wait.Value01() > 0.5f)
			{
				PrefabSingleton<GameSoundManager>.Instance.PlayStompSound();
				PrefabSingleton<CameraGUI>.Instance.Shake();
				stomped = true;
			}
			yield return null;
		}
		pivot.localScale = Vector3.one * toScale;
		StartCoroutine(StampGlowCR(record));
	}

	private IEnumerator StampGlowCR(MetroLabel label)
	{
		Color startColor = Color.red;
		Color finalColor = Color.yellow;
		float delayValue = 0.413793117f;
		Duration delay = new Duration(delayValue);
		while (true)
		{
			label.SetColor(Color.Lerp(startColor, finalColor, delay.Value01()));
			if (delay.IsDone())
			{
				delay = new Duration(delayValue);
				Color tempColor = startColor;
				startColor = finalColor;
				finalColor = tempColor;
			}
			yield return null;
		}
	}

	private void CleanupLoopingStuff()
	{
		PrefabSingleton<GameSpecialFXManager>.Instance.StopGUICoinShowerFX();
		PrefabSingleton<GameSoundManager>.Instance.StopLoopingSound();
		PrefabSingleton<GameSpecialFXManager>.Instance.StopGUIGrindFX();
	}

	private void EnqueuePopup(MetroPopupPage popup)
	{
		AutoSingleton<MetroMenuStack>.Instance.EnqueuePopup(popup);
	}

	private void DequeuePopup()
	{
		AutoSingleton<MetroMenuStack>.Instance.Pop();
	}

	public override void OnFocus()
	{
		base.OnFocus();
		bool flag = _checkForBikeTripPromo && BikeTripPromoUIManager.DisplayEndRunMenuPopup();
		_checkForBikeTripPromo = false;
		if (AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported() && AutoSingleton<GameFacebookManager>.Instance.IsPublishPermissionVerify() && !AutoSingleton<GameFacebookManager>.Instance.IsPublishPermissionGranted() && AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn() && !flag && !_facebookInvitationChecked)
		{
			_facebookInvitationChecked = true;
			PersistenceManager instance = AutoSingleton<PersistenceManager>.Instance;
			if (instance.MustShowFacebookInvitationToPublishPopup && instance.FacebookPublishPopupAttemptCount % 5 == 0)
			{
				ShowFacebookInvitationToPublishPopup();
			}
			instance.FacebookPublishPopupAttemptCount++;
		}
		UpdateCarUpgradeBadge();
	}

	private void ShowFacebookInvitationToPublishPopup()
	{
		MetroMenuPopupFacebookLogin page = MetroMenuPage.Create<MetroMenuPopupFacebookLogin>().Setup(publish: true);
		AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
	}

	protected override void OnMenuUpdate()
	{
		UpdateNextFrenzyButtonText();
		base.OnMenuUpdate();
		if (IsActive())
		{
			ProcessMessageGate.DisplayMessage(this);
		}
	}

	private void UpdateNextFrenzyButtonText()
	{
		if (_gameMode == GameMode.frenzy && _nextButton != null)
		{
			string timerString = AutoSingleton<FrenzyModeManager>.Instance.GetTimerString();
			if (timerString != string.Empty)
			{
				timerString = "Next in: ".Localize() + timerString;
				_nextButton.SetText(timerString);
			}
			else
			{
				_nextButton.SetText(AutoSingleton<FrenzyModeManager>.Instance.GetBadgeCaption() + " " + "REMAINING".Localize());
			}
		}
	}

	private void UpdateCarUpgradeBadge()
	{
		if (_carUpgradeBadge != null)
		{
			CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(_car);
			int upgradeLevel = carProfile.GetUpgradeLevel();
			int num = 5 - upgradeLevel;
			_carUpgradeBadge.UpdateBadge(num.ToString(), num > 0);
		}
	}
}
