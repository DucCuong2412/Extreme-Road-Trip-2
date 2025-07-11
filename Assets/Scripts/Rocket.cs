using UnityEngine;

public class Rocket : MonoBehaviour
{
	public Transform _liftoffFX;

	private bool _liftoff;

	public void LiftOff()
	{
		Object.Instantiate(_liftoffFX, base.transform.position, Quaternion.identity);
		ConstantForce constantForce = base.gameObject.AddComponent<ConstantForce>();
		constantForce.force = new Vector3(0f, 12f, 0f);
		_liftoff = true;
	}

	public void Start()
	{
		GetComponent<Rigidbody>().Sleep();
		Singleton<RocketCamera>.Instance.SetTarget(base.transform);
	}

	public void Update()
	{
		if (_liftoff)
		{
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
	}
}
