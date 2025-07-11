using UnityEngine;

public class MetroMenu : MetroWidget
{
	private MetroMenuNavigator _navigator = new MetroMenuNavigator();

	protected override void OnStart()
	{
		base.OnStart();
		AutoSingleton<InputManager>.Instance.OnFinger += OnFinger;
		Init();
	}

	public virtual void OnDestroy()
	{
		if (AutoSingleton<InputManager>.IsCreated())
		{
			AutoSingleton<InputManager>.Instance.OnFinger -= OnFinger;
		}
	}

	protected virtual Rect ViewRect()
	{
		float num = 0f;
		return new Rect(0f - base.Camera.HalfScreenWidth + num * 0.5f, 0f - base.Camera.HalfScreenHeight + num * 0.5f, base.Camera.ScreenWidth - num, base.Camera.ScreenHeight - num);
	}

	protected void Init()
	{
		Layout(ViewRect());
	}

	public override void Layout(Rect zone)
	{
		base.Layout(zone);
		base.transform.position = GameSettings.OutOfWorldVector;
	}

	private void OnFinger(Finger f)
	{
		HandleFinger(f);
	}

	public void Update()
	{
		OnMenuUpdate();
	}

	protected virtual void OnMenuUpdate()
	{
		if (IsActive())
		{
			_navigator.Navigate(this);
		}
	}

	public static void AddKeyNavigationBehaviour(MetroButton button, string selectedSlice9Color, string unselectedSlice9Color)
	{
		button.OnKeyFocusGained += delegate
		{
			button.AddSlice9Background(selectedSlice9Color);
		};
		button.OnKeyFocusLost += delegate
		{
			button.AddSlice9Background(unselectedSlice9Color);
		};
	}

	public static void AddKeyNavigationBehaviour(MetroButton button, Color selectedColor, Color unselectedColor)
	{
		button.OnKeyFocusGained += delegate
		{
			button.AddSolidBackground(selectedColor);
		};
		button.OnKeyFocusLost += delegate
		{
			button.AddSolidBackground(unselectedColor);
		};
	}
}
