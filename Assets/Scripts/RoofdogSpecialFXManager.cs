using System.Collections.Generic;
using UnityEngine;

public class RoofdogSpecialFXManager<T> : PrefabSingleton<T> where T : MonoBehaviour
{
	private Dictionary<Transform, Transform> _fxCache;

    private void Awake()
    {
        OnAwake();	
    }
    protected void OnAwake()
	{
		_fxCache = new Dictionary<Transform, Transform>();
	}

    public Transform InstantiateFX(Transform prefab)
    {
        if (_fxCache == null)
        {
            Debug.LogError("FX Cache is null! Did you forget to call OnAwake()?");
            _fxCache = new Dictionary<Transform, Transform>();
        }

        if (prefab == null)
        {
            Debug.LogWarning("InstantiateFX called with null prefab.");
            return null;
        }

        if (!_fxCache.ContainsKey(prefab))
        {
            _fxCache[prefab] = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        return _fxCache[prefab];
    }
}
