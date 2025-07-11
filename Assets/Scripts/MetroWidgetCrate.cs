using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MetroWidgetCrate : MetroWidget
{
	private Reward _reward;

	private Transform _crate;

	private int _hp = 4;

	private bool _stopShake;

	private bool shaking;

	[method: MethodImpl(32)]
	private event Action OnBrokenCrate;

	public static MetroWidgetCrate Create(Reward reward, Action onBrokenCrate, float scale = 1f)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetCrate).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroWidgetCrate>().Setup(reward, onBrokenCrate, scale);
	}

	public MetroWidgetCrate Setup(Reward reward, Action onBrokenCrate, float scale)
	{
		_reward = reward;
		_crate = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("MetroCrate"), Vector3.zero, Quaternion.identity)).transform;
		MetroSpacer metroSpacer = MetroSpacer.Create();
		Add(metroSpacer);
		_crate.parent = metroSpacer.transform;
		_crate.localPosition = new Vector3(0f, 0f, -0.1f);
		_crate.localScale *= 1.5f * scale;
		if (onBrokenCrate != null)
		{
			this.OnBrokenCrate = (Action)Delegate.Combine(this.OnBrokenCrate, onBrokenCrate);
		}
		return this;
	}

	protected override void HandleFinger(Finger finger)
	{
		if (IsActive())
		{
			HandleClick(finger);
		}
	}

	private void HandleClick(Finger finger)
	{
		if (!(_crate == null))
		{
			Vector3 position = PrefabSingleton<CameraGUI>.Instance.Camera.ScreenToWorldPoint(finger.Position);
			Vector3 position2 = _crate.transform.position;
			position.z = position2.z - 1f;
			Hit(position);
		}
	}

	private void Hit(Vector3 position)
	{
		PrefabSingleton<GameSoundManager>.Instance.PlayWoodHitSound();
		Shake();
		PrefabSingleton<GameSpecialFXManager>.Instance.PlayGUICrateBreakFX(position);
		_hp--;
		if (_hp == 0)
		{
			Break();
			if (this.OnBrokenCrate != null)
			{
				this.OnBrokenCrate();
			}
		}
	}

	private void Break()
	{
		PrefabSingleton<GameSoundManager>.Instance.PlayWoodCrashSound();
		Transform transform = _crate.Find("Pivot");
		foreach (Transform item in transform)
		{
			item.gameObject.AddComponent<BrokenPiece>();
		}
		UnityEngine.Object.Destroy(_crate.gameObject);
		_crate = null;
		MetroWidgetReward metroWidgetReward = MetroWidgetReward.Create(_reward);
		Add(metroWidgetReward);
		Reflow();
		metroWidgetReward.transform.localRotation = Quaternion.identity;
		PrefabSingleton<GameSoundManager>.Instance.PlayRewardSound(_reward.GetRewardType());
	}

	public void Shake()
	{
		_stopShake = true;
		StartCoroutine(ShakeCR());
	}

	private IEnumerator ShakeCR()
	{
		yield return null;
		while (shaking)
		{
			yield return null;
		}
		shaking = true;
		_stopShake = false;
		Vector3 fromScale = base.transform.localScale;
		float extra = 1.2f;
		RealtimeDuration delay = new RealtimeDuration(0.2f);
		float range2 = UnityEngine.Random.Range(-5f, 5f);
		range2 += 7f * Mathf.Sign(range2);
		while (!delay.IsDone() && !_stopShake)
		{
			base.transform.localScale = fromScale * Mathfx.Berp(extra, 1f, delay.Value01());
			float decay = Mathf.InverseLerp(1f, 0f, delay.Value01());
			base.transform.localRotation = Quaternion.Euler(0f, 0f, range2 * Mathf.Cos(delay.Value01() * (float)Math.PI * 6f) * decay);
			yield return null;
		}
		base.transform.localScale = fromScale;
		base.transform.localRotation = Quaternion.identity;
		shaking = false;
		_stopShake = false;
	}

	private void Update()
	{
		if (_hp > 0 && IsActive() && (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.Space)))
		{
			Hit(base.transform.position);
		}
	}
}
