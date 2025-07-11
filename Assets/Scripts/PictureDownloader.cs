using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PictureDownloader : AutoSingleton<PictureDownloader>
{
	private const int _maxRequest = 5;

	private Dictionary<string, string> _pending;

	private Dictionary<string, string> _downloading;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_pending = new Dictionary<string, string>();
		_downloading = new Dictionary<string, string>();
	}

	public void SaveWebImagesToDisk(string path, string url)
	{
		if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(url))
		{
			if (!_pending.ContainsKey(path) && !_downloading.ContainsKey(path) && !RoofdogFileUtil.ExistsAndValid(path))
			{
				_pending[path] = url;
			}
			ScheduleDownloads();
		}
	}

	private void OnPictureDownloaded(string filename, string url)
	{
		if (_downloading.ContainsKey(filename))
		{
			_downloading.Remove(filename);
		}
		else
		{
			SilentDebug.LogWarning("The picture wasn't in download list: " + filename + ", url: " + url);
		}
		ScheduleDownloads();
	}

	private void ScheduleDownloads()
	{
		List<string> list = new List<string>(_pending.Keys);
		foreach (string item in list)
		{
			if (_downloading.Count >= 5)
			{
				break;
			}
			string text = _pending[item];
			_downloading[item] = text;
			_pending.Remove(item);
			StartCoroutine(LoadJpgCR(item, text));
		}
	}

	private IEnumerator LoadJpgCR(string filename, string url)
	{
		WWW req = new WWW(url);
		yield return req;
		if (string.IsNullOrEmpty(req.error) && req.bytes.Length > 0 && _downloading.ContainsKey(filename))
		{
			try
			{
				File.WriteAllBytes(filename, req.bytes);
			}
			catch (Exception ex)
			{
				Exception e = ex;
				SilentDebug.LogWarning("Error while File.WriteAllBytes(): " + e.ToString());
			}
		}
		OnPictureDownloaded(filename, url);
	}

	public void Cancel(string filename)
	{
		if (_pending.ContainsKey(filename))
		{
			_pending.Remove(filename);
		}
		if (_downloading.ContainsKey(filename))
		{
			_downloading.Remove(filename);
		}
	}
}
