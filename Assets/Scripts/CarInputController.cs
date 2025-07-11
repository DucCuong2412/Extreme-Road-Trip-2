using UnityEngine;

public abstract class CarInputController : MonoBehaviour
{
	protected float _tilt;

	protected bool _slam;

	public bool InputEnabled
	{
		get;
		set;
	}

	public float Tilt => _tilt;

	public bool HasTiltedRight
	{
		get;
		set;
	}

	public bool Slam => _slam;

	private void Awake()
	{
		HasTiltedRight = false;
	}
}
