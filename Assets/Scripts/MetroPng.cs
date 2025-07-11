using System.Collections;
using UnityEngine;

public class MetroPng : MetroIcon
{
	private MetroQuad _pivotQuad;

	private MetroIcon _defaultIcon;

	private Material _material;

	private string _defaultSprite;

	private string _requestImageUrl;

	private bool _isDownloading;

	public static MetroPng CreateFromUrl(string imageUrl, string defaultSprite)
	{
		GameObject gameObject = new GameObject("PNG");
		return gameObject.AddComponent<MetroPng>().Setup(imageUrl, defaultSprite);
	}

	public static MetroPng CreateFromUrl(string imageUrl)
	{
		GameObject gameObject = new GameObject("PNG");
		return gameObject.AddComponent<MetroPng>().Setup(imageUrl, null);
	}

	public static MetroPng CreateFromTexture(Texture2D texture)
	{
		GameObject gameObject = new GameObject("PNG");
		return gameObject.AddComponent<MetroPng>().Setup(texture);
	}

	private MetroPng Setup(string imageUrl, string defaultSprite)
	{
		_material = new Material(Shader.Find("Roofdog/Sprite"));
		_defaultSprite = defaultSprite;
		MetroPivot metroPivot = MetroPivot.Create();
		Add(metroPivot);
		_pivot = metroPivot.transform;
		_pivotQuad = MetroQuad.Create(_pivot, _material);
		StartCoroutine(LoadImage(imageUrl));
		return this;
	}

	private MetroPng Setup(Texture2D texture)
	{
		if (texture != null)
		{
			_material = new Material(Shader.Find("Roofdog/Sprite"));
			MetroPivot metroPivot = MetroPivot.Create();
			Add(metroPivot);
			_pivot = metroPivot.transform;
			_pivotQuad = MetroQuad.Create(_pivot, _material);
			SetMainTexture(texture);
		}
		return this;
	}

	public override MetroIcon SetStretch(MetroStretch stretch)
	{
		if (_defaultIcon != null)
		{
			_defaultIcon.SetStretch(stretch);
		}
		return base.SetStretch(stretch);
	}

	private IEnumerator LoadImage(string imageUrl)
	{
		Texture texture = AutoSingleton<PictureTexturesCache>.Instance.GetTexture(imageUrl);
		if (texture == null)
		{
			_isDownloading = true;
			_pivotQuad.gameObject.SetActive(value: false);
			if (_defaultSprite != null)
			{
				_defaultIcon = MetroIcon.Create(_defaultSprite);
				Add(_defaultIcon);
			}
			WWW req = new WWW(imageUrl);
			yield return req;
			if (string.IsNullOrEmpty(req.error))
			{
				texture = req.textureNonReadable;
				AutoSingleton<PictureTexturesCache>.Instance.UpdateCache(imageUrl, texture);
			}
		}
		_isDownloading = false;
		if (texture != null)
		{
			if (_defaultIcon != null)
			{
				_defaultIcon.Destroy();
			}
			_pivotQuad.gameObject.SetActive(value: true);
			SetMainTexture(texture);
		}
	}

	public override Vector2 GetTextureSize()
	{
		if (_isDownloading && _defaultIcon != null)
		{
			return _defaultIcon.GetTextureSize();
		}
		if (!_isDownloading && _pivotQuad != null)
		{
			return new Vector2(_pivotQuad.Width, _pivotQuad.Height);
		}
		return Vector2.zero;
	}

	private void SetMainTexture(Texture texture)
	{
		_pivotQuad.gameObject.layer = base.gameObject.layer;
		_material.mainTexture = texture;
		float num = (float)texture.width / (float)texture.height;
		float num2 = 1f;
		float num3 = 1f;
		if (num < 1f)
		{
			num2 *= num;
		}
		else
		{
			num3 /= num;
		}
		SetSize(num2, num3);
	}

	public void SetSize(float w, float h)
	{
		if (_pivotQuad != null)
		{
			_pivotQuad.SetSize(w, h);
			LayoutChilds();
		}
	}

	public override MetroWidget SetColor(Color c)
	{
		base.SetColor(c);
		if (_pivotQuad != null)
		{
			_pivotQuad.SetColor(c);
		}
		return this;
	}

	private void OnDestroy()
	{
		if ((bool)_material)
		{
			UnityEngine.Object.Destroy(_material);
		}
	}

	public bool IsLoaded()
	{
		return !_isDownloading && _material.mainTexture != null;
	}
}
