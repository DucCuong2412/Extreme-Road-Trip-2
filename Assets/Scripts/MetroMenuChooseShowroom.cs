using System.Collections.Generic;
using UnityEngine;

public class MetroMenuChooseShowroom : MetroMenuPager
{
	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		metroLayout.Add(MetroStatusBar.Create());
		MetroSpacer metroSpacer = MetroSpacer.Create();
		metroSpacer.SetMass(7f);
		metroLayout.Add(metroSpacer);
		_pager = CreateMetroPager("ChooseShowroomPager");
		metroSpacer.Add(_pager);
		List<Showroom> allShowroom = AutoSingleton<ShowroomDatabase>.Instance.GetAllShowroom();
		base.ColCount = 3f;
		base.RowCount = 2f;
		int num = Mathf.CeilToInt((float)allShowroom.Count / (base.ColCount * base.RowCount));
		int num2 = 0;
		MetroGrid metroGrid = CreateGrid();
		foreach (Showroom item in allShowroom)
		{
			MetroButton metroButton = MetroButton.Create();
			metroButton.SetPadding(0.8f);
			MetroLayout buttonLayout = MetroLayout.Create(Direction.vertical);
			metroButton.Add(buttonLayout);
			buttonLayout.AddSolidBackground(Color.black);
			metroButton.OnKeyFocusGained += delegate
			{
				buttonLayout.AddSolidBackground(MetroSkin.ButtonColorAlert1);
			};
			metroButton.OnKeyFocusLost += delegate
			{
				buttonLayout.AddSolidBackground(Color.black);
			};
			buttonLayout.Add(MetroLabel.Create(item.DisplayName));
			ShowroomProfile showroomProfile = AutoSingleton<ShowroomDatabase>.Instance.GetShowroomProfile(item);
			bool unlocked = showroomProfile.IsUnlocked();
			MetroWidget child = MetroSpacer.Create((!unlocked) ? 3f : 4f).AddSolidBackground().SetMaterial(item._previewMaterial);
			buttonLayout.Add(child);
			if (!unlocked)
			{
				MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(item.Price);
				metroWidgetPrice.SetTextColor(Color.black);
				metroWidgetPrice.SetIconScale(0.45f).SetFont(MetroSkin.DefaultFont);
				buttonLayout.Add(metroWidgetPrice);
			}
			Showroom s = item;
			metroButton.OnButtonClicked += delegate
			{
				if (unlocked)
				{
					SelectShowroom(s);
				}
				else
				{
					AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupUnlockShowroom>().Setup(s), MetroAnimation.popup);
				}
			};
			metroGrid.Add(metroButton);
			if (metroGrid.IsFull())
			{
				num2++;
				_pager.Add(CreatePagerChild(metroGrid, num2 == 1, num2 == num));
				metroGrid = CreateGrid();
			}
		}
		if (!metroGrid.IsEmpty())
		{
			num2++;
			_pager.Add(CreatePagerChild(metroGrid, num2 == 1, num2 == num));
		}
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(2f);
		metroLayout.Add(metroLayout2);
		MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconBack, "BACK");
		metroButton2.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
		};
		metroLayout2.Add(metroButton2);
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		metroLayout2.Add(MetroPagerIndicator.Create(_pager));
		metroLayout2.Add(MetroSpacer.Create(0.5f));
		metroLayout2.Add(MetroSpacer.Create(1f));
		base.OnStart();
	}

	private void SelectShowroom(Showroom showroom)
	{
		AutoSingleton<ShowroomManager>.Instance.SetShowroom(showroom);
		AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
	}
}
