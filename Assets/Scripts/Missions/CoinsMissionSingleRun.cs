namespace Missions
{
	public class CoinsMissionSingleRun : Mission
	{
		private int _coinsPickedUp;

		public override void RegisterEvents(Car car)
		{
			_coinsPickedUp = 0;
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCoinsPickedup += OnCoinsPickedup;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCoinsPickedup -= OnCoinsPickedup;
		}

		public void OnCoinsPickedup(int coinsPickedup)
		{
			_coinsPickedUp += coinsPickedup;
			if (!base.Completed && (float)_coinsPickedUp >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
