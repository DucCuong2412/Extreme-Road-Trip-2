using System.Collections;
using UnityEngine;

public class GameSoundManager : RoofdogSoundManager<GameSoundManager>
{
	public enum LoopingSound
	{
		Beep,
		Coin,
		Flash
	}

	public AudioClip _boost;

	public AudioClip _megaBoost;

	private AudioSource _megaBoostAudioSource;

	public AudioClip _slamming;

	private AudioSource _slammingSource;

	public AudioClip _tireContact;

	public AudioClip _button;

	public AudioClip _cashRegister;

	public AudioClip[] _pickupCoin;

	public AudioClip[] _pickupGas;

	public AudioClip[] _pickupBuck;

	public AudioClip _coinDoublerReward;

	public AudioClip _magnetReward;

	public AudioClip _megaBoostReward;

	public AudioClip _wood;

	public AudioClip _metal;

	public AudioClip _breakRock;

	public AudioClip _cone;

	public AudioClip _balloon;

	public AudioClip _weed;

	public AudioClip _witch;

	public AudioClip _pumpkin;

	public AudioClip _snow;

	public AudioClip _ufo;

	public AudioClip _birds;

	public AudioClip _woodHit;

	public AudioClip _woodCrash;

	public AudioClip _mine;

	public AudioClip _explosion;

	public AudioClip _flash;

	public AudioClip _whish;

	public AudioClip _bweep;

	public AudioClip _suddenDeathAlarm;

	public AudioClip _steamBreak;

	public AudioClip _stomp;

	public AudioClip _crowd;

	public AudioClip _star;

	public AudioClip _xpGoingUp;

	public AudioClip _upgradeDrill;

	public AudioClip _loopingBeep;

	public AudioClip _loopingCoin;

	public AudioClip _loopingFlash;

	private AudioSource _loopingAudioSource;

	public void PlayBoostSound()
	{
		StopSlamming();
		PlayClip(_boost, 0.3f);
	}

	public void StartMegaBoostSound()
	{
		StopSlamming();
		StopMegaBoostSound();
		if (_megaBoostAudioSource == null)
		{
			GameObject gameObject = new GameObject("MegaBoost Audio Source");
			_megaBoostAudioSource = gameObject.AddComponent<AudioSource>();
			_megaBoostAudioSource.volume = 0.7f * _masterVolume;
		}
		_megaBoostAudioSource.clip = _megaBoost;
		_megaBoostAudioSource.Play();
	}

	public void StopMegaBoostSound()
	{
		if (_megaBoostAudioSource != null)
		{
			_megaBoostAudioSource.Stop();
			_megaBoostAudioSource = null;
		}
	}

	public void StopSlamming()
	{
		if (_slammingSource != null && _slammingSource.isPlaying && _slammingSource.clip == _slamming)
		{
			_slammingSource.Stop();
		}
		_slammingSource = null;
	}

	public void PlaySlamming()
	{
		if (_slammingSource != null)
		{
			_slammingSource.Stop();
			_slammingSource = null;
		}
		_slammingSource = FadeClip(_slamming, 0f, 0.3f, 2f);
	}

	public void PlayTireContact()
	{
		PlayClipAtPitch(_tireContact, 0.2f, R(0.9f, 1.2f));
	}

	public void PlayButtonSound()
	{
		PlayClip(_button, 0.3f);
	}

	public void PlayCashRegister()
	{
		PlayClipAtPitch(_cashRegister, 0.7f, 1f);
	}

	public void PlayCollectibleSound(CollectibleType type)
	{
		switch (type)
		{
		case CollectibleType.coin:
		case CollectibleType.pinataCoin:
			PlayClipAtPitch(_pickupCoin, 0.4f, R(1f, 1.2f));
			break;
		case CollectibleType.gas:
		case CollectibleType.pinataGas:
			PlayClipAtPitch(_pickupGas, 1f, R(1f, 1.2f));
			break;
		case CollectibleType.pinataBuck:
			PlayClipAtPitch(_pickupBuck, 1.5f, R(1f, 1.2f));
			break;
		}
	}

