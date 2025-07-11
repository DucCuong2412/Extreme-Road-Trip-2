using System;
using UnityEngine;

public class MetroButtonToggle : MetroButton
{
	private bool _state;

	public Action OnToggle;

	public static MetroButtonToggle Create(Action onToggle)
	{
		GameObject gameObject = new GameObject("buttonToggle");
		gameObject.transform.position = Vector3.zero;
		MetroButtonToggle metroButtonToggle = gameObject.AddComponent<MetroButtonToggle>();
		metroButtonToggle.Setup(onToggle);
		return metroButtonToggle;
	}

	private void Setup(Action onToggle)
	{
		OnToggle = onToggle;
		base.OnButtonClicked += HandleClick;
	}

	private void HandleClick()
	{
		Toggle(!_state);
		if (OnToggle != null)
		{
			OnToggle();
		}
	}

	public void Toggle(bool state)
	{
		_state = state;
	}

	public bool State()
	{
		return _state;
	}
}
