using UnityEngine;

public class ChoppaEngineSound : MonoBehaviour
{
	private const float _setupVolume = 0.7f;

	public AudioClip _soundChoppaLoop;

	private AudioSource _audio;

	public void Awake()
	{
		_audio = base.gameObject.AddComponent<AudioSource>();
		ChoppaBehaviour component = GetComponent<ChoppaBehaviour>();
		component.OnAnimationDone += Shutdown;
	}

	private void Start()
	{
		UpdateVolume();
		PlayLoop();
	}

	private void PlayLoop()
	{
		_audio.volume = 0.7f * (float)AutoSingleton<PersistenceManager>.Instance.SoundsVolume;
		_audio.clip = _soundChoppaLoop;
		_audio.pitch = 0.9f;
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
		UpdateVolume();
	}

	private void UpdateVolume()
	{
		float to = 0.7f * (float)AutoSingleton<PersistenceManager>.Instance.SoundsVolume;
		if (AutoSingleton<PauseManager>.Instance.IsPaused())
		{
			to = 0f;
		}
		_audio.volume = Mathf.Lerp(_audio.volume, to, 0.05f);
	}
}
