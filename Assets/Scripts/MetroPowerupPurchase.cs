using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroPowerupPurchase : MetroWidget
{
	private List<MetroButtonPowerup> _buttons = new List<MetroButtonPowerup>();

	public static MetroPowerupPurchase Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroPowerupPurchase).ToString());
		gameObject.transform.position = Vector3.zero;
		return gameObject.AddComponent<MetroPowerupPurchase>();
	}

	protected override void HandleFinger(Finger finger)
	{
		foreach (MetroButtonPowerup button in _buttons)
		{
			button.OnFinger(finger);
		}
	}

	protected override void OnStart()
	{
		Singleton<GameManager>.Instance.OnGameSetupStarted += Show;
		Singleton<GameManager>.Instance.OnGameSetupEnded += Hide;
		base.OnStart();
	}

	protected override void Cleanup()
	{
		Singleton<GameManager>.Instance.OnGameSetupStarted -= Show;
		Singleton<GameManager>.Instance.OnGameSetupEnded -= Hide;
		base.Cleanup();
	}

	private void Show()
	{
		StartCoroutine(ShowCR());
	}

	private void Hide()
	{
		StartCoroutine(HideCR());
	}

	private IEnumerator ShowCR()
	{
		List<PowerupType> powerups = new List<PowerupType>();
		foreach (int p in Enum.GetValues(typeof(PowerupType)))
		{
			if (AutoSingleton<Player>.Instance.Profile.CanUsePowerup((PowerupType)p))
			{
				powerups.Add((PowerupType)p);
			}
		}
		int buttonCount = powerups.Count;
		float spacing = _zone.width / (float)buttonCount;
		for (int i = 0; i < buttonCount; i++)
		{
			MetroButtonPowerup button = MetroButtonPowerup.Create(powerups[i], base.transform, 0f - _zone.width * 0.5f + ((float)i + 0.5f) * spacing);
			_buttons.Add(button);
		}
		if (_buttons.Count > 0)
		{
			AutoSingleton<MetroWidgetNotificationManager>.Instance.ShowMoney();
		}
		foreach (MetroButtonPowerup b in _buttons)
		{
			b.Show();
			yield return new WaitForSeconds(0.2f);
		}
	}

	private IEnumerator HideCR()
	{
		foreach (MetroButtonPowerup b in _buttons)
		{
			b.Hide();
			yield return null;
		}
	}
}
