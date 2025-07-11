namespace Missions
{
	public class TwoKMission : Mission
	{
		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.On2kTimeRecorded += On2kTimeRecorded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.On2kTimeRecorded -= On2kTimeRecorded;
		}

		public void On2kTimeRecorded(float time)
		{
			if (!base.Completed && time < base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
