using System;
using System.Collections;
using UnityEngine;

public class MetroMenuPage : MetroMenu
{
	protected delegate IEnumerator MenuTransition(Vector3 from, Vector3 to, Action before, Action after);

	public static MetroAnimation DefaultAnimation = MetroAnimation.slideLeft;

	private bool _autoPush = true;

	protected MetroQuad _mask;

	public bool AutoPush
	{
		get
		{
			return _autoPush;
		}
		set
		{
			_autoPush = value;
		}
	}

	public int Layer
	{
		get;
		set;
	}

	public float GetZ()
	{
		return (float)(-Layer) * 10f;
	}

	protected virtual void ShowMask()
	{
		if (_mask == null)
		{
			_mask = MetroQuad.Create(base.transform);
			_mask.transform.localPosition = new Vector3(0f, 0f, -5f);
			_mask.SetSize(_zone.width, _zone.height);
			_mask.SetColor(MetroSkin.Transparent);
			_mask.SetColorOverTime(MetroSkin.DarkMask, 0.3f, null);
		}
	}

	protected virtual void HideMask()
	{
		if (_mask != null)
		{
			MetroQuad temp = _mask;
			temp.SetColorOverTime(MetroSkin.Transparent, 0.3f, delegate
			{
				UnityEngine.Object.Destroy(temp.gameObject);
			});
			_mask = null;
		}
	}

	public void OnPop(MetroAnimation animation)
	{
		_active = false;
		StartCoroutine(OnPopAnimation(animation, delegate
		{
			Cleanup();
			UnityEngine.Object.Destroy(base.gameObject);
		}));
	}

	public void OnPush(MetroAnimation animation)
	{
		StartCoroutine(OnPushAnimation(animation, delegate
		{
			OnFocus();
		}));
	}

	public override void OnFocus()
	{
		_active = true;
		HideMask();
		base.OnFocus();
	}

	public override void OnBlur()
	{
		_active = false;
		ShowMask();
		base.OnBlur();
	}

	protected virtual IEnumerator OnPushAnimation(MetroAnimation animation, Action onDone)
	{
		float w = base.Camera.ScreenWidth;
		float h = base.Camera.ScreenHeight;
		float z = GetZ();
		switch (animation)
		{
		case MetroAnimation.none:
			yield return StartCoroutine(SnapToPosition(new Vector3(0f, 0f, z), null, onDone));
			break;
		case MetroAnimation.slideLeft:
			yield return StartCoroutine(LerpFromTo(new Vector3(w, 0f, z), new Vector3(0f, 0f, z), null, onDone));
			break;
		case MetroAnimation.slideRight:
			yield return StartCoroutine(LerpFromTo(new Vector3(0f - w, 0f, z), new Vector3(0f, 0f, z), null, onDone));
			break;
		case MetroAnimation.slideUp:
			yield return StartCoroutine(LerpFromTo(new Vector3(0f, 0f - h, z), new Vector3(0f, 0f, z), null, onDone));
			break;
		case MetroAnimation.slideDown:
			yield return StartCoroutine(LerpFromTo(new Vector3(0f, h, z), new Vector3(0f, 0f, z), null, onDone));
			break;
		case MetroAnimation.berpDown:
			yield return StartCoroutine(BerpFromTo(new Vector3(0f, h, z), new Vector3(0f, 0f, z), null, onDone));
			break;
		case MetroAnimation.popup:
			yield return StartCoroutine(ScaleFromTo(new Vector3(0.1f, 0.1f, 1f), new Vector3(1f, 1f, 1f), null, onDone));
			break;
		}
	}

