using UnityEngine;

public class RotationDisplayer : MonoBehaviour
{
	private CarController _car;

	private CarInputController _input;

	private float _startingScale;

	private float _scale;

	private float _targetScale;

	private float _timeStartTilt;

	private float _lastTilt;

	private Material _material;

	private Color _color;

	public void Start()
	{
		_car = Singleton<GameManager>.Instance.Car;
		_input = _car.GetComponent<CarInputController>();
		Vector3 localScale = base.transform.localScale;
		_startingScale = localScale.x;
		_material = GetComponent<Renderer>().material;
		_color = new Color(1f, 1f, 1f, 1f);
	}

	private void LateUpdate()
	{
		float tilt = _input.Tilt;
		if (tilt == 0f)
		{
			_targetScale = 0.3f;
			_scale = Mathf.Lerp(_scale, _targetScale, 8f * Time.deltaTime);
			if (_scale < 0.4f)
			{
				GetComponent<Renderer>().enabled = false;
			}
		}
		else
		{
			if (_lastTilt == 0f)
			{
				_timeStartTilt = Time.time;
			}
			_targetScale = 1f;
			_scale = Mathfx.Berp(_scale, _targetScale, (Time.time - _timeStartTilt) * 0.4f);
			if (_scale > 0.4f)
			{
				GetComponent<Renderer>().enabled = true;
			}
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.z += tilt * 360f * Time.deltaTime;
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
		base.transform.position = _car.Position + new Vector3(0f, 0f, 5f);
		base.transform.localScale = new Vector3(Mathf.Sign(tilt) * _startingScale * _scale, _startingScale * _scale, 1f);
		int maxDistance = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetMaxDistance(_car.Car);
		float to = 0.3f * Mathf.InverseLerp(400f, 300f, maxDistance);
		_color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, to, _scale));
		_material.color = _color;
		_lastTilt = tilt;
		if ((float)maxDistance > 400f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
