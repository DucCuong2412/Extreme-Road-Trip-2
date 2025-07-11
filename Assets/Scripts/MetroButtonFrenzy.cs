using UnityEngine;

public class MetroButtonFrenzy : MetroButton
{
	private MetroBadge _badge;

	private MetroLabel _timerLabel;

	public new static MetroButtonFrenzy Create()
	{
		GameObject gameObject = new GameObject(typeof(MetroButtonFrenzy).ToString());
		return gameObject.AddComponent<MetroButtonFrenzy>().Setup();
	}

	private MetroButtonFrenzy Setup()
	{
		AddSlice9Background(MetroSkin.Slice9Button);
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		string timerString = AutoSingleton<FrenzyModeManager>.Instance.GetTimerString();
		bool flag = timerString != string.Empty;
		if (flag)
		{
			metroLayout.Add(MetroSpacer.Create());
		}
		MetroIcon metroIcon = MetroIcon.Create((AutoSingleton<LocalizationManager>.Instance.Language != 0) ? MetroSkin.IconFrenzyRunFrench : MetroSkin.IconFrenzyRunEnglish);
		metroIcon.SetMass(3f);
		metroIcon.SetScale(0.7f);
		metroLayout.Add(metroIcon);
		if (flag)
		{
			_timerLabel = MetroLabel.Create(timerString);
			_timerLabel.SetFont(MetroSkin.VerySmallFont);
			_timerLabel.SetMass(2f);
			metroLayout.Add(_timerLabel);
		}
		_badge = MetroBadge.Create();
		Add(_badge);
		_badge.UpdateBadge(AutoSingleton<FrenzyModeManager>.Instance.GetBadgeCaption(), showIcon: true, !AutoSingleton<FrenzyModeManager>.Instance.HasFreeAccess());
		base.OnButtonClicked += delegate
		{
			AutoSingleton<FrenzyModeManager>.Instance.HandleAccess();
		};
		base.OnKeyFocusGained += delegate
		{
			AddSlice9Background(MetroSkin.Slice9ButtonRed);
			Berp();
		};
		base.OnKeyFocusLost += delegate
		{
			AddSlice9Background(MetroSkin.Slice9Button);
		};
		return this;
	}

	protected override void OnButtonUpdate()
	{
		if (_badge != null)
		{
			FrenzyModeManager instance = AutoSingleton<FrenzyModeManager>.Instance;
			_badge.UpdateBadge(instance.GetBadgeCaption(), showIcon: true, !instance.HasFreeAccess());
		}
		if (_timerLabel != null)
		{
			string text = AutoSingleton<FrenzyModeManager>.Instance.GetTimerString();
			if (text != string.Empty)
			{
				text = "Next in: ".Localize() + text;
			}
			_timerLabel.SetText(text);
		}
		base.OnButtonUpdate();
	}
}
