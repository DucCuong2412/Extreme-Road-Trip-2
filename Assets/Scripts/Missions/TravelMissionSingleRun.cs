namespace Missions
{
	public class TravelMissionSingleRun : Mission
	{
		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCurrentRunDistanceRecorded += OnCurrentRunDistanceRecorded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCurrentRunDistanceRecorded -= OnCurrentRunDistanceRecorded;
		}

		public void OnCurrentRunDistanceRecorded(float currentRunDistance)
		{
			if (!base.Completed && currentRunDistance >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
