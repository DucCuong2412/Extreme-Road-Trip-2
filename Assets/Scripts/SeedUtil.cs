using System;

public class SeedUtil
{
	public static int GetSeed()
	{
		DateTime dateTime = DateTime.UtcNow.Add(new TimeSpan(-8, 0, 0));
		return dateTime.Year * 1000 + dateTime.DayOfYear;
	}

	public static void ResetSeed()
	{
		SetSeed(GetSeed());
	}

	public static void SetSeed(int seed)
	{
		Simplex.SetSeed(seed / 997, seed % 997);
	}
}
