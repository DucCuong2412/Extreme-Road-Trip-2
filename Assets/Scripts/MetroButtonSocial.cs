using System;
using System.Collections;
using UnityEngine;

public class MetroButtonSocial : MetroButton
{
	private MetroIcon _socialIcon;

	private float _iconSize;

	public static MetroButtonSocial Create(SocialNetwork socialNetwork, Action onButtonClicked, float iconSize)
	{
		GameObject gameObject = new GameObject(typeof(MetroButtonSocial).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroButtonSocial metroButtonSocial = gameObject.AddComponent<MetroButtonSocial>();
		metroButtonSocial.Setup(socialNetwork, onButtonClicked, iconSize);
		return metroButtonSocial;
	}

	private void Setup(SocialNetwork socialNetwork, Action onButtonClicked, float iconSize)
	{
		AddIcon(socialNetwork, iconSize);
		base.OnButtonClicked += onButtonClicked;
	}

	private IEnumerator DestroyCR()
	{
		yield return new WaitForSeconds(0.25f);
		Destroy();
	}

	private void AddIcon(SocialNetwork socialNetwork, float iconSize)
	{
		switch (socialNetwork)
		{
		case SocialNetwork.facebook:
			_socialIcon = MetroIcon.Create(MetroSkin.IconFacebook).SetScale(iconSize);
			break;
		case SocialNetwork.twitter:
			_socialIcon = MetroIcon.Create(MetroSkin.IconTwitter).SetScale(iconSize);
			break;
		}
		Add(_socialIcon);
	}

	public void SetIconAlignment(MetroAlign align)
	{
		_socialIcon.SetAlignment(align);
	}
}
