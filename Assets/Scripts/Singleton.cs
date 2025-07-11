using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance => _instance;

	public void Awake()
	{
		if ((Object)_instance == (Object)null)
		{
			_instance = (this as T);
			OnAwake();
		}
		else
		{
			UnityEngine.Debug.LogWarning("Creating more than one instance of a singleton: " + typeof(T).ToString());
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static bool IsCreated()
	{
		return (Object)_instance != (Object)null;
	}

	public void Start()
	{
		OnStart();
	}

	protected virtual void OnAwake()
	{
	}

	protected virtual void OnStart()
	{
	}
}
