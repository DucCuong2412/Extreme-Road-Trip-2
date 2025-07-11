using UnityEngine;

public class GameSpecialFXManager : RoofdogSpecialFXManager<GameSpecialFXManager>
{
	public EmitterController _sparks;

	public EmitterController _crateBreakFX;

	public Transform _coinShowerPrefab;

	private ParticleSystem _coinShower;

	public Transform _grindFXPrefab;

	private ParticleSystem _grindFX;

	public Transform _engineBreakPrefab;

	private ParticleSystem _engineBreak;

	public EmitterController _skid;

	public Transform _dustFXPrefab;

	public EmitterController _dust;

	public Transform _groundChunkFXPrefab;

	public EmitterController _groundChunk;

	public EmitterController _treeChunk;

	public Transform _megaBoostFX;

	public Transform _rocketFX;

	public Transform _boostTrailPrefab;

	public Transform _coinTrail;

	public Transform _gasTrail;

	public Transform _buckTrail;

	public Transform _carShadow;

	public Transform _slamLinesFX;

	public Transform _magnetFX;

	public Transform _coinsFX;

	public Transform _explosionPocketMineCrate;

	public void PlayGUISparksFX(Vector3 position)
	{
		_sparks.SetLayer(8);
		_sparks.Emit(position);
	}

	public void PlaySparksFX(Vector3 position, Vector3 relativeVelocity)
	{
		if (relativeVelocity.sqrMagnitude > 4f)
		{
			_sparks.Emit(position + new Vector3(0f, 0f, -4f));
		}
	}

	public void PlayGUICrateBreakFX(Vector3 position)
	{
		_crateBreakFX.SetLayer(8);
		_crateBreakFX.EmitWithVelocity(position, Vector3.zero);
	}

	public void StartGUICoinShowerFX(Vector3 position)
	{
		if (_coinShower == null)
		{
			Transform transform = (Transform)Object.Instantiate(_coinShowerPrefab, Vector3.zero, Quaternion.identity);
			if (transform != null)
			{
				_coinShower = transform.GetComponent<ParticleSystem>();
				_coinShower.gameObject.layer = 8;
			}
		}
		if (_coinShower != null)
		{
			_coinShower.transform.position = position;
			_coinShower.Play();
		}
	}

	public void StopGUICoinShowerFX()
	{
		if (_coinShower != null)
		{
			_coinShower.Stop();
		}
	}

	public void StartGUIGrindFX(Vector3 position)
	{
		if (_grindFX == null)
		{
			Transform transform = (Transform)Object.Instantiate(_grindFXPrefab, Vector3.zero, _grindFXPrefab.transform.rotation);
			if (transform != null)
			{
				_grindFX = transform.GetComponent<ParticleSystem>();
				_grindFX.gameObject.layer = 8;
			}
		}
		if (_grindFX != null)
		{
			_grindFX.transform.position = position;
			_grindFX.Play();
		}
	}

	public void MoveGUIGrindFX(Vector3 position)
	{
		if (_grindFX != null)
		{
			_grindFX.transform.position = position;
		}
	}

	public void StopGUIGrindFX()
	{
		if (_grindFX != null)
		{
			_grindFX.Stop();
		}
	}

