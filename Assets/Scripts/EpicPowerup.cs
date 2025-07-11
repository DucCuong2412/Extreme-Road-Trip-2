using System;
using System.Collections.Generic;

public abstract class EpicPowerup
{
	private static Dictionary<string, EpicPowerupType> _reverse;

	public Price Price
	{
		get;
		private set;
	}

	public EpicPowerup(Price price)
	{
		Price = price;
	}

	static EpicPowerup()
	{
		_reverse = new Dictionary<string, EpicPowerupType>();
		foreach (int value in Enum.GetValues(typeof(EpicPowerupType)))
		{
			_reverse[((EpicPowerupType)value).ToString()] = (EpicPowerupType)value;
		}
	}

	public abstract EpicPowerupType GetEpicPowerupType();

	public abstract CarController Setup(Car car);

	public abstract string GetIconPath();

	public abstract string GetDescription(int streak);

	public static EpicPowerupType GetTypeFromString(string s)
	{
		if (_reverse.ContainsKey(s))
		{
			return _reverse[s];
		}
		return EpicPowerupType.none;
	}

	public virtual bool UseReplay()
	{
		return true;
	}

	public virtual bool TrackBests()
	{
		return true;
	}

	public virtual float GetGameSetupDistance()
	{
		return 80f;
	}

	public virtual bool CanRecordReplay()
	{
		return true;
	}

	public virtual bool CanPlayReplay()
	{
		return true;
	}
}
