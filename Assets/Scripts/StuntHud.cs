using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntHud : MonoBehaviour
{
	private bool _isLocked;

	private MetroLabel _stuntText;

	private Transform _stuntTextPivot;

	private MetroLabel _landingText;

	private Transform _landingTextPivot;

	private Queue<StuntEvent> _stuntEvents;

	private bool _showtimeMode;

	public bool IsAnimating
	{
		get;
		private set;
	}

	public void OnStuntEvent(StuntEvent stunt)
	{
		_stuntEvents.Enqueue(stunt);
		RefreshStuntText(stunt);
		if (CarStunt.IsLandingStunt(stunt))
		{
			_stuntEvents.Clear();
			if (!_showtimeMode)
			{
				StartCoroutine(BerpOut(1f));
			}
		}
	}

	public StuntHud Setup()
	{
		Singleton<GameManager>.Instance.OnGameEnded += OnGameEnded;
		Singleton<GameManager>.Instance.OnCarControllerChanged += OnCarControllerChanged;
		OnCarControllerChanged(Singleton<GameManager>.Instance.Car);
		IsAnimating = true;
		_showtimeMode = false;
		return this;
	}

	private void OnGameEnded()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnCarControllerChanged(CarController car)
	{
		car.GetComponent<CarAnalyzer>().OnStuntEvent += OnStuntEvent;
	}

	public void StartAnim(Queue<StuntEvent> bestStunt)
	{
		_showtimeMode = true;
		StartCoroutine(ReplayBestStuntCR(bestStunt));
	}

	public void StopAnim()
	{
		IsAnimating = false;
	}

	private IEnumerator ReplayBestStuntCR(Queue<StuntEvent> bestStunt)
	{
		IsAnimating = true;
		if (bestStunt != null)
		{
			foreach (StuntEvent stunt in bestStunt)
			{
				OnStuntEvent(stunt);
				Duration delay = new Duration(0.25f);
				while ((!delay.IsDone() && IsAnimating) || (_isLocked && CarStunt.IsLandingStunt(stunt)))
				{
					yield return null;
				}
			}
		}
		IsAnimating = false;
	}

	public void Awake()
	{
		_stuntEvents = new Queue<StuntEvent>();
		_stuntText = MetroLabel.Create("Stunt Text");
		_stuntTextPivot = new GameObject("Stunt Text Pivot").transform;
		_stuntText.transform.parent = _stuntTextPivot;
		_stuntText.SetColor(MetroSkin.StuntTextColor);
		_stuntText.AddOutline();
		_stuntText.SetLineSpacing(0f);
		_stuntText.SetText(string.Empty);
		_landingText = MetroLabel.Create("Landing Text");
		_landingTextPivot = new GameObject("Landing Text Pivot").transform;
		_landingText.transform.parent = _landingTextPivot;
		_landingText.SetFont(MetroSkin.BigFont);
		_landingText.SetColor(MetroSkin.LandingTextColor);
		_landingText.AddOutline();
		_landingText.SetLineSpacing(0f);
		_landingText.SetText(string.Empty);
	}

	public void Start()
	{
		_landingTextPivot.transform.rotation = Quaternion.Euler(0f, 0f, 10f);
		_stuntTextPivot.parent = base.transform;
		_landingTextPivot.parent = base.transform;
		if (!_showtimeMode)
		{
			_stuntTextPivot.transform.position = new Vector3(PrefabSingleton<CameraGUI>.Instance.HalfScreenWidth * 0.5f, 0f, -0.09f);
			_landingTextPivot.transform.position = new Vector3(PrefabSingleton<CameraGUI>.Instance.HalfScreenWidth * 0.5f, 0f, -0.1f);
		}
		else
		{
			_stuntTextPivot.transform.localPosition = new Vector3(0f, 0f, -0.09f);
			_landingTextPivot.transform.localPosition = new Vector3(0f, 0f, -0.1f);
		}
	}

	public void RefreshStuntText(StuntEvent newEvent)
	{
		if (_stuntEvents == null)
		{
			return;
		}
		int count = _stuntEvents.Count;
		if (CarStunt.IsLandingStunt(newEvent))
		{
			if (count > 1)
			{
				string stuntDescription = CarStunt.GetStuntDescription(newEvent);
				StartCoroutine(BerpIn(_landingText, _landingTextPivot, stuntDescription, isLanding: true));
			}
		}
		else
		{
			string[] array = new string[count];
			int num = 0;
			foreach (StuntEvent stuntEvent in _stuntEvents)
			{
				array[num] = CarStunt.GetStuntDescription(stuntEvent);
				num++;
			}
			string text = string.Join(" + ", array);
			string text2 = StringWrapper.WrapString(text, 30, " +");
			StartCoroutine(BerpIn(_stuntText, _stuntTextPivot, text2, isLanding: false));
		}
	}

	private IEnumerator Lock()
	{
		while (_isLocked)
		{
			yield return null;
		}
		_isLocked = true;
	}

	private void Unlock()
	{
		_isLocked = false;
	}

	private IEnumerator BerpIn(MetroLabel label, Transform pivot, string text, bool isLanding)
	{
		yield return StartCoroutine(Lock());
		float fromScale = (!isLanding) ? 0.1f : 2f;
		float toScale = 1f;
		float time = 0.3f;
		label.SetText(text);
		if ((!isLanding || !_showtimeMode) && IsAnimating)
		{
			PrefabSingleton<GameSoundManager>.Instance.PlayFlashSound();
		}
		else
		{
			time = 0.2f;
		}
		Duration delay = new Duration(time);
		bool stomped = false;
		while (!delay.IsDone() && IsAnimating)
		{
			float scale = Mathfx.Berp(fromScale, toScale, delay.Value01());
			pivot.localScale = Vector3.one * scale;
			if (!stomped && delay.Value01() > 0.5f)
			{
				if (isLanding && _showtimeMode)
				{
					PrefabSingleton<GameSoundManager>.Instance.PlayStompSound();
					PrefabSingleton<CameraGUI>.Instance.Shake();
				}
				stomped = true;
			}
			yield return null;
		}
		pivot.localScale = Vector3.one * toScale;
		Unlock();
	}

	private IEnumerator BerpOut(float wait)
	{
		Duration delay2 = new Duration(wait);
		while (!delay2.IsDone() && IsAnimating)
		{
			yield return null;
		}
		yield return StartCoroutine(Lock());
		delay2 = new Duration(0.15f);
		float finalScale = 0.1f;
		while (!delay2.IsDone() && IsAnimating)
		{
			float scale = Mathf.Lerp(1f, finalScale, delay2.Value01());
			_stuntTextPivot.localScale = Vector3.one * scale;
			_landingTextPivot.localScale = Vector3.one * scale;
			yield return null;
		}
		_stuntTextPivot.localScale = Vector3.one * finalScale;
		_landingTextPivot.localScale = Vector3.one * finalScale;
		_stuntText.SetText(string.Empty);
		_landingText.SetText(string.Empty);
		Unlock();
	}
}
