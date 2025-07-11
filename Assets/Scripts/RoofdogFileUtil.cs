using System;
using System.IO;
using UnityEngine;

public static class RoofdogFileUtil
{
	public static bool ExistsAndValid(string path)
	{
		if (File.Exists(path))
		{
			try
			{
				byte[] array = File.ReadAllBytes(path);
				if (array != null && array.Length > 0)
				{
					return true;
				}
				UnityEngine.Debug.LogWarning("This file is corrupted, delete it! " + path);
				File.Delete(path);
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogWarning("Error while File.ReadAllBytes()");
			}
		}
		return false;
	}

	public static Texture2D LoadImage(string path)
	{
		if (File.Exists(path))
		{
			try
			{
				byte[] array = File.ReadAllBytes(path);
				if (array != null && array.Length > 0)
				{
					Texture2D texture2D = new Texture2D(0, 0);
					texture2D.LoadImage(array);
					return texture2D;
				}
				UnityEngine.Debug.LogWarning("This file is corrupted, delete it! " + path);
				File.Delete(path);
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogWarning("Error while File.ReadAllBytes()");
			}
		}
		return null;
	}

	public static void Delete(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch (Exception)
		{
			UnityEngine.Debug.LogWarning(("Error while deleting file: " + path) ?? string.Empty);
		}
	}
}
