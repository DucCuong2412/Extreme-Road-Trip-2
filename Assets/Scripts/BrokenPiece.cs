using UnityEngine;

public class BrokenPiece : MonoBehaviour
{
	private RandomRange _randomX = new RandomRange(-25f, 25f);

	private RandomRange _randomY = new RandomRange(20f, 30f);

	private RandomRange _randomAngularVelocity = new RandomRange(-15f, 15f);

	private RandomRange _drag = new RandomRange(0f, 0.1f);

	public void Start()
	{
		base.transform.parent = null;
		AddRigidbody();
		base.gameObject.AddComponent<ConstantForce>().force = new Vector3(0f, -80f, 0f);
	}

	private Rigidbody AddRigidbody()
	{
		Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
		rigidbody.constraints = (RigidbodyConstraints)56;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		rigidbody.mass = 1f;
		rigidbody.drag = _drag.Pick();
		rigidbody.velocity = new Vector3(_randomX.Pick(), _randomY.Pick(), 0f);
		rigidbody.angularVelocity = new Vector3(0f, 0f, _randomAngularVelocity.Pick());
		return rigidbody;
	}

	private void OnBecameInvisible()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
