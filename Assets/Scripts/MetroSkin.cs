using System;
using UnityEngine;

public class MetroSkin
{
	public const int DefaultLayer = 0;

	public const int GUILayer = 8;

	public const int BackgroundLayer = 13;

	public static float Padding = 0.4f;

	public static float MenuPageSlideDuration = 0.4f;

	public static float MenuPageBerpDuration = 0.4f;

	public static float MenuPageScaleDuration = 0.3f;

	public static int ClippedGUILayer1 = 14;

	public static int ClippedGUILayer2 = 15;

	public static int ClippedGUILayer3 = 16;

	public static MetroFont BigFont = new MetroFont("SourceSans64", 0.1f);

	public static MetroFont DefaultFont = new MetroFont("SourceSans40", 0.1f);

	public static MetroFont MediumFont = new MetroFont("SourceSans32", 0.1f);

	public static MetroFont SmallFont = new MetroFont("SourceSans24", 0.1f);

	public static MetroFont VerySmallFont = new MetroFont("SourceSans16", 0.1f);

	public static Color ButtonColor1 = ColorUtil.Parse("#111111");

	public static Color ButtonColor2 = ColorUtil.Parse("#333333");

	public static Color ButtonColorAlert1 = ColorUtil.Parse("#aa2222");

	public static Color ButtonColorAlert2 = ColorUtil.Parse("#dd2222");

	public static Color ButtonColorAlt = ColorUtil.Parse("#ff8b00");

	public static Color ButtonInactiveColor = ColorUtil.Parse("#444444");

	public static Color SelectedColor = ColorUtil.Parse("#45678e");

	public static Color UnselectedColor = ColorUtil.Parse("#0a1c30");

	public static Color OnColor = ColorUtil.Parse("#ff8b00");

	public static Color OffColor = ColorUtil.Parse("#222222");

	public static Color DarkColor = Color.black;

	public static Color LevelUpColor = ColorUtil.Parse("#a3070f");

	public static Color NewLevelColor = ColorUtil.Parse("#e30012");

	public static Color FlashColor = Color.white;

	public static Color StuntTextColor = Color.yellow;

	public static Color LandingTextColor = ColorUtil.Parse("#ff4c00");

	public static Color TextNormalColor = Color.white;

	public static Color TextAlertColor = Color.red;

	public static Color TextOutlineColor = Color.black;

	public static Color ButtonLabelColor = Color.black;

	public static Color InformationAreaColor = Color.black;

