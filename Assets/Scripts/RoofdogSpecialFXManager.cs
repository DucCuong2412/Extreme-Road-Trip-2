using System.Collections.Generic;
using UnityEngine;

public class RoofdogSpecialFXManager<T> : PrefabSingleton<T> where T : MonoBehaviour
{
	private Dictionary<Transform, Transform> _fxCache;

	protected void OnAwake()
	{
		_fxCache = new Dictionary<Transform, Transform>();
	}

	public Transform InstantiateFX(Transform prefab)
	{
		if (!_fxCache.ContainsKey(prefab))
		{
			_fxCache[prefab] = (Transform)UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		}
		return _fxCache[prefab];
	}
}
