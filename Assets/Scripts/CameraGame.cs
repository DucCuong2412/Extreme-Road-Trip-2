using System;
using UnityEngine;

public class CameraGame : PrefabSingleton<CameraGame>
{
	private const float _refOrthographicSize = 10f;

	private const float _invRefOrthographicSize = 0.1f;

	private Transform _transform;

	private Transform _target;

	private Vector3 _lastTargetPosition;

	private Vector3 _cameraOffsetBack = new Vector3(0f, 0f, -10f);

	private float _screenRatio = 1.5f;

	private float _orthographicSize = 16f;

	private float _width = 48f;

	private float _height = 32f;

	private float _targetGroundLevel;

	private float _groundLevel;

	private float _cameraBoostLag;

	private float _cameraBoostTime = -1000f;

	private float _lag;

	private float _lagDuration;

	private float _shakeFrequency;

	private float _verticalShake;

	private float _timeShake;

	private Vector3 _keyboardMoveVelocity;

	public float ScreenWidth => _width;

	public float ScreenHeight => _height;

	public float HalfScreenWidth => _width * 0.5f;

	public float HalfScreenHeight => _height * 0.5f;

	public float GetZoomFactor()
	{
		return _orthographicSize * 0.1f;
	}

	public void Shake(float amount, float freq)
	{
		_timeShake = Time.time;
		_verticalShake = amount;
		_shakeFrequency = freq;
	}

	public void OnBoostEvent(float lag, float lagDuration)
	{
		_lag = lag;
		_lagDuration = lagDuration;
		_cameraBoostTime = Time.time;
	}

	public bool IsLeftOfScreen(float posX)
	{
		Vector3 position = _transform.position;
		return posX < position.x - HalfScreenWidth;
	}

	public bool IsRightOfScreen(float posX)
	{
		Vector3 position = _transform.position;
		return posX > position.x + HalfScreenWidth;
	}

	public bool IsAboveScreen(float posY)
	{
		Vector3 position = _transform.position;
		return posY > position.y + HalfScreenHeight;
	}

	public bool IsBelowScreen(float posY)
	{
		Vector3 position = _transform.position;
		return posY < position.y - HalfScreenHeight;
	}

	public bool IsOnScreen(float posX, float posY)
	{
		return !IsLeftOfScreen(posX) && !IsRightOfScreen(posX) && !IsAboveScreen(posY) && !IsBelowScreen(posY);
	}

	protected override void OnAwake()
	{
		_transform = base.transform;
		_screenRatio = (float)Screen.width / (float)Screen.height;
		float orthographicSize = 16f;
		if (Device.IsIPad())
		{
			orthographicSize = 19.2f;
		}
		SetOrthographicSize(orthographicSize);
		base.OnAwake();
	}

	private void SetOrthographicSize(float size)
	{
		base.GetComponent<Camera>().orthographicSize = size;
		_orthographicSize = size;
		_height = _orthographicSize * 2f;
		_width = _height * _screenRatio;
	}

	public void SetTarget(Transform target)
	{
		_target = target;
		_lastTargetPosition = target.position;
	}

	private void SetGroundLevel(float groundLevel)
	{
		_targetGroundLevel = groundLevel;
	}

	private void HandleKeyboardInput()
	{
		float num = 220f;
		Vector3 zero = Vector3.zero;
		if (UnityEngine.Input.GetKey(KeyCode.LeftArrow) || UnityEngine.Input.GetKey(KeyCode.A))
		{
			zero.x -= num;
		}
		if (UnityEngine.Input.GetKey(KeyCode.RightArrow) || UnityEngine.Input.GetKey(KeyCode.D))
		{
			zero.x += num;
		}
		if (UnityEngine.Input.GetKey(KeyCode.UpArrow) || UnityEngine.Input.GetKey(KeyCode.W))
		{
			zero.y += num * 0.3f;
		}
		if (UnityEngine.Input.GetKey(KeyCode.DownArrow) || UnityEngine.Input.GetKey(KeyCode.S))
		{
			zero.y -= num * 0.3f;
		}
		_keyboardMoveVelocity = Vector3.Lerp(_keyboardMoveVelocity, zero, 5f * Time.deltaTime);
		_transform.position += _keyboardMoveVelocity * Time.deltaTime;
		if (UnityEngine.Input.GetKey(KeyCode.KeypadPlus))
		{
			base.GetComponent<Camera>().orthographicSize -= 5f * Time.deltaTime;
		}
		if (UnityEngine.Input.GetKey(KeyCode.KeypadMinus))
		{
			base.GetComponent<Camera>().orthographicSize += 5f * Time.deltaTime;
		}
	}

	private void LateUpdate()
	{
		if (_target == null || AutoSingleton<PauseManager>.Instance.IsPaused() || Time.deltaTime <= float.Epsilon)
		{
			HandleKeyboardInput();
			return;
		}
		GroundManager instance = AutoSingleton<GroundManager>.Instance;
		Vector3 position = _transform.position;
		SetGroundLevel(instance.GetGroundHeightForCamera(position.x));
		_groundLevel = Mathf.Lerp(_groundLevel, _targetGroundLevel, 3f * Time.deltaTime);
		Vector3 position2 = _target.position;
		float y = position2.y;
		float num = HalfScreenWidth * 0.6f;
		float num2 = 0.1f * ScreenHeight;
		float num3 = 0.1f * ScreenHeight;
		float num4 = 0.2f * ScreenHeight;
		float a = y - _groundLevel + num2 + num3 + Mathf.InverseLerp(40f, 0f, y - _groundLevel) * num4;
		float f = (position2.x - _lastTargetPosition.x) / Time.deltaTime;
		float b = 16f * (1f + Mathf.InverseLerp(0f, 100f, Mathf.Abs(f)) * 1.25f);
		float num5 = Mathf.Max(a, b);
		if (num5 != ScreenHeight)
		{
			SetOrthographicSize(Mathf.Lerp(_orthographicSize, num5 * 0.5f, 6f * Time.deltaTime));
		}
		float num6 = Time.time - _cameraBoostTime;
		if (num6 < _lagDuration)
		{
			_cameraBoostLag = Mathf.Lerp(_cameraBoostLag, _lag, 4f * Time.deltaTime);
		}
		else
		{
			_cameraBoostLag = Mathf.Lerp(_lag, 0f, (num6 - _lagDuration) * 0.3f);
		}
		float x = position2.x + num + _cameraBoostLag;
		float y2 = _groundLevel + HalfScreenHeight - num2;
		Vector3 vector = new Vector3(x, y2, 0f);
		vector.y += Mathf.Cos((Time.time - _timeShake) * (float)Math.PI * 2f * _shakeFrequency) * _verticalShake * GetZoomFactor();
		_verticalShake = Mathf.Lerp(_verticalShake, 0f, 5f * Time.deltaTime);
		vector += _cameraOffsetBack;
		_transform.position = vector;
		_lastTargetPosition = position2;
	}
}
