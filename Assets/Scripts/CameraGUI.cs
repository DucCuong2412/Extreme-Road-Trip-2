using System;
using System.Collections;
using UnityEngine;

public class CameraGUI : PrefabSingleton<CameraGUI>
{
	public bool _createAudioListener;

	private Camera _camera;

	public float HalfScreenWidth
	{
		get;
		private set;
	}

	public float HalfScreenHeight
	{
		get;
		private set;
	}

	public float ScreenWidth => HalfScreenWidth * 2f;

	public float ScreenHeight => HalfScreenHeight * 2f;

	public Camera Camera
	{
		get
		{
			if (_camera == null)
			{
				_camera = GetComponentInChildren<Camera>();
			}
			return _camera;
		}
	}

	public float PixelToUnit => 30f;

	protected override void OnAwake()
	{
		float orthographicSize = 16f;
		Camera.orthographicSize = orthographicSize;
		if (_createAudioListener && Camera.GetComponent<AudioListener>() == null)
		{
			Camera.gameObject.AddComponent<AudioListener>();
		}
		HalfScreenWidth = (float)Screen.width / (float)Screen.height * Camera.orthographicSize;
		HalfScreenHeight = Camera.orthographicSize;
	}

	public void Shake()
	{
		StartCoroutine(ShakeCR());
	}

	private IEnumerator ShakeCR()
	{
		Transform t = _camera.transform;
		Vector3 position = t.position;
		Duration delay = new Duration(0.25f);
		float amplitude = 1.5f;
		while (!delay.IsDone())
		{
			t.position = position + new Vector3(Mathf.Cos(delay.Value01() * (float)Math.PI * 2f * 7f) * amplitude * (1f - delay.Value01()), Mathf.Cos(delay.Value01() * (float)Math.PI * 2f * 11f) * amplitude * (1f - delay.Value01()), 0f);
			yield return null;
		}
		t.position = position;
	}
}
