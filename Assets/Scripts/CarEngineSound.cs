using System.Collections;
using UnityEngine;

public class CarEngineSound : MonoBehaviour
{
	private AudioClip _soundEngineStart;

	private AudioClip _soundEngineLoop;

	private bool _running;

	private bool _looping;

	private float _invMaxSpeed;

	private float _setupVolume;

	private CarController _car;

	private CarBoost _carBoost;

	private AudioSource _audio;

	public void Awake()
	{
		_audio = base.gameObject.AddComponent<AudioSource>();
		_car = GetComponent<CarController>();
		_car.OnCrash += Shutdown;
		_carBoost = GetComponent<CarBoost>();
		_soundEngineStart = (Resources.Load("EngineStart") as AudioClip);
		_soundEngineLoop = (Resources.Load("EngineLoop") as AudioClip);
	}

	private void Start()
	{
		_running = false;
		_looping = false;
		_setupVolume = 0.1f;
		_invMaxSpeed = 1f / _car.Car.MaxSpeed;
		UpdateVolume();
		PlayStart();
	}

	private void PlayStart()
	{
		if (!_running)
		{
			_audio.clip = _soundEngineStart;
			_audio.volume = _setupVolume * (float)AutoSingleton<PersistenceManager>.Instance.SoundsVolume;
			_audio.pitch = 1f;
			_audio.loop = false;
			_audio.Play();
			_running = true;
			StartCoroutine(WaitThenPlayLoopCR());
		}
	}

	private IEnumerator WaitThenPlayLoopCR()
	{
		yield return new WaitForSeconds(_audio.clip.length - 0.4f);
		PlayLoop();
	}

	private void PlayLoop()
	{
		_looping = true;
		_audio.clip = _soundEngineLoop;
		UpdateLoopPitch();
		_audio.loop = true;
		_audio.Play();
	}

	private void Shutdown()
	{
		_audio.Stop();
		_audio.volume = 0f;
	}

	public void Update()
	{
		if (_looping)
		{
			UpdateLoopPitch();
			UpdateVolume();
		}
	}

	private void UpdateLoopPitch()
	{
		float magnitude = _car.Velocity.magnitude;
		float num = Mathf.Clamp(magnitude * _invMaxSpeed, 0f, 1f);
		if (_carBoost.IsBoosting())
		{
			num = 1f;
		}
		if (_carBoost.IsMegaBoosting())
		{
			num = 1.25f;
		}
		_audio.pitch = Mathf.Lerp(_audio.pitch, Mathf.Pow(1f + num, 1.75f), 5f * Time.deltaTime);
	}

	private void UpdateVolume()
	{
		float to = _setupVolume * (float)AutoSingleton<PersistenceManager>.Instance.SoundsVolume;
		if (AutoSingleton<PauseManager>.Instance.IsPaused())
		{
			to = 0f;
		}
		_audio.volume = Mathf.Lerp(_audio.volume, to, 0.05f);
	}
}
