using System.Collections;
using UnityEngine;

public class MetroButtonPowerup : MetroButton
{
	private MetroIcon _icon;

	private MetroSpacer _priceSpacer;

	private bool _isAnimating;

	private Rect _moneyViewRect;

	public bool IsSelected
	{
		get;
		private set;
	}

	public static MetroButtonPowerup Create(PowerupType pType, Transform parent, float xPos)
	{
		GameObject gameObject = new GameObject(pType.ToString());
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.parent = parent;
		MetroButtonPowerup metroButtonPowerup = gameObject.AddComponent<MetroButtonPowerup>();
		metroButtonPowerup.Setup(pType, xPos);
		switch (pType)
		{
		case PowerupType.boost:
			metroButtonPowerup.AddSpecialKey(KeyCode.Alpha1);
			metroButtonPowerup.AddSpecialKey(KeyCode.Keypad1);
			break;
		case PowerupType.coinDoubler:
			metroButtonPowerup.AddSpecialKey(KeyCode.Alpha2);
			metroButtonPowerup.AddSpecialKey(KeyCode.Keypad2);
			break;
		case PowerupType.magnet:
			metroButtonPowerup.AddSpecialKey(KeyCode.Alpha3);
			metroButtonPowerup.AddSpecialKey(KeyCode.Keypad3);
			break;
		}
		return metroButtonPowerup;
	}

	public void Show()
	{
		StartCoroutine(BerpYFromTo(_zone.y, 0f));
	}

	public void Hide()
	{
		Vector3 localPosition = _transform.localPosition;
		StartCoroutine(BerpYFromTo(localPosition.y, base.Camera.HalfScreenHeight + _zone.height));
		ClearSpecialKey();
	}

	private IEnumerator BerpYFromTo(float from, float to)
	{
		while (_isAnimating)
		{
			yield return null;
		}
		_isAnimating = true;
		Duration delay = new Duration(1f);
		while (!delay.IsDone())
		{
			float x = Mathfx.Hermite(0f, 1f, delay.Value01());
			Transform transform = _transform;
			Vector3 localPosition = _transform.localPosition;
			float x2 = localPosition.x;
			float y = Mathfx.Berp(from, to, x);
			Vector3 localPosition2 = _transform.localPosition;
			transform.localPosition = new Vector3(x2, y, localPosition2.z);
			yield return null;
		}
		_isAnimating = false;
	}

	public void OnFinger(Finger finger)
	{
		if (TouchIsInZone(finger.Touch))
		{
			HandleFinger(finger);
		}
	}

	protected override bool TouchIsInZone(FakeTouch touch)
	{
		float num = _moneyViewRect.height * 0.5f;
		Vector3 vector = TouchPositionInWorldSpace(touch);
		Vector3 vector2 = base.transform.position - Vector3.up * num;
		float num2 = _zone.width * 0.5f;
		float num3 = _zone.height * 0.5f + num;
		return Mathf.Abs(vector.x - vector2.x) < num2 && Mathf.Abs(vector.y - vector2.y) < num3;
	}

	protected override void Refresh()
	{
		if (!IsActive() && !IsSelected)
		{
			IsSelected = true;
			_icon.Add(MetroIcon.Create(MetroSkin.IconPowerupActivated));
			_icon.Reflow();
			UnityEngine.Object.Destroy(_priceSpacer.gameObject);
		}
		base.Refresh();
	}

