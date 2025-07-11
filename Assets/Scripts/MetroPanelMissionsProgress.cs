using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroPanelMissionsProgress : MetroWidget
{
	private int _progress;

	private List<tk2dAnimatedSprite> _stars;

	public bool IsAnimating
	{
		get;
		private set;
	}

	public static MetroPanelMissionsProgress Create(int progress)
	{
		GameObject gameObject = new GameObject(typeof(MetroPanelMissionsProgress).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		MetroPanelMissionsProgress metroPanelMissionsProgress = gameObject.AddComponent<MetroPanelMissionsProgress>();
		metroPanelMissionsProgress.Setup(progress);
		return metroPanelMissionsProgress;
	}

	public bool IsProgressFull()
	{
		return _progress == 0;
	}

	public Vector3 GetNextStepAnchorPos()
	{
		return _stars[_progress].transform.position;
	}

	public void ShowProgressAnim()
	{
		StartCoroutine(ShowProgressAnimCR());
	}

	public void ShowProgressFullAnim()
	{
		StartCoroutine(ShowProgressFullAnimCR());
	}

	public void StopAnim()
	{
		IsAnimating = false;
	}

	private IEnumerator ShowProgressAnimCR()
	{
		IsAnimating = true;
		Duration delay = new Duration(0.1f);
		int prev = _progress;
		_progress = ++_progress % 5;
		_stars[prev].DefaultClipId = 1;
		_stars[prev].Play();
		while (!delay.IsDone() && IsAnimating)
		{
			yield return null;
		}
		_stars[prev].Stop();
		_stars[prev].SetFrame(0);
		IsAnimating = false;
	}

	private IEnumerator ShowProgressFullAnimCR()
	{
		IsAnimating = true;
		foreach (tk2dAnimatedSprite s3 in _stars)
		{
			s3.DefaultClipId = 1;
			s3.Play();
		}
		PrefabSingleton<GameMusicManager>.Instance.PlayMissionsFullProgressMusic();
		Duration delay = new Duration(0.7f);
		while (!delay.IsDone() && IsAnimating)
		{
			float angle = Mathf.Lerp(0f, 360f, delay.Value01());
			foreach (tk2dAnimatedSprite s2 in _stars)
			{
				s2.transform.eulerAngles = new Vector3(0f, 0f, angle);
			}
			yield return null;
		}
		foreach (tk2dAnimatedSprite s in _stars)
		{
			s.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			s.Stop();
			s.DefaultClipId = 0;
			s.SetFrame(0);
			s.Play();
		}
		IsAnimating = false;
	}

	private void Setup(int progress)
	{
		_progress = progress % 5;
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		Add(metroLayout);
		_stars = new List<tk2dAnimatedSprite>();
		metroLayout.Add(MetroSpacer.Create());
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.AddSlice9Background(MetroSkin.Slice9RoundedSemiTransparent);
		metroLayout2.Mass = 2f;
		metroLayout.Add(metroLayout2);
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load(MetroSkin.Star), Vector3.zero, Quaternion.identity) as GameObject;
			MetroIcon child = MetroIcon.Create(gameObject);
			tk2dAnimatedSprite component = gameObject.GetComponent<tk2dAnimatedSprite>();
			component.DefaultClipId = ((i < _progress) ? 1 : 0);
			component.SetFrame(0);
			metroLayout2.Add(child);
			_stars.Add(component);
		}
		metroLayout.Add(MetroSpacer.Create());
	}
}
