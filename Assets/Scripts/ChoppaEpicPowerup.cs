using UnityEngine;

public class ChoppaEpicPowerup : EpicPowerup
{
	private CarController _car;

	private GameObject _choppaGO;

	private GameObject _carGO;

	private GameObject _carVisualsInstance;

	public ChoppaEpicPowerup(Price price)
		: base(price)
	{
	}

	public override EpicPowerupType GetEpicPowerupType()
	{
		return EpicPowerupType.choppa;
	}

	public override string GetIconPath()
	{
		return MetroSkin.IconChoppa;
	}

	public override string GetDescription(int streak)
	{
		switch (streak)
		{
		case 1:
			return "Get in the chopper! It's cheaper this time!".Localize();
		case 2:
			return "We're going insane! Let's make it even cheaper this time!".Localize();
		default:
			return "It's an evac emergency! Do you want to zip ahead in the chopper?".Localize();
		}
	}

	public override CarController Setup(Car car)
	{
		_carGO = (Object.Instantiate(car.GamePrefab, Vector3.zero, Quaternion.identity) as GameObject);
		_car = _carGO.GetComponent<CarController>();
		_choppaGO = (Object.Instantiate(Resources.Load("Choppa")) as GameObject);
		Transform transform = _choppaGO.transform.FindChild("CarAnchor");
		if (transform != null)
		{
			_car._rigidbody.isKinematic = true;
			_car._rigidbody.useGravity = false;
			_car._rigidbody.interpolation = RigidbodyInterpolation.None;
			_car.transform.parent = transform;
			_car.transform.position = transform.position;
			_car.transform.localRotation = Quaternion.identity;
			Renderer[] componentsInChildren = _carGO.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				renderer.enabled = false;
			}
			GameObject visualPrefab = car.VisualPrefab;
			_carVisualsInstance = (Object.Instantiate(visualPrefab, Vector3.zero, Quaternion.identity) as GameObject);
			_carVisualsInstance.transform.parent = transform;
			RecursiveLayerChange(_carVisualsInstance, LayerMask.NameToLayer(GameSettings.CarLayer));
			Bounds bounds = RendererBounds.ComputeBounds(_carVisualsInstance.transform);
			_carVisualsInstance.transform.localPosition = Vector3.zero;
			_carVisualsInstance.transform.localRotation = Quaternion.identity;
			Transform transform2 = _carVisualsInstance.transform;
			Vector3 position = transform2.position;
			Vector3 up = Vector3.up;
			Vector3 center = bounds.center;
			float num = 0f - center.y;
			Vector3 extents = bounds.extents;
			transform2.position = position + up * (num + extents.y);
		}
		else
		{
			UnityEngine.Debug.LogWarning("This car is missing an anchor point for the choppa.");
		}
		ChoppaBehaviour component = _choppaGO.GetComponent<ChoppaBehaviour>();
		component.OnDestinationReached += OnChoppaDestinationReached;
		component.OnAnimationDone += OnChoppaAnimationDone;
		return _car;
	}

	public override bool CanRecordReplay()
	{
		return false;
	}

	public override bool CanPlayReplay()
	{
		return false;
	}

	public override bool TrackBests()
	{
		return false;
	}

	public override float GetGameSetupDistance()
	{
		return 5000f;
	}

	public void OnChoppaDestinationReached()
	{
		float currentSpeed = _choppaGO.GetComponent<ChoppaBehaviour>().CurrentSpeed;
		UnityEngine.Object.Destroy(_carVisualsInstance);
		Renderer[] componentsInChildren = _carGO.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			renderer.enabled = true;
		}
		_car.transform.parent = null;
		_car._rigidbody.isKinematic = false;
		_car._rigidbody.useGravity = true;
		_car._rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		_car._rigidbody.velocity = new Vector3(currentSpeed, 0f, 0f);
	}

	public void OnChoppaAnimationDone()
	{
		UnityEngine.Object.Destroy(_choppaGO);
		_choppaGO = null;
	}

	private void RecursiveLayerChange(GameObject go, int layer)
	{
		go.layer = layer;
		foreach (Transform item in go.transform)
		{
			RecursiveLayerChange(item.gameObject, layer);
		}
	}
}