	protected virtual IEnumerator OnPopAnimation(MetroAnimation animation, Action onDone)
	{
		float w = base.Camera.ScreenWidth;
		float h = base.Camera.ScreenHeight;
		float z = GetZ();
		switch (animation)
		{
		case MetroAnimation.none:
			yield return StartCoroutine(SnapToPosition(GameSettings.OutOfWorldVector, null, onDone));
			break;
		case MetroAnimation.slideLeft:
			yield return StartCoroutine(LerpFromTo(new Vector3(0f, 0f, z), new Vector3(0f - w, 0f, z), null, onDone));
			break;
		case MetroAnimation.slideRight:
			yield return StartCoroutine(LerpFromTo(new Vector3(0f, 0f, z), new Vector3(w, 0f, z), null, onDone));
			break;
		case MetroAnimation.slideUp:
			yield return StartCoroutine(LerpFromTo(new Vector3(0f, 0f, z), new Vector3(0f, h, z), null, onDone));
			break;
		case MetroAnimation.slideDown:
			yield return StartCoroutine(LerpFromTo(new Vector3(0f, 0f, z), new Vector3(0f, 0f - h, z), null, onDone));
			break;
		case MetroAnimation.berpDown:
			yield return StartCoroutine(BerpFromTo(new Vector3(0f, 0f, z), new Vector3(0f, 0f - h, z), null, onDone));
			break;
		case MetroAnimation.popup:
			yield return StartCoroutine(ScaleFromTo(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 1f), null, onDone));
			break;
		}
	}

	protected IEnumerator SnapToPosition(Vector3 to, Action before, Action after)
	{
		yield return null;
		before?.Invoke();
		base.transform.localPosition = to;
		yield return null;
		after?.Invoke();
	}

	protected IEnumerator LerpFromTo(Vector3 from, Vector3 to, Action before, Action after)
	{
		yield return null;
		before?.Invoke();
		RealtimeDuration delay = new RealtimeDuration(MetroSkin.MenuPageSlideDuration);
		PrefabSingleton<GameSoundManager>.Instance.PlayWhishSound();
		while (!delay.IsDone())
		{
			float x = Mathfx.Hermite(0f, 1f, delay.Value01());
			base.transform.localPosition = Vector3.Lerp(from, to, x);
			yield return null;
		}
		base.transform.localPosition = to;
		after?.Invoke();
	}

	protected IEnumerator BerpFromTo(Vector3 from, Vector3 to, Action before, Action after)
	{
		yield return null;
		Vector3 at = to - from;
		before?.Invoke();
		RealtimeDuration delay = new RealtimeDuration(MetroSkin.MenuPageBerpDuration);
		PrefabSingleton<GameSoundManager>.Instance.PlayWhishSound();
		while (!delay.IsDone())
		{
			float x = Mathfx.Berp(0f, 1f, delay.Value01());
			base.transform.localPosition = from + at * x;
			yield return null;
		}
		base.transform.localPosition = to;
		after?.Invoke();
	}

	protected IEnumerator ScaleFromTo(Vector3 from, Vector3 to, Action before, Action after)
	{
		yield return null;
		before?.Invoke();
		base.transform.localPosition = new Vector3(0f, 0f, GetZ());
		base.transform.localScale = from;
		RealtimeDuration delay = new RealtimeDuration(MetroSkin.MenuPageScaleDuration);
		while (!delay.IsDone())
		{
			float x = Mathfx.Hermite(0f, 1f, delay.Value01());
			base.transform.localScale = Vector3.Lerp(from, to, x);
			yield return null;
		}
		base.transform.localScale = to;
		after?.Invoke();
	}

	public static T Create<T>() where T : MetroMenuPage
	{
		GameObject gameObject = new GameObject(typeof(T).ToString());
		gameObject.transform.position = GameSettings.OutOfWorldVector;
		T result = gameObject.AddComponent<T>();
		result.AutoPush = false;
		return result;
	}

	protected override void OnAwake()
	{
		_active = false;
		base.OnAwake();
	}

	protected override void OnStart()
	{
		if (AutoPush)
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(this, MetroAnimation.slideDown);
		}
		base.OnStart();
	}
}