	private void Setup(PowerupType pType, float xPos)
	{
		_transform = base.transform;
		Powerup p = AutoSingleton<PowerupManager>.Instance.GetPowerup(pType);
		Price price = p.Price;
		bool hasPowerup = AutoSingleton<Player>.Instance.Profile.GetNumPowerups(pType) > 0;
		base.OnButtonClicked += delegate
		{
			if (hasPowerup)
			{
				if (!IsSelected)
				{
					AutoSingleton<Player>.Instance.Profile.OnPowerupRemoved(pType, 1);
					EnablePowerUp(p);
				}
			}
			else if (AutoSingleton<CashManager>.Instance.CanBuy(price))
			{
				if (!IsSelected)
				{
					AutoSingleton<CashManager>.Instance.Buy(price);
					EnablePowerUp(p);
				}
			}
			else
			{
				AutoSingleton<CashManager>.Instance.HandleNotEnoughCurrency(price, pauseOnFocus: true);
			}
		};
		string iconName = string.Empty;
		switch (p.GetPowerupType())
		{
		case PowerupType.boost:
			iconName = MetroSkin.IconPowerupBoost;
			break;
		case PowerupType.coinDoubler:
			iconName = MetroSkin.IconPowerupCoinDoubler;
			break;
		case PowerupType.magnet:
			iconName = MetroSkin.IconPowerupMagnet;
			break;
		}
		_icon = MetroIcon.Create(iconName);
		_icon.SetMass(1f);
		Add(_icon);
		Bounds bounds = RendererBounds.ComputeBounds(_transform);
		Vector3 size = bounds.size;
		float x = size.x;
		Vector3 size2 = bounds.size;
		float y = size2.y;
		float left = xPos - x * 0.5f;
		float top = base.Camera.HalfScreenHeight + y;
		Rect zone = new Rect(left, top, x, y);
		Layout(zone);
		_priceSpacer = MetroSpacer.Create();
		_priceSpacer.AddSlice9Background(MetroSkin.Slice9RoundedSemiTransparent);
		MetroLabel metroLabel = MetroLabel.Create("0");
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.AddOutline();
		MetroWidgetPrice metroWidgetPrice = MetroWidgetPrice.Create(price);
		metroWidgetPrice.SetFont(MetroSkin.MediumFont);
		metroWidgetPrice.AddOutline();
		float num = 0f;
		Bounds bounds2 = RendererBounds.ComputeBounds(metroLabel.transform);
		Bounds bounds3 = RendererBounds.ComputeBounds(metroWidgetPrice.transform);
		float num2 = x + 0.5f;
		Vector3 size3 = bounds2.size;
		float y2 = size3.y;
		Vector3 size4 = bounds3.size;
		float num3 = Mathf.Max(y2, size4.y) + 1f + num;
		float left2 = (0f - num2) * 0.5f;
		float top2 = (0f - y) * 0.5f - num3;
		_moneyViewRect = new Rect(left2, top2, num2, num3);
		if (hasPowerup)
		{
			metroWidgetPrice.Destroy();
			if (AutoSingleton<Player>.Instance.Profile.IsNewPowerup(p.GetPowerupType()))
			{
				metroLabel.SetText("TRY IT!");
				StartCoroutine(GlowCR(metroLabel));
			}
			else
			{
				metroLabel.SetText("X" + AutoSingleton<Player>.Instance.Profile.GetNumPowerups(p.GetPowerupType()).ToString());
			}
			_priceSpacer.Add(metroLabel);
		}
		else
		{
			metroLabel.Destroy();
			_priceSpacer.Add(metroWidgetPrice);
		}
		_priceSpacer.transform.parent = _icon.transform;
		_priceSpacer.Layout(_moneyViewRect);
	}

	private void EnablePowerUp(Powerup p)
	{
		AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordPowerUpUsage(p.GetPowerupType());
		SetActive(active: false);
		p.Enable();
	}

	private IEnumerator GlowCR(MetroLabel label)
	{
		Color startColor = MetroSkin.NewPowerupGlowColor1;
		Color finalColor = MetroSkin.NewPowerupGlowColor2;
		float delayValue = 0.413793117f;
		Duration delay = new Duration(delayValue);
		while (label != null)
		{
			label.SetColor(Color.Lerp(startColor, finalColor, delay.Value01()));
			if (delay.IsDone())
			{
				delay = new Duration(delayValue);
				Color tempColor = startColor;
				startColor = finalColor;
				finalColor = tempColor;
			}
			yield return null;
		}
	}
}
