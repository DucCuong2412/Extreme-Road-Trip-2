namespace Missions
{
	public class UpsideDownDistanceMission : Mission
	{
		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnUpsideDownDistanceRecorded += OnUpsideDownDistanceRecorded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnUpsideDownDistanceRecorded -= OnUpsideDownDistanceRecorded;
		}

		private void OnUpsideDownDistanceRecorded(float distance)
		{
			if (!base.Completed && distance >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
