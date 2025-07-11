using System.Collections;
using UnityEngine;

public class ReplayRecorder : MonoBehaviour
{
	private CarController _car;

	private Replay _replay;

	private void Awake()
	{
		_car = GetComponent<CarController>();
	}

	public void Record(string playerId, string playerAlias)
	{
		_replay = new Replay(_car.Car, playerId, playerAlias);
		StartCoroutine(RecordReplayCR());
	}

	private IEnumerator RecordReplayCR()
	{
		int rate = _replay.Rate();
		int fixedUpdateRate = Device.GetFixedUpdateRate();
		int skip = fixedUpdateRate / rate - 1;
		while (!Singleton<GameManager>.Instance.IsGameOver())
		{
			_replay.AddSample(_car);
			yield return new WaitForFixedUpdate();
			for (int i = 0; i < skip; i++)
			{
				yield return new WaitForFixedUpdate();
			}
		}
		int distance = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetValue(_car.Car, GameStats.CarStats.Type.maxDistance);
		int coins = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetValue(_car.Car, GameStats.CarStats.Type.coinsPickedUp);
		int stunts = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetValue(_car.Car, GameStats.CarStats.Type.numberOfStunts);
		int duration = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetValue(_car.Car, GameStats.CarStats.Type.longestRaceInTime);
		_replay.Complete(distance, coins, stunts, duration);
		AutoSingleton<ReplayManager>.Instance.SaveReplay(_replay);
	}
}
