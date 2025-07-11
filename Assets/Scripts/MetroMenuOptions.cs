using System;
using System.Collections.Generic;
using UnityEngine;

public class MetroMenuOptions : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		metroLayout.Add(MetroStatusBar.Create());
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		metroLayout2.SetMass(9f);
		metroLayout.Add(metroLayout2);
		MetroGrid metroGrid = MetroGrid.Create(4, 4);
		metroGrid.SetMass(3f);
		metroLayout2.Add(metroGrid);
		bool musicState = AutoSingleton<PersistenceManager>.Instance.MusicVolume != 0;
		string iconName = (!musicState) ? MetroSkin.IconMusicOff : MetroSkin.IconMusicOn;
		MetroButton music = MetroSkin.CreateMenuButton(iconName, "MUSIC");
		MetroIcon musicIcon = music.GetComponentInChildren<MetroIcon>();
		musicIcon.SetScale(0.8f);
		music.OnButtonClicked += delegate
		{
			musicState = !musicState;
			string iconName11 = (!musicState) ? MetroSkin.IconMusicOff : MetroSkin.IconMusicOn;
			MetroIcon metroIcon5 = MetroIcon.Create(iconName11);
			metroIcon5.SetScale(0.8f);
			musicIcon.Replace(metroIcon5);
			musicIcon.Destroy();
			musicIcon = metroIcon5;
			music.Reflow();
			AutoSingleton<PersistenceManager>.Instance.MusicVolume = (musicState ? 1 : 0);
			PrefabSingleton<GameMusicManager>.Instance.RefreshVolume();
		};
		metroGrid.Add(music);
		bool soundsState = AutoSingleton<PersistenceManager>.Instance.SoundsVolume != 0;
		string iconName2 = (!soundsState) ? MetroSkin.IconSoundsOff : MetroSkin.IconSoundsOn;
		MetroButton sounds = MetroSkin.CreateMenuButton(iconName2, "SOUNDS");
		MetroIcon soundsIcon = sounds.GetComponentInChildren<MetroIcon>();
		soundsIcon.SetScale(0.8f);
		sounds.OnButtonClicked += delegate
		{
			soundsState = !soundsState;
			string iconName10 = (!soundsState) ? MetroSkin.IconSoundsOff : MetroSkin.IconSoundsOn;
			MetroIcon metroIcon4 = MetroIcon.Create(iconName10);
			metroIcon4.SetScale(0.8f);
			soundsIcon.Replace(metroIcon4);
			soundsIcon.Destroy();
			soundsIcon = metroIcon4;
			sounds.Reflow();
			AutoSingleton<PersistenceManager>.Instance.SoundsVolume = (soundsState ? 1 : 0);
			PrefabSingleton<GameSoundManager>.Instance.RefreshVolume();
		};
		metroGrid.Add(sounds);
		bool ghostsState = AutoSingleton<ReplayManager>.Instance.IsActive();
		string iconName3 = (!ghostsState) ? MetroSkin.IconGhostsOff : MetroSkin.IconGhostsOn;
		MetroButton ghosts = MetroSkin.CreateMenuButton(iconName3, "GHOSTS");
		MetroIcon ghostsIcon = ghosts.GetComponentInChildren<MetroIcon>();
		ghostsIcon.SetScale(0.8f);
		ghosts.OnButtonClicked += delegate
		{
			ghostsState = !ghostsState;
			string iconName9 = (!ghostsState) ? MetroSkin.IconGhostsOff : MetroSkin.IconGhostsOn;
			MetroIcon metroIcon3 = MetroIcon.Create(iconName9);
			metroIcon3.SetScale(0.8f);
			ghostsIcon.Replace(metroIcon3);
			ghostsIcon.Destroy();
			ghostsIcon = metroIcon3;
			ghosts.Reflow();
			AutoSingleton<ReplayManager>.Instance.SetActive(ghostsState);
		};
		metroGrid.Add(ghosts);
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconLocationPlaylist, "MAPS LIST");
		metroGrid.Add(metroButton);
		metroButton.OnButtonClicked += delegate
		{
			ShowMapsListPopup();
		};
		if (AutoSingleton<PlatformCapabilities>.Instance.UseShowroom())
		{
			MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconShowroom, "SHOWROOM");
			metroGrid.Add(metroButton2);
			metroButton2.OnButtonClicked += delegate
			{
				AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
			};
			if (!AutoSingleton<PersistenceManager>.Instance.HasSeenShowroom)
			{
				MetroBadge metroBadge = MetroBadge.Create();
				metroButton2.Add(metroBadge);
				metroBadge.UpdateBadge("!", showIcon: true);
			}
		}
		bool notificationsState = Preference.UseNotifications();
		string iconName4 = (!notificationsState) ? MetroSkin.IconNotificationsOff : MetroSkin.IconNotificationsOn;
		MetroButton notifications = MetroSkin.CreateMenuButton(iconName4, "NOTIFICATIONS");
		MetroIcon notificationsIcon = notifications.GetComponentInChildren<MetroIcon>();
		notificationsIcon.SetScale(0.8f);
		notifications.OnButtonClicked += delegate
		{
			notificationsState = !notificationsState;
			string iconName8 = (!notificationsState) ? MetroSkin.IconNotificationsOff : MetroSkin.IconNotificationsOn;
			MetroIcon metroIcon2 = MetroIcon.Create(iconName8);
			metroIcon2.SetScale(0.8f);
			notificationsIcon.Replace(metroIcon2).Destroy();
			notificationsIcon = metroIcon2;
			notifications.Reflow();
			Preference.SetUseNotifications(notificationsState);
		};
		metroGrid.Add(notifications);
		string iconName5 = MetroSkin.IconFrench;
		string text = "FRANÇAIS";
		if (AutoSingleton<LocalizationManager>.Instance.Language == LanguageType.french)
		{
			iconName5 = MetroSkin.IconEnglish;
			text = "ENGLISH";
		}
		MetroButton metroButton3 = MetroSkin.CreateMenuButton(iconName5, text);
		metroGrid.Add(metroButton3);
		metroButton3.OnButtonClicked += delegate
		{
			AutoSingleton<LocalizationManager>.Instance.ToggleLanguage();
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.main));
		};
		MetroButton metroButton4 = MetroSkin.CreateMenuButton(MetroSkin.IconStats, "STATS");
		metroGrid.Add(metroButton4);
		metroButton4.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Replace(MetroMenuPage.Create<MetroMenuStats>(), MetroAnimation.slideRight);
		};
		if (AutoSingleton<PlatformCapabilities>.Instance.IsFeedbackSupported())
		{
			MetroButton metroButton5 = MetroSkin.CreateMenuButton(MetroSkin.IconFeedback, "FEEDBACK");
			metroGrid.Add(metroButton5);
			metroButton5.OnButtonClicked += delegate
			{
				string str = "Please leave your comments here:".Localize() + "\n\n\n---------------------------\nID: " + AutoSingleton<BackendSessionManager>.Instance.GetPublicId() + "\nVersion: ".Localize() + GameVersion.VERSION;
				Application.OpenURL("mailto:feedback@roofdog.ca?subject=" + "Extreme Road Trip 2 Feedback".Localize() + "&body=" + str);
			};
		}
		MetroButton metroButton6 = MetroSkin.CreateMenuButton(MetroSkin.IconTutorial, "TUTORIAL");
		MetroIcon componentInChildren = metroButton6.GetComponentInChildren<MetroIcon>();
		componentInChildren.SetScale(0.8f);
		metroButton6.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigTutorial(LoadConfigMenu.NextMenuPage.options));
		};
		metroGrid.Add(metroButton6);
		MetroButton metroButton7 = MetroSkin.CreateMenuButton(MetroSkin.IconCredits, "CREDITS");
		metroGrid.Add(metroButton7);
		metroButton7.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Replace(MetroMenuPage.Create<MetroMenuCredits>(), MetroAnimation.slideRight);
		};
		if (AutoSingleton<PlatformCapabilities>.Instance.AreInvertedControlsSupported())
		{
			string iconName6 = MetroSkin.IconNormalCtrl;
			if (AutoSingleton<PersistenceManager>.Instance.UseInvertedControl)
			{
				iconName6 = MetroSkin.IconInvertedCtrl;
			}
			MetroButton control = MetroSkin.CreateMenuButton(iconName6, "CONTROLS");
			MetroIcon controlIcon = control.GetComponentInChildren<MetroIcon>();
			controlIcon.SetScale(1.2f);
			control.OnButtonClicked += delegate
			{
				bool flag = !AutoSingleton<PersistenceManager>.Instance.UseInvertedControl;
				AutoSingleton<PersistenceManager>.Instance.UseInvertedControl = flag;
				string iconName7 = (!flag) ? MetroSkin.IconNormalCtrl : MetroSkin.IconInvertedCtrl;
				MetroIcon metroIcon = MetroIcon.Create(iconName7);
				metroIcon.SetScale(1.2f);
				controlIcon.Replace(metroIcon);
				controlIcon.Destroy();
				controlIcon = metroIcon;
				control.Reflow();
			};
			metroGrid.Add(control);
		}
		if (AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported())
		{
			string empty = string.Empty;
			Action value;
			if (AutoSingleton<GameFacebookManager>.Instance.IsLoggedIn())
			{
				value = delegate
				{
					AutoSingleton<GameFacebookManager>.Instance.Logout();
					AutoSingleton<MetroMenuStack>.Instance.Replace(MetroMenuPage.Create<MetroMenuMain>(), MetroAnimation.slideLeft);
				};
				empty = "LOG OUT";
			}
			else
			{
				value = delegate
				{
					AutoSingleton<GameFacebookManager>.Instance.Login(delegate
					{
						AutoSingleton<BackendFacebook>.Instance.Authenticate();
						AutoSingleton<MetroMenuStack>.Instance.Replace(MetroMenuPage.Create<MetroMenuMain>(), MetroAnimation.slideLeft);
					}, isPublishNeeded: false);
				};
				empty = "LOG IN";
			}
			MetroButton metroButton8 = MetroSkin.CreateMenuButton(MetroSkin.IconFacebook, empty, null, 0.5f);
			metroButton8.OnButtonClicked += value;
			metroGrid.Add(metroButton8);
		}
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.Add(metroLayout3);
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.vertical);
		metroLayout3.Add(metroLayout4);
		string publicId = AutoSingleton<BackendSessionManager>.Instance.RoofdogIdentity.PublicId;
		metroLayout4.SetMass(1f);
		metroLayout4.Add(MetroSpacer.Create(2f));
		metroLayout4.Add(MetroLabel.Create("ID: " + publicId + "   ").SetFont(MetroSkin.SmallFont).SetAlignment(MetroAlign.Left)
			.SetColor(Color.black));
		metroLayout4.Add(MetroLabel.Create("Version: " + GameVersion.VERSION).SetFont(MetroSkin.SmallFont).SetAlignment(MetroAlign.Left)
			.SetColor(Color.black));
		MetroButtonMoreGames metroButtonMoreGames = MetroButtonMoreGames.CreateBanner();
		metroLayout3.Add(metroButtonMoreGames);
		metroButtonMoreGames.SetMass(3f);
		MetroButton metroButton9 = MetroSkin.CreateMenuButton(MetroSkin.IconSkip, "BACK", MetroSkin.Slice9ButtonRed);
		metroButton9.SetGradient(MetroSkin.ButtonColorAlert1, MetroSkin.ButtonColorAlert2);
		metroLayout3.Add(metroButton9);
		metroButton9.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Replace(MetroMenuPage.Create<MetroMenuMain>(), MetroAnimation.slideLeft);
		};
		base.OnStart();
	}

	private void ShowMapsListPopup()
	{
		List<MetroWidgetCheckboxListItem> widgetList = new List<MetroWidgetCheckboxListItem>();
		List<Location> allLocations = AutoSingleton<LocationDatabase>.Instance.GetAllLocations();
		List<string> playlist = AutoSingleton<MapsListManager>.Instance.GetPlaylist();
		int num = 0;
		foreach (Location item2 in allLocations)
		{
			bool isSelected = playlist.Contains(item2.name);
			MetroWidgetLocationCheckbox item = MetroWidgetLocationCheckbox.Create(item2, num, MetroSkin.GameCenterPlayerEvenRowColor, MetroSkin.GameCenterPlayerOddRowColor, isSelected);
			widgetList.Add(item);
			num++;
		}
		Action onConfirmButtonClicked = delegate
		{
			List<string> list = new List<string>();
			foreach (MetroWidgetCheckboxListItem item3 in widgetList)
			{
				if (item3 != null && item3.IsSelected())
				{
					list.Add(item3.Id);
				}
			}
			AutoSingleton<MapsListManager>.Instance.SetPlaylist(list);
		};
		MetroMenuPage metroMenuPage = MetroMenuPage.Create<MetroPopupCheckboxList>().Setup(widgetList, onConfirmButtonClicked, "MAPS LIST", "APPLY");
		metroMenuPage.Reflow();
		AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPage);
	}
}
