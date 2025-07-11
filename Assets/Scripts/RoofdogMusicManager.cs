using System.Collections;
using UnityEngine;

public class RoofdogMusicManager<T> : PrefabSingleton<T> where T : MonoBehaviour
{
	private const float _musicLevel = 0.6f;

	private float _volume;

	protected void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		RefreshVolume();
		GetComponent<AudioSource>().volume = _volume;
		base.OnAwake();
	}

	protected void SemiFade()
	{
		StartCoroutine(FadeTo(_volume * 0.65f, 1f));
	}

	public void StopMusic()
	{
		StartCoroutine(FadeOut(0.5f));
	}

	protected IEnumerator FadeToMusic(AudioClip clip, bool loop = true, float duration = 1f)
	{
		if (clip != GetComponent<AudioSource>().clip)
		{
			if (GetComponent<AudioSource>().clip != null)
			{
				yield return StartCoroutine(FadeOut(duration));
			}
			GetComponent<AudioSource>().clip = clip;
			GetComponent<AudioSource>().loop = loop;
			GetComponent<AudioSource>().Play();
		}
		StartCoroutine(FadeTo(_volume, duration));
	}

	protected IEnumerator FadeTo(float volume, float duration)
	{
		RealtimeDuration delay = new RealtimeDuration(duration);
		float starting = GetComponent<AudioSource>().volume;
		while (!delay.IsDone())
		{
			GetComponent<AudioSource>().volume = Mathf.Lerp(starting, volume, delay.Value01());
			yield return null;
		}
		GetComponent<AudioSource>().volume = volume;
	}

	protected IEnumerator FadeOut(float duration)
	{
		RealtimeDuration delay = new RealtimeDuration(duration);
		while (!delay.IsDone())
		{
			GetComponent<AudioSource>().volume = Mathf.Lerp(_volume, 0f, delay.Value01());
			yield return null;
		}
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().clip = null;
	}

	public void RefreshVolume()
	{
		_volume = 0.6f * (float)AutoSingleton<PersistenceManager>.Instance.MusicVolume;
		StartCoroutine(FadeTo(_volume, 0.5f));
	}
}
