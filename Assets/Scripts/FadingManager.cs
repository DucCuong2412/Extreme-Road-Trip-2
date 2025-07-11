using System.Collections;
using UnityEngine;

public class FadingManager : AutoSingleton<FadingManager>
{
	private Camera _camera;

	private QuadColored _quad;

	protected override void OnAwake()
	{
		base.transform.position = Vector3.one * 2000f;
		GameObject gameObject = new GameObject("Quad");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.forward;
		_quad = gameObject.AddComponent<QuadColored>();
		_quad.SetColor(Color.black);
		_quad.GetComponent<Renderer>().enabled = true;
		gameObject = new GameObject("Camera");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		_camera = gameObject.AddComponent<Camera>();
		_camera.orthographic = true;
		_camera.orthographicSize = 0.1f;
		_camera.depth = 1000000f;
		_camera.clearFlags = CameraClearFlags.Nothing;
	}

	public void FadeIn()
	{
		Fade(new Color(0f, 0f, 0f, 1f), new Color(0f, 0f, 0f, 0f), destroy: true);
	}

	public void FadeOut()
	{
		Fade(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), destroy: false);
	}

	private void Fade(Color from, Color to, bool destroy)
	{
		StartCoroutine(FadeCoroutine(from, to, destroy));
	}

	private IEnumerator FadeCoroutine(Color from, Color to, bool destroy)
	{
		Duration delay = new Duration(0.3f);
		while (!delay.IsDone())
		{
			_quad.SetColor(Color.Lerp(from, to, delay.Value01()));
			yield return null;
		}
		_quad.SetColor(to);
		if (destroy)
		{
			Destroy();
		}
	}
}
