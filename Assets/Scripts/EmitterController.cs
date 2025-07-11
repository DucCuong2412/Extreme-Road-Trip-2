using System;
using UnityEngine;

[Serializable]
public class EmitterController
{
	public Transform _prefab;

	private Transform _spawn;

	private bool _initialized;

	private ParticleEmitter[] _emitters;

	private void Init()
	{
		if (!_initialized && _prefab != null)
		{
			_spawn = PrefabSingleton<GameSpecialFXManager>.Instance.InstantiateFX(_prefab);
			if (_spawn != null)
			{
				_emitters = _spawn.GetComponentsInChildren<ParticleEmitter>();
			}
		}
		_initialized = true;
	}

	public void SetLayer(int layer)
	{
		Init();
		if (_spawn != null)
		{
			ParticleEmitter[] emitters = _emitters;
			foreach (ParticleEmitter particleEmitter in emitters)
			{
				particleEmitter.gameObject.layer = layer;
			}
		}
	}

	public void Emit(Vector3 position)
	{
		Init();
		if (_spawn != null)
		{
			_spawn.position = position;
			ParticleEmitter[] emitters = _emitters;
			foreach (ParticleEmitter particleEmitter in emitters)
			{
				particleEmitter.Emit();
			}
		}
	}

	public void EmitWithVelocity(Vector3 position, Vector3 velocity)
	{
		Init();
		if (_spawn != null)
		{
			_spawn.position = position;
			ParticleEmitter[] emitters = _emitters;
			foreach (ParticleEmitter particleEmitter in emitters)
			{
				particleEmitter.worldVelocity = velocity;
				particleEmitter.Emit();
			}
		}
	}

	public void EmitWithRotation(Vector3 position, Quaternion rotation)
	{
		Init();
		if (_spawn != null)
		{
			_spawn.position = position;
			_spawn.rotation = rotation;
			ParticleEmitter[] emitters = _emitters;
			foreach (ParticleEmitter particleEmitter in emitters)
			{
				particleEmitter.Emit();
			}
		}
	}

	public void StartEmit(Transform attach, Vector3 position)
	{
		Init();
		if (_spawn != null)
		{
			_spawn.parent = attach;
			_spawn.localPosition = position;
			ParticleEmitter[] emitters = _emitters;
			foreach (ParticleEmitter particleEmitter in emitters)
			{
				particleEmitter.emit = true;
			}
		}
	}

	public void StopEmit()
	{
		if (!_initialized)
		{
			Init();
			_initialized = true;
		}
		ParticleEmitter[] emitters = _emitters;
		foreach (ParticleEmitter particleEmitter in emitters)
		{
			particleEmitter.emit = false;
		}
	}
}
