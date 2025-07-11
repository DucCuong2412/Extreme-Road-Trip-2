using UnityEngine;

public class BaseJumper : MonoBehaviour
{
	private BaseJumpInputController _input;

	private Rigidbody _rigidbody;

	private float _tiltVelocity = 7f;

	private float _tiltOppositeBoost = 1.3f;

	private float _maxVelocity = 50f;

	private bool _crashed;

	private void Start()
	{
		_input = GetComponent<BaseJumpInputController>();
		_rigidbody = GetComponent<Rigidbody>();
		Singleton<BaseJumpCamera>.Instance.SetTarget(base.transform);
		_rigidbody.velocity = new Vector3(UnityEngine.Random.Range(4f, 6f), UnityEngine.Random.Range(4f, 6f), 0f);
		_rigidbody.angularVelocity = new Vector3(0f, 0f, UnityEngine.Random.Range(60f, 80f));
	}

	public bool IsCrashed()
	{
		return _crashed;
	}

	public void OnCollisionEnter(Collision coll)
	{
		if (coll.relativeVelocity.magnitude > 5f)
		{
			_crashed = true;
		}
	}

	private void FixedUpdate()
	{
		float tilt = _input.Tilt;
		if (tilt != 0f)
		{
			Vector3 angularVelocity = _rigidbody.angularVelocity;
			float z = angularVelocity.z;
			float num = 1f;
			if (tilt * z < 0f)
			{
				num = _tiltOppositeBoost;
			}
			_rigidbody.angularVelocity += new Vector3(0f, 0f, _tiltVelocity * tilt * num * Time.deltaTime);
		}
		Vector3 lhs = base.transform.rotation * Vector3.right;
		Vector3 normalized = _rigidbody.velocity.normalized;
		float num2 = Vector3.Dot(lhs, normalized);
		float num3 = (!_input.Slam) ? 5f : 7f;
		float num4 = Mathf.Max(0f, num3 * _rigidbody.mass * num2);
		Vector3 vector = base.transform.rotation * new Vector3(0f - num4, 0f, 0f);
		UnityEngine.Debug.DrawLine(base.transform.position, base.transform.position + vector, Color.red);
		_rigidbody.AddForce(vector);
		float magnitude = _rigidbody.velocity.magnitude;
		if (magnitude > _maxVelocity)
		{
			_rigidbody.velocity *= _maxVelocity / magnitude;
		}
	}
}
