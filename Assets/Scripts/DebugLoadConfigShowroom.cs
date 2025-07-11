using UnityEngine;

public class DebugLoadConfigShowroom : MonoBehaviour
{
	private void Start()
	{
		if (AutoSingleton<LoadingManager>.Instance.IsBooting())
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
		}
	}
}
