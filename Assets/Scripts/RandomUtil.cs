using System;

public class RandomUtil
{
	private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private static readonly Random _rng = new Random((int)DateTime.Now.Ticks);

	public static string RandomString(int size)
	{
		char[] array = new char[size];
		for (int i = 0; i < size; i++)
		{
			array[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[_rng.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)];
		}
		return new string(array);
	}
}
