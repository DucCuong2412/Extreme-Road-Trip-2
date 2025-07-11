namespace Missions
{
	public class MostFlipJumpMission : Mission
	{
		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMostFlipRecorded += OnMostFlipRecorded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMostFlipRecorded -= OnMostFlipRecorded;
		}

		public void OnMostFlipRecorded(int flipCount)
		{
			if (!base.Completed && (float)flipCount >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
