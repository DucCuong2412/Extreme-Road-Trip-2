using UnityEngine;

public class TimeUtil : MonoBehaviour
{
	public static string Format(float secondsTotal)
	{
		float num = Mathf.Floor(secondsTotal / 3600f);
		float num2 = secondsTotal % 3600f;
		float num3 = Mathf.Floor(num2 / 60f);
		float num4 = num2 % 60f;
		if (secondsTotal < 60f)
		{
			return $"{num4:0.00}";
		}
		if (secondsTotal < 3600f)
		{
			return $"{num3:0}:{num4:00.00}";
		}
		return $"{num:0}:{num3:00}:{Mathf.Floor(num4):00}";
	}
}
