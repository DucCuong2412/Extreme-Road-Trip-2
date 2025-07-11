using System;
using UnityEngine;

[Serializable]
public class PickupElement
{
	public int _frequency;

	public Transform _prefab;

	public static WeightedList<Transform> ArrayToWeightedList(PickupElement[] array)
	{
		WeightedList<Transform> weightedList = new WeightedList<Transform>();
		foreach (PickupElement pickupElement in array)
		{
			weightedList.Add(pickupElement._prefab, pickupElement._frequency);
		}
		return weightedList;
	}
}
