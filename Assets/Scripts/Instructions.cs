using System.Collections;
using UnityEngine;

public class Instructions : MonoBehaviour
{
	private const float _amplitude = 0.01f;

	private Vector3 _scale;

	private CarController _car;

	private MetroLabel _label;

	public void Awake()
	{
		_scale = base.transform.localScale;
		_label = MetroLabel.Create("Instructions");
	}

	public void Start()
	{
		_label.SetText("Touch sides of screen\n to tilt the car!");
		_scale *= 0.8f;
		StartCoroutine(WobbleThenDestroy());
	}

	private IEnumerator WobbleThenDestroy()
	{
		while (_car == null)
		{
			_car = Singleton<GameManager>.Instance.Car;
			yield return null;
		}
		while (_car.Input.Tilt == 0f)
		{
			base.transform.localScale = _scale * (Mathf.Cos(Time.time * 10f) * 0.01f + 1f);
			yield return null;
		}
		Duration delay = new Duration(0.2f);
		while (!delay.IsDone())
		{
			base.transform.localScale = _scale * Mathf.Lerp(1f, 0.1f, delay.Value01());
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
