using UnityEngine;

public class MetroButtonCar : MetroButton
{
	private Car _car;

	private CarProfile _profile;

	private MetroLabel _carName;

	private MetroIcon _carIcon;

	private MetroIcon _lockIcon;

	private MetroSpacer _spacerForLabel;

	private bool _unlocked;

	public Car GetCar()
	{
		return _car;
	}

	public static MetroButtonCar Create(Car car, float scale)
	{
		GameObject gameObject = new GameObject(car.Id);
		gameObject.transform.position = Vector3.zero;
		MetroButtonCar metroButtonCar = gameObject.AddComponent<MetroButtonCar>();
		metroButtonCar.Setup(car, scale);
		return metroButtonCar;
	}

	private void Setup(Car car, float scale)
	{
		_car = car;
		MetroWidget metroWidget = MetroLayout.Create(_car.Id + "layout", Direction.vertical);
		Add(metroWidget);
		metroWidget.Add(MetroSpacer.Create(0.5f));
		_carIcon = MetroIcon.Create(_car);
		_carIcon.SetMass(1.5f);
		_carIcon.SetScale(scale);
		_carIcon.SetAlignment(MetroAlign.Bottom);
		SetCarIconColor(Color.black);
		metroWidget.Add(_carIcon);
		MetroSpacer metroSpacer = MetroSpacer.Create();
		metroSpacer.SetPadding(MetroSkin.Padding * 2f, MetroSkin.Padding);
		metroSpacer.AddSlice9Background(MetroSkin.Slice9BlackCircle);
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		metroSpacer.Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(4f));
		_lockIcon = MetroIcon.Create(MetroSkin.IconLock);
		_lockIcon.SetScale(0.35f);
		metroLayout.Add(_lockIcon);
		metroWidget.Add(metroSpacer);
		_carName = MetroLabel.Create(_car.DisplayName);
		_carName.SetFont(MetroSkin.SmallFont);
		_carName.SetColor(new Color(0.5f, 0.5f, 0.5f));
		metroSpacer.Add(_carName);
		_spacerForLabel = metroSpacer;
		PollUnlocked();
	}

	protected override void OnButtonUpdate()
	{
		PollUnlocked();
		base.OnButtonUpdate();
	}

	public override MetroWidget AddSlice9Background(string color)
	{
		_spacerForLabel.AddSlice9Background(color);
		return this;
	}

	private void PollUnlocked()
	{
		if (!_unlocked)
		{
			if (_profile == null)
			{
				_profile = AutoSingleton<CarManager>.Instance.GetCarProfile(_car);
			}
			if (_profile.IsUnlocked())
			{
				_unlocked = true;
				ShowUnlocked();
			}
		}
	}

	private void ShowUnlocked()
	{
		_carName.SetColor(Color.white);
		SetCarIconColor(Color.white);
		_lockIcon.Destroy();
	}

	private void SetCarIconColor(Color color)
	{
		tk2dSprite[] componentsInChildren = _carIcon.GetComponentsInChildren<tk2dSprite>();
		foreach (tk2dSprite tk2dSprite in componentsInChildren)
		{
			tk2dSprite.color = color;
		}
	}
}
