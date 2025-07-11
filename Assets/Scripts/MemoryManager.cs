using UnityEngine;

public class MemoryManager : AutoSingleton<MemoryManager>
{
	private bool _unusedResourceUnloaded;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		base.OnAwake();
	}

	private void OnMemoryWarning(string message)
	{
		if (!_unusedResourceUnloaded)
		{
			Resources.UnloadUnusedAssets();
			_unusedResourceUnloaded = true;
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			_unusedResourceUnloaded = false;
		}
	}
}
