using UnityEngine;

public class CarShadow : MonoBehaviour
{
	private Projector _projector;

	private CarController _car;

	private float _invAspect;

	public void Awake()
	{
		_projector = GetComponent<Projector>();
		_invAspect = 1f / _projector.aspectRatio;
	}

	public void Setup(CarController car)
	{
		_car = car;
	}

	private void LateUpdate()
	{
		if (!(_car == null))
		{
			if (_car.IsCrashed())
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Bounds visualBounds = _car.GetVisualBounds();
			Vector3 center = visualBounds.center;
			float groundHeight = AutoSingleton<GroundManager>.Instance.GetGroundHeight(center.x);
			float num = (center.y - groundHeight) * 0.05f;
			base.transform.position = new Vector3(center.x, groundHeight + 0.2f * num, -2f);
			Vector3 size = visualBounds.size;
			float num2 = size.x * _invAspect * 0.75f;
			_projector.orthographicSize = Mathf.Lerp(num2, num2 * 0.3f, num);
		}
	}
}
