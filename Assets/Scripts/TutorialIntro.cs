using System.Collections;
using UnityEngine;

public class TutorialIntro : Tutorial
{
    public Transform _car;
    public Transform _backWheel;
    public Transform _frontWheel;
    public Transform _dustPivot;

    private float _dustTime;
    private ParticleSystem _dustFX;

    protected override void Reset()
    {
    }

    protected override IEnumerator AnimCR()
    {
        Vector3 localPosition = _car.localPosition;
        float from = localPosition.y;
        float to = from + Random.Range(-0.2f, 0.2f);
        Duration delay = new Duration(0.4f);

        while (!delay.IsDone() && _isAnimating)
        {
            float bounceValue = Mathfx.Bounce(delay.Value01());
            Vector3 pos = _car.localPosition;
            float y = Mathf.Lerp(from, to, bounceValue);
            _car.localPosition = new Vector3(pos.x, y, pos.z);
            yield return null;
        }

        _finished = true;
    }

    protected override void Awake()
    {
        base.Awake();

        // Instantiate dust effect
        Transform dustTransform = Object.Instantiate(
            PrefabSingleton<GameSpecialFXManager>.Instance._dustFXPrefab,
            Vector3.zero,
            Quaternion.identity);

        dustTransform.gameObject.layer = 8;
        dustTransform.SetParent(_dustPivot);
        dustTransform.localPosition = Vector3.zero;

        _dustFX = dustTransform.GetComponent<ParticleSystem>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _dustTime += Time.fixedDeltaTime;
        if (_dustTime >= 0.1f)
        {
            _dustTime -= 0.1f;

            if (_dustFX != null)
            {
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
                {
                    velocity = new Vector3(-25f, 1f, 0f)
                };
                _dustFX.Emit(emitParams, 1);
            }
        }

        float angle = Time.fixedDeltaTime * -500f;
        _backWheel.Rotate(Vector3.forward, angle);
        _frontWheel.Rotate(Vector3.forward, angle);
    }
}
