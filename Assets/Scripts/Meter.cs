using UnityEngine;

public class Meter : MonoBehaviour
{
	private const float MIN_VALUE = 0f;

	private const float FILL_Z = -0.01f;

	private const float _width = 20f;

	private const float _height = 1.7f;

	private MetroSlice9 _background;

	private MetroSlice9 _fill;

	private float _value;

	private float _current01;

	private float _paddingX;

	private float _paddingY;

	public void SnapDisplay()
	{
		_value = _current01;
		UpdateFill();
	}

	public static Meter Create(string sliceName, float paddingX, float paddingY)
	{
		GameObject gameObject = new GameObject("Meter");
		return gameObject.AddComponent<Meter>().Setup(sliceName, paddingX, paddingY);
	}

	public Meter Setup(string sliceName, float paddingX, float paddingY)
	{
		_paddingX = paddingX;
		_paddingY = paddingY;
		MetroSlice9Texture component = (Resources.Load(MetroSkin.Slice9MeterBackground) as GameObject).GetComponent<MetroSlice9Texture>();
		_background = MetroSlice9.Create(base.transform, component);
		_background.SetSize(20f, 1.7f);
		component = (Resources.Load(sliceName) as GameObject).GetComponent<MetroSlice9Texture>();
		_fill = MetroSlice9.Create(base.transform, component);
		return this;
	}

	private void Update()
	{
		_value = Mathf.Lerp(_value, _current01, 5f * Time.deltaTime);
		UpdateFill();
	}

	private void UpdateFill()
	{
		if (_value > 0f)
		{
			_fill.enabled = true;
			float num = 20f;
			float num2 = num * 0.5f;
			float num3 = num * _value - _paddingX * _value;
			_fill.SetSize(num3, 1.7f - _paddingY);
			_fill.transform.localPosition = new Vector3(0f - num2 + num3 / 2f + _paddingX / 2f, 0f, -0.01f);
		}
		else
		{
			_fill.enabled = false;
		}
	}

	public void SetValue01(float val)
	{
		if (val > 0f)
		{
			_current01 = val;
		}
		else
		{
			_current01 = 0f;
		}
	}
}
