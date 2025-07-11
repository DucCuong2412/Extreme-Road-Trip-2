public class MetroPopupUnlockShowroom : MetroPopupPage
{
	private Showroom _showroom;

	public MetroPopupUnlockShowroom Setup(Showroom showroom)
	{
		_showroom = showroom;
		return this;
	}

	protected override void OnStart()
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel metroLabel = MetroLabel.Create("UNLOCK".Localize() + " " + _showroom.DisplayName);
		metroLabel.SetFont((!Device.IsIPad()) ? MetroSkin.BigFont : MetroSkin.DefaultFont);
		metroLayout2.Add(metroLabel);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout3);
		metroLayout3.SetMass(3f);
		metroLayout3.Add(MetroSpacer.Create());
		MetroWidget child = MetroSpacer.Create().AddSolidBackground().SetMaterial(_showroom._previewMaterial);
		metroLayout3.Add(child);
		metroLayout3.Add(MetroSpacer.Create());
		MetroLabel metroLabel2 = MetroLabel.Create(_showroom.Description);
		metroLabel2.SetFont(MetroSkin.MediumFont);
		metroLayout.Add(metroLabel2);
		MetroWidgetPrice child2 = MetroWidgetPrice.Create(_showroom.Price).SetIconScale(0.6f).SetFont(MetroSkin.BigFont);
		metroLayout.Add(child2);
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		metroLayout4.Add(MetroSpacer.Create(0.4f));
		MetroButton metroButton = MetroButton.Create("PURCHASE");
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			TryBuyShowroom();
		};
		metroLayout4.Add(metroButton);
		metroLayout4.Add(MetroSpacer.Create(0.4f));
		metroLayout.Add(MetroSpacer.Create(0.2f));
		base.OnStart();
	}

	private void TryBuyShowroom()
	{
		ShowroomProfile showroomProfile = AutoSingleton<ShowroomDatabase>.Instance.GetShowroomProfile(_showroom);
		if (!showroomProfile.TryUnlock(_showroom.Price))
		{
			AutoSingleton<CashManager>.Instance.HandleNotEnoughCurrency(_showroom.Price, pauseOnFocus: false);
			return;
		}
		AutoSingleton<ShowroomDatabase>.Instance.SaveShowroomProfile(_showroom, showroomProfile);
		AutoSingleton<ShowroomManager>.Instance.SetShowroom(_showroom);
		AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
	}
}
