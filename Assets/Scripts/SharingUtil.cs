using System.Collections;
using System.IO;
using UnityEngine;

public class SharingUtil : AutoSingleton<SharingUtil>
{
	public void ShareLink(string text, string url)
	{
		AndroidNative.Share(url, text);
	}

	public void ShareScreenshot(string text)
	{
		StartCoroutine(ShareScreenshotCR(text));
	}

	public void ShareScreenArea(Rect screenRect, string text)
	{
		StartCoroutine(ShareScreenAreaCR(screenRect, text));
	}

	public void ShareImage(string imagePath, string text)
	{
		if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
		{
			UnityEngine.Debug.LogError("Invalid image path: " + imagePath);
		}
		else
		{
			AndroidNative.Share(text, "Fishing Break!", imagePath);
		}
	}

	private IEnumerator ShareScreenshotCR(string text)
	{
		string imageName = "share.png";
		string path = GetImagePath(imageName);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		while (File.Exists(path))
		{
			yield return null;
		}
		UnityEngine.ScreenCapture.CaptureScreenshot(imageName);
		while (!File.Exists(path))
		{
			yield return null;
		}
		ShareImage(path, text);
	}

	private IEnumerator ShareScreenAreaCR(Rect area, string text)
	{
		yield return new WaitForEndOfFrame();
		byte[] imageData = CaptureScreenArea(area);
		string path = GetImagePath("share.png");
		File.WriteAllBytes(path, imageData);
		while (!File.Exists(path))
		{
			yield return null;
		}
		ShareImage(path, text);
	}

	public static string GetImagePath(string imageName)
	{
		return Path.Combine(Application.persistentDataPath, imageName);
	}

	private byte[] CaptureScreenArea(Rect area)
	{
		Texture2D texture2D = new Texture2D((int)area.width, (int)area.height, TextureFormat.RGB24, mipChain: false);
		texture2D.ReadPixels(area, 0, 0);
		texture2D.Apply();
		return texture2D.EncodeToPNG();
	}
}
