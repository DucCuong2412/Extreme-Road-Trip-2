using System.Collections;

namespace Missions
{
	public class ConsecutiveLandingMission : Mission
	{
		private StuntEvent _landingType;

		public override void Load(Hashtable jsonTable)
		{
			base.Load(jsonTable);
			switch (JsonUtil.ExtractString(jsonTable, "landingType", string.Empty))
			{
			case "perfectLanding":
				_landingType = StuntEvent.perfectLanding;
				break;
			case "perfectSlamLanding":
				_landingType = StuntEvent.perfectSlamLanding;
				break;
			}
		}

		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnConsecutiveLanding += OnConsecutiveLanding;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnConsecutiveLanding -= OnConsecutiveLanding;
		}

		public void OnConsecutiveLanding(int landingCount, StuntEvent landingType)
		{
			if (!base.Completed && (float)landingCount >= base.Objective && _landingType == landingType)
			{
				OnMissionCompleted();
			}
		}
	}
}
