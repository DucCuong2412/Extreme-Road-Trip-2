using System.Collections;
using UnityEngine;

public class LoadingManager : AutoSingleton<LoadingManager>
{
	private LoadConfig _currentConfig;

	private LoadConfig _nextConfig;

	private bool _loadingMutex;

	public LoadConfig GetCurrentConfig()
	{
		return _currentConfig;
	}

	public bool IsBooting()
	{
		return _currentConfig == null;
	}

	public void Load(LoadConfig loadConfig)
	{
		if (!_loadingMutex)
		{
			_loadingMutex = true;
			_nextConfig = loadConfig;
			StartCoroutine(LoadCR());
		}
	}

	public void Reload()
	{
		if (_currentConfig == null)
		{
			_currentConfig = new LoadConfigScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}
		Load(_currentConfig);
	}

	private IEnumerator LoadCR()
	{
		if (_currentConfig != null)
		{
			yield return StartCoroutine(_currentConfig.PreLoad());
			yield return StartCoroutine(_currentConfig.Unload());
		}
		_currentConfig = _nextConfig;
		_nextConfig = null;
		yield return StartCoroutine(_currentConfig.Load());
		yield return StartCoroutine(_currentConfig.PostLoad());
		_loadingMutex = false;
	}

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
