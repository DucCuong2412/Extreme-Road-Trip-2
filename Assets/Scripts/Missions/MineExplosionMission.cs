namespace Missions
{
	public class MineExplosionMission : Mission
	{
		private int _mineExplodedCount;

		public override void RegisterEvents(Car car)
		{
			_mineExplodedCount = 0;
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMineExplosion += OnMineExplosion;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnMineExplosion -= OnMineExplosion;
		}

		public void OnMineExplosion(int explosionCount)
		{
			_mineExplodedCount += explosionCount;
			if (!base.Completed && (float)_mineExplodedCount >= base.Objective)
			{
				OnMissionCompleted();
			}
		}
	}
}
