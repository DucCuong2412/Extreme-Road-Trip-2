using UnityEngine;

[AddComponentMenu("Detonator/Shockwave")]
[RequireComponent(typeof(Detonator))]
public class DetonatorShockwave : DetonatorComponent
{
	private float _baseSize = 1f;

	private float _baseDuration = 0.25f;

	private Vector3 _baseVelocity = new Vector3(0f, 0f, 0f);

	private Color _baseColor = Color.white;

	private GameObject _shockwave;

	private DetonatorBurstEmitter _shockwaveEmitter;

	public Material shockwaveMaterial;

	public ParticleSystemRenderMode renderMode;
	//public ParticleRenderMode renderMode;

	public override void Init()
	{
		FillMaterials(wipe: false);
		BuildShockwave();
	}

	public void FillMaterials(bool wipe)
	{
		if (!shockwaveMaterial || wipe)
		{
			shockwaveMaterial = MyDetonator().shockwaveMaterial;
		}
	}

	public void BuildShockwave()
	{
		_shockwave = new GameObject("Shockwave");
		//_shockwaveEmitter = (DetonatorBurstEmitter)_shockwave.AddComponent("DetonatorBurstEmitter");
		_shockwave.transform.parent = base.transform;
		_shockwave.transform.localRotation = Quaternion.identity;
		_shockwave.transform.localPosition = localPosition;
		_shockwaveEmitter.material = shockwaveMaterial;
		_shockwaveEmitter.exponentialGrowth = false;
		_shockwaveEmitter.useWorldSpace = MyDetonator().useWorldSpace;
	}

	public void UpdateShockwave()
	{
		_shockwave.transform.localPosition = Vector3.Scale(localPosition, new Vector3(size, size, size));
		_shockwaveEmitter.color = color;
		_shockwaveEmitter.duration = duration;
		_shockwaveEmitter.durationVariation = duration * 0.1f;
		_shockwaveEmitter.count = 1f;
		_shockwaveEmitter.detail = 1f;
		_shockwaveEmitter.particleSize = 25f;
		_shockwaveEmitter.sizeVariation = 0f;
		_shockwaveEmitter.velocity = new Vector3(0f, 0f, 0f);
		_shockwaveEmitter.startRadius = 0f;
		_shockwaveEmitter.sizeGrow = 202f;
		_shockwaveEmitter.size = size;
		_shockwaveEmitter.explodeDelayMin = explodeDelayMin;
		_shockwaveEmitter.explodeDelayMax = explodeDelayMax;
		_shockwaveEmitter.renderMode = renderMode;
	}

	public void Reset()
	{
		FillMaterials(wipe: true);
		on = true;
		size = _baseSize;
		duration = _baseDuration;
		explodeDelayMin = 0f;
		explodeDelayMax = 0f;
		color = _baseColor;
		velocity = _baseVelocity;
	}

	public override void Explode()
	{
		if (on)
		{
			UpdateShockwave();
			_shockwaveEmitter.Explode();
		}
	}
}
