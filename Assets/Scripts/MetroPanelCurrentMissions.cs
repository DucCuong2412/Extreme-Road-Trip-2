using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MetroPanelCurrentMissions : MetroWidget
{
	private bool _showtimeMode;

	private bool _prestigeLevelUp;

	private int _progress;

	private List<MetroPanelMission> _missionPanels;

	private MetroLayout _missionsLayout;

	private MetroPanelMissionsProgress _progressLayout;

	public bool IsAnimating
	{
		get;
		private set;
	}

	public bool IsPaused
	{
		get;
		private set;
	}

	[method: MethodImpl(32)]
	public event Action OnProgressFull;

	[method: MethodImpl(32)]
	public event Action OnAnimationEnd;

	public static MetroPanelCurrentMissions Create(Car car, List<Mission> missions, int progress, bool showtimeMode = false)
	{
		GameObject gameObject = new GameObject(typeof(MetroPanelCurrentMissions).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		MetroPanelCurrentMissions metroPanelCurrentMissions = gameObject.AddComponent<MetroPanelCurrentMissions>();
		metroPanelCurrentMissions.Setup(car, missions, progress, showtimeMode);
		return metroPanelCurrentMissions;
	}

	public void StartAnim(Car car, List<Mission> newMissions)
	{
		IsPaused = false;
		StartCoroutine(ShowMissionsProgressCR(car, newMissions));
	}

	public void ResumeAnim()
	{
		IsPaused = false;
		IsAnimating = true;
	}

	public void StopAnim()
	{
		IsAnimating = false;
	}

	private IEnumerator ShowMissionsProgressCR(Car car, List<Mission> newMissions)
	{
		IsAnimating = true;
		int completedMissionCount = 0;
		foreach (MetroPanelMission p2 in _missionPanels)
		{
			if (p2.IsMissionCompleted)
			{
				completedMissionCount++;
				p2.ShowIsCompleted(completedMissionCount);
				while (p2.IsAnimating && IsAnimating)
				{
					yield return null;
				}
				p2.StopAnim();
				MetroIcon flyingStar = MetroIcon.Create(MetroSkin.StarWithGlow);
				Duration delay = new Duration(0.35f);
				while (!delay.IsDone() && IsAnimating)
				{
					float x = Mathfx.Hermite(0f, 1f, delay.Value01());
					flyingStar.transform.localPosition = Vector3.Lerp(p2.GetStarAnchorPos(), _progressLayout.GetNextStepAnchorPos() + Vector3.back, x);
					yield return null;
				}
				UnityEngine.Object.Destroy(flyingStar.gameObject);
				_progressLayout.ShowProgressAnim();
				while (_progressLayout.IsAnimating && IsAnimating)
				{
					yield return null;
				}
				_progressLayout.StopAnim();
				if (_progressLayout.IsProgressFull())
				{
					_progressLayout.ShowProgressFullAnim();
					while (_progressLayout.IsAnimating && IsAnimating)
					{
						yield return null;
					}
					_progressLayout.StopAnim();
					if (this.OnProgressFull != null)
					{
						this.OnProgressFull();
					}
					IsPaused = true;
					while (IsPaused)
					{
						yield return null;
					}
				}
			}
		}
		bool allComplete = (newMissions == null || newMissions.Count == 0) && (_missionPanels.Count == 0 || _missionPanels.TrueForAll((MetroPanelMission p) => p.IsMissionCompleted));
		foreach (MetroPanelMission p3 in _missionPanels)
		{
			if (p3.IsMissionCompleted)
			{
				MetroPanelMission metroPanelMission = p3;
				Vector3 localPosition = p3.transform.localPosition;
				Vector3 from = new Vector3(0f, localPosition.y);
				float width = _zone.width;
				Vector3 localPosition2 = p3.transform.localPosition;
				metroPanelMission.Move(from, new Vector3(width, localPosition2.y));
				while (p3.IsAnimating && IsAnimating)
				{
					yield return null;
				}
				p3.StopAnim();
			}
			yield return null;
			if ((p3.IsMissionCompleted || _prestigeLevelUp) && newMissions != null && newMissions.Count > 0)
			{
				Mission i = newMissions[0];
				p3.UpdateMission(car, i);
				newMissions.RemoveAt(0);
				MetroPanelMission metroPanelMission2 = p3;
				float width2 = _zone.width;
				Vector3 localPosition3 = p3.transform.localPosition;
				Vector3 from2 = new Vector3(width2, localPosition3.y);
				Vector3 localPosition4 = p3.transform.localPosition;
				metroPanelMission2.Move(from2, new Vector3(0f, localPosition4.y));
				while (p3.IsAnimating && IsAnimating)
				{
					yield return null;
				}
				p3.StopAnim();
			}
		}
		if (allComplete)
		{
			AddMissionsCompleteLabel();
		}
		_prestigeLevelUp = false;
		IsAnimating = false;
		this.OnAnimationEnd();
	}

	public void OnPrestigeActivated(Car car, int maxNbCurrentMissions)
	{
		_missionsLayout.Clear();
		_missionPanels.Clear();
		for (int i = 0; i < maxNbCurrentMissions; i++)
		{
			MetroPanelMission metroPanelMission = MetroPanelMission.Create(car);
			metroPanelMission.AddSlice9Background(MetroSkin.Slice9RoundedSemiTransparent);
			_missionPanels.Add(metroPanelMission);
			_missionsLayout.Add(metroPanelMission);
			metroPanelMission.transform.Translate(_zone.width, 0f, 0f);
		}
		_progressLayout = MetroPanelMissionsProgress.Create(_progress);
		_missionsLayout.Add(_progressLayout);
		Reflow();
		_prestigeLevelUp = true;
	}

	private void AddMissionsCompleteLabel()
	{
		foreach (MetroPanelMission missionPanel in _missionPanels)
		{
			_missionsLayout.Remove(missionPanel);
		}
		_missionPanels.Clear();
		MetroLabel metroLabel = MetroLabel.Create("COMPLETE").SetFont(MetroSkin.BigFont);
		metroLabel.SetColor(MetroSkin.LandingTextColor);
		metroLabel.SetAlignment(MetroAlign.Center);
		metroLabel.transform.rotation = Quaternion.Euler(0f, 0f, 10f);
		_missionsLayout.Add(MetroSpacer.Create(2f));
		_missionsLayout.Add(metroLabel);
		_missionsLayout.Add(MetroSpacer.Create(2f));
		Reflow();
	}

	private void Setup(Car car, List<Mission> missions, int progress, bool showtimeMode)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		_missionsLayout = MetroLayout.Create(Direction.vertical);
		_missionsLayout.SetMass(4f);
		metroLayout.Add(_missionsLayout);
		_missionPanels = new List<MetroPanelMission>();
		_progress = progress;
		_showtimeMode = showtimeMode;
		if (!_showtimeMode && (missions == null || missions.Count == 0))
		{
			AddMissionsCompleteLabel();
		}
		else
		{
			foreach (Mission mission in missions)
			{
				MetroPanelMission metroPanelMission = MetroPanelMission.Create(car, mission);
				metroPanelMission.AddSlice9Background(MetroSkin.Slice9RoundedSemiTransparent);
				_missionPanels.Add(metroPanelMission);
				_missionsLayout.Add(metroPanelMission);
			}
			_progressLayout = MetroPanelMissionsProgress.Create(progress);
			_missionsLayout.Add(_progressLayout);
		}
		metroLayout.Add(MetroSpacer.Create(0.2f));
	}
}
