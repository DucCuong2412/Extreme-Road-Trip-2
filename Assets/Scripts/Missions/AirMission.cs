namespace Missions
{
	public class AirMission : Mission
	{
		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnJumpLengthRecorded += OnJumpLengthRecorded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnJumpLengthRecorded -= OnJumpLengthRecorded;
		}

		public void OnJumpLengthRecorded(float jumpLength)
		{
			if (!base.Completed && jumpLength >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
