using System;
using System.Collections.Generic;

public class Pool<T>
{
	private List<T> _pool;

	private int _index;

	public Pool(int size, Func<T> create)
	{
		_pool = new List<T>();
		for (int i = 0; i < size; i++)
		{
			_pool.Add(create());
		}
		_index = 0;
	}

	public T Get()
	{
		int count = _pool.Count;
		if (_index >= count)
		{
			_index = 0;
		}
		T result = _pool[_index];
		_index++;
		return result;
	}
}
