using System.Collections;
using UnityEngine;

public class BumpableItem : TangibleItem
{
	public override void Reset()
	{
		base.Reset();
		Rigidbody rigidbody = _transform.rigidbody;
		if (rigidbody != null)
		{
			UnityEngine.Object.Destroy(rigidbody);
		}
		_collider.isTrigger = true;
	}

	public override void Activate()
	{
		_go.layer = LayerMask.NameToLayer(GameSettings.BumpableOnLayer);
		base.Activate();
	}

	protected override IEnumerator CollideImpCR(CarController car)
	{
		PrefabSingleton<GameSoundManager>.Instance.PlayTangibleItemSound(_sound);
		Rigidbody r = AddRigidbody(_go, car);
		r.detectCollisions = true;
		_go.layer = LayerMask.NameToLayer(GameSettings.BumpableOffLayer);
		_collider.isTrigger = false;
		yield return new WaitForFixedUpdate();
	}
}
