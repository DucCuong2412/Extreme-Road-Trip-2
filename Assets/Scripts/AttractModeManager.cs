using System.Collections;
using UnityEngine;

public class AttractModeManager : Singleton<AttractModeManager>
{
	private CarController _car;

	protected override void OnAwake()
	{
		base.OnAwake();
		StartCoroutine(PlayAttractMode());
	}

	private IEnumerator PlayAttractMode()
	{
		_car = SpawnCar();
		AutoSingleton<WorldManager>.Instance.Create(GameMode.normal);
		PrefabSingleton<CameraGame>.Instance.SetTarget(_car.transform);
		_car.OnGameSetupStarted();
		while (!_car.IsTouchingGround() && !_car.IsCrashed())
		{
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(0.5f);
		while (!_car.IsCrashed())
		{
			Vector3 position = _car.Position;
			if (!(position.x <= 80f))
			{
				break;
			}
			yield return null;
		}
		_car.OnGameSetupEnded();
		while (!_car.IsCrashed())
		{
			yield return null;
		}
		yield return new WaitForSeconds(2f);
	}

	private CarController SpawnCar()
	{
		Car randomLockedCar = AutoSingleton<CarManager>.Instance.GetRandomLockedCar();
		GameObject gamePrefab = randomLockedCar.GamePrefab;
		GameObject gameObject = (GameObject)Object.Instantiate(gamePrefab, Vector3.zero, Quaternion.identity);
		CarController component = gameObject.GetComponent<CarController>();
		gameObject.AddComponent<AIInputController>();
		component.Setup(randomLockedCar);
		return component;
	}
}
