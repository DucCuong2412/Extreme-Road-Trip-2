using System.Collections.Generic;
using UnityEngine;

public class MetroWidget : MonoBehaviour
{
	protected Transform _transform;

	protected MetroQuad _quad;

	protected MetroSlice9 _slice9;

	protected float _paddingX = MetroSkin.Padding;

	protected float _paddingY = MetroSkin.Padding;

	protected MetroAlign _alignment;

	private MetroWidget _parent;

	protected List<MetroWidget> _childs = new List<MetroWidget>();

	protected Rect _zone;

	protected Rect _childsZone;

	private float _mass;

	private CameraGUI _camera;

	protected bool _active = true;

	public float AlignmentOffset
	{
		get;
		set;
	}

	public MetroWidget Parent
	{
		get
		{
			return _parent;
		}
		private set
		{
			_parent = value;
		}
	}

	public float Mass
	{
		get
		{
			return _mass;
		}
		set
		{
			_mass = value;
		}
	}

	public float ZoneHeight => _zone.height;

	protected CameraGUI Camera
	{
		get
		{
			if (_camera == null)
			{
				_camera = PrefabSingleton<CameraGUI>.Instance;
			}
			return _camera;
		}
	}

	public virtual MetroWidget AddSolidBackground()
	{
		if (_quad == null)
		{
			_quad = MetroQuad.Create(base.transform);
			Refresh();
		}
		return this;
	}

	public virtual MetroWidget AddSolidBackground(Color color)
	{
		AddSolidBackground();
		SetColor(color);
		return this;
	}

	public virtual MetroWidget AddSlice9Background(string slice9TextureName)
	{
		if (_slice9 != null)
		{
			UnityEngine.Object.Destroy(_slice9.gameObject);
			_slice9 = null;
		}
		MetroSlice9Texture component = (Resources.Load(slice9TextureName) as GameObject).GetComponent<MetroSlice9Texture>();
		if (component != null)
		{
			_slice9 = MetroSlice9.Create(base.transform, component);
		}
		LayoutBackground();
		Refresh();
		return this;
	}

	public MetroWidget SetPadding(float p)
	{
		return SetPadding(p, p);
	}

	public MetroWidget SetPadding(float px, float py)
	{
		_paddingX = px;
		_paddingY = py;
		return this;
	}

	public MetroWidget SetAlignment(MetroAlign alignment)
	{
		_alignment = alignment;
		return this;
	}

	protected virtual void Align()
	{
	}

	public List<MetroWidget> GetChilds()
	{
		return _childs;
	}

	public MetroWidget SetMass(float mass)
	{
		Mass = mass;
		return this;
	}

	public virtual MetroWidget Clear()
	{
		List<MetroWidget> list = new List<MetroWidget>(_childs);
		foreach (MetroWidget item in list)
		{
			item.Remove().Destroy();
		}
		return this;
	}

    public virtual MetroWidget Add(MetroWidget child)
    {
        if (child == null)
        {
            Debug.LogWarning("Tried to add null child to " + gameObject.name);
            return null;
        }

        _childs.Add(child);
        child._parent = this;
        child.transform.parent = base.transform;
        return child;
    }

    public virtual MetroWidget Replace(MetroWidget replaced, MetroWidget child)
	{
		_childs[_childs.IndexOf(replaced)] = child;
		replaced._parent = null;
		child._parent = this;
		child.transform.parent = base.transform;
		child.SetMass(replaced.Mass);
		return replaced;
	}

	public virtual MetroWidget Replace(MetroWidget child)
	{
		if (_parent != null)
		{
			return _parent.Replace(this, child);
		}
		return this;
	}

	public virtual MetroWidget Remove(MetroWidget child)
	{
		_childs.Remove(child);
		child._parent = null;
		return child;
	}

	public virtual MetroWidget Remove()
	{
		if (_parent != null)
		{
			return _parent.Remove(this);
		}
		return this;
	}

