using System;

public class SeedManager
{
	private static int GetSeed()
	{
		DateTime now = DateTime.Now;
		return now.Year * 1000 + now.DayOfYear;
	}

	public static void ResetSeed()
	{
		int seed = GetSeed();
		Simplex.SetSeed(seed / 997, seed % 997);
	}
}
