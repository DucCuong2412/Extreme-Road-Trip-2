using System;
using System.Collections.Generic;
using UnityEngine;

public class MetroMenuMain : MetroMenuPage
{
    // private bool _videoShown;

    // private MetroWidget _videoButton;

    private MetroWidget _inviteButton;

    private bool _facebookLoginPopupChecked;

    private bool _checkForBikeTripPromo = true;

    private bool _popupQuitDisplayed;

    private Color GetTextColor()
    {
        return Color.white;
    }

    protected override void OnStart()
    {
        MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
        Add(metroLayout);
        MetroIcon metroIcon = MetroIcon.Create(MetroSkin.SpriteTitle);
        metroIcon.SetMass(4f);
        metroLayout.Add(metroIcon);
        metroLayout.Add(MetroSpacer.Create(2.5f));
        MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
        metroLayout2.SetMass(1.5f);

        // if (AutoSingleton<PlatformCapabilities>.Instance.IsIncentivedVideoSupported())
        // {
        // 	_videoShown = true;
        // 	_videoButton = AutoSingleton<GameAdProvider>.Instance.CreateFreeCrateButtonMenuMain(delegate { });
        // }
        // else
        // {
        // 	_videoShown = false;
        // 	_videoButton = MetroSpacer.Create();
        // }
        // _videoButton.SetMass(1f);
        // metroLayout2.Add(_videoButton);

        metroLayout2.Add(MetroSpacer.Create(2f));

        if (AutoSingleton<PlatformCapabilities>.Instance.UseFacebookInvite() && !AutoSingleton<PersistenceManager>.Instance.FacebookInviteSent)
        {
            _inviteButton = CreateInviteButton();
            metroLayout2.Add(_inviteButton);
        }
        else
        {
            metroLayout2.Add(MetroSpacer.Create());
        }
        metroLayout.Add(metroLayout2);
        MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
        metroLayout3.SetMass(2f);
        metroLayout.Add(metroLayout3);
        MetroButtonMoreGames metroButtonMoreGames = MetroButtonMoreGames.Create();
        if (metroButtonMoreGames != null)
        {
            metroLayout3.Add(metroButtonMoreGames);
        }
        MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconOptions, "OPTIONS");
        metroButton.OnButtonClicked += delegate
        {
            AutoSingleton<MetroMenuStack>.Instance.Replace(MetroMenuPage.Create<MetroMenuOptions>(), MetroAnimation.slideRight);
        };
        metroLayout3.Add(metroButton);
        if (AutoSingleton<PlatformCapabilities>.Instance.IsGameCenterSupported())
        {
            MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconGameCenter, "GAME CENTER", null, 1f);
            metroLayout3.Add(metroButton2);
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
        metroLayout3.Add(MetroButtonFrenzy.Create());
        MetroButton mainMenuMultiplayerButton = BikeTripPromoUIManager.GetMainMenuMultiplayerButton();
        if (mainMenuMultiplayerButton != null)
        {
            metroLayout3.Add(mainMenuMultiplayerButton);
        }
        MetroButton metroButton3 = MetroSkin.CreateMenuButton(MetroSkin.IconPlay, "PLAY", MetroSkin.Slice9ButtonRed);
        metroButton3.SetGradient(MetroSkin.ButtonColorAlert1, MetroSkin.ButtonColorAlert2);
        metroLayout3.Add(metroButton3);
        metroButton3.OnButtonClicked += delegate
        {
            if (!AutoSingleton<PersistenceManager>.Instance.HasSeenTutorial)
            {
                AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigTutorial(LoadConfigMenu.NextMenuPage.chooseCar));
                AutoSingleton<PersistenceManager>.Instance.HasSeenTutorial = true;
                AutoSingleton<PersistenceManager>.Instance.Save();
            }
            else
            {
                AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.chooseCar));
            }
        };
        PrefabSingleton<GameMusicManager>.Instance.PlayTitleMusic();

        // if (AutoSingleton<PlatformCapabilities>.Instance.IsIncentivedVideoSupported())
        // {
        // 	GameAdProvider.OnVideoAvailable = (Action)Delegate.Combine(GameAdProvider.OnVideoAvailable, new Action(OnVideoAvailable));
        // 	GameAdProvider.OnVideoNotAvailable = (Action)Delegate.Combine(GameAdProvider.OnVideoNotAvailable, new Action(OnVideoUnavailable));
        // }

        // AdEvents.OnBoot();

        base.OnStart();
    }

    public override void OnFocus()
    {
        base.OnFocus();
        //bool flag = CanPushPopup() && _checkForBikeTripPromo && BikeTripPromoUIManager.DisplayMainMenuPopup();
        //_checkForBikeTripPromo = false;
        //if (CanPushPopup() && AutoSingleton<PlatformCapabilities>.Instance.IsFacebookSupported() && !_facebookLoginPopupChecked)
        //{
        //    _facebookLoginPopupChecked = true;
        //    PersistenceManager instance = AutoSingleton<PersistenceManager>.Instance;
        //    if (instance.MustShowFacebookInvitationToLoginPopup && instance.FacebookLoginPopupAttemptCount % 5 == 0 && !AutoSingleton<GameFacebookManager>.Instance.IsPublishPermissionGranted())
        //    {
        //        ShowFacebookInvitationToLoginPopup();
        //    }
        //    instance.FacebookLoginPopupAttemptCount++;
        //}
    }

    public void OnDisable()
    {
        // GameAdProvider.OnVideoAvailable = (Action)Delegate.Remove(GameAdProvider.OnVideoAvailable, new Action(OnVideoAvailable));
        // GameAdProvider.OnVideoNotAvailable = (Action)Delegate.Remove(GameAdProvider.OnVideoNotAvailable, new Action(OnVideoUnavailable));
    }

    protected override void OnMenuUpdate()
    {
        base.OnMenuUpdate();
        if (IsActive())
        {
            ProcessMessageGate.DisplayMessage(this);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !_popupQuitDisplayed)
        {
            _popupQuitDisplayed = true;
            string title = "QUIT";
            string message = "Are you sure you want to quit the application?";
            string yes = "YES";
            string no = "NO";
            MetroMenuPopupYesNoLater metroMenuPopupYesNoLater = MetroMenuPage.Create<MetroMenuPopupYesNoLater>().Setup(title, message, yes, no, MetroSkin.Slice9ButtonRed);
            metroMenuPopupYesNoLater.OnButtonYes(delegate
            {
                Application.Quit();
            });
            metroMenuPopupYesNoLater.OnButtonNo(delegate
            {
                AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
                _popupQuitDisplayed = false;
            });
            metroMenuPopupYesNoLater.OnClose(delegate
            {
                AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
                _popupQuitDisplayed = false;
            });
            AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPopupYesNoLater, MetroAnimation.popup);
        }
    }

    private void ShowFacebookInvitationToLoginPopup()
    {
        //MetroMenuPopupFacebookLogin page = MetroMenuPage.Create<MetroMenuPopupFacebookLogin>().Setup();
        //AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
    }

    private MetroButton CreateInviteButton()
    {
        MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconFacebook, "FREE @!", MetroSkin.Slice9ButtonBlue, 0.4f);
        metroButton.OnButtonClicked += delegate
        {
            string titleString = "INVITE FRIENDS";
            string messageString = "Get ".Localize() + 50.ToString() + "@ for each Facebook friends invited.\n\nYou can only do this once!".Localize();
            string buttonString = "INVITE";
            Action buttonAction = delegate
            {
                Action<int> onInviteCompleted = delegate (int numberOfFriendsInvite)
                {
                    if (numberOfFriendsInvite > 0)
                    {
                        Reward reward = new Reward(RewardType.coins, 50 * numberOfFriendsInvite);
                        Action onDismiss = delegate
                        {
                            reward.Activate();
                            AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
                        };
                        List<Reward> rewards = new List<Reward> { reward };
                        MetroPopupRewards page = MetroMenuPage.Create<MetroPopupRewards>().Setup("Thanks for sharing!", "Here's your gift! :)", rewards, onDismiss);
                        AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
                        AutoSingleton<PersistenceManager>.Instance.FacebookInviteSent = true;
                        RemoveFacebookButton();
                    }
                };
                AutoSingleton<GameFacebookManager>.Instance.InviteFriends(onInviteCompleted);
                AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
            };
            AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupMessage>().Setup(titleString, messageString, buttonString, MetroSkin.Slice9ButtonBlue, buttonAction), MetroAnimation.popup);
        };
        return metroButton;
    }

    private void RemoveFacebookButton()
    {
        if (_inviteButton != null)
        {
            MetroSpacer metroSpacer = MetroSpacer.Create();
            _inviteButton.Replace(metroSpacer).Destroy();
            _inviteButton = metroSpacer;
            _inviteButton.Parent.Reflow();
        }
    }

    // private void OnVideoAvailable()
    // {
    // 	if (!_videoShown)
    // 	{
    // 		_videoShown = true;
    // 		MetroButton metroButton = AutoSingleton<GameAdProvider>.Instance.CreateFreeCrateButtonMenuMain(delegate { });
    // 		_videoButton.Replace(metroButton).Destroy();
    // 		_videoButton = metroButton;
    // 		_videoButton.Parent.Reflow();
    // 	}
    // }

    // private void OnVideoUnavailable()
    // {
    // 	if (_videoShown)
    // 	{
    // 		_videoShown = false;
    // 		MetroSpacer metroSpacer = MetroSpacer.Create();
    // 		_videoButton.Replace(metroSpacer).Destroy();
    // 		_videoButton = metroSpacer;
    // 		_videoButton.Parent.Reflow();
    // 	}
    // }

    private bool CanPushPopup()
    {
        return AutoSingleton<MetroMenuStack>.Instance.Peek() == this;
    }
}
