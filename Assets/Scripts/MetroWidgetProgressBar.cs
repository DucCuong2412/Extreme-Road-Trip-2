using UnityEngine;

public class MetroWidgetProgressBar : MetroWidget
{
	public struct ProgressBarColorPalette
	{
		public Color _backColor;

		public Color _fillColor1;

		public Color _fillColor2;

		public ProgressBarColorPalette(Color back, Color fill1, Color fill2)
		{
			_backColor = back;
			_fillColor1 = fill1;
			_fillColor2 = fill2;
		}
	}

	private const float _height = 2f;

	protected MetroIcon _icon;

	protected MetroLabel _label;

	protected MetroQuad _endBack;

	protected MetroQuad _back;

	protected Transform _fill;

	protected Transform _endFill;

	protected string _text;

	protected float _val;

	protected ProgressBarColorPalette _colors;

	private float _endW;

	private float _fullWidth;

	public static MetroWidgetProgressBar Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetProgressBar).ToString());
		gameObject.transform.position = Vector3.zero;
		return gameObject.AddComponent<MetroWidgetProgressBar>();
	}

	public static MetroWidgetProgressBar Create(string prefix, float val, ProgressBarColorPalette colors, Material fillMat)
	{
		MetroWidgetProgressBar metroWidgetProgressBar = Create();
		metroWidgetProgressBar.Setup(prefix, val, colors, fillMat);
		return metroWidgetProgressBar;
	}

	public void SetValue(string text, float val)
	{
		_text = text;
		_label.SetText(text);
		_val = val;
		float num = (_fullWidth + _endW) * val;
		float num2 = Mathf.Min(num, _fullWidth);
		_fill.transform.localScale = new Vector3(num2, 2f, 1f);
		float num3 = Mathf.Max(num - num2, 0f);
		_endFill.GetComponentInChildren<MetroQuad>().Crop(num3 / _endW, 1f);
		_endFill.transform.localScale = new Vector3(num3, 2f, 1f);
	}

	protected Vector3 GetBarPos()
	{
		Vector3 position = _fill.position;
		float x = position.x;
		Vector3 localScale = _fill.localScale;
		position.x = x + localScale.x;
		return position;
	}

	protected MetroWidgetProgressBar Setup(string text, float val, ProgressBarColorPalette colors, Material fillMat)
	{
		_text = text;
		_val = val;
		_colors = colors;
		SetPadding(0f, 0f);
		_icon = MetroIcon.Create(MetroSkin.LevelBadge);
		_icon.transform.parent = base.transform;
		MetroLabel metroLabel = MetroLabel.Create("LEVEL");
		metroLabel.SetFont(MetroSkin.VerySmallFont);
		metroLabel.transform.parent = _icon.transform.Find("Pivot/Level");
		metroLabel.transform.localPosition = new Vector3(0f, 0f, -0.1f);
		_label = MetroLabel.Create(string.Empty);
		_label.SetFont(MetroSkin.MediumFont);
		_label.transform.parent = _icon.transform.Find("Pivot/Number");
		_label.transform.localPosition = new Vector3(0f, 0f, -0.1f);
		_back = MetroQuad.Create(base.transform);
		_back.gameObject.name = "_back";
		_back.SetColor(colors._backColor);
		_fill = new GameObject("_fillPivot").transform;
		_fill.parent = base.transform;
		_fill.transform.localPosition = Vector3.zero;
		MetroQuad metroQuad = MetroQuad.Create(base.transform);
		metroQuad.gameObject.name = "_fill";
		metroQuad.SetGradient(colors._fillColor1, colors._fillColor2);
		metroQuad.transform.parent = _fill;
		metroQuad.transform.localPosition = new Vector3(0.5f, 0f, 0f);
		_endBack = MetroQuad.Create(base.transform, fillMat);
		_endBack.gameObject.name = "_endBack";
		_endBack.SetColor(colors._backColor);
		_endFill = new GameObject("_endFillPivot").transform;
		_endFill.parent = base.transform;
		_endFill.transform.localPosition = Vector3.zero;
		MetroQuad metroQuad2 = MetroQuad.Create(_endFill, fillMat);
		metroQuad2.gameObject.name = "_endFill";
		metroQuad2.SetGradient(colors._fillColor1, colors._fillColor2);
		metroQuad2.transform.parent = _endFill;
		metroQuad2.Crop(1f, 1f);
		metroQuad2.transform.localPosition = new Vector3(0.5f, 0f, 0f);
		return this;
	}

	public override void LayoutChilds()
	{
		float width = _zone.width;
		float num = width * 0.5f;
		Vector3 size = RendererBounds.ComputeBounds(_endBack.transform).size;
		_endW = size.x;
		float num2 = _endW * 0.5f;
		Vector3 size2 = RendererBounds.ComputeBounds(_icon.transform).size;
		float x = size2.x;
		float num3 = x * 0.5f;
		_icon.transform.localPosition = new Vector3(0f - num + num3, 0f, -0.1f);
		_back.transform.localScale = new Vector3(width - num3 - _endW, 2f, 1f);
		_back.transform.localPosition = new Vector3((num3 - _endW) * 0.5f, 0f, 0f);
		_fill.transform.localScale = new Vector3(1f, 2f, 1f);
		_fill.transform.localPosition = new Vector3(0f - num + num3, 0f, -0.05f);
		_endBack.transform.localScale = new Vector3(1f, 2f, 1f);
		_endBack.transform.localPosition = new Vector3(num - num2, 0f, 0f);
		_endFill.transform.localScale = new Vector3(1f, 2f, 1f);
		_endFill.transform.localPosition = new Vector3(num - _endW, 0f, -0.05f);
		_fullWidth = width - (num3 + _endW);
		base.LayoutChilds();
	}

	protected override void OnStart()
	{
		SetValue(_text, _val);
		base.OnStart();
	}
}
