namespace Missions
{
	public class MegaBoostMission : Mission
	{
		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMegaBoostActivated += OnMegaBoostActivated;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMegaBoostActivated -= OnMegaBoostActivated;
		}

		public void OnMegaBoostActivated(int megaBoostCount)
		{
			if (!base.Completed && (float)megaBoostCount >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
