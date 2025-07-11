using System.Collections;
using UnityEngine;

public class ScreenshotAllTheCars : MonoBehaviour
{
	private void Start()
	{
		AutoSingleton<CarManager>.Instance.Create();
		StartCoroutine(ScreenshotAllTheCarsCR());
	}

	private IEnumerator ScreenshotAllTheCarsCR()
	{
		yield return null;
		foreach (Car car in AutoSingleton<CarDatabase>.Instance.GetAllCars())
		{
			GameObject visualPrefab = car.VisualPrefab;
			GameObject go = (GameObject)UnityEngine.Object.Instantiate(visualPrefab, Vector3.zero, Quaternion.identity);
			UnityEngine.ScreenCapture.CaptureScreenshot("/Users/Guillaume/tmp/Cars/" + car.Id + "Sprite.png");
			yield return new WaitForSeconds(0.1f);
			UnityEngine.Object.Destroy(go);
		}
	}
}
