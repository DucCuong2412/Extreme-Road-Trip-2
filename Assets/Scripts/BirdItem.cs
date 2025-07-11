using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdItem : CollidableItem
{
	public Transform[] _birds;

	private List<tk2dAnimatedSprite> _animations;

	private Dictionary<Transform, Vector3> _initPos;

	private bool _anim;

	public override void Awake()
	{
		_animations = new List<tk2dAnimatedSprite>();
		_initPos = new Dictionary<Transform, Vector3>();
		Transform[] birds = _birds;
		foreach (Transform transform in birds)
		{
			_animations.Add(transform.GetComponentInChildren<tk2dAnimatedSprite>());
			_initPos.Add(transform, transform.localPosition);
		}
		base.Awake();
	}

	public override void Reset()
	{
		_anim = false;
		foreach (tk2dAnimatedSprite animation in _animations)
		{
			animation.Play("Idle");
			animation.Stop();
		}
		Transform[] birds = _birds;
		foreach (Transform transform in birds)
		{
			transform.localPosition = _initPos[transform];
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
		base.Reset();
	}

	public override void Activate()
	{
		_anim = true;
		base.Activate();
	}

	protected override IEnumerator CollideImpCR(CarController car)
	{
		if (!(car.Velocity.magnitude <= 60f))
		{
			yield break;
		}
		PrefabSingleton<GameSoundManager>.Instance.PlayBirdsSound();
		foreach (tk2dAnimatedSprite anim in _animations)
		{
			anim.Play("Flight");
		}
		Vector2[] offsets = new Vector2[_birds.Length];
		for (int j = 0; j < offsets.Length; j++)
		{
			offsets[j].x = Random.Range(80f, 120f);
			offsets[j].y = Random.Range(30f, 70f);
		}
		Duration delay = new Duration(2f);
		while (_anim && !delay.IsDone())
		{
			float x = Mathfx.Hermite(0f, 1f, delay.Value01());
			for (int i = 0; i < _birds.Length; i++)
			{
				Transform t = _birds[i];
				Vector2 v = offsets[i];
				Vector3 initPos = _initPos[t];
				t.localPosition = Vector3.Lerp(initPos, new Vector3(initPos.x + v.x, initPos.y + v.y, initPos.z), x);
				t.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 1f), x);
			}
			yield return null;
		}
	}
}
