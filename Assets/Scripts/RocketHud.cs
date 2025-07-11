using UnityEngine;

public class RocketHud : MetroMenuPage
{
	public Rocket _rocket;

	private bool _crashed;

	private bool _liftoff;

	private MetroLabel _distance;

	private MetroLabel _countdown;

	private float _timeStart;

	protected override void OnAwake()
	{
		_distance = MetroLabel.Create("Distance");
		_countdown = MetroLabel.Create("Countdown");
		_timeStart = Time.time;
	}

	protected override void OnMenuUpdate()
	{
		if (Time.time - _timeStart < 10f)
		{
			_distance.SetText(string.Empty);
			_countdown.SetText((10 - Mathf.RoundToInt(Time.time - _timeStart)).ToString());
		}
		else
		{
			if (!_liftoff)
			{
				_liftoff = true;
				_rocket.LiftOff();
			}
			_countdown.SetText(string.Empty);
			MetroLabel distance = _distance;
			Vector3 position = _rocket.transform.position;
			distance.SetText(Mathf.RoundToInt(position.y).ToString());
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
	}
}
