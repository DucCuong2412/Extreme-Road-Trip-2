using UnityEngine;

public class PrefabSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				string path = typeof(T).ToString();
				_instance = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load(path))).GetComponent<T>();
			}
			return _instance;
		}
	}

	public void Awake()
	{
		if ((Object)_instance == (Object)null)
		{
			_instance = (this as T);
			OnAwake();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Start()
	{
		OnStart();
	}

	public virtual void Create()
	{
	}

	protected virtual void OnAwake()
	{
	}

	protected virtual void OnStart()
	{
	}

	protected virtual void Destroy()
	{
		_instance = (T)null;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
