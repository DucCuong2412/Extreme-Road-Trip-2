using UnityEngine;

public class MetroTestMetroPie : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroSpacer metroSpacer = MetroSpacer.Create();
		Add(metroSpacer);
		Material mat = Resources.Load("UpgradePieMat") as Material;
		MetroPie.Create(metroSpacer.transform, mat, 0.75f);
		base.OnStart();
	}
}
