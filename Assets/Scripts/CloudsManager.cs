using UnityEngine;

public class CloudsManager : AutoSingleton<CloudsManager>
{
	protected override void OnStart()
	{
		CameraGame instance = PrefabSingleton<CameraGame>.Instance;
		instance.camera.backgroundColor = new Color(0.1f, 0.4f, 0.7f);
		base.OnStart();
	}
}
