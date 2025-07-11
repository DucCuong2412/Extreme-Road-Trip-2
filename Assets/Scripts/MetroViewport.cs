using System.Collections;
using UnityEngine;

public class MetroViewport : MetroWidget
{
	private MetroViewportCamera _viewportCamera;

	private bool _enabled;

	public int GUILayer
	{
		get;
		private set;
	}

	private void SetEnabled(bool enabled)
	{
		_enabled = enabled;
		_viewportCamera.gameObject.SetActive(_enabled);
		SetLayer((!_enabled) ? 8 : GUILayer);
	}

	public override void OnBlur()
	{
		StopCoroutine("WaitAnimationAndEnable");
		SetEnabled(enabled: false);
		base.OnBlur();
	}

	public override void OnFocus()
	{
		StartCoroutine("WaitAnimationAndEnable");
		base.OnFocus();
	}

	private IEnumerator WaitAnimationAndEnable()
	{
		yield return new WaitForSeconds(MetroSkin.MenuPageSlideDuration);
		SetEnabled(enabled: true);
	}

	protected override void OnAwake()
	{
		base.OnAwake();
	}

	protected override void OnStart()
	{
		base.gameObject.layer = GUILayer;
		_viewportCamera = MetroViewportCamera.Create(this);
		SetEnabled(enabled: true);
		base.OnStart();
	}

	public void Update()
	{
		_viewportCamera.SetZone(_childsZone);
	}

	public override void Layout(Rect rect)
	{
		SetLayer((!_enabled) ? 8 : GUILayer);
		base.Layout(rect);
	}

	public static MetroViewport Create(int layer)
	{
		GameObject gameObject = new GameObject("MetroViewport");
		MetroViewport metroViewport = gameObject.AddComponent<MetroViewport>();
		metroViewport.GUILayer = layer;
		return metroViewport;
	}
}
