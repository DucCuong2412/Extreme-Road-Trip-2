using System;
using System.Collections.Generic;

public static class ListExtension
{
	private static Random _random = new Random();

	public static void Shuffle<T>(this List<T> list)
	{
		Random random = _random;
		for (int num = list.Count; num > 1; num--)
		{
			int index = random.Next(num);
			T value = list[index];
			list[index] = list[num - 1];
			list[num - 1] = value;
		}
	}
}
