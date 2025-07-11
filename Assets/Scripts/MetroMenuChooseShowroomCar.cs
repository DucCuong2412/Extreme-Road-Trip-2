using System.Collections.Generic;
using UnityEngine;

public class MetroMenuChooseShowroomCar : MetroMenuPager
{
	private int _carSlotIndex;

	private void SelectCar(Car car)
	{
		AutoSingleton<ShowroomManager>.Instance.SetCar(car, _carSlotIndex);
		AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
	}

	protected override void OnStart()
	{
		_carSlotIndex = ((AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigShowroom)?.CarSlotIndex ?? 0);
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).AddSolidBackground().SetColor(Color.black);
		metroLayout.Add(metroWidget);
		MetroLabel metroLabel = MetroLabel.Create("SELECT A CAR");
		metroWidget.Add(metroLabel);
		metroLabel.SetFont(MetroSkin.BigFont);
		MetroSpacer metroSpacer = MetroSpacer.Create();
		metroSpacer.SetMass(7f);
		metroLayout.Add(metroSpacer);
		_pager = CreateMetroPager("ChooseShowroomCarPager");
		metroSpacer.Add(_pager);
		List<Car> allUnlockedForSaleCars = AutoSingleton<CarManager>.Instance.GetAllUnlockedForSaleCars();
		base.ColCount = 4f;
		base.RowCount = 3f;
		int num = Mathf.CeilToInt((float)allUnlockedForSaleCars.Count / (base.ColCount * base.RowCount));
		int num2 = 0;
		MetroGrid metroGrid = CreateGrid();
		foreach (Car item in allUnlockedForSaleCars)
		{
			MetroButtonShowroomCarCard carButton = MetroButtonShowroomCarCard.Create(item, (!Device.IsIPad()) ? 1.25f : 1f);
			carButton.SetPadding(0.8f);
			int page = num2;
			CarCategory cat = item.Category;
			carButton.OnKeyFocusGained += delegate
			{
				_pager.ChangePage(page);
				carButton.SwitchSlice9Background((cat == CarCategory.soldForCoins) ? MetroSkin.Slice9CardBaseCoinsSelected : ((cat != CarCategory.soldForBucks) ? MetroSkin.Slice9CardSuperSelected : MetroSkin.Slice9CardBaseBucksSelected));
				carButton.Berp();
			};
			carButton.OnKeyFocusLost += delegate
			{
				carButton.SwitchSlice9Background((cat == CarCategory.soldForCoins) ? MetroSkin.Slice9CardBaseCoins : ((cat != CarCategory.soldForBucks) ? MetroSkin.Slice9CardSuper : MetroSkin.Slice9CardBaseBucks));
			};
			Car c = item;
			carButton.OnButtonClicked += delegate
			{
				SelectCar(c);
			};
			metroGrid.Add(carButton);
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
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconBack, "BACK");
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
		};
		metroLayout2.Add(metroButton);
		metroLayout2.Add(MetroSpacer.Create(0.2f));
		metroLayout2.Add(MetroPagerIndicator.Create(_pager));
		metroLayout2.Add(MetroSpacer.Create(0.2f));
		metroLayout2.Add(MetroSpacer.Create(1f));
		base.OnStart();
	}
}
