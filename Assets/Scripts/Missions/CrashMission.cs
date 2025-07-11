using System.Collections;

namespace Missions
{
	public class CrashMission : Mission
	{
		private float _minValue;

		private float _maxValue;

		public override void Load(Hashtable jsonTable)
		{
			base.Load(jsonTable);
			_minValue = JsonUtil.ExtractFloat(jsonTable, "minValue", 0f);
			_maxValue = JsonUtil.ExtractFloat(jsonTable, "maxValue", 0f);
		}

		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.Overall.OnGameEnded += OnGameEnded;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.Overall.OnGameEnded -= OnGameEnded;
		}

		public void OnGameEnded(float distance)
		{
			if (!base.Completed && distance >= _minValue && distance <= _maxValue)
			{
				OnMissionCompleted();
			}
		}
	}
}
