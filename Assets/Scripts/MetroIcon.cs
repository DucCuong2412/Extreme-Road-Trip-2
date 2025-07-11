using UnityEngine;

public class MetroIcon : MetroWidget
{
	protected Transform _pivot;

	protected Renderer _renderer;

	private Vector3 _offset;

	private float _scaleX = 1f;

	private float _scaleY = 1f;

	private MetroStretch _stretch;

	public Renderer Renderer => _renderer;

	public virtual MetroIcon SetStretch(MetroStretch stretch)
	{
		_stretch = stretch;
		return this;
	}

	protected override void Align()
	{
		if (!(_pivot == null))
		{
			Bounds bounds = RendererBounds.ComputeBounds(_pivot);
			Vector3 vector = _pivot.position - bounds.center;
			vector.z = 0f;
			float width = _childsZone.width;
			float height = _childsZone.height;
			if (_alignment == MetroAlign.Left || _alignment == MetroAlign.TopLeft || _alignment == MetroAlign.BottomLeft)
			{
				Vector3 a = vector;
				float num = (0f - width) * 0.5f;
				Vector3 size = bounds.size;
				vector = a + new Vector3(num + size.x * 0.5f + _paddingX, 0f, 0f);
				vector.x += base.AlignmentOffset;
			}
			if (_alignment == MetroAlign.Right || _alignment == MetroAlign.TopRight || _alignment == MetroAlign.BottomRight)
			{
				Vector3 a2 = vector;
				float num2 = width * 0.5f;
				Vector3 size2 = bounds.size;
				vector = a2 + new Vector3(num2 - size2.x * 0.5f - _paddingX, 0f, 0f);
				vector.x += base.AlignmentOffset;
			}
			if (_alignment == MetroAlign.Top || _alignment == MetroAlign.TopLeft || _alignment == MetroAlign.TopRight)
			{
				Vector3 a3 = vector;
				float num3 = height * 0.5f;
				Vector3 size3 = bounds.size;
				vector = a3 + new Vector3(0f, num3 - size3.y * 0.5f - _paddingY, 0f);
				vector.y += base.AlignmentOffset;
			}
			if (_alignment == MetroAlign.Bottom || _alignment == MetroAlign.BottomLeft || _alignment == MetroAlign.BottomRight)
			{
				Vector3 a4 = vector;
				float num4 = (0f - height) * 0.5f;
				Vector3 size4 = bounds.size;
				vector = a4 + new Vector3(0f, num4 + size4.y * 0.5f + _paddingY, 0f);
				vector.y += base.AlignmentOffset;
			}
			_pivot.localPosition = vector + _offset;
		}
	}

	public MetroIcon SetScale(float scale)
	{
		return SetScale(scale, scale);
	}

	public MetroIcon SetScale(float scaleX, float scaleY)
	{
		_scaleX = scaleX;
		_scaleY = scaleY;
		_pivot.localScale = new Vector3(_scaleX, _scaleY, 1f);
		Align();
		return this;
	}

	public void SetSprite(Sprite sprite)
	{
		SpriteRenderer spriteRenderer = Renderer as SpriteRenderer;
		if (spriteRenderer != null)
		{
			spriteRenderer.sprite = sprite;
		}
	}

