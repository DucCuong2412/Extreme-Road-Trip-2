using System.Collections;
using UnityEngine;

public class RoofdogSoundManager<T> : PrefabSingleton<T> where T : MonoBehaviour
{
	private const int _poolSize = 16;

	protected float _masterVolume;

	private AudioSource[] _pool;

	private int _poolIndex;
    private void Awake()
    {
        OnAwake();
		
    }

    protected void PlayClip(AudioClip clip, float volume)
	{
		AudioSource nextAudioSource = GetNextAudioSource();
		nextAudioSource.clip = clip;
		nextAudioSource.loop = false;
		nextAudioSource.pitch = R(0.9f, 1.1f);
		nextAudioSource.volume = volume * _masterVolume;
		nextAudioSource.Play();
	}

	protected AudioSource FadeClip(AudioClip clip, float from, float to, float t)
	{
		AudioSource nextAudioSource = GetNextAudioSource();
		nextAudioSource.clip = clip;
		nextAudioSource.loop = false;
		nextAudioSource.pitch = R(0.9f, 1.1f);
		StartCoroutine(FadeVolumeCR(nextAudioSource, from, to, t));
		nextAudioSource.Play();
		return nextAudioSource;
	}

	private IEnumerator FadeVolumeCR(AudioSource audio, float from, float to, float t)
	{
		RealtimeDuration delay = new RealtimeDuration(t);
		while (!delay.IsDone())
		{
			audio.volume = Mathfx.Lerp(from * _masterVolume, to * _masterVolume, delay.Value01());
			yield return null;
		}
		audio.volume = to * _masterVolume;
	}

	protected float GetPitch(int n)
	{
		return Mathf.Pow(2f, (float)n * 0.0625f);
	}

	protected void PlayClipAtPitch(AudioClip[] clips, float volume, float pitch)
	{
		if (clips != null && clips.Length > 0)
		{
			PlayClipAtPitch(clips[R(0, clips.Length)], volume, pitch);
		}
	}

	protected void PlayClipAtPitch(AudioClip clip, float volume, float pitch)
	{
		AudioSource nextAudioSource = GetNextAudioSource();
		nextAudioSource.clip = clip;
		nextAudioSource.loop = false;
		nextAudioSource.pitch = pitch;
		nextAudioSource.volume = volume * _masterVolume;
		nextAudioSource.Play();
	}

	protected void OnAwake()
	{
		_pool = new AudioSource[16];
		for (int i = 0; i < 16; i++)
		{
			GameObject gameObject = new GameObject("Audio Source Pool Item " + i.ToString());
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = Vector3.zero;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			_pool[i] = audioSource;
		}
		RefreshVolume();
		base.OnAwake();
	}

    private AudioSource GetNextAudioSource()
    {
        if (_pool == null)
        {
            Debug.LogError("AudioSource pool is null! Did you forget to initialize RoofdogSoundManager?");
            return null;
        }

        int num = 4;
        AudioSource audioSource = null;

        do
        {
            num--;
            int poolIndex = _poolIndex;
            _poolIndex = (_poolIndex + 1) % _pool.Length;

            audioSource = _pool[poolIndex];
            if (audioSource == null)
            {
                Debug.LogError($"AudioSource at pool index {poolIndex} is null!");
                break;
            }
        }
        while (audioSource.isPlaying && num > 0);

        return audioSource;
    }


    public void RefreshVolume()
	{
		_masterVolume = AutoSingleton<PersistenceManager>.Instance.SoundsVolume;
	}

	protected float R(float a, float b)
	{
		return UnityEngine.Random.Range(a, b);
	}

	protected int R(int a, int b)
	{
		return UnityEngine.Random.Range(a, b);
	}
}
