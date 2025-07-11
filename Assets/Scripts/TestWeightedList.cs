using UnityEngine;

public class TestWeightedList : MonoBehaviour
{
	private WeightedList<int> _wl;

	public void Start()
	{
		_wl = new WeightedList<int>();
		_wl.Add(0, 18);
		_wl.Add(1, 1);
		_wl.Add(2, 1);
		int num = 100000;
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			num2 += (float)_wl.Pick();
		}
		num2 /= (float)num;
		UnityEngine.Debug.Log("f: " + num2);
	}
}
