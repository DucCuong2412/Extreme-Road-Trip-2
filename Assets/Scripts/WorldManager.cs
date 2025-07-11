using UnityEngine;

public class WorldManager : AutoSingleton<WorldManager>
{
	private class Decor
	{
		public Transform Transform
		{
			get;
			private set;
		}

		public Renderer Renderer
		{
			get;
			private set;
		}

		public Bounds Bounds => Renderer.bounds;

		public Decor(Transform prefab)
		{
			Transform = (Transform)Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
			Renderer = Transform.GetComponentInChildren<Renderer>();
		}

		public bool IsLeftOfScreen()
		{
			Vector3 max = Renderer.bounds.max;
			return max.x < 0f - 0.5f * _width;
		}

		public bool IsRightOfScreen()
		{
			Vector3 min = Renderer.bounds.min;
			return min.x > 0.5f * _width;
		}

		public void Move(float offset)
		{
			Transform.Translate(new Vector3(offset, 0f, 0f));
		}
	}

	private static float _width = 48f;

	private CameraGame _camera;

	private Vector3 _lastCameraPosition;

	private Vector3 _deltaCameraPosition;

	private Material _skyboxMaterial;

	private Transform[] _backgrounds;

	private Decor[] _clouds;

	public void UpdateCamera()
	{
		Vector3 position = _camera.transform.position;
		_deltaCameraPosition = position - _lastCameraPosition;
		_lastCameraPosition = position;
	}

	public Vector3 GetDeltaCameraPosition()
	{
		return _deltaCameraPosition;
	}

	public Vector3 GetCameraPosition()
	{
		return _lastCameraPosition;
	}

	private void CreateSkybox()
	{
		Transform skyboxPrefab = AutoSingleton<LocationManager>.Instance.GetSkyboxPrefab();
		if (skyboxPrefab != null)
		{
			Transform transform = (Transform)Object.Instantiate(skyboxPrefab, Vector3.zero, Quaternion.identity);
			Renderer componentInChildren = transform.GetComponentInChildren<Renderer>();
			componentInChildren.transform.localScale = new Vector3(_width, _width, 1f);
			_skyboxMaterial = componentInChildren.material;
		}
	}

	private void UpdateSkybox()
	{
		Material skyboxMaterial = _skyboxMaterial;
		Vector3 cameraPosition = GetCameraPosition();
		skyboxMaterial.mainTextureOffset = new Vector2((0f - cameraPosition.x) * 0.0002f % 1f, 0f);
	}

	private void CreateBackground()
	{
		Transform[] backgroundPrefabs = AutoSingleton<LocationManager>.Instance.GetBackgroundPrefabs();
		if (backgroundPrefabs != null)
		{
			_backgrounds = new Transform[2];
			_backgrounds[0] = (Transform)Object.Instantiate(backgroundPrefabs[0], Vector3.zero, Quaternion.identity);
			_backgrounds[1] = (Transform)Object.Instantiate(backgroundPrefabs[1], Vector3.zero, Quaternion.identity);
			Transform[] backgrounds = _backgrounds;
			foreach (Transform transform in backgrounds)
			{
				transform.GetComponentInChildren<Renderer>().transform.localScale = new Vector3(_width, _width, 1f);
			}
		}
	}

	private void UpdateBackground()
	{
		if (_backgrounds != null)
		{
			Vector3 cameraPosition = GetCameraPosition();
			float num = 0.03f * cameraPosition.x;
			_backgrounds[0].transform.position = new Vector3(_width - num % (_width * 2f), 0f, 0f);
			_backgrounds[1].transform.position = new Vector3(_width - (num + _width) % (_width * 2f), 0f, 0f);
		}
	}

	private void CreateClouds()
	{
		Transform[] cloudPrefabs = AutoSingleton<LocationManager>.Instance.GetCloudPrefabs();
		int num = cloudPrefabs.Length;
		_clouds = new Decor[num];
		for (int i = 0; i < num; i++)
		{
			_clouds[i] = new Decor(cloudPrefabs[i]);
			float num2 = _width / (float)num;
			_clouds[i].Transform.position = new Vector3((0f - _width) * 0.5f + num2 * 0.5f + num2 * (float)i, 0f, 0f);
		}
	}

	private void UpdateClouds()
	{
		float num = -0.05f;
		Decor[] clouds = _clouds;
		foreach (Decor decor in clouds)
		{
			Vector3 min = decor.Bounds.min;
			if (min.x > 0.5f * _width + 50f)
			{
				Decor decor2 = decor;
				float num2 = 0f - _width;
				Vector3 size = decor.Bounds.size;
				decor2.Move(num2 - size.x - 50f);
			}
			else
			{
				Decor decor3 = decor;
				float num3 = 0f - num;
				Vector3 deltaCameraPosition = GetDeltaCameraPosition();
				decor3.Move(num3 * deltaCameraPosition.x);
			}
		}
	}

	private void CreateStatics()
	{
		Transform[] statics = AutoSingleton<LocationManager>.Instance.GetStatics();
		foreach (Transform original in statics)
		{
			Object.Instantiate(original, Vector3.zero, Quaternion.identity);
		}
	}

	private void CreateParticles()
	{
		Transform particleSystem = AutoSingleton<LocationManager>.Instance.GetParticleSystem();
		if (particleSystem != null)
		{
			Transform transform = Object.Instantiate(particleSystem, particleSystem.transform.position, particleSystem.transform.rotation) as Transform;
			if (transform != null)
			{
				ParticleSystem component = transform.GetComponent<ParticleSystem>();
				component.Play();
			}
		}
	}

	public void Create(GameMode mode)
	{
		AutoSingleton<GroundManager>.Instance.Create(mode);
	}

	protected override void OnAwake()
	{
		_camera = PrefabSingleton<CameraGame>.Instance;
		_width = PrefabSingleton<CameraBackground>.Instance.ScreenWidth();
		CreateSkybox();
		CreateBackground();
		CreateClouds();
		CreateStatics();
		CreateParticles();
		base.OnStart();
	}

	public void Update()
	{
		UpdateCamera();
		UpdateSkybox();
		UpdateBackground();
		UpdateClouds();
	}
}
