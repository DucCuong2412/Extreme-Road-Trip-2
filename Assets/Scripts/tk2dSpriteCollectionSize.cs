using System;

[Serializable]
public class tk2dSpriteCollectionSize
{
	public enum Type
	{
		Explicit,
		PixelsPerMeter
	}

	public Type type = Type.PixelsPerMeter;

	public float orthoSize = 10f;

	public float pixelsPerMeter = 100f;

	public float width = 960f;

	public float height = 640f;

	public float OrthoSize
	{
		get
		{
			switch (type)
			{
			case Type.Explicit:
				return orthoSize;
			case Type.PixelsPerMeter:
				return 0.5f;
			default:
				return orthoSize;
			}
		}
	}

	public float TargetHeight
	{
		get
		{
			switch (type)
			{
			case Type.Explicit:
				return height;
			case Type.PixelsPerMeter:
				return pixelsPerMeter;
			default:
				return height;
			}
		}
	}

	public static tk2dSpriteCollectionSize Explicit(float orthoSize, float targetHeight)
	{
		return ForResolution(orthoSize, targetHeight, targetHeight);
	}

	public static tk2dSpriteCollectionSize PixelsPerMeter(float pixelsPerMeter)
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = new tk2dSpriteCollectionSize();
		tk2dSpriteCollectionSize.type = Type.PixelsPerMeter;
		tk2dSpriteCollectionSize.pixelsPerMeter = pixelsPerMeter;
		return tk2dSpriteCollectionSize;
	}

	public static tk2dSpriteCollectionSize ForResolution(float orthoSize, float width, float height)
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = new tk2dSpriteCollectionSize();
		tk2dSpriteCollectionSize.type = Type.Explicit;
		tk2dSpriteCollectionSize.orthoSize = orthoSize;
		tk2dSpriteCollectionSize.width = width;
		tk2dSpriteCollectionSize.height = height;
		return tk2dSpriteCollectionSize;
	}

	public static tk2dSpriteCollectionSize ForTk2dCamera()
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = new tk2dSpriteCollectionSize();
		tk2dSpriteCollectionSize.type = Type.PixelsPerMeter;
		tk2dSpriteCollectionSize.pixelsPerMeter = 1f;
		return tk2dSpriteCollectionSize;
	}

	public static tk2dSpriteCollectionSize ForTk2dCamera(tk2dCamera camera)
	{
		tk2dSpriteCollectionSize tk2dSpriteCollectionSize = new tk2dSpriteCollectionSize();
		tk2dCameraSettings cameraSettings = camera.SettingsRoot.CameraSettings;
		if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Orthographic)
		{
			switch (cameraSettings.orthographicType)
			{
			case tk2dCameraSettings.OrthographicType.PixelsPerMeter:
				tk2dSpriteCollectionSize.type = Type.PixelsPerMeter;
				tk2dSpriteCollectionSize.pixelsPerMeter = cameraSettings.orthographicPixelsPerMeter;
				break;
			case tk2dCameraSettings.OrthographicType.OrthographicSize:
				tk2dSpriteCollectionSize.type = Type.Explicit;
				tk2dSpriteCollectionSize.height = camera.nativeResolutionHeight;
				tk2dSpriteCollectionSize.orthoSize = cameraSettings.orthographicSize;
				break;
			}
		}
		else if (cameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			tk2dSpriteCollectionSize.type = Type.PixelsPerMeter;
			tk2dSpriteCollectionSize.pixelsPerMeter = 100f;
		}
		return tk2dSpriteCollectionSize;
	}

	public static tk2dSpriteCollectionSize Default()
	{
		return PixelsPerMeter(100f);
	}

	public void CopyFromLegacy(bool useTk2dCamera, float orthoSize, float targetHeight)
	{
		if (useTk2dCamera)
		{
			type = Type.PixelsPerMeter;
			pixelsPerMeter = 1f;
		}
		else
		{
			type = Type.Explicit;
			height = targetHeight;
			this.orthoSize = orthoSize;
		}
	}

	public void CopyFrom(tk2dSpriteCollectionSize source)
	{
		type = source.type;
		width = source.width;
		height = source.height;
		orthoSize = source.orthoSize;
		pixelsPerMeter = source.pixelsPerMeter;
	}
}
