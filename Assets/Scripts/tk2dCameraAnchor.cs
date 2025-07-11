using UnityEngine;

[AddComponentMenu("2D Toolkit/Camera/tk2dCameraAnchor")]
[ExecuteInEditMode]
public class tk2dCameraAnchor : MonoBehaviour
{
	[SerializeField]
	private int anchor = -1;

	[SerializeField]
	private tk2dBaseSprite.Anchor _anchorPoint = tk2dBaseSprite.Anchor.UpperLeft;

	[SerializeField]
	private bool anchorToNativeBounds;

	[SerializeField]
	private Vector2 offset = Vector2.zero;

	[SerializeField]
	private tk2dCamera tk2dCamera;

	[SerializeField]
	private Camera _anchorCamera;

	private Camera _anchorCameraCached;

	private tk2dCamera _anchorTk2dCamera;

	private Transform _myTransform;

	public tk2dBaseSprite.Anchor AnchorPoint
	{
		get
		{
			if (anchor != -1)
			{
				if (anchor >= 0 && anchor <= 2)
				{
					_anchorPoint = (tk2dBaseSprite.Anchor)(anchor + 6);
				}
				else if (anchor >= 6 && anchor <= 8)
				{
					_anchorPoint = (tk2dBaseSprite.Anchor)(anchor - 6);
				}
				else
				{
					_anchorPoint = (tk2dBaseSprite.Anchor)anchor;
				}
				anchor = -1;
			}
			return _anchorPoint;
		}
		set
		{
			_anchorPoint = value;
		}
	}

	public Vector2 AnchorOffsetPixels
	{
		get
		{
			return offset;
		}
		set
		{
			offset = value;
		}
	}

	public bool AnchorToNativeBounds
	{
		get
		{
			return anchorToNativeBounds;
		}
		set
		{
			anchorToNativeBounds = value;
		}
	}

	public Camera AnchorCamera
	{
		get
		{
			if (tk2dCamera != null)
			{
				_anchorCamera = tk2dCamera.GetComponent<Camera>();
				tk2dCamera = null;
			}
			return _anchorCamera;
		}
		set
		{
			_anchorCamera = value;
			_anchorCameraCached = null;
		}
	}

	private tk2dCamera AnchorTk2dCamera
	{
		get
		{
			if (_anchorCameraCached != _anchorCamera)
			{
				_anchorTk2dCamera = _anchorCamera.GetComponent<tk2dCamera>();
				_anchorCameraCached = _anchorCamera;
			}
			return _anchorTk2dCamera;
		}
	}

	private Transform myTransform
	{
		get
		{
			if (_myTransform == null)
			{
				_myTransform = base.transform;
			}
			return _myTransform;
		}
	}

	private void Start()
	{
		UpdateTransform();
	}

	private void UpdateTransform()
	{
		if (AnchorCamera == null)
		{
			return;
		}
		float num = 1f;
		Vector3 localPosition = myTransform.localPosition;
		tk2dCamera tk2dCamera = (!(AnchorTk2dCamera != null) || AnchorTk2dCamera.CameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective) ? null : AnchorTk2dCamera;
		Rect rect = default(Rect);
		if (tk2dCamera != null)
		{
			rect = ((!anchorToNativeBounds) ? tk2dCamera.ScreenExtents : tk2dCamera.NativeScreenExtents);
			num = tk2dCamera.GetSizeAtDistance(1f);
		}
		else
		{
			rect.Set(0f, 0f, AnchorCamera.pixelWidth, AnchorCamera.pixelHeight);
		}
		float yMin = rect.yMin;
		float yMax = rect.yMax;
		float y = (yMin + yMax) * 0.5f;
		float xMin = rect.xMin;
		float xMax = rect.xMax;
		float x = (xMin + xMax) * 0.5f;
		Vector3 a = Vector3.zero;
		switch (AnchorPoint)
		{
		case tk2dBaseSprite.Anchor.UpperLeft:
			a = new Vector3(xMin, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			a = new Vector3(x, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			a = new Vector3(xMax, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			a = new Vector3(xMin, y, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			a = new Vector3(x, y, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			a = new Vector3(xMax, y, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerLeft:
			a = new Vector3(xMin, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			a = new Vector3(x, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			a = new Vector3(xMax, yMin, localPosition.z);
			break;
		}
		Vector3 vector = a + new Vector3(num * offset.x, num * offset.y, 0f);
		if (tk2dCamera == null)
		{
			Vector3 vector2 = AnchorCamera.ScreenToWorldPoint(vector);
			if (myTransform.position != vector2)
			{
				myTransform.position = vector2;
			}
		}
		else
		{
			Vector3 localPosition2 = myTransform.localPosition;
			if (localPosition2 != vector)
			{
				myTransform.localPosition = vector;
			}
		}
	}

	public void ForceUpdateTransform()
	{
		UpdateTransform();
	}

	private void LateUpdate()
	{
		UpdateTransform();
	}
}
