using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class CarAnalyzer : MonoBehaviour
{
	private enum State
	{
		none,
		bothOnGround,
		backOnGround,
		frontOnGround,
		airborne
	}

	public delegate void StuntEventHandler(StuntEvent stuntEvent);

	private State _state;

	private State _prevState;

	private State _nextState;

	private CarController _car;

	private Transform _transform;

	private CarWheel _frontWheel;

	private bool _frontWheelTouching;

	private CarWheel _backWheel;

	private bool _backWheelTouching;

	private float _stateTime;

	private float _prevStateTime;

	private float _airborneStartOffset;

	private float _airborneStartHeight;

	private float _closeCallDistance;

	private float _slamStartHeight;

	private float _handsFreeDelay;

	private float _boostStartOffset;

	private float _onGroundStartOffset;

	private float _handsfreeOffset;

	private bool _longJump;

	private bool _hangTime;

	private bool _bigAir;

	private bool _wheelie;

	private bool _slamming;

	private bool _isOnBoost;

	private bool _isOnGround;

	private bool _isUpsideDown;

	private StuntEvent _previousFlipStunt;

	private int _numberOfFlipInAJumpCounter;

	private int _halfFlipCounter;

	private int _tumblerCounter;

	private int _closeCallCounter;

	private float _lastAngle;

	private float _startingFlipAngle;

	private float _lastDirection;

	private float _upsideDownStartOffset;

	private float _leftieStartOffset;

	private StuntEvent _previousWheelieStunt;

	private float _wheelieStartOffset;

	private float _wheelieDistance;

	private float _wheelieTriggerTime;

	private GameStats _gameStats;

	public int distance;

	[method: MethodImpl(32)]
	public event StuntEventHandler OnStuntEvent;

	private void HandleStuntEvent(StuntEvent stuntEvent)
	{
		AnalyzeStunt(stuntEvent);
		if (this.OnStuntEvent != null)
		{
			this.OnStuntEvent(stuntEvent);
		}
	}

	private void Awake()
	{
		_transform = base.transform;
		_car = GetComponent<CarController>();
		_frontWheel = _car._frontWheel.GetComponent<CarWheel>();
		_backWheel = _car._backWheel.GetComponent<CarWheel>();
		_state = State.none;
		_prevState = State.none;
		_nextState = State.none;
		_tumblerCounter = 0;
		_closeCallCounter = 0;
		_numberOfFlipInAJumpCounter = 0;
		_boostStartOffset = 0f;
		_isOnBoost = false;
		_previousWheelieStunt = StuntEvent.none;
		_wheelieStartOffset = 0f;
		_wheelieDistance = 0f;
		_wheelieTriggerTime = 0f;
		_leftieStartOffset = 0f;
		_gameStats = AutoSingleton<GameStatsManager>.Instance.CurrentRun;
	}

	private void FixedUpdate()
	{

		if (_car.IsCrashed())
		{
			return;
		}
		GameStats gameStats = _gameStats;
		Car car = _car.Car;
		Vector3 position = _car.Position;
		gameStats.RecordMaxDistance(car, position.x);
		distance=Mathf.CeilToInt(transform.position.x);	
        Debug.Log("Distance: " + distance);	
        if (_car.IsInSetup)
		{
			Vector3 position2 = _car.Position;
			_leftieStartOffset = position2.x;
			Vector3 position3 = _car.Position;
			_handsfreeOffset = position3.x;
			Vector3 position4 = _car.Position;
			_airborneStartOffset = position4.x;
			return;
		}
		if (_car.Input.HasTiltedRight)
		{
			Vector3 position5 = _car.Position;
			_leftieStartOffset = position5.x;
			_car.Input.HasTiltedRight = false;
		}
		else
		{
			AchievementsManager instance = AutoSingleton<AchievementsManager>.Instance;
			Vector3 position6 = _car.Position;
			instance.CheckLeftieAchievement(position6.x - _leftieStartOffset);
		}
		if (_car.Input.Tilt == 0f && !_car.Input.Slam)
		{
			if (_handsfreeOffset == 0f)
			{
				Vector3 position7 = _car.Position;
				_handsfreeOffset = position7.x;
			}
			AchievementsManager instance2 = AutoSingleton<AchievementsManager>.Instance;
			Vector3 position8 = _car.Position;
			instance2.CheckHandsfreeMaxDistance(position8.x - _handsfreeOffset);
		}
		else
		{
			_handsfreeOffset = 0f;
		}
		if (_backWheelTouching != _backWheel.IsOnGround())
		{
			_backWheelTouching = !_backWheelTouching;
		}
		if (_frontWheelTouching != _frontWheel.IsOnGround())
		{
			_frontWheelTouching = !_frontWheelTouching;
		}
		bool flag = _backWheelTouching && _frontWheelTouching;
		bool flag2 = _backWheelTouching || _frontWheelTouching;
		if (flag)
		{
			_nextState = State.bothOnGround;
		}
		else if (!flag2)
		{
			_nextState = State.airborne;
		}
		else if (!_frontWheelTouching)
		{
			_nextState = State.backOnGround;
		}
		else
		{
			_nextState = State.frontOnGround;
		}
		bool flag3 = false;
		if (_nextState != 0 && _nextState != _state)
		{
			_prevState = _state;
			_state = _nextState;
			_nextState = State.none;
			_prevStateTime = _stateTime;
			_stateTime = Time.time;
			flag3 = true;
		}
		if (_prevState != State.airborne || flag3)
		{
		}
		switch (_state)
		{
		case State.bothOnGround:
		{
			if (!flag3)
			{
				break;
			}
			CheckForLongestJump();
			StopWheelie();
			StartOnGroundMeter();
			float num3 = _stateTime - _prevStateTime;
			if (_prevState == State.airborne || ((_prevState == State.backOnGround || _prevState == State.frontOnGround) && num3 < 0.02f))
			{
				if (_handsFreeDelay >= 0.5f)
				{
					HandleStuntEvent(StuntEvent.handsFreeLanding);
				}
				else
				{
					HandleStuntEvent((!_slamming) ? StuntEvent.perfectLanding : StuntEvent.perfectSlamLanding);
				}
			}
			else if (_prevState == State.backOnGround || _prevState == State.frontOnGround)
			{
				if (num3 < 0.06f)
				{
					HandleStuntEvent(StuntEvent.greatLanding);
				}
				else if (num3 < 0.09f)
				{
					HandleStuntEvent(StuntEvent.goodLanding);
				}
				else if (num3 < 1f)
				{
					HandleStuntEvent(StuntEvent.landing);
				}
				else
				{
					HandleStuntEvent(StuntEvent.wheelieLanding);
				}
			}
			break;
		}
		case State.backOnGround:
			if (flag3)
			{
				CheckForLongestJump();
				StartOnGroundMeter();
			}
			UpdateWheelie(StuntEvent.backWheelie);
			break;
		case State.frontOnGround:
			if (flag3)
			{
				CheckForLongestJump();
				StartOnGroundMeter();
			}
			UpdateWheelie(StuntEvent.frontWheelie);
			break;
		case State.airborne:
		{
			if (flag3)
			{
				_halfFlipCounter = 0;
				_lastDirection = 0f;
				Vector3 position9 = _car.Position;
				_airborneStartOffset = position9.x;
				Vector3 position10 = _car.Position;
				_airborneStartHeight = position10.y;
				Vector3 position11 = _car.Position;
				_closeCallDistance = position11.x;
				_slamStartHeight = 0f;
				_handsFreeDelay = 0f;
				_longJump = false;
				_hangTime = false;
				_bigAir = false;
				_slamming = false;
				_previousFlipStunt = StuntEvent.none;
				StopOnGroundMeter();
			}
			GameStats gameStats2 = _gameStats;
			Car car2 = _car.Car;
			Vector3 position12 = _car.Position;
			gameStats2.RecordHeight(car2, position12.y - _airborneStartHeight);
			float num = Time.time - _stateTime;
			if (!_hangTime && num > 2f)
			{
				HandleStuntEvent(StuntEvent.hangTime);
				_hangTime = true;
			}
			if (!_bigAir)
			{
				Vector3 position13 = _car.Position;
				if (position13.y > 20f + _airborneStartHeight)
				{
					HandleStuntEvent(StuntEvent.bigAir);
					_bigAir = true;
				}
			}
			if (!_longJump)
			{
				Vector3 position14 = _car.Position;
				if (position14.x - _airborneStartOffset > 100f)
				{
					HandleStuntEvent(StuntEvent.longJump);
					_longJump = true;
				}
			}
			if (_car.Slamming)
			{
				if (!_slamming)
				{
					Vector3 position15 = _car.Position;
					_slamStartHeight = position15.y;
					_slamming = true;
				}
				if (_isUpsideDown)
				{
					_gameStats.RecordUpsideDownSlam();
				}
				float slamStartHeight = _slamStartHeight;
				Vector3 position16 = _car.Position;
				if (slamStartHeight - position16.y >= 25f)
				{
					Vector3 position17 = _car.Position;
					_slamStartHeight = position17.y;
					HandleStuntEvent(StuntEvent.longSlam);
				}
			}
			else
			{
				_slamming = false;
			}
			if (_car.Input.Tilt == 0f && !_car.Input.Slam)
			{
				_handsFreeDelay += Time.deltaTime;
			}
			else
			{
				_handsFreeDelay = 0f;
			}
			Vector3 eulerAngles = _transform.eulerAngles;
			float z = eulerAngles.z;
			float num2 = Mathf.Sign(Mathf.DeltaAngle(_lastAngle, z));
			if (z >= 100f && z <= 260f)
			{
				Vector3 position18 = _car.Position;
				if (position18.x - _closeCallDistance > 20f)
				{
					Vector3 velocity = _car.Velocity;
					if (velocity.y <= 5f)
					{
						Vector3 position19 = _car.Position;
						float y = position19.y;
						GroundManager instance3 = AutoSingleton<GroundManager>.Instance;
						Vector3 position20 = _car.Position;
						if (y - instance3.GetGroundHeight(position20.x) <= 6f)
						{
							Vector3 position21 = _car.Position;
							_closeCallDistance = position21.x;
							HandleStuntEvent(StuntEvent.closeCall);
						}
					}
				}
			}
			if (z >= 135f && z <= 225f)
			{
				if (!_isUpsideDown)
				{
					Vector3 position22 = _car.Position;
					_upsideDownStartOffset = position22.x;
					_isUpsideDown = true;
				}
				GameStats gameStats3 = _gameStats;
				Vector3 position23 = _car.Position;
				gameStats3.RecordUpsideDownDistance(position23.x - _upsideDownStartOffset);
			}
			else if (_isUpsideDown)
			{
				_isUpsideDown = false;
				_upsideDownStartOffset = 0f;
			}
			if (num2 != _lastDirection)
			{
				_startingFlipAngle = z + 90f * num2;
				_halfFlipCounter = 0;
			}
			else if (Mathf.DeltaAngle(_startingFlipAngle, z) * Mathf.DeltaAngle(_startingFlipAngle, _lastAngle) < 0f)
			{
				_halfFlipCounter++;
				if (_halfFlipCounter % 2 == 0)
				{
					_numberOfFlipInAJumpCounter++;
					if (Mathf.Sign(num2) > 0f)
					{
						HandleStuntEvent(StuntEvent.backFlip);
						if (_previousFlipStunt == StuntEvent.frontFlip)
						{
							HandleStuntEvent(StuntEvent.tumbler);
						}
						_previousFlipStunt = StuntEvent.backFlip;
					}
					else
					{
						HandleStuntEvent(StuntEvent.frontFlip);
						if (_previousFlipStunt == StuntEvent.backFlip)
						{
							HandleStuntEvent(StuntEvent.tumbler);
						}
						_previousFlipStunt = StuntEvent.frontFlip;
					}
				}
			}
			_lastAngle = z;
			_lastDirection = num2;
			break;
		}
		}
		CheckBoost();
	}

	private void AnalyzeStunt(StuntEvent stuntEvent)
	{
		switch (stuntEvent)
		{
		case StuntEvent.tumbler:
			_tumblerCounter++;
			break;
		case StuntEvent.closeCall:
			_closeCallCounter++;
			break;
		}
		if (CarStunt.IsLandingStunt(stuntEvent))
		{
			OnCarLanding(stuntEvent);
		}
	}

	private void OnCarLanding(StuntEvent stuntEvent)
	{
		switch (stuntEvent)
		{
		case StuntEvent.perfectLanding:
			_gameStats.RecordPerfectLanding(_car.Car, 1);
			break;
		case StuntEvent.perfectSlamLanding:
			_gameStats.RecordPerfectSlamLanding(_car.Car, 1);
			break;
		}
		_gameStats.RecordMostFlip(_car.Car, _numberOfFlipInAJumpCounter);
		AutoSingleton<AchievementsManager>.Instance.CheckTumblerAchievements(_tumblerCounter);
		AutoSingleton<AchievementsManager>.Instance.CheckCloseCallAchievements(_closeCallCounter);
		AutoSingleton<AchievementsManager>.Instance.CheckFlipAchievements(_numberOfFlipInAJumpCounter);
		_tumblerCounter = 0;
		_closeCallCounter = 0;
		_numberOfFlipInAJumpCounter = 0;
	}

	private void CheckBoost()
	{
		if (_car.CarBoost.IsBoosting() && !_isOnBoost)
		{
			Vector3 position = _car.Position;
			_boostStartOffset = position.x;
		}
		else if (!_car.CarBoost.IsBoosting() && _isOnBoost)
		{
			GameStats gameStats = _gameStats;
			Car car = _car.Car;
			Vector3 position2 = _car.Position;
			gameStats.RecordTotalDistanceOnBoost(car, position2.x - _boostStartOffset);
			_boostStartOffset = 0f;
		}
		_isOnBoost = _car.CarBoost.IsBoosting();
	}

	private void CheckForLongestJump()
	{
		if (_prevState == State.airborne)
		{
			Vector3 position = _car.Position;
			float length = position.x - _airborneStartOffset;
			_gameStats.RecordJumpLength(_car.Car, length);
			_gameStats.RecordTotalJumpLength(_car.Car, length);
		}
	}

	private void UpdateWheelie(StuntEvent wheelieEvent)
	{
		if (_wheelie)
		{
			if (_prevState == State.airborne)
			{
				if (wheelieEvent != _previousWheelieStunt)
				{
					StopWheelie();
				}
				Vector3 position = _car.Position;
				_wheelieStartOffset = position.x;
			}
			else
			{
				float wheelieDistance = _wheelieDistance;
				Vector3 position2 = _car.Position;
				_wheelieDistance = wheelieDistance + (position2.x - _wheelieStartOffset);
			}
		}
		else
		{
			_wheelieTriggerTime += Time.fixedDeltaTime;
			if (_wheelieTriggerTime > 1f)
			{
				StartWheelie(wheelieEvent);
				HandleStuntEvent(wheelieEvent);
			}
		}
	}

	private void StartWheelie(StuntEvent wheelieEvent)
	{
		_wheelie = true;
		Vector3 position = _car.Position;
		_wheelieStartOffset = position.x;
		_wheelieDistance = 0f;
		_wheelieTriggerTime = 0f;
		_previousWheelieStunt = wheelieEvent;
	}

	private void StopWheelie()
	{
		if (_wheelie)
		{
			_gameStats.RecordTotalDistanceOnWheelie(_car.Car, _wheelieDistance);
		}
		_wheelie = false;
		_wheelieStartOffset = 0f;
		_wheelieDistance = 0f;
		_wheelieTriggerTime = 0f;
		_previousWheelieStunt = StuntEvent.none;
	}

	private void StartOnGroundMeter()
	{
		if (!_isOnGround)
		{
			_isOnGround = true;
			Vector3 position = _car.Position;
			_onGroundStartOffset = position.x;
		}
	}

	private void StopOnGroundMeter()
	{
		if (_isOnGround)
		{
			_isOnGround = false;
			Vector3 position = _car.Position;
			float distance = position.x - _onGroundStartOffset;
			_gameStats.RecordTotalDistanceOnGround(_car.Car, distance);
			_gameStats.RecordMaxDistanceOnGround(_car.Car, distance);
		}
	}
}
