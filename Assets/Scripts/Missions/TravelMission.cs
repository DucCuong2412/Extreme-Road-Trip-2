using System.Collections;
using UnityEngine;

namespace Missions
{
	public class TravelMission : Mission
	{
		private float _remainingDistance;

		private float _startOffsetDistance;

		private float _overallDistance;

		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCurrentRunDistanceRecorded += OnCurrentRunDistanceRecorded;
			_overallDistance = AutoSingleton<GameStatsManager>.Instance.Overall.GetValue(car, GameStats.CarStats.Type.totalDistance);
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnCurrentRunDistanceRecorded -= OnCurrentRunDistanceRecorded;
		}

		public void OnCurrentRunDistanceRecorded(float currentRunDistance)
		{
			_remainingDistance = base.Objective - (_overallDistance - _startOffsetDistance + currentRunDistance);
			if (_remainingDistance > base.Objective)
			{
				_remainingDistance = 0f;
			}
			if (!base.Completed && _remainingDistance <= 0f)
			{
				OnMissionCompleted();
			}
		}

		public override void OnSetCurrent(Car currentCar)
		{
			_remainingDistance = base.Objective;
			_startOffsetDistance = AutoSingleton<GameStatsManager>.Instance.Overall.GetValue(currentCar, GameStats.CarStats.Type.totalDistance);
		}

		public override Hashtable GetCustomData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Remaining"] = Mathf.RoundToInt(_remainingDistance);
			hashtable["StartOffset"] = Mathf.RoundToInt(_startOffsetDistance);
			return hashtable;
		}

		public override void SetCustomData(Hashtable customData)
		{
			_remainingDistance = JsonUtil.ExtractFloat(customData, "Remaining", 0f);
			_startOffsetDistance = JsonUtil.ExtractFloat(customData, "StartOffset", 0f);
		}

		public override string GetRemainingValueBeforeCompletion(Car currentCar)
		{
			if (base.Completed)
			{
				return string.Empty;
			}
			return " (" + Mathf.RoundToInt(_remainingDistance).ToString() + "m remaining".Localize() + ")";
		}
	}
}
