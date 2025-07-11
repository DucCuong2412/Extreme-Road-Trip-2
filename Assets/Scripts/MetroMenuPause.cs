public class MetroMenuPause : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroStatusBar metroStatusBar = MetroStatusBar.Create();
		metroStatusBar.Bucks.SetActive(active: false);
		metroStatusBar.Coins.SetActive(active: false);
		metroLayout.Add(metroStatusBar);
		MetroWidget metroWidget = MetroLayout.Create(Direction.vertical).SetMass(7f);
		metroLayout.Add(metroWidget);
		MetroLabel metroLabel = MetroLabel.Create("GAME PAUSED");
		metroLabel.SetFont(MetroSkin.BigFont);
		metroWidget.Add(metroLabel);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(4f);
		metroWidget.Add(metroLayout2);
		LoadConfigGame loadConfigGame = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigGame;
		if (loadConfigGame.GameMode != GameMode.frenzy)
		{
			metroLayout2.Add(MetroSpacer.Create(0.7f));
			MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
			metroLayout3.SetMass(2f);
			metroLayout3.AddSlice9Background(MetroSkin.Slice9PopupBackground);
			metroLayout2.Add(metroLayout3);
			metroLayout3.Add(MetroSpacer.Create().SetMass(0.03f));
			MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
			metroLayout3.Add(metroLayout4);
			metroLayout3.Add(MetroSpacer.Create().SetMass(0.03f));
			MetroLabel metroLabel2 = MetroLabel.Create("MISSIONS");
			metroLabel2.AddOutline();
			metroLayout4.Add(metroLabel2);
			Car carRef = Singleton<GameManager>.Instance.CarRef;
			MissionManager instance = AutoSingleton<MissionManager>.Instance;
			MetroPanelCurrentMissions metroPanelCurrentMissions = MetroPanelCurrentMissions.Create(carRef, instance.GetMissions(carRef), instance.GetCompletedMissionCount() - instance.GetCurrentCompletedMissionCount(carRef));
			metroPanelCurrentMissions.SetMass(3f);
			metroLayout4.Add(metroPanelCurrentMissions);
			metroLayout2.Add(MetroSpacer.Create(0.7f));
		}
		MetroLayout metroLayout5 = MetroLayout.Create(Direction.horizontal);
		metroLayout5.SetMass(2f);
		metroLayout.Add(metroLayout5);
		bool ghostsState = AutoSingleton<ReplayManager>.Instance.IsActive();
		string iconName = (!ghostsState) ? MetroSkin.IconGhostsOff : MetroSkin.IconGhostsOn;
		MetroButton ghosts = MetroSkin.CreateMenuButton(iconName, "GHOSTS");
		MetroIcon ghostsIcon = ghosts.GetComponentInChildren<MetroIcon>();
		ghostsIcon.SetScale(0.8f);
		ghosts.OnButtonClicked += delegate
		{
			ghostsState = !ghostsState;
			string iconName6 = (!ghostsState) ? MetroSkin.IconGhostsOff : MetroSkin.IconGhostsOn;
			MetroIcon metroIcon3 = MetroIcon.Create(iconName6);
			metroIcon3.SetScale(0.8f);
			ghostsIcon.Replace(metroIcon3);
			ghostsIcon.Destroy();
			ghostsIcon = metroIcon3;
			ghosts.Reflow();
			AutoSingleton<ReplayManager>.Instance.SetActive(ghostsState);
		};
		metroLayout5.Add(ghosts);
		bool musicState = AutoSingleton<PersistenceManager>.Instance.MusicVolume != 0;
		string iconName2 = (!musicState) ? MetroSkin.IconMusicOff : MetroSkin.IconMusicOn;
		MetroButton music = MetroSkin.CreateMenuButton(iconName2, "MUSIC");
		MetroIcon musicIcon = music.GetComponentInChildren<MetroIcon>();
		musicIcon.SetScale(0.8f);
		music.OnButtonClicked += delegate
		{
			musicState = !musicState;
			string iconName5 = (!musicState) ? MetroSkin.IconMusicOff : MetroSkin.IconMusicOn;
			MetroIcon metroIcon2 = MetroIcon.Create(iconName5);
			metroIcon2.SetScale(0.8f);
			musicIcon.Replace(metroIcon2);
			musicIcon.Destroy();
			musicIcon = metroIcon2;
			music.Reflow();
			AutoSingleton<PersistenceManager>.Instance.MusicVolume = (musicState ? 1 : 0);
			PrefabSingleton<GameMusicManager>.Instance.RefreshVolume();
		};
		metroLayout5.Add(music);
		bool soundsState = AutoSingleton<PersistenceManager>.Instance.SoundsVolume != 0;
		string iconName3 = (!soundsState) ? MetroSkin.IconSoundsOff : MetroSkin.IconSoundsOn;
		MetroButton sounds = MetroSkin.CreateMenuButton(iconName3, "SOUNDS");
		MetroIcon soundsIcon = sounds.GetComponentInChildren<MetroIcon>();
		soundsIcon.SetScale(0.8f);
		sounds.OnButtonClicked += delegate
		{
			soundsState = !soundsState;
			string iconName4 = (!soundsState) ? MetroSkin.IconSoundsOff : MetroSkin.IconSoundsOn;
			MetroIcon metroIcon = MetroIcon.Create(iconName4);
			metroIcon.SetScale(0.8f);
			soundsIcon.Replace(metroIcon);
			soundsIcon.Destroy();
			soundsIcon = metroIcon;
			sounds.Reflow();
			AutoSingleton<PersistenceManager>.Instance.SoundsVolume = (soundsState ? 1 : 0);
			PrefabSingleton<GameSoundManager>.Instance.RefreshVolume();
		};
		metroLayout5.Add(sounds);
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconChangeCar, "CHANGE CAR");
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<PauseManager>.Instance.Resume();
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.chooseCar));
		};
		metroLayout5.Add(metroButton);
		LoadConfigGame loadConfigGame2 = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigGame;
		if (loadConfigGame2 == null || loadConfigGame2.GameMode == GameMode.normal)
		{
			MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconCredits, "RESTART");
			metroButton2.OnButtonClicked += delegate
			{
				MetroMenuPopupYesNoLater metroMenuPopupYesNoLater = MetroMenuPage.Create<MetroMenuPopupYesNoLater>().Setup("RESTART", "Restart this run and lose all progress?", "RESTART", "CANCEL", MetroSkin.Slice9ButtonRed);
				metroMenuPopupYesNoLater.OnButtonYes(delegate
				{
					AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
					Restart();
				});
				metroMenuPopupYesNoLater.OnButtonNo(delegate
				{
					AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
				});
				AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPopupYesNoLater, MetroAnimation.popup);
			};
			metroLayout5.Add(metroButton2);
		}
		MetroButton metroButton3 = MetroSkin.CreateMenuButton(MetroSkin.IconPlay, "RESUME", MetroSkin.Slice9ButtonRed);
		metroButton3.SetGradient(MetroSkin.ButtonColorAlert1, MetroSkin.ButtonColorAlert2);
		metroButton3.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.none);
			AutoSingleton<PauseManager>.Instance.Resume();
		};
		metroLayout5.Add(metroButton3);
		base.OnStart();
	}

	private void Restart()
	{
		int value = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetValue(Singleton<GameManager>.Instance.CarRef, GameStats.CarStats.Type.coinsPickedUp);
		AutoSingleton<CashManager>.Instance.AddCoins(value);
		AutoSingleton<Player>.Instance.Profile.SetNextEpicPowerup(EpicPowerupType.none);
		AutoSingleton<PauseManager>.Instance.Resume();
		AutoSingleton<LoadingManager>.Instance.Reload();
	}

	public override void OnFocus()
	{
		AutoSingleton<PauseManager>.Instance.Pause();
		base.OnFocus();
	}
}
