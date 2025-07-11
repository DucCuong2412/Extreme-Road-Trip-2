using UnityEngine;

public class MetroButtonShowroomCar : MetroButton
{
	public Car Car
	{
		get;
		private set;
	}

	public static MetroButtonShowroomCar Create(Transform carSlot, Car car, int upgradeLevel, int carSlotIndex, float carPositionOffset, float carIconScale)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = Vector3.zero;
		MetroButtonShowroomCar metroButtonShowroomCar = gameObject.AddComponent<MetroButtonShowroomCar>();
		metroButtonShowroomCar.Setup(carSlot, car, upgradeLevel, carSlotIndex, carPositionOffset, carIconScale);
		return metroButtonShowroomCar;
	}

	public void OnFinger(Finger finger)
	{
		if (TouchIsInZone(finger.Touch))
		{
			HandleFinger(finger);
		}
	}

	private void Setup(Transform carSlot, Car car, int upgradeLevel, int carSlotIndex, float carPositionOffset, float carIconScale)
	{
		base.IsKeyNavigatorAccessible = false;
		_transform = base.transform;
		_transform.parent = carSlot;
		Car = car;
		base.OnButtonClicked += delegate
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(carSlotIndex));
		};
		Transform child = carSlot.GetChild(0);
		Vector3 localPosition = carSlot.localPosition;
		carSlot.localPosition = new Vector3(localPosition.x * carPositionOffset, localPosition.y * carPositionOffset, localPosition.z);
		child.localScale = new Vector3(carPositionOffset, carPositionOffset, 1f);
		Vector3 extents = child.GetComponent<Renderer>().bounds.extents;
		float y = extents.y;
		Bounds bounds = RendererBounds.ComputeBounds(carSlot);
		if (Car != null)
		{
			MetroIcon metroIcon = MetroIcon.Create(Car, asPrefab: true);
			metroIcon.SetScale(carIconScale);
			metroIcon.transform.localScale = new Vector3(carPositionOffset, carPositionOffset, 1f);
			metroIcon.SetAlignment(MetroAlign.Bottom);
			metroIcon.SetPadding(0f);
			Add(metroIcon);
			_transform.localPosition = Vector3.zero;
			MetroWidgetCarUpgrade metroWidgetCarUpgrade = MetroWidgetCarUpgrade.Create(upgradeLevel, 0.55f);
			metroWidgetCarUpgrade.transform.parent = carSlot;
			metroWidgetCarUpgrade.transform.localPosition = new Vector3(0f, -2.75f, 0f);
			bounds = RendererBounds.ComputeBounds(carSlot);
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
		else
		{
			MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
			Add(metroLayout);
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
			metroLayout2.SetMass(2f);
			MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).AddSlice9Background(MetroSkin.Slice9Button);
			metroWidget.SetMass(2f);
			metroLayout.Add(MetroSpacer.Create());
			metroLayout.Add(metroLayout2);
			metroLayout.Add(MetroSpacer.Create());
			metroLayout2.Add(MetroSpacer.Create());
			metroLayout2.Add(metroWidget);
			metroLayout2.Add(MetroSpacer.Create());
			MetroLabel metroLabel = MetroLabel.Create("ADD CAR");
			metroWidget.Add(metroLabel);
			metroLabel.SetFont(MetroSkin.SmallFont);
			_transform.localPosition = child.localPosition;
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
		Vector3 size = bounds.size;
		float x = size.x;
		Vector3 size2 = bounds.size;
		float y2 = size2.y;
		float left = (0f - x) * 0.5f;
		float top = (0f - y2) * 0.5f;
		Rect zone = new Rect(left, top, x, y2);
		SetPadding(0f);
		Layout(zone);
		if (Car != null)
		{
			_transform.localPosition = new Vector3(0f, y2 * 0.5f - y, 0f);
		}
	}
}
