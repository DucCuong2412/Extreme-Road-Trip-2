using UnityEngine;

public class DetonatorBurstEmitter : DetonatorComponent
{
    private ParticleSystem _particleEmitter;

    private ParticleSystem _particleRenderer;

    private ParticleSystem _particleAnimator;

    private float _baseDamping = 0.1300004f;

    private float _baseSize = 1f;

    private Color _baseColor = Color.white;

    public float damping = 1f;

    public float startRadius = 1f;

    public float maxScreenSize = 2f;

    public bool explodeOnAwake;

    public bool oneShot = true;

    public float sizeVariation;

    public float particleSize = 1f;

    public float count = 1f;

    public float sizeGrow = 20f;

    public bool exponentialGrowth = true;

    public float durationVariation;

    public bool useWorldSpace = true;

    public float upwardsBias;

    public float angularVelocity = 20f;

    public bool randomRotation = true;

    public ParticleSystemRenderMode renderMode = ParticleSystemRenderMode.Billboard;

    public bool useExplicitColorAnimation;

    public Color[] colorAnimation = new Color[5];

    private bool _delayedExplosionStarted;

    private float _explodeDelay;

    public Material material;

    private float _emitTime;

    private float speed = 3f;

    private float initFraction = 0.1f;

    private static float epsilon = 0.01f;

    private float _tmpParticleSize;

    private Vector3 _tmpPos;

    private Vector3 _tmpDir;

    private Vector3 _thisPos;

    private float _tmpDuration;

    private float _tmpCount;

    private float _scaledDuration;

    private float _scaledDurationVariation;

    private float _scaledStartRadius;

    private float _scaledColor;

    private float _randomizedRotation;

    private float _tmpAngularVelocity;

    public override void Init()
    {
        MonoBehaviour.print("UNUSED");
    }

    public void Awake()
    {
        // Create and configure ParticleSystem for Unity 2021+
        _particleEmitter = base.gameObject.GetComponent<ParticleSystem>();
        if (_particleEmitter == null)
        {
            _particleEmitter = base.gameObject.AddComponent<ParticleSystem>();
        }
        var main = _particleEmitter.main;
        main.startSize = _baseSize;
        main.startColor = _baseColor;
        main.startLifetime = 1f;
        main.simulationSpace = useWorldSpace ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local;
        main.maxParticles = Mathf.CeilToInt(count * detail);

        var emission = _particleEmitter.emission;
        emission.enabled = false;

        var shape = _particleEmitter.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = startRadius;

        var renderer = base.gameObject.GetComponent<ParticleSystemRenderer>();
        if (renderer == null)
        {
            renderer = base.gameObject.AddComponent<ParticleSystemRenderer>();
        }
        renderer.material = material;
        renderer.maxParticleSize = maxScreenSize;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        // Store references for later use
        _particleRenderer = _particleEmitter;
        _particleAnimator = _particleEmitter;
        // _particleEmitter = (base.gameObject.AddComponent("EllipsoidParticleEmitter") as ParticleEmitter);
        // _particleRenderer = (base.gameObject.AddComponent("ParticleRenderer") as ParticleRenderer);
        // _particleAnimator = (base.gameObject.AddComponent("ParticleAnimator") as ParticleAnimator);
        // _particleEmitter.hideFlags = HideFlags.HideAndDontSave;
        // _particleRenderer.hideFlags = HideFlags.HideAndDontSave;
        // _particleAnimator.hideFlags = HideFlags.HideAndDontSave;
        // _particleAnimator.damping = _baseDamping;
        // _particleEmitter.emit = false;
        // _particleRenderer.maxParticleSize = maxScreenSize;
        // _particleRenderer.material = material;
        // _particleRenderer.material.color = Color.white;
        // _particleAnimator.sizeGrow = sizeGrow;
        if (explodeOnAwake)
        {
            Explode();
        }
    }

