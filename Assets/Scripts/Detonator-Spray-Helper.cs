using System;
using UnityEngine;

[Serializable]
public class Detonator_0020Spray_0020Helper : MonoBehaviour
{
    public float startTimeMin;
    public float startTimeMax;
    public float stopTimeMin = 10f;
    public float stopTimeMax = 10f;

    public Material firstMaterial;
    public Material secondMaterial;

    private float startTime;
    private float stopTime;
    private float spawnTime;

    private bool isReallyOn;
    private ParticleSystem _ps;

    public void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        if (_ps == null)
        {
            Debug.LogError("Missing ParticleSystem on GameObject!");
            return;
        }

        // Lưu trạng thái ban đầu (giả sử đang chạy)
        isReallyOn = _ps.isPlaying;
        _ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        spawnTime = Time.time;
        startTime = UnityEngine.Random.Range(startTimeMin, startTimeMax) + Time.time;
        stopTime = UnityEngine.Random.Range(stopTimeMin, stopTimeMax) + Time.time;

        // Đổi vật liệu ngẫu nhiên
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = UnityEngine.Random.value <= 0.5f ? firstMaterial : secondMaterial;
        }
    }

    public void FixedUpdate()
    {
        if (_ps == null) return;

        if (Time.time > startTime && !_ps.isPlaying && isReallyOn)
        {
            _ps.Play();
        }

        if (Time.time > stopTime && _ps.isPlaying)
        {
            _ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public void Main()
    {
    }
}
