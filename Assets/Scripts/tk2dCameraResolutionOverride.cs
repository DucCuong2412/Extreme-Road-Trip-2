using System;
using UnityEngine;

[Serializable]
public class tk2dCameraResolutionOverride
{
	public enum MatchByType
	{
		Resolution,
		AspectRatio,
		Wildcard
	}

	public enum AutoScaleMode
	{
		None,
		FitWidth,
		FitHeight,
		FitVisible,
		StretchToFit,
		ClosestMultipleOfTwo,
		PixelPerfect,
		Fill
	}

	public enum FitMode
	{
		Constant,
		Center
	}

	public string name;

	public MatchByType matchBy;

	public int width;

	public int height;

	public float aspectRatioNumerator = 4f;

	public float aspectRatioDenominator = 3f;

	public float scale = 1f;

	public Vector2 offsetPixels = new Vector2(0f, 0f);

	public AutoScaleMode autoScaleMode;

	public FitMode fitMode;

	public static tk2dCameraResolutionOverride DefaultOverride
	{
		get
		{
			tk2dCameraResolutionOverride tk2dCameraResolutionOverride = new tk2dCameraResolutionOverride();
			tk2dCameraResolutionOverride.name = "Override";
			tk2dCameraResolutionOverride.matchBy = MatchByType.Wildcard;
			tk2dCameraResolutionOverride.autoScaleMode = AutoScaleMode.FitVisible;
			tk2dCameraResolutionOverride.fitMode = FitMode.Center;
			return tk2dCameraResolutionOverride;
		}
	}

	public bool Match(int pixelWidth, int pixelHeight)
	{
		switch (matchBy)
		{
		case MatchByType.Wildcard:
			return true;
		case MatchByType.Resolution:
			return pixelWidth == width && pixelHeight == height;
		case MatchByType.AspectRatio:
		{
			float num = (float)pixelHeight / (float)pixelWidth;
			float num2 = num * aspectRatioNumerator;
			float num3 = Mathf.Abs(num2 - aspectRatioDenominator);
			return num3 < 0.05f;
		}
		default:
			return false;
		}
	}

	public void Upgrade(int version)
	{
		if (version == 0)
		{
			matchBy = (((width == -1 && height == -1) || (width == 0 && height == 0)) ? MatchByType.Wildcard : MatchByType.Resolution);
		}
	}
}
