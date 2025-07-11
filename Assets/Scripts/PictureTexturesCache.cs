using System.Collections.Generic;
using UnityEngine;

public class PictureTexturesCache : AutoSingleton<PictureTexturesCache>
{
	private const int _maxCache = 50;

	private Dictionary<string, Texture> _cache;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_cache = new Dictionary<string, Texture>();
	}

	public Texture GetTexture(string path)
	{
		if (_cache.ContainsKey(path))
		{
			return _cache[path];
		}
		return null;
	}

	public void UpdateCache(string path, Texture texture)
	{
		if (_cache.Count < 50 && texture != null)
		{
			_cache[path] = texture;
		}
	}
}
