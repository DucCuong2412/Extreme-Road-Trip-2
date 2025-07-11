using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
	public float _ratio = 0.01f;

	public void Update()
	{
		Transform transform = base.transform;
		float ratio = _ratio;
		Vector3 deltaCameraPosition = AutoSingleton<WorldManager>.Instance.GetDeltaCameraPosition();
		transform.Translate(new Vector3(ratio * (0f - deltaCameraPosition.x), 0f, 0f));
	}
}
