using UnityEngine;

public static class PictureManager
{
	public static void StorePicture(string filename, string url)
	{
		string path = PicturePath(filename);
		if (!IsLoaded(path))
		{
			Store(path, url);
		}
	}

	public static MetroPng CreateMetroPicture(string pictureId)
	{
		MetroPng result = null;
		string text = PicturePath(pictureId);
		if (RoofdogFileUtil.ExistsAndValid(text))
		{
			result = MetroPng.CreateFromUrl("file://" + text);
		}
		return result;
	}

	public static bool IsPictureLoaded(string filename)
	{
		string path = PicturePath(filename);
		return IsLoaded(path);
	}

	private static bool IsLoaded(string path)
	{
		return PicturePathIfExist(path) != null;
	}

	public static void Delete(string filename)
	{
		string text = PicturePath(filename);
		RoofdogFileUtil.Delete(text);
		if (AutoSingleton<PictureDownloader>.IsCreated())
		{
			AutoSingleton<PictureDownloader>.Instance.Cancel(text);
		}
	}

	private static void Store(string path, string url)
	{
		AutoSingleton<PictureDownloader>.Instance.SaveWebImagesToDisk(path, url);
	}

	private static string PicturePathIfExist(string path)
	{
		if (RoofdogFileUtil.ExistsAndValid(path))
		{
			return path;
		}
		return null;
	}

	private static string PicturePath(string filename)
	{
		filename = filename.Replace("G:", "G");
		string str = ".png";
		return Application.persistentDataPath + "/" + filename + str;
	}
}
