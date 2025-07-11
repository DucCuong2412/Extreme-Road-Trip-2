using UnityEngine;

public class MetroLabel : MetroWidget
{
	protected TextMesh _textMesh;

	protected TextMesh _textMeshOutline;

	public string _text = "label";

	public bool _localize = true;

	private MetroFont _font;

	private bool _dirty;

	private bool HasOutline()
	{
		return _textMeshOutline != null;
	}

	protected override void OnAwake()
	{
		_alignment = MetroAlign.Center;
		SetFont(MetroSkin.DefaultFont);
		base.OnAwake();
	}

	public void SetLineSpacing(float spacing)
	{
	}

	public override MetroWidget SetColor(Color c)
	{
		_textMesh.GetComponent<Renderer>().material.color = c;
		return this;
	}

	public MetroLabel AddOutline()
	{
		SetOutlineColor(MetroSkin.TextOutlineColor);
		return this;
	}

	public void SetColor(Color textColor, Color outlineColor)
	{
		SetColor(textColor);
		SetOutlineColor(outlineColor);
	}

	public void SetOutlineColor(Color c)
	{
		if (!HasOutline())
		{
			MakeOutline();
		}
		_textMeshOutline.GetComponent<Renderer>().material.color = c;
	}

	public MetroLabel SetFont(MetroFont font)
	{
		_font = font;
		if (_textMesh != null)
		{
			UnityEngine.Object.Destroy(_textMesh.gameObject);
		}
		MakeLabel();
		if (HasOutline())
		{
			UnityEngine.Object.Destroy(_textMeshOutline.gameObject);
			MakeOutline();
		}
		SetText(_text);
		return this;
	}

	public float GetLineHeight()
	{
		return 1f;
	}

	private void MakeLabel()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load(_font.FontName), Vector3.zero, Quaternion.identity);
		gameObject.layer = 8;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.back;
		_textMesh = gameObject.GetComponent<TextMesh>();
		_textMesh.anchor = TextAnchor.MiddleCenter;
	}

	private void MakeOutline()
	{
		float outlineOffset = _font.OutlineOffset;
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load(_font.FontName), Vector3.zero, Quaternion.identity);
		gameObject.layer = 8;
		gameObject.transform.parent = _textMesh.transform;
		gameObject.transform.localPosition = new Vector3(outlineOffset, 0f - outlineOffset, 0.0001f);
		_textMeshOutline = gameObject.GetComponent<TextMesh>();
		_textMeshOutline.anchor = TextAnchor.MiddleCenter;
		SetText(_text);
	}

	public override void Layout(Rect zone)
	{
		base.Layout(zone);
		_dirty = true;
	}

	protected override void Align()
	{
		if (_dirty)
		{
			Vector3 vector = Vector3.zero;
			float width = _childsZone.width;
			if (_alignment == MetroAlign.Left)
			{
				Renderer renderer = _textMesh.GetComponent<Renderer>();
				Bounds bounds = renderer.bounds;
				Vector3 a = vector;
				Vector3 left = Vector3.left;
				float num = width;
				Vector3 size = bounds.size;
				vector = a + left * (num - size.x) * 0.5f;
			}
			if (_alignment == MetroAlign.Right)
			{
				Renderer renderer2 = _textMesh.GetComponent<Renderer>();
				Bounds bounds2 = renderer2.bounds;
				Vector3 a2 = vector;
				Vector3 right = Vector3.right;
				float num2 = width;
				Vector3 size2 = bounds2.size;
				vector = a2 + right * (num2 - size2.x) * 0.5f;
			}
			vector.x += base.AlignmentOffset;
			_textMesh.transform.localPosition = vector;
			_dirty = false;
		}
	}

	public virtual MetroLabel SetText(string text)
	{
		if (text == null)
		{
			text = " ";
		}
		_text = ((!_localize) ? text : text.Localize());
		_text = _text.Replace("@", "<quad material=1 size=20 x=.74 y=.74 width=.26 height=.26 />");
		_textMesh.text = _text;
		if (HasOutline())
		{
			_textMeshOutline.text = _text;
		}
		_dirty = true;
		Align();
		return this;
	}

	public static MetroLabel Create(string content)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = Vector3.zero;
		MetroLabel metroLabel = gameObject.AddComponent<MetroLabel>();
		metroLabel.SetText(content);
		return metroLabel;
	}
}
