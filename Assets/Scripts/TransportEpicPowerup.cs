using UnityEngine;

public class TransportEpicPowerup : EpicPowerup
{
	private Car _car;

	private GameObject _carVisualsInstance;

	private int _gasLevel;

	private int _speedLevel;

	private int _flipLevel;

	private int _boostLevel;

	private int _slamLevel;

	private float _mass;

	private float _suspensionDistance;

	private float _suspensionSpring;

	private float _suspensionDamper;

	public TransportEpicPowerup(Price price)
		: base(price)
	{
	}

	public override EpicPowerupType GetEpicPowerupType()
	{
		return EpicPowerupType.transport;
	}

	public override string GetIconPath()
	{
		return MetroSkin.IconTransport;
	}

	public override string GetDescription(int streak)
	{
		switch (streak)
		{
		case 1:
			return "Keep it coming! Enjoy this reduced price on the epic carrier!".Localize();
		case 2:
			return "We're going insane! Let's make it even cheaper this time!".Localize();
		default:
			return "An epic carrier is passing by! Do you want to hop on for a ride?".Localize();
		}
	}

	public override CarController Setup(Car car)
	{
		_car = car;
		Car car2 = AutoSingleton<CarManager>.Instance.GetCar("TacticalTransport");
		_gasLevel = _car.GasLevel;
		_speedLevel = _car.SpeedLevel;
		_flipLevel = _car.FlipLevel;
		_boostLevel = _car.BoostLevel;
		_slamLevel = _car.SlamLevel;
		_mass = _car.Mass;
		_suspensionDistance = _car.SuspensionDistance;
		_suspensionSpring = _car.SuspensionSpring;
		_suspensionDamper = _car.SuspensionDamper;
		_car.GasLevel = car2.GasLevel;
		_car.SpeedLevel = car2.SpeedLevel;
		_car.FlipLevel = car2.FlipLevel;
		_car.BoostLevel = car2.BoostLevel;
		_car.SlamLevel = car2.SlamLevel;
		_car.Mass = car2.Mass;
		_car.SuspensionDistance = car2.SuspensionDistance;
		_car.SuspensionSpring = car2.SuspensionSpring;
		_car.SuspensionDamper = car2.SuspensionDamper;
		GameObject gamePrefab = car2.GamePrefab;
		GameObject gameObject = Object.Instantiate(gamePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		GameObject visualPrefab = car.VisualPrefab;
		_carVisualsInstance = (Object.Instantiate(visualPrefab, Vector3.zero, Quaternion.identity) as GameObject);
		Transform transform = gameObject.transform.FindChild("CarAnchor");
		if (transform != null)
		{
			_carVisualsInstance.transform.parent = transform;
			RecursiveLayerChange(_carVisualsInstance, LayerMask.NameToLayer(GameSettings.CarLayer));
			Bounds bounds = RendererBounds.ComputeBounds(_carVisualsInstance.transform);
			Transform transform2 = _carVisualsInstance.transform;
			Vector3 center = bounds.center;
			float num = 0f - center.y;
			Vector3 extents = bounds.extents;
			transform2.localPosition = new Vector3(0f, num + extents.y, 0f);
		}
		CarController component = gameObject.GetComponent<CarController>();
		component.OnCrash += OnCrash;
		return component;
	}

	private void RecursiveLayerChange(GameObject go, int layer)
	{
		go.layer = layer;
		foreach (Transform item in go.transform)
		{
			RecursiveLayerChange(item.gameObject, layer);
		}
	}

	private void OnCrash()
	{
		_car.GasLevel = _gasLevel;
		_car.SpeedLevel = _speedLevel;
		_car.FlipLevel = _flipLevel;
		_car.BoostLevel = _boostLevel;
		_car.SlamLevel = _slamLevel;
		_car.Mass = _mass;
		_car.SuspensionDistance = _suspensionDistance;
		_car.SuspensionSpring = _suspensionSpring;
		_car.SuspensionDamper = _suspensionDamper;
		GroundManager instance = AutoSingleton<GroundManager>.Instance;
		Vector3 position = _carVisualsInstance.transform.position;
		float groundHeight = instance.GetGroundHeight(position.x);
		Vector3 position2 = _carVisualsInstance.transform.position;
		Vector3 position3 = new Vector3(position2.x, groundHeight + 1f, 0f);
		UnityEngine.Object.Destroy(_carVisualsInstance);
		GameObject gamePrefab = _car.GamePrefab;
		GameObject gameObject = Object.Instantiate(gamePrefab, position3, Quaternion.identity) as GameObject;
		Vector3 explosionPosition = new Vector3(position3.x - 3f, position3.y - 2f, position3.z);
		gameObject.rigidbody.AddExplosionForce(300000f * ((float)Device.GetFixedUpdateRate() / 60f), explosionPosition, 0f);
		Singleton<GameManager>.Instance.SetCarController(gameObject.GetComponent<CarController>());
	}

	public override bool CanRecordReplay()
	{
		return false;
	}
}
