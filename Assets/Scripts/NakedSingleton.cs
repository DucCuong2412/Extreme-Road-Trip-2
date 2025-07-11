using UnityEngine;

public class NakedSingleton<T> where T : MonoBehaviour
{
	public static GameObject Create()
	{
		string text = typeof(T).ToString();
		GameObject gameObject = GameObject.Find("/" + text);
		if (gameObject == null)
		{
			GameObject gameObject2 = new GameObject(text);
			gameObject2.AddComponent<T>();
			gameObject = gameObject2;
		}
		return gameObject;
	}
}
