using System;
using System.Collections;
using UnityEngine;

public class CameraBackground : PrefabSingleton<CameraBackground>
{
	private float _halfScreenWidth;

	private float _halfScreenHeight;

	private Camera _camera;

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

	public float ScreenWidth()
	{
		return _halfScreenWidth * 2f;
	}

	public float ScreenHeight()
	{
		return _halfScreenHeight * 2f;
	}

	protected override void OnAwake()
	{
		float orthographicSize = 16f;
		Camera.orthographicSize = orthographicSize;
		_halfScreenWidth = (float)Screen.width / (float)Screen.height * Camera.orthographicSize;
		_halfScreenHeight = Camera.orthographicSize;
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
