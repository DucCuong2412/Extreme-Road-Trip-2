using System;
using System.Collections;
using UnityEngine;

public class ProxyCoroutine : MonoBehaviour
{
	private void StartCoroutineImpl(Func<IEnumerator> coroutine)
	{
		StartCoroutine(StartCoroutineImplCR(coroutine));
	}

	private IEnumerator StartCoroutineImplCR(Func<IEnumerator> coroutine)
	{
		yield return StartCoroutine(coroutine());
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static void Go(Func<IEnumerator> coroutine)
	{
		GameObject gameObject = new GameObject();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		gameObject.AddComponent<ProxyCoroutine>().StartCoroutineImpl(coroutine);
	}
}
