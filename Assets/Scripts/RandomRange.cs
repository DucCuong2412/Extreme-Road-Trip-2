using System;
using UnityEngine;

[Serializable]
public class RandomRange
{
	public float _min;

	public float _max;

	public RandomRange(float min, float max)
	{
		_min = min;
		_max = max;
	}

	public float Pick()
	{
		return UnityEngine.Random.Range(_min, _max);
	}
}
