using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MetroButton : MetroWidget
{
	private Color _color;

	private List<KeyCode> _specialKeyList;

	private bool _isKeyNavigatorAccessible = true;

	private bool _stopBerp;

	private bool _berping;

	public bool IsKeyNavigatorAccessible
	{
		get
		{
			return _isKeyNavigatorAccessible;
		}
		set
		{
			_isKeyNavigatorAccessible = value;
		}
	}

	[method: MethodImpl(32)]
	public event Action OnButtonClicked;

	[method: MethodImpl(32)]
	public event Action OnKeyFocusGained;

	[method: MethodImpl(32)]
	public event Action OnKeyFocusLost;

	public void AddSpecialKey(KeyCode key)
	{
		_specialKeyList.Add(key);
	}

	public void ClearSpecialKey()
	{
		_specialKeyList.Clear();
	}

	public override void SetActive(bool active)
	{
		base.SetActive(active);
		KeyFocusLost();
		Refresh();
	}

	public MetroButton SetFont(MetroFont font)
	{
		MetroLabel[] componentsInChildren = GetComponentsInChildren<MetroLabel>();
		foreach (MetroLabel metroLabel in componentsInChildren)
		{
			metroLabel.SetFont(font);
		}
		return this;
	}

	public MetroButton SetText(string text)
	{
		MetroLabel[] componentsInChildren = GetComponentsInChildren<MetroLabel>();
		foreach (MetroLabel metroLabel in componentsInChildren)
		{
			metroLabel.SetText(text);
		}
		return this;
	}

	public MetroButton SetTextColor(Color color)
	{
		MetroLabel[] componentsInChildren = GetComponentsInChildren<MetroLabel>();
		foreach (MetroLabel metroLabel in componentsInChildren)
		{
			metroLabel.SetColor(color);
		}
		return this;
	}

	protected override void Refresh()
	{
		if (IsActive())
		{
			SetGradient(MetroSkin.ButtonColor1, MetroSkin.ButtonColor2);
		}
		else
		{
			SetColor(MetroSkin.ButtonInactiveColor);
		}
		base.Refresh();
	}

	protected override void OnAwake()
	{
		_specialKeyList = new List<KeyCode>();
		Refresh();
		base.OnAwake();
	}

	public void Flash()
	{
		StartCoroutine(FlashCR());
	}

	private IEnumerator FlashCR()
	{
		RealtimeDuration delay = new RealtimeDuration(0.2f);
		Color flashColor = MetroSkin.FlashColor;
		while (!delay.IsDone())
		{
			if (delay.Value01() < 0.5f)
			{
				SetColor(Color.Lerp(_color, flashColor, delay.Value01()), changeState: false);
			}
			else
			{
				SetColor(Color.Lerp(flashColor, _color, delay.Value01() - 0.5f), changeState: false);
			}
			yield return null;
		}
		SetColor(_color, changeState: false);
	}

	public void Berp()
	{
		_stopBerp = true;
		StartCoroutine(BerpCR());
	}

	private IEnumerator BerpCR()
	{
		yield return null;
		while (_berping)
		{
			yield return null;
		}
		_berping = true;
		_stopBerp = false;
		Vector3 fromScale = base.transform.localScale;
		float extra = 1.2f;
		RealtimeDuration delay = new RealtimeDuration(0.25f);
		while (!delay.IsDone() && !_stopBerp)
		{
			base.transform.localScale = fromScale * Mathfx.Berp(extra, 1f, delay.Value01());
			yield return null;
		}
		base.transform.localScale = fromScale;
		_berping = false;
		_stopBerp = false;
	}

	public override MetroWidget SetColor(Color color)
	{
		SetColor(color, changeState: true);
		return this;
	}

	private void SetColor(Color color, bool changeState)
	{
		if (changeState)
		{
			_color = color;
		}
		base.SetColor(color);
	}

	protected override void HandleFinger(Finger finger)
	{
		if (IsActive())
		{
			OnButtonSelected();
		}
	}

	protected virtual void PlayClickFX()
	{
		Berp();
		PrefabSingleton<GameSoundManager>.Instance.PlayButtonSound();
	}

	public void KeyFocusGained()
	{
		if (this.OnKeyFocusGained != null)
		{
			this.OnKeyFocusGained();
		}
	}

	public void KeyFocusLost()
	{
		if (this.OnKeyFocusLost != null)
		{
			this.OnKeyFocusLost();
		}
	}

	private void OnButtonSelected()
	{
		if (base.enabled)
		{
			PlayClickFX();
			if (this.OnButtonClicked != null)
			{

				this.OnButtonClicked();
			}
		}
	}

	public void OnKey()
	{
		if (IsActive())
		{
			OnButtonSelected();
		}
	}

	public void Update()
	{
		OnButtonUpdate();
	}

	protected virtual void OnButtonUpdate()
	{
		foreach (KeyCode specialKey in _specialKeyList)
		{
			if (UnityEngine.Input.GetKeyDown(specialKey))
			{
				OnKey();
				break;
			}
		}
	}

	public static MetroButton Create()
	{
		GameObject gameObject = new GameObject("button");
		gameObject.transform.position = Vector3.zero;
		return gameObject.AddComponent<MetroButton>();
	}

	public static MetroButton Create(string label)
	{
		MetroButton metroButton = Create();
		metroButton.gameObject.name = label;
		metroButton.Add(MetroLabel.Create(label));
		return metroButton;
	}
}
