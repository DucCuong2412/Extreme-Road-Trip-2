using UnityEngine;

public class LocationManager : AutoSingleton<LocationManager>
{
	private Location _location;

	protected override void OnAwake()
	{
		LoadConfigGame loadConfigGame = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigGame;
		if (loadConfigGame != null)
		{
			_location = loadConfigGame.Location;
		}
		else
		{
			_location = AutoSingleton<LocationDatabase>.Instance.GetRandomLocation();
		}
		base.OnAwake();
	}

	public Location GetLocation()
	{
		return _location;
	}

	public Transform GetSkyboxPrefab()
	{
		return _location._skybox;
	}

	public Transform[] GetBackgroundPrefabs()
	{
		return _location._background;
	}

	public Transform[] GetCloudPrefabs()
	{
		return _location._clouds;
	}

	public Transform[] GetPropsPrefabs()
	{
		return _location._props;
	}

	public Transform[] GetStatics()
	{
		return _location._statics;
	}

	public Material GetSurfaceMaterial()
	{
		return _location._surfaceMaterial;
	}

	public Material GetGroundMaterial()
	{
		return _location._groundMaterial;
	}

	public Transform GetParticleSystem()
	{
		return _location._particleSystem;
	}
}