	public void PlayEngineBreakFX(Transform t, Vector3 offset)
	{
		if (_engineBreak == null)
		{
			Transform transform = (Transform)Object.Instantiate(_engineBreakPrefab, Vector3.zero, Quaternion.identity);
			if (transform != null)
			{
				_engineBreak = transform.GetComponent<ParticleSystem>();
			}
		}
		if (_engineBreak != null)
		{
			_engineBreak.transform.parent = t;
			_engineBreak.transform.localPosition = offset;
			_engineBreak.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 180f));
			_engineBreak.Play();
		}
	}

	public void PlaySkidFX(Vector3 position)
	{
		_skid.Emit(position);
	}

	public void PlayDustFX(Vector3 position)
	{
		_dust.Emit(position);
	}

	public void PlayGroundChunkFX(Vector3 position)
	{
		_groundChunk.Emit(position);
	}

	public void PlayTreeChunkFX(Vector3 position)
	{
		_treeChunk.Emit(position);
	}

	public ParticleSystem AddCarMegaBoostFX(Transform car)
	{
		Transform transform = Object.Instantiate(_megaBoostFX, Vector3.zero, Quaternion.identity) as Transform;
		if (transform != null)
		{
			transform.parent = car;
			Transform transform2 = transform;
			Vector3 size = RendererBounds.ComputeBounds(car).size;
			transform2.localPosition = new Vector3(size.x * -0.5f + 0.5f, 0f, 1f);
			transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
			return transform.GetComponent<ParticleSystem>();
		}
		return null;
	}

	public ParticleSystem AddRocketFX(Transform rocket)
	{
		Transform transform = Object.Instantiate(_rocketFX, Vector3.zero, Quaternion.identity) as Transform;
		if (transform != null)
		{
			transform.parent = rocket;
			Transform transform2 = transform;
			Vector3 size = RendererBounds.ComputeBounds(rocket).size;
			transform2.localPosition = new Vector3(size.x * -0.5f + 0.5f, 0f, 1f);
			transform.localRotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
			return transform.GetComponent<ParticleSystem>();
		}
		return null;
	}

	public Transform AddCollectibleTrail(Transform t, CollectibleType type)
	{
		Transform original;
		switch (type)
		{
		case CollectibleType.pinataBuck:
			original = _buckTrail;
			break;
		case CollectibleType.coin:
		case CollectibleType.pinataCoin:
			original = _coinTrail;
			break;
		case CollectibleType.gas:
		case CollectibleType.pinataGas:
			original = _gasTrail;
			break;
		default:
			original = _coinTrail;
			break;
		}
		Transform transform = (Transform)Object.Instantiate(original, t.position, Quaternion.identity);
		if (transform != null)
		{
			transform.parent = t;
		}
		return transform;
	}

	public void AddCarShadow(CarController car)
	{
		Transform transform = (Transform)Object.Instantiate(_carShadow, Vector3.zero, Quaternion.identity);
		if (transform != null)
		{
			CarShadow component = transform.GetComponent<CarShadow>();
			component.Setup(car);
		}
	}

	public ParticleSystem AddCarSlamLinesFX(Transform car)
	{
		Transform transform = (Transform)Object.Instantiate(_slamLinesFX, Vector3.zero, Quaternion.identity);
		if (transform != null)
		{
			transform.parent = car;
			transform.localPosition = new Vector3(0f, 0f, 3f);
			transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
			return transform.GetComponent<ParticleSystem>();
		}
		return null;
	}

	public ParticleSystem AddCarMagnetFX(CarController car)
	{
		Transform transform = Object.Instantiate(_magnetFX, Vector3.zero, Quaternion.identity) as Transform;
		if (transform != null)
		{
			transform.parent = car.transform;
			transform.localPosition = new Vector3(0f, 0f, 0.5f);
			return transform.GetComponent<ParticleSystem>();
		}
		return null;
	}

	public ParticleSystem AddCarCoinsFX(CarController car)
	{
		Transform transform = Object.Instantiate(_coinsFX, Vector3.zero, Quaternion.identity) as Transform;
		if (transform != null)
		{
			transform.parent = car.transform;
			transform.localPosition = new Vector3(0f, 0f, 0.5f);
			return transform.GetComponent<ParticleSystem>();
		}
		return null;
	}

	public void ExplodeCrate(Vector3 position)
	{
		Transform transform = (Transform)Object.Instantiate(_explosionPocketMineCrate, Vector3.zero, Quaternion.identity);
		if (transform != null)
		{
			transform.position = position;
			transform.GetComponent<ParticleSystem>().Play();
		}
	}
}
