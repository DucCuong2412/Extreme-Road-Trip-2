using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroPager : MetroWidget
{
	private const float _normalMoveSpeed = 200f;

	private const float _fastMoveSpeed = 400f;

	private string _id;

	private Transform _pivot;

	private float _pagerWidth;

	private float _pagerHeight;

	private float _currentSpeed = 200f;

	private MetroWidget _focus;

	private int _currentPage;

	private bool _following;

	private Vector3 _lastDelta = Vector3.zero;

	public override MetroWidget Add(MetroWidget child)
	{
		base.Add(child);
		child.transform.parent = _pivot;
		return child;
	}

	public static MetroPager Create(string id)
	{
		GameObject gameObject = new GameObject(id);
		gameObject.transform.position = Vector3.zero;
		MetroPager metroPager = gameObject.AddComponent<MetroPager>();
		metroPager._pivot = new GameObject("Pivot").transform;
		metroPager._pivot.localPosition = Vector3.zero;
		metroPager._pivot.parent = gameObject.transform;
		metroPager._id = id;
		metroPager._currentPage = Preference.GetInt(id);
		return metroPager;
	}

	public override void LayoutChilds()
	{
		float width = _zone.width;
		float height = _zone.height;
		float left = (0f - width) * 0.5f;
		float top = (0f - height) * 0.5f;
		PagerHorizontal(zone: new Rect(left, top, width, height), childs: _childs);
	}

	public void ChangePage(bool next)
	{
		int numberOfPages = GetNumberOfPages();
		_currentPage = Mathf.Clamp(_currentPage + (next ? 1 : (-1)), 0, numberOfPages - 1);
		Preference.SetInt(_id, _currentPage);
		_currentSpeed = 200f;
	}

	public void ChangePage(int page, bool fastSpeed = false)
	{
		int numberOfPages = GetNumberOfPages();
		_currentPage = Mathf.Clamp(page, 0, numberOfPages - 1);
		Preference.SetInt(_id, _currentPage);
		if (fastSpeed)
		{
			_currentSpeed = 400f;
		}
		else
		{
			_currentSpeed = 200f;
		}
	}

	private void PagerHorizontal(List<MetroWidget> childs, Rect zone)
	{
		int count = childs.Count;
		float width = zone.width;
		float height = zone.height;
		for (int i = 0; i < count; i++)
		{
			MetroWidget metroWidget = childs[i];
			metroWidget.Layout(new Rect(zone.x + width * (float)i, zone.y, width, height));
		}
		_pagerWidth = Mathf.Max(0f, width * ((float)count - 1f));
		_pagerHeight = height;
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
				_lastDelta = Vector3.Lerp(_lastDelta, Vector3.zero, 3f * Time.deltaTime);
			}
			yield return null;
		}
		if (finger.IsFinished() && finger.Duration < 0.5f && finger.Travel < 2f)
		{
			BaseHandleFinger(finger);
		}
		else
		{
			if (_lastDelta.x < -0.5f)
			{
				ChangePage(next: true);
			}
			if (_lastDelta.x > 0.5f)
			{
				ChangePage(next: false);
			}
		}
		_following = false;
	}

	protected override void OnStart()
	{
		Vector3 localPosition = _pivot.localPosition;
		float num = localPosition.x = (float)(-_currentPage) * PageWidth();
		_pivot.localPosition = localPosition;
		base.OnStart();
	}

	public void Update()
	{
		if (!_following)
		{
			SnapToPage();
		}
	}

	private void MoveClamped(Vector3 movement)
	{
		Vector3 vector = _pivot.localPosition;
		movement.y = 0f;
		vector += movement;
		if (vector.x > 0f)
		{
			vector.x = 0f;
		}
		if (vector.y < 0f)
		{
			vector.y = 0f;
		}
		if (vector.x < 0f - _pagerWidth)
		{
			vector.x = 0f - _pagerWidth;
		}
		if (vector.y > _pagerHeight)
		{
			vector.y = _pagerHeight;
		}
		_pivot.localPosition = vector;
	}

	private float PageWidth()
	{
		if (_childs.Count > 1)
		{
			return _pagerWidth / (float)(_childs.Count - 1);
		}
		return _pagerWidth;
	}

	public int ComputePage()
	{
		float num = PageWidth();
		int value = 0;
		if (num != 0f)
		{
			Vector3 localPosition = _pivot.localPosition;
			value = Mathf.RoundToInt((0f - localPosition.x) / num);
		}
		return Mathf.Clamp(value, 0, GetNumberOfPages() - 1);
	}

	public int GetNumberOfPages()
	{
		return _childs.Count;
	}

	private void SnapToPage()
	{
		Vector3 localPosition = _pivot.localPosition;
		int currentPage = _currentPage;
		float num = (float)(-currentPage) * PageWidth();
		float f = num - localPosition.x;
		float num2 = Mathf.Sign(f);
		localPosition.x += num2 * _currentSpeed * Time.deltaTime;
		if (Mathf.Sign(num - localPosition.x) != num2)
		{
			localPosition.x = num;
		}
		if (localPosition.x > 0f)
		{
			localPosition.x = 0f;
		}
		if (localPosition.y < 0f)
		{
			localPosition.y = 0f;
		}
		if (localPosition.x < 0f - _pagerWidth)
		{
			localPosition.x = 0f - _pagerWidth;
		}
		if (localPosition.y > _pagerHeight)
		{
			localPosition.y = _pagerHeight;
		}
		_pivot.localPosition = localPosition;
	}

	public bool IsSnapped()
	{
		Vector3 localPosition = _pivot.localPosition;
		float num = Mathf.Abs(localPosition.x % PageWidth());
		return num < 0.1f || PageWidth() - num < 0.1f;
	}
}
