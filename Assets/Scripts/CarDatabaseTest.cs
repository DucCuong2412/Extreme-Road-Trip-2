using UnityEngine;

public class CarDatabaseTest : MonoBehaviour
{
	public void Start()
	{
		UnityEngine.Debug.Log("--- BEGIN CAR DATABASE TEST ---");
		foreach (Car allCar in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			UnityEngine.Debug.Log("Car name: " + allCar.Id);
		}
		UnityEngine.Debug.Log("--- END CAR DATABASE TEST ---");
	}
}
