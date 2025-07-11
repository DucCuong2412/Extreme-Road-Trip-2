using System.Collections.Generic;
using UnityEngine;

public class WeightedList<T>
{
	private class ListElement<TT>
	{
		public int weight;

		public TT obj;

		public ListElement(TT obj, int weight)
		{
			this.obj = obj;
			this.weight = weight;
		}
	}

	private int _sum;

	private List<ListElement<T>> _list;

	public WeightedList()
	{
		_sum = 0;
		_list = new List<ListElement<T>>();
	}

	public void Add(T obj, int weight)
	{
		_list.Add(new ListElement<T>(obj, weight));
		_sum += weight;
	}

	public T Pick()
	{
		int num = Random.Range(0, _sum);
		int num2 = 0;
		foreach (ListElement<T> item in _list)
		{
			num2 += item.weight;
			if (num < num2)
			{
				return item.obj;
			}
		}
		return default(T);
	}
}
