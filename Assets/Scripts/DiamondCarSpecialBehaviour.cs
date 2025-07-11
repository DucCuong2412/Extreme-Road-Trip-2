using UnityEngine;

public class DiamondCarSpecialBehaviour : CarSpecialBehaviour
{
	public Transform _diamondChunkFXPrefab;

	private ParticleSystem _diamondChunkFX;

	private CarController _carController;

	private void Start()
	{
		_carController = base.gameObject.GetComponent<CarController>();
		_carController.OnCrash += OnCrash;
		_diamondChunkFX = SetupEmitter(_diamondChunkFXPrefab, 0f, -1f);
	}

	private void Explode()
	{
		if (_diamondChunkFX != null)
		{
			Vector3 position = _carController.Position;
			position.y += 1f;
			_diamondChunkFX.gameObject.transform.position = position;
			_diamondChunkFX.Play();
		}
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

	private void OnCrash()
	{
		Explode();
	}
}
