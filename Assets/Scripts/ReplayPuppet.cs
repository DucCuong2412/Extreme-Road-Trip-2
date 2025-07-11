using System;
using System.Collections;
using UnityEngine;

public class ReplayPuppet : MonoBehaviour
{
	private Replay _replay;

	private bool _stop;

	private Transform _transform;

	private Transform[] _wheels;

	private float _invWheelCircumference;

	public void Awake()
	{
		ReplayManager instance = AutoSingleton<ReplayManager>.Instance;
		instance.OnReplaysVisibilityChange = (Action<bool>)Delegate.Combine(instance.OnReplaysVisibilityChange, new Action<bool>(OnReplaysVisibilityChange));
		_transform = base.transform;
	}

	public void Start()
	{
		int layer = 0;
		base.gameObject.layer = layer;
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			transform.gameObject.layer = layer;
		}
		_wheels = new Transform[2];
		_wheels[0] = base.transform.Find("Pivot/WheelBack");
		_wheels[1] = base.transform.Find("Pivot/WheelFront");
		if (_wheels[0] == null || _wheels[1] == null)
		{
			UnityEngine.Debug.LogWarning("Giving up on rotating wheels on car: " + base.name);
			_wheels = null;
		}
		else
		{
			Vector3 extents = RendererBounds.ComputeBounds(_wheels[0]).extents;
			float x = extents.x;
			_invWheelCircumference = 1f / ((float)Math.PI * 2f * x);
		}
		PuppetLabel.Create(_transform, _replay.PlayerAlias());
		tk2dSprite[] componentsInChildren2 = GetComponentsInChildren<tk2dSprite>();
		foreach (tk2dSprite tk2dSprite in componentsInChildren2)
		{
			Color color = tk2dSprite.color;
			color = (tk2dSprite.color = color * 0.5f);
		}
	}

	public void Play()
	{
		StartCoroutine(PlayReplayCR());
	}

	public void Stop()
	{
		_stop = true;
	}

	private IEnumerator PlayReplayCR()
	{
		_stop = false;
		int len = _replay.GetLength();
		int rate = _replay.Rate();
		float invRate = 1f / (float)rate;
		int i = 1;
		float startingTime = Time.time;
		float time = startingTime;
		while (!_stop && i < len)
		{
			float prevFrametime = startingTime + (float)(i - 1) * invRate;
			float frameTime = startingTime + (float)i * invRate;
			ReplayFrame prevFrame = _replay.GetFrame(i - 1);
			ReplayFrame frame = _replay.GetFrame(i);
			Vector3 prevPosition = prevFrame.Position();
			Quaternion prevRotation = prevFrame.Rotation();
			Vector3 position = frame.Position();
			Quaternion rotation = frame.Rotation();
			for (; time < frameTime; time += Time.deltaTime)
			{
				float x = Mathf.InverseLerp(prevFrametime, frameTime, time);
				Vector3 p = Vector3.Lerp(prevPosition, position, x);
				Quaternion r = Quaternion.Lerp(prevRotation, rotation, x);
				if (_wheels != null)
				{
					float x2 = p.x;
					Vector3 position2 = base.transform.position;
					float dx = x2 - position2.x;
					Transform[] wheels = _wheels;
					foreach (Transform w in wheels)
					{
						w.Rotate(0f, 0f, dx * _invWheelCircumference * -360f);
					}
				}
				_transform.position = p;
				_transform.rotation = r;
				yield return null;
			}
			i++;
		}
		if (!_stop && i >= len)
		{
			CameraGame instance = PrefabSingleton<CameraGame>.Instance;
			Vector3 position3 = _transform.position;
			float x3 = position3.x;
			Vector3 position4 = _transform.position;
			bool shakeAndSound = instance.IsOnScreen(x3, position4.y);
			AutoSingleton<ExplosionManager>.Instance.Explode(_transform.position, shakeAndSound, shakeAndSound);
		}
		Destroy();
	}

	private void Destroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (AutoSingleton<ReplayManager>.IsCreated())
		{
			ReplayManager instance = AutoSingleton<ReplayManager>.Instance;
			instance.OnReplaysVisibilityChange = (Action<bool>)Delegate.Remove(instance.OnReplaysVisibilityChange, new Action<bool>(OnReplaysVisibilityChange));
		}
	}

	private void OnReplaysVisibilityChange(bool visible)
	{
		if (!visible)
		{
			StopAllCoroutines();
			base.gameObject.SetActive(value: false);
		}
	}

	public static ReplayPuppet Create(Replay replay)
	{
		Car car = replay.Car();
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(car.VisualPrefab, Vector3.zero, Quaternion.identity);
		if (gameObject != null)
		{
			Transform transform = gameObject.transform;
			ReplayPuppet replayPuppet = transform.gameObject.AddComponent<ReplayPuppet>();
			replayPuppet._replay = replay;
			return replayPuppet;
		}
		return null;
	}
}
