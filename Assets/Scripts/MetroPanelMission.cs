using System.Collections;
using UnityEngine;

public class MetroPanelMission : MetroWidget
{
	private Transform _starAnchor;

	private MetroLabel _label;

	private MetroIcon _star;

	public bool IsAnimating
	{
		get;
		private set;
	}

	public bool IsMissionCompleted
	{
		get;
		private set;
	}

	public static MetroPanelMission Create(Car car, Mission mission = null)
	{
		GameObject gameObject = new GameObject(typeof(MetroPanelMission).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		MetroPanelMission metroPanelMission = gameObject.AddComponent<MetroPanelMission>();
		metroPanelMission.Setup(car, mission);
		return metroPanelMission;
	}

	public Vector3 GetStarAnchorPos()
	{
		return _starAnchor.position;
	}

	public void Move(Vector3 from, Vector3 to)
	{
		StartCoroutine(MoveCR(from, to));
	}

	public void ShowIsCompleted(int rank)
	{
		StartCoroutine(ShowIsCompletedCR(rank));
	}

	public void StopAnim()
	{
		IsAnimating = false;
	}

	public void UpdateMission(Car car, Mission m)
	{
		string str = m.Description + m.GetRemainingValueBeforeCompletion(car);
		_label.SetText("  " + str);
		IsMissionCompleted = m.Completed;
		if (!IsMissionCompleted)
		{
			MetroLabel metroLabel = MetroLabel.Create("NEW").SetFont(MetroSkin.SmallFont);
			metroLabel.SetColor(Color.yellow);
			metroLabel.AddOutline();
			metroLabel.transform.parent = _starAnchor;
			metroLabel.transform.localPosition = new Vector3(0f, 0f, -0.1f);
			metroLabel.transform.rotation = Quaternion.Euler(0f, 0f, 10f);
		}
	}

	protected override void OnAwake()
	{
		_transform = base.transform;
		base.OnAwake();
	}

	private IEnumerator MoveCR(Vector3 from, Vector3 to)
	{
		IsAnimating = true;
		Duration delay = new Duration(0.5f);
		_transform.localPosition = from;
		if (IsAnimating)
		{
			PrefabSingleton<GameSoundManager>.Instance.PlayWhishSound();
		}
		while (!delay.IsDone() && IsAnimating)
		{
			float x = Mathfx.Hermite(0f, 1f, delay.Value01());
			_transform.localPosition = Vector3.Lerp(from, to, x);
			yield return null;
		}
		_transform.localPosition = to;
		IsAnimating = false;
	}

	private IEnumerator ShowIsCompletedCR(int rank)
	{
		IsAnimating = true;
		MetroIcon starRing = MetroIcon.Create(MetroSkin.StarParticleRing);
		starRing.transform.parent = _starAnchor;
		starRing.transform.localPosition = new Vector3(0f, 0f, -0.1f);
		tk2dSprite ringSpr = starRing.GetComponentInChildren<tk2dSprite>();
		PrefabSingleton<GameSoundManager>.Instance.PlayStarSound(rank);
		Duration delay2 = new Duration(0.5f);
		while (!delay2.IsDone() && IsAnimating)
		{
			float scale2 = Mathfx.Berp(1f, 2f, delay2.Value01());
			starRing.SetScale(scale2);
			_star.SetScale(scale2);
			float angle = Mathf.Lerp(0f, 360f, delay2.Value01());
			_star.transform.eulerAngles = new Vector3(0f, 0f, angle);
			float alpha = Mathfx.Lerp(1f, 0f, delay2.Value01());
			Color c = ringSpr.color;
			c.a = alpha;
			ringSpr.color = c;
			yield return null;
		}
		_star.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		delay2 = new Duration(0.3f);
		while (!delay2.IsDone() && IsAnimating)
		{
			float scale = Mathfx.Berp(2f, 1f, delay2.Value01());
			_star.SetScale(scale);
			yield return null;
		}
		UnityEngine.Object.Destroy(starRing.gameObject);
		UnityEngine.Object.Destroy(_star.gameObject);
		IsAnimating = false;
	}

	private void Setup(Car car, Mission mission)
	{
		_label = MetroLabel.Create(string.Empty).SetFont(MetroSkin.SmallFont);
		_label.SetAlignment(MetroAlign.Left);
		Add(_label);
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load(MetroSkin.StarCircle), Vector3.zero, Quaternion.identity);
		MetroIcon metroIcon = MetroIcon.Create(gameObject);
		metroIcon.SetAlignment(MetroAlign.Right);
		_label.Add(metroIcon);
		_starAnchor = gameObject.transform;
		if (mission != null)
		{
			string str = mission.Description + mission.GetRemainingValueBeforeCompletion(car);
			_label.SetText("  " + str);
			IsMissionCompleted = mission.Completed;
			if (IsMissionCompleted)
			{
				_star = MetroIcon.Create(MetroSkin.StarNoGlow);
				_star.transform.parent = _starAnchor;
				_star.transform.localPosition = new Vector3(0f, 0f, -0.2f);
			}
		}
	}
}
