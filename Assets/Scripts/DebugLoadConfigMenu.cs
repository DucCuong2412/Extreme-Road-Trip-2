using UnityEngine;

public class DebugLoadConfigMenu : MonoBehaviour
{
	public LoadConfigMenu.NextMenuPage _page;

	private void Start()
	{
		if (AutoSingleton<LoadingManager>.Instance.IsBooting())
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(_page));
		}
	}
}
