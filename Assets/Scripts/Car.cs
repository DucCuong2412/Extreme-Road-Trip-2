using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : IComparable
{
	private GameObject _gamePrefab;

	private GameObject _visualPrefab;

	private string _displayName;

	private string _description;

	public string Id
	{
		get;
		private set;
	}

	public GameObject GamePrefab
	{
		get
		{
			if (_gamePrefab == null)
			{
				_gamePrefab = (Resources.Load(Id + "/" + Id + "CarGamePrefab") as GameObject);
			}
			return _gamePrefab;
		}
	}

	public GameObject VisualPrefab
	{
		get
		{
			if (_visualPrefab == null)
			{
				_visualPrefab = (Resources.Load(Id + "/" + Id + "CarVisualPrefab") as GameObject);
			}
			return _visualPrefab;
		}
	}

	public string DisplayName
	{
		get
		{
			return _displayName.Localize();
		}
		set
		{
			_displayName = value;
		}
	}

	public string Description
	{
		get
		{
			return _description.Localize();
		}
		set
		{
			_description = value;
		}
	}

	public int Rank
	{
		get;
		set;
	}

	public List<Price> Prices
	{
		get;
		set;
	}

	public Price Price => Prices[0];

	public CarCategory Category
	{
		get;
		set;
	}

	public int GasLevel
	{
		get;
		set;
	}

	public int SpeedLevel
	{
		get;
		set;
	}

	public int FlipLevel
	{
		get;
		set;
	}

	public int BoostLevel
	{
		get;
		set;
	}

	public int SlamLevel
	{
		get;
		set;
	}

	public float MaxSpeed
	{
		get;
		set;
	}

	public float Mass
	{
		get;
		set;
	}

	public float SuspensionDistance
	{
		get;
		set;
	}

	public float SuspensionSpring
	{
		get;
		set;
	}

	public float SuspensionDamper
	{
		get;
		set;
	}

	public Car(string id)
	{
		Id = id;
		Prices = new List<Price>();
	}

	public bool IsFree()
	{
		bool flag = true;
		foreach (Price price in Prices)
		{
			flag &= price.IsFree();
		}
		return flag;
	}

	public int CompareTo(object o)
	{
		if (o == null)
		{
			return -1;
		}
		Car car = o as Car;
		return Rank - car.Rank;
	}
}
