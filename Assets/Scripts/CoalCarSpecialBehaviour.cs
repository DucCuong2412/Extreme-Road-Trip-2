using UnityEngine;

public class CoalCarSpecialBehaviour : CarSpecialBehaviour
{
	public Transform _coalChunkFXPrefab;

	private ParticleEmitter _coalChunkFX;

	private Transform _coalEmitterAnchor;

	private CarController _carController;

	private void Start()
	{
		_carController = base.gameObject.GetComponent<CarController>();
		_carController.OnSlammingGround += OnSlammingGround;
		_coalEmitterAnchor = base.transform.FindChild("CoalEmitterAnchor");
		_coalChunkFX = SetupEmitter(_coalChunkFXPrefab, 0f, -1f);
	}

	private void OnSlammingGround()
	{
		ShootCoal();
	}

	private void ShootCoal()
	{
		if (_coalChunkFX != null)
		{
			Vector3 vector = _carController.Velocity;
			vector.y = 0f;
			vector.x *= 0.5f;
			vector += new Vector3(0f, 4f, 0f);
			_coalChunkFX.worldVelocity = vector;
			_coalChunkFX.Emit();
		}
	}

	private ParticleEmitter SetupEmitter(Transform prefab, float offsetX, float offsetZ)
	{
		Transform transform = (Transform)Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		if (transform != null)
		{
			transform.parent = _coalEmitterAnchor;
			transform.localPosition = new Vector3(offsetX, 0f, offsetZ);
			return transform.particleEmitter;
		}
		return null;
	}
}