	public static Color XPWidgetBackColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);

	public static Color XPWidgetBarColor1 = new Color(0.1f, 0.9f, 0.1f);

	public static Color XPWidgetBarColor2 = new Color(0.4f, 0.8f, 0.4f);

	public static Color XPWidgetBarGrindColor1 = new Color(1f, 0.6f, 0f);

	public static Color XPWidgetBarGrindColor2 = new Color(1f, 0.9f, 0.26f);

	public static Color StatsSelectedItemColor = new Color(0f, 0f, 0f, 0.6f);

	public static Color OptionsBackgroundColor = new Color(0f, 0f, 0f, 0.4f);

	public static Color NewPowerupGlowColor1 = ColorUtil.Parse("#FF1600");

	public static Color NewPowerupGlowColor2 = ColorUtil.Parse("#FF9500");

	public static Color GameCenterPlayerEvenRowColor = ColorUtil.Parse("#00795a");

	public static Color GameCenterPlayerOddRowColor = ColorUtil.Parse("#18a642");

	public static Color FacebookEvenRowColor = ColorUtil.Parse("#697FB3");

	public static Color FacebookOddRowColor = ColorUtil.Parse("#3B5998");

	public static Color ChallengeButtonColor = ColorUtil.Parse("#298722");

	public static Color Transparent = new Color(0f, 0f, 0f, 0f);

	public static Color SemiTransparent = new Color(0f, 0f, 0f, 0.5f);

	public static Color DarkMask = new Color(0f, 0f, 0f, 0.9f);

	public static string IconAchievements = "IconAchievements";

	public static string IconAmazonStore = "IconAmazonStore";

	public static string IconAppleStore = "IconAppleStore";

	public static string IconBack = "IconBack";

	public static string IconBadge = "IconBadge";

	public static string IconBadgeLarge = "IconBadgeLarge";

	public static string IconBucks = "IconBucks";

	public static string IconCamera = "IconCamera";

	public static string IconChangeCar = "IconChangeCar";

	public static string IconCoin = "IconCoin";

	public static string IconChoppa = "IconChoppa";

	public static string IconCoal = "IconCoal";

	public static string IconCrate = "IconCrate";

	public static string IconCrate2 = "IconCrate2";

	public static string IconClose = "IconClose";

	public static string IconCredits = "IconCredits";

	public static string IconDiamond = "IconDiamond";

	public static string IconDirt = "IconDirt";

	public static string IconEnglish = "IconEnglish";

	public static string IconFacebook = "IconFacebook";

	public static string IconFacebookLike = "IconFacebookLike";

	public static string IconFrench = "IconFrench";

	public static string IconFriends = "IconFriends";

	public static string IconFlag = "IconFlag";

	public static string IconHeal = "IconHeal";

	public static string IconHighscore = "IconHighscore";

	public static string IconMissionStar = "IconMissionStar";

	public static string IconGameCenter = "IconGameCenter";

	public static string IconGooglePlay = "IconGooglePlay";

	public static string IconLeaderboard = "IconLeaderboard";

	public static string IconLeaderboards = "IconLeaderboards";

	public static string IconLocationPlaylist = "IconLocationPlaylist";

	public static string IconLock = "IconLock";

	public static string IconMoreGames = "IconMoreGames";

	public static string IconMusicOff = "IconMusicOff";

	public static string IconMusicOn = "IconMusicOn";

	public static string IconOptions = "IconOptions";

	public static string IconPause = "IconPause";

	public static string IconPlay = "IconPlay";

	public static string IconPrestigeToken = "IconPrestigeToken";

	public static string IconRock = "IconRock";

	public static string IconShowroom = "IconShowroom";

	public static string IconSkip = "IconSkip";

	public static string IconSoundsOff = "IconSoundsOff";

	public static string IconSoundsOn = "IconSoundsOn";

	public static string IconStats = "IconStats";

	public static string IconTwitter = "IconTwitter";

	public static string IconTutorial = "IconTutorials";

	public static string IconOn = "IconOn";

	public static string IconOff = "IconOff";

	public static string IconOffSpecial = "IconOffSpecial";

	public static string IconChecked = "IconChecked";

	public static string IconUnchecked = "IconUnchecked";

	public static string IconGhostsOn = "IconGhostsOn";

	public static string IconGhostsOff = "IconGhostsOff";

	public static string IconShare = "IconShare";

	public static string IconFeedback = "IconFeedback";

	public static string IconiCloudOn = "IconiCloudOn";

	public static string IconiCloudOff = "IconiCloudOff";

	public static string IconStore = "IconStore";

	public static string IconStoreTag = "IconStoreTag";

	public static string IconRocket = "IconRocket";

	public static string IconTransport = "IconTransport";

	public static string IconInterstitial = "IconInterstitial";

	public static string IconFrenzyRunEnglish = "IconFrenzyRunEnglish";

	public static string IconFrenzyRunFrench = "IconFrenzyRunFrench";

	public static string IconStoreRestore = "IconStoreRestore";

	public static string IconStoreSoldEnglish = "IconStoreSoldEnglish";

	public static string IconStoreSoldFrench = "IconStoreSoldFrench";

	public static string IconUpgradeCar = "IconUpgradeCar";

	public static string IconWatchVideo = "IconWatchVideo";

	public static string IconNotificationsOn = "IconNotificationsOn";

	public static string IconNotificationsOff = "IconNotificationsOff";

	public static string IconNormalCtrl = "IconNormalCtrl";

	public static string IconInvertedCtrl = "IconInvertedCtrl";

	public static string IconMinnow = "IconMinnow";

	public static string IconPeamouth = "IconPeamouth";

	public static string IconTrout = "IconTrout";

	public static string IconClownfish = "IconClownfish";

	public static string IconBarracuda = "IconBarracuda";

	public static string IconGreatWhiteShark = "IconGreatWhiteShark";

	public static string IconBerserker = "IconPM3Berserker";

	public static string IconGreenOrc = "IconPM3GreenOrc";

	public static string IconPinky = "IconPM3Pinky";

	public static string IconGoblin = "IconPM3Goblin";

	public static string IconBlackSpider = "IconPM3BlackSpider";

	public static string IconSentinel = "IconPM3Sentinel";

	public static string Star = "Star";

	public static string StarBlack = "StarBlack";

	public static string StarCircle = "StarCircle";

	public static string StarNoGlow = "StarNoGlow";

	public static string StarParticleRing = "StarParticleRing";

	public static string StarWithGlow = "StarWithGlow";

	public static string IconCoinsPackA = "IconCoinsPackA";

	public static string IconCoinsPackB = "IconCoinsPackB";

	public static string IconCoinsPackC = "IconCoinsPackC";

	public static string IconCoinsPackD = "IconCoinsPackD";

	public static string IconCoinsPackE = "IconCoinsPackE";

	public static string IconBucksPackA = "IconBucksPackA";

	public static string IconBucksPackB = "IconBucksPackB";

	public static string IconBucksPackC = "IconBucksPackC";

	public static string IconBucksPackD = "IconBucksPackD";

	public static string IconBucksPackE = "IconBucksPackE";

	public static string BikeTripPromoPopup = "BikeTripPopupIcon";

	public static string IconBikePromoEnglish = "IconBikePromoEnglish";

	public static string IconBikePromoFrench = "IconBikePromoFrench";

	public static string IconPowerupBoost = "IconPowerupBoost";

	public static string IconPowerupCoinDoubler = "IconPowerupCoinDoubler";

	public static string IconPowerupMagnet = "IconPowerupMagnet";

	public static string IconKey1 = "IconKey1";

	public static string IconKey2 = "IconKey2";

	public static string IconKey3 = "IconKey3";

	public static string IconPowerupActivated = "IconPowerupOverlay";

	public static string ProgressBarMat = "ProgressBarMat";

	public static string Slice9Button = "Slice9Button";

	public static string Slice9ButtonRed = "Slice9ButtonRed";

	public static string Slice9ButtonGreen = "Slice9ButtonGreen";

	public static string Slice9ButtonBlue = "Slice9ButtonBlue";

	public static string Slice9BlackCircle = "Slice9BlackCircle";

	public static string Slice9PopupBackground = "Slice9PopupBackground";

	public static string Slice9RoundedSemiTransparent = "Slice9RoundedSemiTransparent";

	public static string Slice9RoundedSemiTransparentRed = "Slice9RoundedSemiTransparentRed";

	public static string Slice9YellowRoadSign = "Slice9Button";

	public static string Slice9RedCircle = "Slice9LevelUpRedCircle";

	public static string Slice9StoreSquare = "Slice9StoreSquare";

	public static string Slice9StoreSquareSelected = "Slice9StoreSquareSelected";

	public static string Slice9MeterBackground = "Slice9MeterBackground";

	public static string Slice9MeterRed = "Slice9MeterRed";

	public static string Slice9MeterYellow = "Slice9MeterYellow";

	public static string Slice9MeterBlue = "Slice9MeterBlue";

	public static string SpriteTitle = "SpriteTitle";

	public static string LevelBadge = "LevelBadge";

	public static string Spinner = "SpriteSpinner";

	public static string Arrow = "Arrow";

	public static string UpgradeBar = "UpgradeBar";

	public static string Slice9CardBaseCoins = "Slice9CardBaseCoins";

	public static string Slice9CardBaseBucks = "Slice9CardBaseBucks";

	public static string Slice9CardSuper = "Slice9CardSuper";

	public static string Slice9CardBasePrestige = "Slice9CardBasePrestige";

	public static string Slice9CardBasePocketMine = "Slice9CardBasePocketmine";

	public static string Slice9CardBasePocketmineSelected = "Slice9CardBasePocketmineSelected";

	public static string Slice9CardBasePRT = "Slice9CardBasePRT";

	public static string Slice9CardBasePRTSelected = "Slice9CardBasePRTSelected";

	public static string Slice9CardBaseCoinsSelected = "Slice9CardBaseCoinsSelected";

	public static string Slice9CardBaseBucksSelected = "Slice9CardBaseBucksSelected";

	public static string Slice9CardSuperSelected = "Slice9CardSuperSelected";

	public static string Slice9CardBasePrestigeSelected = "Slice9CardBasePrestigeSelected";

	public static string SpriteCardLock = "SpriteCardLock";

	public static string SpriteCardMedalLocked = "SpriteCardMedalLocked";

	public static string SpriteCardMedalBronze = "SpriteCardMedalBronze";

	public static string SpriteCardMedalSilver = "SpriteCardMedalSilver";

	public static string SpriteCardMedalGold = "SpriteCardMedalGold";

	public static string SpriteCardMissionCircleOutline = "SpriteCardMissionCircleOutline";

	public static string SpriteCardMissionMeter = "SpriteCardMissionMeter";

	public static string SpriteCardMissionMeterLocked = "SpriteCardMissionMeterLocked";

	public static string SpriteCardMissionPieChart = "SpriteCardMissionPieChart";

	public static string SpriteCardPrestigeBadgeLocked = "SpriteCardPrestigeBadgeLocked";

	public static string SpriteCardPrestigeBadgeUnlocked = "SpriteCardPrestigeBadgeUnlocked";

	public static string SpriteCardPrestigeBadge1 = "SpriteCardPrestigeBadge1";

	public static string SpriteCardPrestigeBadge2 = "SpriteCardPrestigeBadge2";

	public static MetroButton CreateMenuButton(string iconName, string text, string slice9 = null, float scale = 0.7f)
	{
		return CreateMenuButton(iconName, text, Color.white, slice9, scale);
	}

	public static MetroButton CreateMenuButton(string iconName, string text, Color textColor, string slice9 = null, float scale = 0.7f)
	{
		MetroButton button = MetroButton.Create();
		button.AddSlice9Background(slice9 ?? Slice9Button);
		button.OnKeyFocusGained += delegate
		{
			button.AddSlice9Background(Slice9ButtonRed);
			button.Berp();
		};
		button.OnKeyFocusLost += delegate
		{
			button.AddSlice9Background(Slice9Button);
		};
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		button.Add(metroLayout);
		MetroIcon metroIcon = MetroIcon.Create(iconName);
		metroIcon.SetMass(5f);
		metroIcon.SetScale(scale);
		metroLayout.Add(metroIcon);
		if (text != null)
		{
			MetroLabel metroLabel = MetroLabel.Create(text);
			metroLabel.SetFont(SmallFont);
			metroLabel.SetColor(textColor);
			metroLayout.Add(metroLabel);
			metroLayout.Add(MetroSpacer.Create());
		}
		return button;
	}

	public static MetroWidget CreatePopupCloseButton(Action onClose)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(0.5f);
		metroLayout.Add(metroLayout2);
		metroLayout2.Add(MetroSpacer.Create(0.25f));
		MetroButton metroButton = MetroButton.Create();
		metroButton.AddSpecialKey(KeyCode.Escape);
		metroButton.IsKeyNavigatorAccessible = false;
		metroButton.SetMass(0.5f);
		metroButton.OnButtonClicked += onClose;
		metroLayout2.Add(metroButton);
		metroButton.Add(MetroIcon.Create(IconClose));
		metroLayout2.Add(MetroSpacer.Create(4f));
		metroLayout.Add(MetroSpacer.Create(3.5f));
		return metroLayout;
	}
}
