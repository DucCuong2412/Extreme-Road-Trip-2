using UnityEngine;

public class MetroViewportCamera : MonoBehaviour
{
	private Camera _camera;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void Setup(MetroViewport viewport)
	{
		base.transform.parent = viewport.transform;
		base.transform.localPosition = new Vector3(0f, 0f, -100f);
	}

	public void SetZone(Rect zone)
	{
		float num = 16f;
		float num2 = (float)Screen.width / (float)Screen.height;
		float num3 = 2f * num;
		float num4 = 2f * num2 * num;
		Vector3 position = base.transform.position;
		float num5 = position.x - zone.width * 0.5f;
		Vector3 position2 = base.transform.position;
		float num6 = position2.y - zone.height * 0.5f;
		float num7 = num4 * 0.5f;
		float num8 = num3 * 0.5f;
		float left = (num5 + num7) / num4;
		float top = (num6 + num8) / num3;
		float height = zone.height / num3;
		float width = zone.width / num4;
		_camera.rect = new Rect(left, top, width, height);
		float num9 = zone.height / num3;
		_camera.orthographicSize = num * num9;
		Transform transform = base.transform;
		Vector3 position3 = base.transform.position;
		float x = position3.x;
		Vector3 position4 = base.transform.position;
		transform.position = new Vector3(x, position4.y, -100f);
	}

	public static MetroViewportCamera Create(MetroViewport viewport)
	{
		GameObject gameObject = new GameObject("MetroViewportCamera");
		Camera camera = gameObject.AddComponent<Camera>();
		camera.orthographic = true;
		camera.cullingMask = 1 << viewport.GUILayer;
		camera.clearFlags = CameraClearFlags.Nothing;
		camera.depth = 1f;
		camera.orthographicSize = 0.1f;
		MetroViewportCamera metroViewportCamera = gameObject.AddComponent<MetroViewportCamera>();
		metroViewportCamera.Setup(viewport);
		return metroViewportCamera;
	}
}
