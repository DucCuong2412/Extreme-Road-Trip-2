using System.Collections;
using UnityEngine;

public class MineItem : CollidableItem
{
	private RandomRange _randomDelay = new RandomRange(0f, 0.07f);

	private float _force = 300000f;

	private float _forceRadius = 10f;

	private float _forceUpwardsModifier = 2f;

	protected override IEnumerator CollideImpCR(CarController car)
	{
		AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordMineExplosion(1);
		PrefabSingleton<GameSoundManager>.Instance.PlayMineSound();
		Vector3 carPos = car.Position;
		yield return new WaitForSeconds(_randomDelay.Pick());
		car.GetComponent<Rigidbody>().AddExplosionForce(_force * ((float)Device.GetFixedUpdateRate() / 60f), carPos, _forceRadius, _forceUpwardsModifier);
		AutoSingleton<ExplosionManager>.Instance.Explode(base.Position);
		yield return new WaitForFixedUpdate();
		_transform.position = GameSettings.OutOfWorldVector;
	}
}
