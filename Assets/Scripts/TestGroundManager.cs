using UnityEngine;

public class TestGroundManager : MonoBehaviour
{
	private void Start()
	{
		AutoSingleton<GroundManager>.Instance.Create();
	}

	private void Update()
	{
	}
}