	public void PlayRewardSound(RewardType type)
	{
		switch (type)
		{
		case RewardType.xp:
			break;
		case RewardType.coins:
		case RewardType.bucks:
			PlayClipAtPitch(_cashRegister, 0.7f, 1f);
			break;
		case RewardType.boost:
			PlayClip(_megaBoostReward, 0.7f);
			break;
		case RewardType.coinDoubler:
			PlayClip(_coinDoublerReward, 0.7f);
			break;
		case RewardType.magnet:
			PlayClip(_magnetReward, 0.7f);
			break;
		}
	}

	public void PlayTangibleItemSound(TangibleItemSound sound)
	{
		switch (sound)
		{
		case TangibleItemSound.cone:
			PlayClip(_cone, 0.7f);
			break;
		case TangibleItemSound.crate:
		case TangibleItemSound.wood:
			PlayClip(_wood, 0.7f);
			break;
		case TangibleItemSound.rock:
			PlayClip(_breakRock, 0.7f);
			break;
		case TangibleItemSound.metal:
			PlayClip(_metal, 0.8f);
			break;
		case TangibleItemSound.balloon:
			PlayClip(_balloon, 0.8f);
			break;
		case TangibleItemSound.weed:
			PlayClip(_weed, 0.7f);
			break;
		case TangibleItemSound.witch:
			PlayClip(_witch, 1f);
			break;
		case TangibleItemSound.pumpkin:
			PlayClip(_pumpkin, 1f);
			break;
		case TangibleItemSound.snow:
			PlayClip(_snow, 1f);
			break;
		case TangibleItemSound.ufo:
			PlayClip(_ufo, 1f);
			break;
		}
	}

	public void PlayBirdsSound()
	{
		PlayClip(_birds, 0.9f);
	}

	public void PlayWoodHitSound()
	{
		PlayClip(_woodHit, 0.7f);
	}

	public void PlayWoodCrashSound()
	{
		PlayClip(_woodCrash, 0.7f);
	}

	public void PlayMineSound()
	{
		PlayClipAtPitch(_mine, 0.7f, 1f);
	}

	public void PlayExplosionSound()
	{
		PlayClip(_explosion, 0.7f);
	}

	public void PlayFlashSound()
	{
		PlayClip(_flash, 0.7f);
	}

	public void PlayWhishSound()
	{
		PlayClip(_whish, 0.1f);
	}

	public void PlayBweepSound()
	{
		PlayClip(_bweep, 0.5f);
	}

	public void PlaySuddenDeathAlarm()
	{
		StartCoroutine(PlaySuddenDeathAlarmCR());
	}

	private IEnumerator PlaySuddenDeathAlarmCR()
	{
		float t = _suddenDeathAlarm.length;
		for (int i = 0; i < 3; i++)
		{
			PlayClipAtPitch(_suddenDeathAlarm, 0.7f, 1f);
			yield return new WaitForSeconds(t - 0.07f);
		}
	}

	public void PlaySteamBreak()
	{
		PlayClipAtPitch(_steamBreak, 1.2f, 1f);
	}

	public void PlayStompSound()
	{
		PlayClip(_stomp, 0.7f);
	}

	public void PlayCrowdSound()
	{
		PlayClip(_crowd, 0.7f);
	}

	public void PlayStarSound(int rank)
	{
		PlayClipAtPitch(_star, 0.8f, GetPitch(rank - 2));
	}

	public void PlayXPGoingUp(int rank)
	{
		PlayClipAtPitch(_xpGoingUp, 0.8f, GetPitch(rank));
	}

	public void PlayUpgradeSound()
	{
		PlayClip(_upgradeDrill, 0.7f);
	}

	public void StartLoopingSound(LoopingSound sound)
	{
		if (_loopingAudioSource == null)
		{
			GameObject gameObject = new GameObject("Looping Beep Audio Source");
			_loopingAudioSource = gameObject.AddComponent<AudioSource>();
			_loopingAudioSource.volume = 0.7f * _masterVolume;
		}
		AudioClip clip = null;
		switch (sound)
		{
		case LoopingSound.Beep:
			clip = _loopingBeep;
			break;
		case LoopingSound.Coin:
			clip = _loopingCoin;
			break;
		case LoopingSound.Flash:
			clip = _loopingFlash;
			break;
		}
		_loopingAudioSource.clip = clip;
		_loopingAudioSource.loop = true;
		_loopingAudioSource.Play();
	}

	public void StopLoopingSound()
	{
		if (_loopingAudioSource != null)
		{
			_loopingAudioSource.loop = false;
		}
	}
}
