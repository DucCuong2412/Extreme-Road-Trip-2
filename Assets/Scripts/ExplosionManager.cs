using UnityEngine;

public class ExplosionManager : AutoSingleton<ExplosionManager>
{
	private Transform _explosion;

	protected override void OnAwake()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("DetonatorMobile"), Vector3.zero, Quaternion.identity);
		if (gameObject != null)
		{
			_explosion = gameObject.transform;
		}
		base.OnAwake();
	}

	protected override void OnStart()
	{
		Prime();
		base.OnStart();
	}

	private void Prime()
	{
		if (_explosion != null)
		{
			Vector3 outOfWorldVector = GameSettings.OutOfWorldVector;
			_explosion.position = outOfWorldVector;
			_explosion.GetComponent<Detonator>().Explode();
		}
	}

	public void Explode(Vector3 position, bool shake = true, bool useSound = true)
	{
		if (_explosion != null)
		{
			_explosion.position = position;
			_explosion.GetComponent<Detonator>().Explode();
		}
		if (shake && Singleton<GameManager>.Instance != null)
		{
			PrefabSingleton<CameraGame>.Instance.Shake(2f, 3f);
		}
		if (useSound)
		{
			PrefabSingleton<GameSoundManager>.Instance.PlayExplosionSound();
		}
	}
}
