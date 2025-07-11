using UnityEngine;

public class RockHardCarSpecialBehaviour : CarSpecialBehaviour
{
	public Transform _Prefab;

	private CarController _carController;

	public Transform _CrashFXPrefab;

	private ParticleSystem _crashChunkFX;

	private void Start()
	{
		_carController = base.gameObject.GetComponent<CarController>();
		_carController.OnCrash += OnCrash;
		_crashChunkFX = SetupEmitter(_CrashFXPrefab, 0f, -1f);
	}

	private ParticleSystem SetupEmitter(Transform prefab, float offsetX, float offsetZ)
	{
		Transform transform = (Transform)Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		if (transform != null)
		{
			return transform.particleSystem;
		}
		return null;
	}

	private void Collapse()
	{
		Transform transform = (Transform)Object.Instantiate(_Prefab, Vector3.zero, Quaternion.identity);
		transform.position = _carController.Position;
		transform.rotation = _carController._rigidbody.rotation;
		Rigidbody[] componentsInChildren = transform.GetComponentsInChildren<Rigidbody>();
		Rigidbody[] array = componentsInChildren;
		foreach (Rigidbody rigidbody in array)
		{
			rigidbody.AddExplosionForce(500f, transform.position, 0f, 0f);
		}
		if (_crashChunkFX != null)
		{
			Vector3 position = _carController.Position;
			position.y += 1f;
			_crashChunkFX.gameObject.transform.position = position;
			_crashChunkFX.Play();
		}
	}

	private void OnCrash()
	{
		Collapse();
	}
}
