using UnityEngine;

public class GroundModeManager : Singleton<GroundModeManager>
{
	public int _seed1;

	public int _seed2;

	protected override void OnAwake()
	{
		base.OnAwake();
		Random.seed = Mathf.RoundToInt(Time.realtimeSinceStartup * 1000f);
		_seed1 = Random.Range(-10000, 10000);
		_seed2 = Random.Range(-10000, 10000);
		Simplex.SetSeed(_seed1, _seed2);
		UnityEngine.Debug.Log($"Seed: ({_seed1},{_seed2})");
		AutoSingleton<WorldManager>.Instance.Create();
	}
}
