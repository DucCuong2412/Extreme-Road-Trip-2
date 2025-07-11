using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CarBoost : MonoBehaviour
{
	public enum BoostState
	{
		none,
		boost,
		megaBoost
	}

	private CarController _car;

	private ParticleSystem _megaBoostFX;

	private float _boostTime;

	private BoostState _state;

	private bool _usingRocket;

	[method: MethodImpl(32)]
	public event Action<bool> OnLandingMegaboostState;

	public void TriggerMegaBoost(bool usingRocket = false)
	{
		_usingRocket = usingRocket;
		_boostTime = 11f;
	}

	public bool IsMegaBoosting()
	{
		return _state == BoostState.megaBoost;
	}

	public void Start()
	{
		_state = BoostState.none;
		_car = GetComponent<CarController>();
		_car.OnCrash += OnCrash;
		_megaBoostFX = PrefabSingleton<GameSpecialFXManager>.Instance.AddCarMegaBoostFX(_car.transform);
	}

	public float GetLandStuntScore(Queue<StuntEvent> stuntEvents, StuntEvent stunt)
	{
		float num = 0f;
		if (stuntEvents.Count > 1)
		{
			float landEventBoostFactor = GameSettings.GetLandEventBoostFactor(stunt);
			num = (float)stuntEvents.Count * landEventBoostFactor;
			num *= _car.Config._boostExtraFactor;
			if (num > 0.1f)
			{
				PrefabSingleton<GameSoundManager>.Instance.PlayBoostSound();
				PrefabSingleton<CameraGame>.Instance.OnBoostEvent(-2f, 1f);
				PrefabSingleton<CameraGame>.Instance.Shake(landEventBoostFactor, 3f);
			}
		}
		return num;
	}

	public void OnLandStunt(float stuntScore)
	{
		bool flag = false;
		if (_state != BoostState.megaBoost)
		{
			_boostTime += stuntScore;
			flag = (_boostTime >= 10f);
			if (flag)
			{
				AutoSingleton<AchievementsManager>.Instance.OnMegaBoostActivated();
				AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordMegaBoost(_car.Car, 1f);
			}
		}
		if (this.OnLandingMegaboostState != null)
		{
			this.OnLandingMegaboostState(flag);
		}
	}

	public bool IsBoosting()
	{
		return _state != BoostState.none;
	}

	public float GetBoost01()
	{
		switch (_state)
		{
		case BoostState.megaBoost:
			return Mathf.Clamp01(_boostTime / 5f);
		case BoostState.boost:
			return Mathf.Clamp01(_boostTime / 10f);
		default:
			return 0f;
		}
	}

	public void OnCrash()
	{
		_boostTime = 0f;
		if (_megaBoostFX != null)
		{
			_megaBoostFX.Stop();
		}
		PrefabSingleton<GameSoundManager>.Instance.StopMegaBoostSound();
	}

	private void Update()
	{
		if (_boostTime <= 0f)
		{
			_boostTime = 0f;
			_state = BoostState.none;
			if (_megaBoostFX != null)
			{
				_megaBoostFX.Stop();
			}
		}
		else if (_state == BoostState.none && _boostTime > 0f)
		{
			_state = BoostState.boost;
		}
		else if (_state == BoostState.boost && _boostTime > 10f)
		{
			_state = BoostState.megaBoost;
			_boostTime = 5f;
			if (!_usingRocket && _megaBoostFX != null)
			{
				_megaBoostFX.Play();
			}
			PrefabSingleton<GameSoundManager>.Instance.PlayBoostSound();
			PrefabSingleton<GameSoundManager>.Instance.StartMegaBoostSound();
			PrefabSingleton<CameraGame>.Instance.OnBoostEvent(-6f, 1.5f);
			PrefabSingleton<CameraGame>.Instance.Shake(5f, 3f);
		}
		else if (IsBoosting())
		{
			if (!_usingRocket)
			{
				_boostTime -= Time.deltaTime;
			}
			if (!_usingRocket && _megaBoostFX != null)
			{
				_megaBoostFX.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
			}
		}
	}
}
