public static class BikeTripPromoUIManager
{
	public static bool DisplayMainMenuPopup()
	{
		bool flag = BikeTripCrossPromoManager.DisplayMainMenuPopup();
		if (flag)
		{
			CreateAndLaunchCrossPromoPopup(BikeTripCrossPromoManager.GetMainMenuCloseDelay());
		}
		return flag;
	}

	public static bool DisplayEndRunMenuPopup()
	{
		bool flag = BikeTripCrossPromoManager.DisplayEndRunPopup();
		if (flag)
		{
			CreateAndLaunchCrossPromoPopup(BikeTripCrossPromoManager.GetEndRunCloseDelay());
		}
		return flag;
	}

	public static MetroButton GetMainMenuMultiplayerButton()
	{
		MetroButton result = null;
		if (BikeTripCrossPromoManager.IsMainMenuMutiplayerButtonEnabled())
		{
			result = GetMultiplayerButton();
		}
		return result;
	}

	public static MetroButton GetEndRunMultiplayerButton()
	{
		MetroButton result = null;
		if (BikeTripCrossPromoManager.IsEndRunMultipluerButtonEnabled())
		{
			result = GetMultiplayerButton();
		}
		return result;
	}

	private static void CreateAndLaunchCrossPromoPopup(int closeButtonDelaySec)
	{
		PopupBikeTripPromo popupBikeTripPromo = MetroMenuPage.Create<PopupBikeTripPromo>();
		popupBikeTripPromo.Setup(AutoSingleton<BikeTripCrossPromoManager>.Instance.GetMobileTrackingUrl(), closeButtonDelaySec);
		AutoSingleton<MetroMenuStack>.Instance.Push(popupBikeTripPromo);
	}

	private static MetroButton GetMultiplayerButton()
	{
		string iconName = (AutoSingleton<LocalizationManager>.Instance.Language != 0) ? MetroSkin.IconBikePromoFrench : MetroSkin.IconBikePromoEnglish;
		MetroButton metroButton = MetroSkin.CreateMenuButton(iconName, null);
		metroButton.OnButtonClicked += delegate
		{
			CreateAndLaunchCrossPromoPopup(0);
		};
		return metroButton;
	}
}
