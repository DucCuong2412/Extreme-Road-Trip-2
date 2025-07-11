using UnityEngine;

public class DebugReload : MonoBehaviour
{
	public void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 100f, 50f), "Reload"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
	}
}
