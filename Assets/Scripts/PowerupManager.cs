using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : AutoSingleton<PowerupManager>
{
	private Dictionary<PowerupType, Powerup> _allPowerups;

	public Powerup GetPowerup(PowerupType p)
	{
		return _allPowerups[p];
	}

	public void DisablePowerUp()
	{
		foreach (Powerup value in _allPowerups.Values)
		{
			if (value.Enabled)
			{
				value.Disable();
			}
		}
	}

	public List<Powerup> GetEnabledPowerups()
	{
		List<Powerup> list = new List<Powerup>();
		foreach (Powerup value in _allPowerups.Values)
		{
			if (value.Enabled)
			{
				list.Add(value);
			}
		}
		return list;
	}

	protected override void OnAwake()
	{
		_allPowerups = new Dictionary<PowerupType, Powerup>();
		TextAsset textAsset = Resources.Load("powerups.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			Hashtable data = textAsset.text.hashtableFromJson();
			foreach (int value in Enum.GetValues(typeof(PowerupType)))
			{
				Hashtable data2 = JsonUtil.ExtractHashtable(data, ((PowerupType)value).ToString());
				Price price = new Price(JsonUtil.ExtractHashtable(data2, "price"));
				switch (value)
				{
				case 0:
					_allPowerups[(PowerupType)value] = new BoostPowerup(price);
					break;
				case 1:
					_allPowerups[(PowerupType)value] = new CoinsPowerup(price);
					break;
				case 2:
					_allPowerups[(PowerupType)value] = new MagnetPowerup(price);
					break;
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file powerups.json");
		}
		base.OnAwake();
	}
}
