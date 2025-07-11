namespace Missions
{
	public class UpsideDownSlamMission : Mission
	{
		private int _slamCount;

		public override void RegisterEvents(Car car)
		{
			_slamCount = 0;
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnUpsideDownSlam += OnUpsideDownSlam;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnUpsideDownSlam -= OnUpsideDownSlam;
		}

		private void OnUpsideDownSlam()
		{
			_slamCount++;
			if (!base.Completed && (float)_slamCount >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
