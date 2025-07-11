using UnityEngine;

public class TestCarInputController : CarInputController
{
	private void Update()
	{
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		_tilt = Mathf.Sign(0f - Mathf.DeltaAngle(0f, eulerAngles.z));
		Car car = AutoSingleton<CarDatabase>.Instance.GetAllCars()[0];
		int maxDistance = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetMaxDistance(car);
		if (maxDistance < 5)
		{
			_tilt = 1f;
		}
	}
}