	public virtual void Layout(Rect zone)
	{
		Vector2 center = zone.center;
		float x = center.x;
		Vector2 center2 = zone.center;
		Vector3 localPosition = new Vector3(x, center2.y, -0.1f);
		base.transform.localPosition = localPosition;
		_zone = zone;
		float width = zone.width;
		float height = zone.height;
		float left = (0f - width) * 0.5f;
		float top = (0f - height) * 0.5f;
		_childsZone = new Rect(left, top, width, height);
		LayoutBackground();
		LayoutChilds();
	}

	public virtual void LayoutChilds()
	{
		foreach (MetroWidget child in _childs)
		{
			child.Layout(_childsZone);
		}
	}

	public void LayoutBackground()
	{
		float w = _childsZone.width - _paddingX;
		float h = _childsZone.height - _paddingY;
		if (_quad != null)
		{
			_quad.SetSize(w, h);
		}
		if (_slice9 != null)
		{
			_slice9.SetSize(w, h);
		}
	}

	protected virtual void Refresh()
	{
	}

	protected virtual void Cleanup()
	{
		foreach (MetroWidget child in _childs)
		{
			child.Cleanup();
		}
	}

	public void Destroy()
	{
		if (Parent != null)
		{
			Parent.Remove(this);
		}
		Cleanup();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Reflow()
	{
		Layout(_zone);
	}

	public void Awake()
	{
		_transform = base.transform;
		SetLayer(8);
		Mass = 1f;
		OnAwake();
	}

	public void Start()
	{
		OnStart();
		Align();
	}

	protected virtual void OnAwake()
	{
	}

	protected virtual void OnStart()
	{
	}

	public virtual void OnBlur()
	{
		foreach (MetroWidget child in _childs)
		{
			child.OnBlur();
		}
	}

    public virtual void OnFocus()
    {
        foreach (MetroWidget child in _childs)
        {
            if (child != null)
            {
                child.OnFocus();
            }
        }
    }
    public void SetLayer(int layer)
	{
		base.gameObject.layer = layer;
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			transform.gameObject.layer = layer;
		}
	}

	public int GetLayer()
	{
		return base.gameObject.layer;
	}

	public virtual MetroWidget SetColor(Color c)
	{
		if (_quad != null)
		{
			_quad.SetColor(c);
		}
		return this;
	}

	public virtual MetroWidget SetGradient(Color c1, Color c2)
	{
		if (_quad != null)
		{
			_quad.SetGradient(c1, c2);
		}
		return this;
	}

	public virtual MetroWidget SetMaterial(Material m)
	{
		if (_quad != null)
		{
			_quad.SetMaterial(m);
		}
		return this;
	}

	public virtual void SetActive(bool active)
	{
		_active = active;
	}

	public virtual bool IsActive()
	{
		if (_parent == null)
		{
			return _active;
		}
		return _active && _parent.IsActive();
	}

	protected Vector3 TouchPositionInWorldSpace(FakeTouch touch)
	{
		return Camera.Camera.ScreenToWorldPoint(touch.position);
	}

	protected virtual bool TouchIsInZone(FakeTouch touch)
	{
		Vector3 vector = TouchPositionInWorldSpace(touch);
		Vector3 position = base.transform.position;
		float num = _zone.width * 0.5f;
		float num2 = _zone.height * 0.5f;
		return Mathf.Abs(vector.x - position.x) < num && Mathf.Abs(vector.y - position.y) < num2;
	}

	protected virtual void HandleFinger(Finger finger)
	{
		if (TouchIsInZone(finger.Touch))
		{
			foreach (MetroWidget item in new List<MetroWidget>(_childs))
			{
				if (item.IsActive() && item.TouchIsInZone(finger.Touch))
				{
					item.HandleFinger(finger);
				}
			}
		}
	}

	public virtual Bounds GetBounds()
	{
		return RendererBounds.ComputeBounds(base.transform);
	}
}
