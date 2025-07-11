using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MetroWidgetXPBar : MetroWidgetProgressBar
{
	private const float _animTimeForFullProgress = 0.6f;

	public bool IsAnimating
	{
		get;
		private set;
	}

	public bool IsPaused
	{
		get;
		private set;
	}

	[method: MethodImpl(32)]
	public event Action<int> OnLevelUp;

	public static MetroWidgetXPBar Create(XPProfile xpProfile)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetXPBar).ToString());
		gameObject.transform.position = Vector3.zero;
		ProgressBarColorPalette colors = new ProgressBarColorPalette(MetroSkin.XPWidgetBackColor, MetroSkin.XPWidgetBarColor1, MetroSkin.XPWidgetBarColor2);
		return gameObject.AddComponent<MetroWidgetXPBar>().Setup(xpProfile.Level.ToString(), xpProfile.GetProgress01(), colors, Resources.Load(MetroSkin.ProgressBarMat) as Material) as MetroWidgetXPBar;
	}

	public void StartAnim(string prefix, int startLevel, float startProgress01, int endLevel, float endProgress01, ProgressBarColorPalette colors)
	{
		IsPaused = false;
		StartCoroutine(LerpProgress(prefix, startLevel, startProgress01, endLevel, endProgress01, colors));
	}

	public void ResumeAnim()
	{
		IsPaused = false;
		IsAnimating = true;
	}

	public void StopAnim()
	{
		IsAnimating = false;
	}

	private IEnumerator LerpProgress(string prefix, int startLevel, float startProgress01, int endLevel, float endProgress01, ProgressBarColorPalette colors)
	{
		IsAnimating = true;
		SetValue(prefix + startLevel.ToString(), startProgress01);
		AnimateColors(colors);
		float unitIncreaseDelay = 0.6f;
		for (int i = startLevel; i <= endLevel; i++)
		{
			if (!IsAnimating)
			{
				break;
			}
			PrefabSingleton<GameSoundManager>.Instance.PlayXPGoingUp(i - startLevel);
			PrefabSingleton<GameSpecialFXManager>.Instance.StartGUIGrindFX(GetBarPos());
			float fromVal = _val;
			float toVal = (i != endLevel) ? 1f : endProgress01;
			Duration transitDelay2 = new Duration(unitIncreaseDelay);
			while (!transitDelay2.IsDone() && IsAnimating)
			{
				SetValue(val: Mathf.Lerp(fromVal, toVal, Mathfx.Hermite(0f, 1f, transitDelay2.Value01())), text: prefix + i.ToString());
				PrefabSingleton<GameSpecialFXManager>.Instance.MoveGUIGrindFX(GetBarPos());
				yield return null;
			}
			PrefabSingleton<GameSpecialFXManager>.Instance.StopGUIGrindFX();
			if (toVal >= 1f)
			{
				transitDelay2 = new Duration(0.2f);
				while (!transitDelay2.IsDone() && IsAnimating)
				{
					PrefabSingleton<GameSpecialFXManager>.Instance.PlayGUISparksFX(_label.transform.position);
					yield return null;
				}
				if (this.OnLevelUp != null)
				{
					this.OnLevelUp(i + 1);
					IsPaused = true;
					while (IsPaused)
					{
						yield return null;
					}
				}
			}
			_val = 0f;
		}
		SetValue(prefix + endLevel.ToString(), endProgress01);
		PrefabSingleton<GameSpecialFXManager>.Instance.StopGUIGrindFX();
		AnimateColors(_colors);
		IsAnimating = false;
	}

	private void AnimateColors(ProgressBarColorPalette colors)
	{
		_fill.GetComponentInChildren<MetroQuad>().SetGradientOverTime(colors._fillColor1, colors._fillColor2, 0.5f, null);
		_endFill.GetComponentInChildren<MetroQuad>().SetGradientOverTime(colors._fillColor1, colors._fillColor2, 0.5f, null);
	}
}
