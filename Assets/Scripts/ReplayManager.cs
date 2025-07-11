using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : AutoSingleton<ReplayManager>
{
	private const int SESSION_COUNT_BEFORE_LOCAL_REPLAY = 5;

	public Action<bool> OnReplaysVisibilityChange;

	private List<Replay> _incomingReplays;

	private List<Replay> _networkReplays;

	private Replay _localReplay;

	private int _savedReplaySeed;

	private int _savedReplayBest;

	private PersistentInt _gameSessionCount;

	private PersistentBool _replaysActive;

	public bool IsActive()
	{
		return _replaysActive.Get();
	}

	public void SetActive(bool active)
	{
		_replaysActive.Set(active);
		if (OnReplaysVisibilityChange != null)
		{
			OnReplaysVisibilityChange(active);
		}
	}

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		AutoSingleton<LeaderboardsManager>.Instance.OnScoresRetrievedCallback += OnScoresRetrievedCallback;
		_replaysActive = new PersistentBool("_replaysActive", def: true);
		_networkReplays = new List<Replay>();
		base.OnAwake();
	}

	private void OnScoresRetrievedCallback(SocialPlatform platform, LeaderboardType leaderboardType, List<LeaderboardScore> scores)
	{
		if ((platform == SocialPlatform.gameCenter && leaderboardType == LeaderboardType.highestLevel) || (platform == SocialPlatform.facebook && leaderboardType == LeaderboardType.longestRoadTrip && AutoSingleton<PlatformCapabilities>.Instance.UseFacebookAsSocialPlatform()))
		{
			RefreshReplays();
		}
	}

	public void RefreshReplays()
	{
		LoadReplaysFromNetwork();
	}

	public void RecordReplay(CarController car)
	{
		if (IsActive())
		{
			string playerId = AutoSingleton<BackendManager>.Instance.PlayerIdentifier();
			string playerAlias = AutoSingleton<BackendManager>.Instance.PlayerAlias();
			car.gameObject.AddComponent<ReplayRecorder>().Record(playerId, playerAlias);
		}
	}

	private void ValidateReplays()
	{
		_networkReplays.RemoveAll((Replay replay) => replay.Seed() != SeedUtil.GetSeed());
		if (_localReplay != null && _localReplay.Seed() != SeedUtil.GetSeed())
		{
			_localReplay = null;
		}
	}

	public void PlayReplay()
	{
		if (!IsActive())
		{
			return;
		}
		IntegrateNewReplays();
		ValidateReplays();
		if (_networkReplays.Count == 0)
		{
			if (AutoSingleton<PersistenceManager>.Instance.RunCount > 5 && _localReplay != null)
			{
				ReplayPuppet replayPuppet = ReplayPuppet.Create(_localReplay);
				if (replayPuppet != null)
				{
					replayPuppet.Play();
				}
			}
		}
		else
		{
			foreach (Replay networkReplay in _networkReplays)
			{
				if (networkReplay.IsReadyToPlay())
				{
					ReplayPuppet replayPuppet2 = ReplayPuppet.Create(networkReplay);
					if (replayPuppet2 != null)
					{
						replayPuppet2.Play();
					}
				}
			}
		}
	}

	public void SaveReplay(Replay replay)
	{
		if (IsActive())
		{
			int num = replay.Seed();
			int num2 = replay.Distance();
			if (num != _savedReplaySeed || num2 > _savedReplayBest)
			{
				_savedReplaySeed = num;
				_savedReplayBest = num2;
				SaveReplayToNetwork(replay);
				_localReplay = replay;
			}
		}
	}

	private void SaveReplayToNetwork(Replay replay)
	{
		AutoSingleton<BackendManager>.Instance.SaveReplay(replay);
	}

	private void LoadReplaysFromNetwork()
	{
		AutoSingleton<BackendManager>.Instance.LoadReplays(OnReplayLoaded);
	}

	private void AddNewReplay(Replay r)
	{
		AutoSingleton<BackendManager>.Instance.LoadReplayDataBlob(r);
		if (_incomingReplays == null)
		{
			_incomingReplays = new List<Replay>();
		}
		_incomingReplays.RemoveAll((Replay replay) => replay.PlayerId() == r.PlayerId());
		_incomingReplays.Add(r);
	}

	private void IntegrateNewReplays()
	{
		if (_incomingReplays != null)
		{
			using (List<Replay>.Enumerator enumerator = _incomingReplays.GetEnumerator())
			{
				Replay r;
				while (enumerator.MoveNext())
				{
					r = enumerator.Current;
					_networkReplays.RemoveAll((Replay replay) => replay.PlayerId() == r.PlayerId());
					_networkReplays.Add(r);
				}
			}
		}
		_incomingReplays = null;
	}

	private void OnReplayLoaded(Replay replay)
	{
		foreach (Replay networkReplay in _networkReplays)
		{
			if (networkReplay.BlobKey() == replay.BlobKey())
			{
				return;
			}
		}
		AddNewReplay(replay);
	}
}
