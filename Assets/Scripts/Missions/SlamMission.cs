namespace Missions
{
	public class SlamMission : Mission
	{
		private int _slamCount;

		public override void RegisterEvents(Car car)
		{
			_slamCount = 0;
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnSlamRecorded += OnSlamRecorded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnSlamRecorded -= OnSlamRecorded;
		}

		public void OnSlamRecorded(int slamCount)
		{
			_slamCount += slamCount;
			if (!base.Completed && (float)_slamCount >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
