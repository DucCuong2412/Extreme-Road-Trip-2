using UnityEngine;

public class Location : MonoBehaviour
{
	private string _displayName;

	public Transform _skybox;

	public Transform[] _background;

	public Transform[] _clouds;

	public Transform[] _props;

	public Transform[] _statics;

	public Material _surfaceMaterial;

	public Material _groundMaterial;

	public Transform _particleSystem;

	public string DisplayName
	{
		get
		{
			return _displayName.Localize();
		}
		set
		{
			_displayName = value;
		}
	}
}
