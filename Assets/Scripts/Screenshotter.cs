using UnityEngine;

public class Screenshotter : MonoBehaviour
{
	private string _path = "/Users/guillaume/Desktop/Screenshots";

	private int _r;

	private void Awake()
	{
		if (Application.isEditor)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		_r = UnityEngine.Random.Range(0, 1000000);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKey(KeyCode.S))
		{
			UnityEngine.ScreenCapture.CaptureScreenshot($"{_path:s}/screenshot_{_r}_{Time.frameCount}.png");
		}
	}
}
