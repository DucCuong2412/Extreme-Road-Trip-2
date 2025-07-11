using UnityEngine;

public class PlatformEnforcer : MonoBehaviour
{
	public bool _mobileOnly = true;

	public bool _androidOnly;

	public bool _iPhoneOnly;

	public void Awake()
	{
		if (_iPhoneOnly)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}
}
