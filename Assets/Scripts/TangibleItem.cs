using UnityEngine;

public abstract class TangibleItem : CollidableItem
{
	public TangibleItemSound _sound;

	public float _velocityFactor = 1f;

	public RandomRange _randomX = new RandomRange(2f, 4f);

	public RandomRange _randomY = new RandomRange(1f, 4f);

	public RandomRange _randomAngularVelocity = new RandomRange(-5f, 5f);

	public RandomRange _drag = new RandomRange(0f, 0.1f);

	protected Rigidbody AddRigidbody(GameObject go, CarController car)
	{
		if (go.GetComponent<Rigidbody>() == null)
		{
			go.AddComponent<Rigidbody>();
		}
		Rigidbody rigidbody = go.GetComponent<Rigidbody>();
		rigidbody.constraints = (RigidbodyConstraints)56;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		rigidbody.mass = 0.01f;
		rigidbody.drag = _drag.Pick();
		Vector3 velocity = car.Velocity;
		Vector3 a = velocity;
		a.y = Mathf.Max(2f, a.y);
		rigidbody.velocity = a * _velocityFactor + new Vector3(_randomX.Pick(), _randomY.Pick(), 0f);
		rigidbody.angularVelocity = new Vector3(0f, 0f, _randomAngularVelocity.Pick());
		return rigidbody;
	}
}
