using UnityEngine;

public class FadeWithDistance : MonoBehaviour
{
	private tk2dSprite _sprite;

	private Color _color;

	public float _distanceStartFade = 300f;

	public float _distanceEndFade = 400f;

	public float _startingFade = 0.25f;

	public void Start()
	{
		_sprite = GetComponent<tk2dSprite>();
		_color = _sprite.color;
		if (!AutoSingleton<PlatformCapabilities>.Instance.IsGameTouchControlSupported())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void FixedUpdate()
	{
		int maxDistance = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetMaxDistance(Singleton<GameManager>.Instance.CarRef);
		float a = _startingFade * Mathf.InverseLerp(_distanceEndFade, _distanceStartFade, maxDistance);
		Color color = _color;
		color.a = a;
		_sprite.color = color;
		if ((float)maxDistance > _distanceEndFade)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
