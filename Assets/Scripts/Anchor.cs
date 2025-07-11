using UnityEngine;

public class Anchor : MonoBehaviour
{
	public HorizontalAlignement _horizontalAlignement = HorizontalAlignement.none;

	public float _horizontalOffset;

	public VerticalAlignement _verticalAlignement = VerticalAlignement.none;

	public float _verticalOffset;

	public bool _ignoreBounds;

	public void Start()
	{
		Bounds bounds = RendererBounds.ComputeBounds(base.transform);
		Vector3 vector = base.transform.position - bounds.center;
		float halfScreenWidth = PrefabSingleton<CameraGUI>.Instance.HalfScreenWidth;
		float num;
		if (_ignoreBounds)
		{
			num = 0f;
		}
		else
		{
			Vector3 extents = bounds.extents;
			num = extents.x;
		}
		float num2 = halfScreenWidth - num;
		float halfScreenHeight = PrefabSingleton<CameraGUI>.Instance.HalfScreenHeight;
		float num3;
		if (_ignoreBounds)
		{
			num3 = 0f;
		}
		else
		{
			Vector3 extents2 = bounds.extents;
			num3 = extents2.y;
		}
		float num4 = halfScreenHeight - num3;
		Vector3 localPosition = base.transform.localPosition;
		float num5 = localPosition.x;
		switch (_horizontalAlignement)
		{
		case HorizontalAlignement.left:
			num5 = 0f - num2 + _horizontalOffset + vector.x;
			break;
		case HorizontalAlignement.center:
			num5 = _horizontalOffset;
			break;
		case HorizontalAlignement.right:
			num5 = num2 + _horizontalOffset + vector.x;
			break;
		}
		Vector3 localPosition2 = base.transform.localPosition;
		float num6 = localPosition2.y;
		switch (_verticalAlignement)
		{
		case VerticalAlignement.top:
			num6 = num4 + _verticalOffset + vector.y;
			break;
		case VerticalAlignement.middle:
			num6 = _verticalOffset;
			break;
		case VerticalAlignement.bottom:
			num6 = 0f - num4 + _verticalOffset - vector.y;
			break;
		}
		Transform transform = base.transform;
		float x = num5;
		float y = num6;
		Vector3 localPosition3 = base.transform.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition3.z);
	}
}
