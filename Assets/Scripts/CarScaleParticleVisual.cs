using UnityEngine;

public class CarScaleParticleVisual : MonoBehaviour
{
	private const float _defaultScale = 2f;

	private void Start()
	{
		ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>();
		if (componentsInChildren != null)
		{
			Vector3 localScale = base.transform.Find("Pivot").localScale;
			float num = localScale.x * 2f;
			ParticleSystem[] array = componentsInChildren;
			foreach (ParticleSystem particleSystem in array)
			{
				particleSystem.startSize *= num;
			}
		}
	}
}
