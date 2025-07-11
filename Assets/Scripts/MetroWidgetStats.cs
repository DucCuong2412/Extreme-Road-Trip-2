using System.Collections.Generic;
using UnityEngine;

public class MetroWidgetStats : MetroWidget
{
	private MetroWidget _panel;

	private MetroSlider _slider;

	private List<MetroButton> _sliderButtons;

	private MetroButton _selectedButton;

	private Car _car;

	public static MetroWidgetStats Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetStats).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetStats metroWidgetStats = gameObject.AddComponent<MetroWidgetStats>();
		metroWidgetStats.Setup();
		return metroWidgetStats;
	}

	private void Setup()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).SetMass(9f);
		metroLayout.Add(metroWidget);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		metroWidget.Add(metroLayout2);
		_sliderButtons = new List<MetroButton>();
		_slider = MetroSlider.Create(Direction.vertical, 3.5f);
		_slider.SetMass(0.7f);
		metroLayout2.Add(_slider);
		MetroButton globalStatsButton = MetroButton.Create("OVERALL");
		globalStatsButton.OnButtonClicked += delegate
		{
			SelectButton(globalStatsButton);
		};
		_slider.Add(globalStatsButton);
		_sliderButtons.Add(globalStatsButton);
		List<Car> allCars = AutoSingleton<CarDatabase>.Instance.GetAllCars();
		foreach (Car item in allCars)
		{
			CarProfile carProfile = AutoSingleton<CarManager>.Instance.GetCarProfile(item);
			if (carProfile.IsUnlocked())
			{
				MetroButton button = MetroButtonCar.Create(item, 1f);
				button.OnButtonClicked += delegate
				{
					SelectButton(button);
				};
				MetroMenu.AddKeyNavigationBehaviour(button, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
				_slider.Add(button);
				_sliderButtons.Add(button);
			}
		}
		MetroWidget metroWidget2 = MetroLayout.Create(Direction.horizontal).SetMass(2f);
		metroWidget.Add(metroWidget2);
		_panel = MetroSpacer.Create("_panel");
		metroWidget2.Add(_panel);
		if (_car == null)
		{
			SelectButton(_sliderButtons[0]);
		}
		else
		{
			foreach (MetroButton sliderButton in _sliderButtons)
			{
				if (sliderButton is MetroButtonCar)
				{
					MetroButtonCar metroButtonCar = sliderButton as MetroButtonCar;
					if (metroButtonCar.GetCar() == _car)
					{
						SelectButton(sliderButton);
						break;
					}
				}
			}
		}
	}

	private void SelectButton(MetroButton button)
	{
		if (_selectedButton != button)
		{
			_sliderButtons.ForEach(delegate(MetroButton b)
			{
				b.SetColor(MetroSkin.OptionsBackgroundColor);
			});
			button.SetColor(MetroSkin.StatsSelectedItemColor);
			_selectedButton = button;
			_slider.Focus(_selectedButton);
			RefreshPanel();
		}
	}

	private void RefreshPanel()
	{
		MetroWidget metroWidget = null;
		if (_selectedButton is MetroButtonCar)
		{
			MetroButtonCar metroButtonCar = _selectedButton as MetroButtonCar;
			metroWidget = MetroPanelStatsCar.Create(metroButtonCar.GetCar());
		}
		else
		{
			metroWidget = MetroPanelStatsGlobal.Create();
		}
		_panel.Replace(metroWidget);
		UnityEngine.Object.Destroy(_panel.gameObject);
		_panel = metroWidget;
		_panel.Parent.Reflow();
	}
}