	public virtual Vector2 GetTextureSize()
	{
		if (Renderer != null)
		{
			Texture texture = null;
			if (Renderer is SpriteRenderer)
			{
				SpriteRenderer spriteRenderer = Renderer as SpriteRenderer;
				Vector3 size = spriteRenderer.sprite.bounds.size;
				float x = size.x;
				Vector3 size2 = spriteRenderer.sprite.bounds.size;
				float y = size2.y;
				return new Vector2(x, y);
			}
			if (Renderer is MeshRenderer)
			{
				MeshRenderer meshRenderer = Renderer as MeshRenderer;
				texture = Renderer.material.mainTexture;
				if (texture != null)
				{
					Bounds bounds = meshRenderer.bounds;
					Vector3 size3 = bounds.size;
					float x2 = size3.x;
					Vector3 size4 = bounds.size;
					float y2 = size4.y;
					return new Vector2(x2, y2);
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("MetroIcon: Unknow renderer");
			}
		}
		return Vector2.zero;
	}

	public MetroIcon SetOffset(Vector3 offset)
	{
		_offset = offset;
		return this;
	}

	public Vector3 GetOffset()
	{
		return _offset;
	}

	public static MetroIcon Create(string iconName)
	{
		Object @object = Resources.Load(iconName);
		if (@object != null)
		{
			GameObject pivot = (GameObject)Object.Instantiate(@object, Vector3.zero, Quaternion.identity);
			return Create(pivot);
		}
		return null;
	}

	public static MetroIcon Create(GameObject pivot)
	{
		GameObject gameObject = new GameObject(pivot.name);
		pivot.transform.parent = gameObject.transform;
		pivot.transform.localPosition = Vector3.zero;
		pivot.name = "Pivot";
		MetroIcon metroIcon = gameObject.AddComponent<MetroIcon>();
		metroIcon._pivot = pivot.transform;
		metroIcon._renderer = pivot.GetComponentInChildren<Renderer>();
		return metroIcon;
	}

	public static MetroIcon Create(GameObject pivot, float scale)
	{
		MetroIcon metroIcon = Create(pivot);
		metroIcon.SetScale(scale);
		return metroIcon;
	}

	public static MetroIcon Create(Quad quad, float scale = 1f)
	{
		GameObject gameObject = new GameObject(quad.name);
		quad.transform.parent = gameObject.transform;
		quad.transform.localPosition = Vector3.zero;
		quad.transform.localScale = Vector3.one * scale;
		quad.name = "Pivot";
		MetroIcon metroIcon = gameObject.AddComponent<MetroIcon>();
		metroIcon._pivot = quad.transform;
		metroIcon._scaleX = scale;
		metroIcon._scaleY = scale;
		metroIcon._renderer = quad.Renderer;
		return metroIcon;
	}

	public override Bounds GetBounds()
	{
		return (!(Renderer != null)) ? base.GetBounds() : Renderer.bounds;
	}

	public override void LayoutChilds()
	{
		base.LayoutChilds();
		if (_stretch == MetroStretch.none)
		{
			return;
		}
		float width = _childsZone.width;
		float height = _childsZone.height;
		float num = 0f;
		float num2 = 0f;
		Vector2 textureSize = GetTextureSize();
		if (textureSize.x > 0f && textureSize.y > 0f && width > 0f && height > 0f)
		{
			num = ((_stretch != MetroStretch.height) ? (width / textureSize.x) : 1f);
			num2 = ((_stretch != MetroStretch.width) ? (height / textureSize.y) : 1f);
			if (_stretch == MetroStretch.full || _stretch == MetroStretch.fullRatio)
			{
				num = width / textureSize.x;
				num2 = height / textureSize.y;
				if (_stretch == MetroStretch.fullRatio)
				{
					if (num < num2)
					{
						num2 = num;
					}
					else
					{
						num = num2;
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.Log("Size cannot be 0 or negative");
		}
		if (num > 0f && num2 > 0f)
		{
			SetScale(num, num2);
		}
	}

	public static MetroIcon Create(Car car, bool asPrefab = false)
	{
		GameObject gameObject = null;
		if (asPrefab)
		{
			GameObject visualPrefab = car.VisualPrefab;
			gameObject = (GameObject)Object.Instantiate(visualPrefab, Vector3.zero, Quaternion.identity);
		}
		else
		{
			gameObject = AutoSingleton<CarSpriteManager>.Instance.MakeCarSprite(car);
		}
		MetroIcon metroIcon = gameObject.AddComponent<MetroIcon>();
		metroIcon._pivot = gameObject.transform.Find("Pivot");
		if (metroIcon._pivot == null)
		{
			UnityEngine.Debug.LogWarning("Pivot is null for car: " + car.Id);
		}
		return metroIcon;
	}
}
