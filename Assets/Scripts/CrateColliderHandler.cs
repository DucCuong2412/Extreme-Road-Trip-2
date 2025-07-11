using UnityEngine;

public class CrateColliderHandler : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == GameSettings.GroundColliderTag)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			Explode(base.gameObject.transform.position);
		}
	}

	private void Explode(Vector3 position)
	{
		PrefabSingleton<GameSpecialFXManager>.Instance.ExplodeCrate(position);
		PrefabSingleton<GameSoundManager>.Instance.PlayExplosionSound();
	}
}
