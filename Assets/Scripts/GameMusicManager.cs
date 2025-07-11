using UnityEngine;

public class GameMusicManager : RoofdogMusicManager<GameMusicManager>
{
	public AudioClip _titleMusic;

	public AudioClip[] _gameMusic;

	public AudioClip _levelUpMusic;

	public AudioClip _missionsFullProgressMusic;

	public void PlayTitleMusic()
	{
		StartCoroutine(FadeToMusic(_titleMusic));
	}

	public void PlayGameMusic()
	{
		StartCoroutine(FadeToMusic(_gameMusic[Random.Range(0, _gameMusic.Length)]));
	}

	public void PlayLevelUpMusic()
	{
		StartCoroutine(FadeToMusic(_levelUpMusic, loop: false, 0.2f));
	}

	public void PlayMissionsFullProgressMusic()
	{
		StartCoroutine(FadeToMusic(_missionsFullProgressMusic, loop: false, 0.2f));
	}
}
