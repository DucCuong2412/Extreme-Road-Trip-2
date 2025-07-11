using UnityEngine;

public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				GameObject gameObject = new GameObject(typeof(T).ToString());
				_instance = gameObject.AddComponent<T>();
			}
			return _instance;
		}
	}

	public void Awake()
	{
		Debug.Log("Awake cccccccccccc: " + typeof(T).ToString());
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

	public void Create()
	{
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

	protected virtual void OnDestroy()
	{
		_instance = (T)null;
	}

	public virtual void Destroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
