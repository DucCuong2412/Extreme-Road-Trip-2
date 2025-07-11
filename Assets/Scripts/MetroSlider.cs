using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroSlider : MetroWidget
{
	private Direction _slideDirection;

	private float _onScreenCount;

	private Transform _pivot;

	private float _sliderWidth;

	private float _sliderHeight;

	private MetroWidget _focus;

	private bool _following;

	private Vector3 _lastDelta = Vector3.zero;

	public override MetroWidget Add(MetroWidget child)
	{
		base.Add(child);
		child.transform.parent = _pivot;
		return child;
	}

	private void Setup(Direction slideDirection, float onScreenCount)
	{
		_slideDirection = slideDirection;
		_onScreenCount = onScreenCount;
	}

	public static MetroSlider Create(Direction slideDirection, float onScreenCount)
	{
		return Create("Slider", slideDirection, onScreenCount);
	}

	public static MetroSlider Create(string id, Direction slideDirection, float onScreenCount)
	{
		GameObject gameObject = new GameObject(id);
		gameObject.transform.position = Vector3.zero;
		MetroSlider metroSlider = gameObject.AddComponent<MetroSlider>();
		metroSlider.Setup(slideDirection, onScreenCount);
		metroSlider._pivot = new GameObject("Pivot").transform;
		metroSlider._pivot.localPosition = Vector3.zero;
		metroSlider._pivot.parent = gameObject.transform;
		return metroSlider;
	}

	public override void LayoutChilds()
	{
		float width = _zone.width;
		float height = _zone.height;
		float left = (0f - width) * 0.5f;
		float top = (0f - height) * 0.5f;
		Rect zone = new Rect(left, top, width, height);
		if (_slideDirection == Direction.vertical)
		{
			SliderVertical(_childs, zone);
		}
		else
		{
			SliderHorizontal(_childs, zone);
		}
		UpdateFocus();
	}

	private void SliderHorizontal(List<MetroWidget> childs, Rect zone)
	{
		int count = childs.Count;
		float num = zone.width / _onScreenCount;
		float height = zone.height;
		for (int i = 0; i < count; i++)
		{
			MetroWidget metroWidget = childs[i];
			metroWidget.Layout(new Rect(zone.x + num * (float)i, zone.y, num, height));
		}
		_sliderWidth = Mathf.Max(0f, num * ((float)count - _onScreenCount));
		_sliderHeight = height;
	}

	private void SliderVertical(List<MetroWidget> childs, Rect zone)
	{
		int count = childs.Count;
		float width = zone.width;
		float num = zone.height / _onScreenCount;
		for (int i = 0; i < count; i++)
		{
			MetroWidget metroWidget = childs[i];
			metroWidget.Layout(new Rect(zone.x, zone.y + zone.height - num - num * (float)i, width, num));
		}
		_sliderWidth = width;
		_sliderHeight = Mathf.Max(0f, num * ((float)count - _onScreenCount));
	}

	protected override void HandleFinger(Finger finger)
	{
		StartCoroutine(FollowFinger(finger));
	}

	private void BaseHandleFinger(Finger finger)
	{
		base.HandleFinger(finger);
	}

	private IEnumerator FollowFinger(Finger finger)
	{
		if (_following)
		{
			yield break;
		}
		_following = true;
		while (!finger.IsFinished())
		{
			FakeTouch touch = finger.Touch;
			if (touch.phase == TouchPhase.Moved)
			{
				Vector2 movement2 = touch.deltaPosition;
				movement2 *= 2f * base.Camera.HalfScreenHeight / (float)Screen.height;
				MoveClamped(_lastDelta = new Vector3(movement2.x, movement2.y, 0f));
			}
			else
			{
				_lastDelta = Vector3.zero;
			}
			yield return null;
		}
		if (finger.IsFinished() && finger.Duration < 0.5f && finger.Travel < 2f)
		{
			BaseHandleFinger(finger);
		}
		_following = false;
	}

	public void Update()
	{
		if (!_following)
		{
			_lastDelta = Vector3.Lerp(_lastDelta, Vector3.zero, 5f * Time.deltaTime);
			MoveClamped(_lastDelta);
		}
	}

	private void MoveClamped(Vector3 movement)
	{
		Vector3 vector = _pivot.localPosition;
		if (_slideDirection == Direction.vertical)
		{
			movement.x = 0f;
		}
		if (_slideDirection == Direction.horizontal)
		{
			movement.y = 0f;
		}
		vector += movement;
		if (vector.x > 0f)
		{
			vector.x = Mathf.Lerp(vector.x, 0f, 10f * Time.deltaTime);
		}
		if (vector.y < 0f)
		{
			vector.y = Mathf.Lerp(vector.y, 0f, 10f * Time.deltaTime);
		}
		if (vector.x < 0f - _sliderWidth)
		{
			vector.x = Mathf.Lerp(vector.x, 0f - _sliderWidth, 10f * Time.deltaTime);
		}
		if (vector.y > _sliderHeight)
		{
			vector.y = Mathf.Lerp(vector.y, _sliderHeight, 10f * Time.deltaTime);
		}
		_pivot.localPosition = vector;
	}

	public void Focus(MetroWidget child)
	{
		_focus = child;
	}

	public void UpdateFocus()
	{
		int num = _childs.IndexOf(_focus);
		if (num >= 0 && _childs.Count > 1)
		{
			Vector3 localPosition = _pivot.localPosition;
			float num2 = (float)num / (float)(_childs.Count - 1);
			if (_slideDirection == Direction.vertical)
			{
				localPosition.y = _sliderHeight * num2;
			}
			else
			{
				localPosition.x = (0f - _sliderWidth) * num2;
			}
			_pivot.localPosition = localPosition;
		}
	}

	public void Translate(Vector3 vec)
	{
		_pivot.Translate(vec);
	}
}
