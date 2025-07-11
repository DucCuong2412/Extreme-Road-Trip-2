using UnityEngine;

public class MetroSpinner : MetroWidget
{
	private Transform _pivot;

	public MetroSpinner SetScale(float scale)
	{
		_pivot.localScale = Vector3.one * scale;
		return this;
	}

	public static MetroSpinner Create(string iconName)
	{
		GameObject obj = (GameObject)Object.Instantiate(Resources.Load(iconName), Vector3.zero, Quaternion.identity);
		return Create(obj);
	}

	public static MetroSpinner Create(GameObject obj)
	{
		GameObject gameObject = new GameObject(obj.name);
		obj.transform.parent = gameObject.transform;
		obj.transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroSpinner>().Setup(obj.transform);
	}

	private MetroSpinner Setup(Transform pivot)
	{
		_pivot = pivot;
		return this;
	}

	public void Update()
	{
		_pivot.rotation = Quaternion.Euler(0f, 0f, -360f * Time.time);
	}
}
