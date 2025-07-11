using System.Collections;

namespace Missions
{
	public class CoinsMission : Mission
	{
		private float _remainingCoinsToPick;

		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCoinsPickedup += OnCoinsPickedup;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCoinsPickedup -= OnCoinsPickedup;
		}

		public void OnCoinsPickedup(int coinsPickedup)
		{
			_remainingCoinsToPick -= coinsPickedup;
			if (!base.Completed && _remainingCoinsToPick <= 0f)
			{
				OnMissionCompleted();
			}
		}

		public override void OnSetCurrent(Car currentCar)
		{
			_remainingCoinsToPick = base.Objective;
		}

		public override Hashtable GetCustomData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Remaining"] = _remainingCoinsToPick;
			return hashtable;
		}

		public override void SetCustomData(Hashtable customData)
		{
			_remainingCoinsToPick = JsonUtil.ExtractFloat(customData, "Remaining", 0f);
		}

		public override string GetRemainingValueBeforeCompletion(Car currentCar)
		{
			if (base.Completed)
			{
				return string.Empty;
			}
			return " (" + _remainingCoinsToPick + "@ remaining".Localize() + ")";
		}
	}
}
