using System;
using System.Collections.Generic;

public abstract class Powerup
{
	private static Dictionary<string, PowerupType> _reverse;

	public Price Price
	{
		get;
		private set;
	}

	public bool Enabled
	{
		get;
		private set;
	}

	public Powerup(Price price)
	{
		Price = price;
	}

	static Powerup()
	{
		_reverse = new Dictionary<string, PowerupType>();
		foreach (int value in Enum.GetValues(typeof(PowerupType)))
		{
			_reverse[((PowerupType)value).ToString()] = (PowerupType)value;
		}
	}

	public virtual void Enable()
	{
		Enabled = true;
	}

	public virtual void Disable()
	{
		Enabled = false;
	}

	public abstract PowerupType GetPowerupType();

	protected abstract void Activate();

	protected abstract void Deactivate();

	public static PowerupType GetTypeFromString(string s)
	{
		if (_reverse.ContainsKey(s))
		{
			return _reverse[s];
		}
		return PowerupType.boost;
	}
}
