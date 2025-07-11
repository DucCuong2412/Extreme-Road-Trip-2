using UnityEngine;

public class CarPhysics : AutoSingleton<CarPhysics>
{
	private PhysicMaterial _bumper;

	private PhysicMaterial _tire;

	public PhysicMaterial Bumper
	{
		get
		{
			if (_bumper == null)
			{
				_bumper = (Resources.Load("Bumper") as PhysicMaterial);
			}
			return _bumper;
		}
	}

	public PhysicMaterial Tire
	{
		get
		{
			if (_tire == null)
			{
				_tire = (Resources.Load("Tire") as PhysicMaterial);
			}
			return _tire;
		}
	}

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		base.OnAwake();
	}
}
