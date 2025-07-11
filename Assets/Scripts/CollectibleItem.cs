using System.Collections;
using UnityEngine;

public class CollectibleItem : CollidableItem
{
	public CollectibleType _collectableType;

	public override bool IsCollectible => true;

	protected override IEnumerator CollideImpCR(CarController car)
	{
		if (_collectableType == CollectibleType.pinataCoin || _collectableType == CollectibleType.pinataGas || _collectableType == CollectibleType.pinataBuck)
		{
			yield return new WaitForSeconds(Random.Range(0.4f, 0.6f));
		}
		Duration delay = new Duration(0.5f);
		Transform trail = PrefabSingleton<GameSpecialFXManager>.Instance.AddCollectibleTrail(_transform.GetChild(0), _collectableType);
		Vector3 from = base.Position;
		Quaternion rotFrom = _transform.rotation;
		while (!delay.IsDone() && _collided)
		{
			Vector3 to = car.Position + Vector3.forward;
			if ((base.Position - to).sqrMagnitude < 2.25f)
			{
				break;
			}
			_transform.position = Vector3.Lerp(from, to, delay.Value01());
			_transform.rotation = Quaternion.Slerp(rotFrom, Quaternion.LookRotation(Vector3.forward, to - from), delay.Value01());
			yield return null;
		}
		PrefabSingleton<GameSoundManager>.Instance.PlayCollectibleSound(_collectableType);
		OnCollect(car);
		if (trail != null)
		{
			trail.parent = null;
			UnityEngine.Object.Destroy(trail.gameObject);
		}
		_transform.position = GameSettings.OutOfWorldVector;
	}

	private void OnCollect(CarController car)
	{
		float collectibleValue = GameSettings.GetCollectibleValue(_collectableType);
		switch (_collectableType)
		{
		case CollectibleType.pinataBuck:
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordBucksMadeByCar(car.Car, (int)collectibleValue);
			break;
		case CollectibleType.coin:
		case CollectibleType.pinataCoin:
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordCoinsMadeByCar(car.Car, (int)collectibleValue);
			break;
		case CollectibleType.gas:
		case CollectibleType.pinataGas:
			car.CarGas.AddGas(collectibleValue);
			break;
		}
	}
}
