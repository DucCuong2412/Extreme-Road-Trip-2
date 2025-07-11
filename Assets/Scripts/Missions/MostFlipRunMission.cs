namespace Missions
{
	public class MostFlipRunMission : Mission
	{
		private int _flipCount;

		public override void RegisterEvents(Car car)
		{
			_flipCount = 0;
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMostFlipRecorded += OnMostFlipRecorded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMostFlipRecorded -= OnMostFlipRecorded;
		}

		public void OnMostFlipRecorded(int flipCount)
		{
			_flipCount += flipCount;
			if (!base.Completed && (float)_flipCount >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
