using System.Collections;
using UnityEngine;

public class TestExplosion : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Test());
	}

	private IEnumerator Test()
	{
		yield return new WaitForSeconds(1f);
		AutoSingleton<ExplosionManager>.Instance.Explode(Vector3.zero);
	}
}
