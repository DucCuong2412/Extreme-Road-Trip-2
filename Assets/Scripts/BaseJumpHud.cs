using UnityEngine;

public class BaseJumpHud : MetroMenuPage
{
	public BaseJumper _baseJumper;

	private bool _crashed;

	private MetroLabel _distance;

	private MetroLabel _retry;

	protected override void OnAwake()
	{
		_distance = MetroLabel.Create("Distance");
		_retry = MetroLabel.Create("Retry");
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		metroLayout.Add(_distance);
		metroLayout.Add(_retry);
		Add(metroLayout);
	}

	protected override void OnMenuUpdate()
	{
		if (!_baseJumper.IsCrashed())
		{
			_retry.SetText(string.Empty);
			MetroLabel distance = _distance;
			Vector3 position = _baseJumper.transform.position;
			distance.SetText(Mathf.RoundToInt(Mathf.Abs(position.y)).ToString() + "m");
		}
		else if (!_crashed)
		{
			_crashed = true;
			_retry.SetText("Press Spacebar to retry");
			MetroLabel distance2 = _distance;
			Vector3 position2 = _baseJumper.transform.position;
			distance2.SetText("Crashed: " + Mathf.RoundToInt(Mathf.Abs(position2.y)));
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
	}
}
