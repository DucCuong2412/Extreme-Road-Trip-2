using System.Collections.Generic;
using UnityEngine;

public class CarStunt : MonoBehaviour
{
	private CarController _car;

	private Queue<StuntEvent> _stuntEvents;

	private Queue<StuntEvent> _bestStunt;

	private float _bestStuntScore;

	private bool _crashed;

	private bool _lastLandingActivatedMegaboost;

	private int _numberOfConsecutiveLanding;

	private StuntEvent _consecutiveLandingType;

	public string StuntText
	{
		get;
		set;
	}

	public void OnStuntEvent(StuntEvent stunt)
	{
		if (_crashed)
		{
			return;
		}
		_stuntEvents.Enqueue(stunt);
		if (!IsLandingStunt(stunt))
		{
			return;
		}
		if (_stuntEvents.Count > 1)
		{
			if (_consecutiveLandingType != stunt)
			{
				_numberOfConsecutiveLanding = 0;
			}
			_consecutiveLandingType = stunt;
			_numberOfConsecutiveLanding++;
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordNumberOfStunts(_car.Car, _stuntEvents.Count);
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordMostStunt(_car.Car, _stuntEvents.Count);
			AutoSingleton<GameStatsManager>.Instance.CurrentRun.RecordConsecutiveLanding(_car.Car, _numberOfConsecutiveLanding, stunt);
			AutoSingleton<AchievementsManager>.Instance.OnMostStuntRecorded(_stuntEvents.Count);
			AutoSingleton<AchievementsManager>.Instance.OnConsecutiveLanding(_numberOfConsecutiveLanding, stunt);
			CarBoost component = GetComponent<CarBoost>();
			float landStuntScore = component.GetLandStuntScore(_stuntEvents, stunt);
			component.OnLandStunt(landStuntScore);
			if (landStuntScore > _bestStuntScore)
			{
				_bestStuntScore = landStuntScore;
				Queue<StuntEvent> bestStunt = _bestStunt;
				_bestStunt = _stuntEvents;
				_stuntEvents = bestStunt;
			}
		}
		_stuntEvents.Clear();
	}

	public bool HasBestStunt()
	{
		return _bestStuntScore > 0f;
	}

	public Queue<StuntEvent> GetBestStunt()
	{
		return _bestStunt;
	}

	public static bool IsLandingStunt(StuntEvent stunt)
	{
		return stunt == StuntEvent.landing || stunt == StuntEvent.goodLanding || stunt == StuntEvent.greatLanding || stunt == StuntEvent.perfectLanding || stunt == StuntEvent.wheelieLanding || stunt == StuntEvent.perfectSlamLanding || stunt == StuntEvent.handsFreeLanding;
	}

	public static bool IsWheelieStunt(StuntEvent stunt)
	{
		return stunt == StuntEvent.backWheelie || stunt == StuntEvent.frontWheelie;
	}

	public void OnCrash()
	{
		_crashed = true;
	}

	public void Awake()
	{
		_crashed = false;
		_lastLandingActivatedMegaboost = false;
		_stuntEvents = new Queue<StuntEvent>();
		_bestStunt = new Queue<StuntEvent>();
		_bestStuntScore = 0f;
		_numberOfConsecutiveLanding = 0;
		_consecutiveLandingType = StuntEvent.none;
		CarBoost component = GetComponent<CarBoost>();
		if (component != null)
		{
			component.OnLandingMegaboostState += OnLandingMegaboostState;
		}
	}

	private void OnLandingMegaboostState(bool currentLandingActivateMegaboost)
	{
		if (currentLandingActivateMegaboost && _lastLandingActivatedMegaboost)
		{
			AutoSingleton<AchievementsManager>.Instance.CompleteAchievement(AchievementType.rocketship);
		}
		_lastLandingActivatedMegaboost = currentLandingActivateMegaboost;
	}

	public void Start()
	{
		_car = GetComponent<CarController>();
		_car.OnCrash += OnCrash;
		GetComponent<CarAnalyzer>().OnStuntEvent += OnStuntEvent;
	}

	public static string GetStuntDescription(StuntEvent stunt)
	{
		string s;
		switch (stunt)
		{
		case StuntEvent.frontWheelie:
			s = "Front Wheelie";
			break;
		case StuntEvent.backWheelie:
			s = "Back Wheelie";
			break;
		case StuntEvent.bigAir:
			s = "Big Air";
			break;
		case StuntEvent.hangTime:
			s = "Hang Time";
			break;
		case StuntEvent.longJump:
			s = "Long Jump";
			break;
		case StuntEvent.frontFlip:
			s = "Front Flip";
			break;
		case StuntEvent.backFlip:
			s = "Back Flip";
			break;
		case StuntEvent.perfectLanding:
			s = "Perfect\nLanding!";
			break;
		case StuntEvent.greatLanding:
			s = "Great Landing";
			break;
		case StuntEvent.goodLanding:
			s = "Good Landing";
			break;
		case StuntEvent.landing:
			s = "Sloppy Landing";
			break;
		case StuntEvent.wheelieLanding:
			s = "Wheelie Landing";
			break;
		case StuntEvent.closeCall:
			s = "Close Call";
			break;
		case StuntEvent.longSlam:
			s = "Long Slam";
			break;
		case StuntEvent.tumbler:
			s = "Tumbler";
			break;
		case StuntEvent.perfectSlamLanding:
			s = "Perfect Slam\nLanding!";
			break;
		case StuntEvent.handsFreeLanding:
			s = "Hands Free\nLanding!";
			break;
		default:
			s = "Unknown Stunt";
			break;
		}
		return s.Localize();
	}
}
