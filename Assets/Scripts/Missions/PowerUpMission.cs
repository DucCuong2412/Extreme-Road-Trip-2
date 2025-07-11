using System.Collections;

namespace Missions
{
	public class PowerUpMission : Mission
	{
		public PowerupType PowerUpToActivate
		{
			get;
			private set;
		}

		public override void Load(Hashtable jsonTable)
		{
			base.Load(jsonTable);
			PowerUpToActivate = Powerup.GetTypeFromString(JsonUtil.ExtractString(jsonTable, "powerup", string.Empty));
		}

		public override void RegisterEvents(Car car)
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnPowerUpActivated += OnPowerUpActivated;
		}

		public override void UnRegisterEvents()
		{
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.OnPowerUpActivated -= OnPowerUpActivated;
		}

		public void OnPowerUpActivated(PowerupType powerUp)
		{
			if (!base.Completed && PowerUpToActivate == powerUp)
			{
				OnMissionCompleted();
			}
		}
	}
}
