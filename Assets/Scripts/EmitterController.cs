using System;
using UnityEngine;

[Serializable]
public class EmitterController
{
    public Transform _prefab;

    private Transform _spawn;
    private bool _initialized;
    private ParticleSystem[] _emitters;

    private void Init()
    {
        if (!_initialized && _prefab != null)
        {
            _spawn = PrefabSingleton<GameSpecialFXManager>.Instance.InstantiateFX(_prefab);
            if (_spawn != null)
            {
                _emitters = _spawn.GetComponentsInChildren<ParticleSystem>();
            }
        }
        _initialized = true;
    }

    public void SetLayer(int layer)
    {
        Init();
        if (_spawn != null)
        {
            foreach (ParticleSystem emitter in _emitters)
            {
                emitter.gameObject.layer = layer;
            }
        }
    }

    public void Emit(Vector3 position)
    {
        Init();
        if (_spawn != null)
        {
            _spawn.position = position;
            foreach (ParticleSystem emitter in _emitters)
            {
                emitter.Emit(1);
            }
        }
    }

    public void EmitWithVelocity(Vector3 position, Vector3 velocity)
    {
        Init();
        if (_spawn != null)
        {
            _spawn.position = position;
            foreach (ParticleSystem emitter in _emitters)
            {
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
                {
                    velocity = velocity
                };
                emitter.Emit(emitParams, 1);
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
            foreach (ParticleSystem emitter in _emitters)
            {
                emitter.Emit(1);
            }
        }
    }

    public void StartEmit(Transform attach, Vector3 position)
    {
        Init();
        if (_spawn != null)
        {
            _spawn.SetParent(attach);
            _spawn.localPosition = position;
            foreach (ParticleSystem emitter in _emitters)
            {
                emitter.Play(true); // đảm bảo phát hạt nếu loop
            }
        }
    }

    public void StopEmit()
    {
        Init();
        if (_emitters == null) return;

        foreach (ParticleSystem emitter in _emitters)
        {
            emitter.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