    private ParticleSystem _particleSystem;
    private ParticleSystem.SizeOverLifetimeModule _sizeOverLifetime;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _sizeOverLifetime = _particleSystem.sizeOverLifetime;
        _sizeOverLifetime.enabled = true;
    }

    // Update mới
    private void Update()
    {
        if (exponentialGrowth)
        {
            float num = Time.time - _emitTime;
            float num2 = SizeFunction(num - epsilon);
            float num3 = SizeFunction(num);
            float num4 = (num3 / num2 - 1f) / epsilon;

            // Cập nhật size grow động bằng curve
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0f, 0f); // bắt đầu nhỏ
            curve.AddKey(1f, num4); // kết thúc lớn dần theo tốc độ tăng trưởng

            _sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);
        }
        else
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0f, 0f);
            curve.AddKey(1f, sizeGrow); // dùng giá trị cố định

            _sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);
        }

        if (_delayedExplosionStarted)
        {
            _explodeDelay -= Time.deltaTime;
            if (_explodeDelay <= 0f)
            {
                Explode();
            }
        }
    }

    private float SizeFunction(float elapsedTime)
    {
        float num = 1f - 1f / (1f + elapsedTime * speed);
        return initFraction + (1f - initFraction) * num;
    }

    public void Reset()
    {
        size = _baseSize;
        color = _baseColor;
        damping = _baseDamping;
    }

    public override void Explode()
    {
        if (!on)
            return;
        // đảm bảo _particleSystem đã được khởi tạo
        if (_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            if (_particleSystem == null)
            {
                Debug.LogError("DetonatorBurstEmitter: Missing ParticleSystem.");
                return;
            }
        }

        _scaledDuration = timeScale * duration;
        _scaledDurationVariation = timeScale * durationVariation;
        _scaledStartRadius = size * startRadius;

        var main = _particleSystem.main;
        main.simulationSpace = useWorldSpace ? ParticleSystemSimulationSpace.World : ParticleSystemSimulationSpace.Local;

        var renderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = renderMode;
        renderer.material = material;

        if (!_delayedExplosionStarted)
        {
            _explodeDelay = explodeDelayMin + Random.value * (explodeDelayMax - explodeDelayMin);
        }

        if (_explodeDelay <= 0f)
        {
            // Thiết lập Color Animation
            var colorOverLifetime = _particleSystem.colorOverLifetime;
            colorOverLifetime.enabled = true;

            Gradient gradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[5];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];

            if (useExplicitColorAnimation)
            {
                for (int i = 0; i < 5; i++)
                {
                    colorKeys[i] = new GradientColorKey(colorAnimation[i], i / 4f);
                    alphaKeys[i] = new GradientAlphaKey(colorAnimation[i].a, i / 4f);
                }
            }
            else
            {
                colorKeys[0] = new GradientColorKey(color, 0f);
                alphaKeys[0] = new GradientAlphaKey(color.a * 0.7f, 0f);

                colorKeys[1] = new GradientColorKey(color, 0.25f);
                alphaKeys[1] = new GradientAlphaKey(color.a * 1f, 0.25f);

                colorKeys[2] = new GradientColorKey(color, 0.5f);
                alphaKeys[2] = new GradientAlphaKey(color.a * 0.5f, 0.5f);

                colorKeys[3] = new GradientColorKey(color, 0.75f);
                alphaKeys[3] = new GradientAlphaKey(color.a * 0.3f, 0.75f);

                colorKeys[4] = new GradientColorKey(color, 1f);
                alphaKeys[4] = new GradientAlphaKey(color.a * 0f, 1f);
            }

            gradient.SetKeys(colorKeys, alphaKeys);
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            // Tính số lượng hạt
            _tmpCount = count * detail;
            if (_tmpCount < 1f) _tmpCount = 1f;

            _thisPos = useWorldSpace ? transform.position : Vector3.zero;

            for (int i = 0; i < (int)_tmpCount; i++)
            {
                _tmpPos = Vector3.Scale(Random.insideUnitSphere, new Vector3(_scaledStartRadius, _scaledStartRadius, _scaledStartRadius));
                _tmpPos += _thisPos;

                _tmpDir = Vector3.Scale(Random.insideUnitSphere, new Vector3(velocity.x, velocity.y, velocity.z));
                _tmpDir.y += 2f * Mathf.Abs(_tmpDir.y) * upwardsBias;

                if (randomRotation)
                {
                    _randomizedRotation = Random.Range(-1f, 1f);
                    _tmpAngularVelocity = Random.Range(-1f, 1f) * angularVelocity;
                }
                else
                {
                    _randomizedRotation = 0f;
                    _tmpAngularVelocity = angularVelocity;
                }

                _tmpDir *= size;
                _tmpParticleSize = size * (particleSize + Random.value * sizeVariation);
                _tmpDuration = _scaledDuration + Random.value * _scaledDurationVariation;

                // Emit particle
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
                {
                    position = _tmpPos,
                    velocity = _tmpDir,
                    startSize = _tmpParticleSize,
                    startLifetime = _tmpDuration,
                    startColor = color,
                    rotation = _randomizedRotation,
                    angularVelocity = _tmpAngularVelocity
                };

                _particleSystem.Emit(emitParams, 1);
            }

            _emitTime = Time.time;
            _delayedExplosionStarted = false;
            _explodeDelay = 0f;
        }
        else
        {
            _delayedExplosionStarted = true;
        }
    }

}
