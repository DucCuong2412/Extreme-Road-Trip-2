using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ScreenshotManager : AutoSingleton<ScreenshotManager>
{
	private static string _filename = "GameScreenshot.png";

	public static string GetPath()
	{
		return Application.persistentDataPath + "/" + _filename;
	}

	public void TakeScreenshot(Action<byte[]> onScreenshotDone)
	{
		StartCoroutine(TakeScreenshotCR(onScreenshotDone));
	}

	private IEnumerator TakeScreenshotCR(Action<byte[]> onScreenshotDone)
	{
		if (IsScreenshotTaken())
		{
			File.Delete(GetPath());
		}
		string screenshotPath = _filename;
		UnityEngine.ScreenCapture.CaptureScreenshot(screenshotPath);
		yield return StartCoroutine(WaitForScreenshot());
		if (onScreenshotDone != null && IsScreenshotTaken())
		{
			onScreenshotDone(File.ReadAllBytes(GetPath()));
		}
		yield return null;
	}

	private IEnumerator WaitForScreenshot()
	{
		Duration delay = new Duration(5f);
		while (!IsScreenshotTaken() && !delay.IsDone())
		{
			yield return new WaitForSeconds(0.1f);
		}
	}

	private bool IsScreenshotTaken()
	{
		return File.Exists(GetPath());
	}
}
