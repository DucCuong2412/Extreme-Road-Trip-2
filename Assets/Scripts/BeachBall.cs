using UnityEngine;

public class BeachBall : MonoBehaviour
{
	private Rigidbody _rigidbody;

	private bool _achievementSent;

	private void Awake()
	{
		_rigidbody = null;
		_achievementSent = false;
	}

	private void Update()
	{
		if (_rigidbody == null)
		{
			_rigidbody = base.gameObject.GetComponent<Rigidbody>();
		}
		if (_achievementSent || !(_rigidbody != null))
		{
			return;
		}
		Vector3 velocity = _rigidbody.velocity;
		if (velocity.x > 80f)
		{
			Vector3 velocity2 = _rigidbody.velocity;
			if (velocity2.y > 35f)
			{
				AutoSingleton<AchievementsManager>.Instance.CompleteAchievement(AchievementType.kicktheBall);
				_achievementSent = true;
			}
		}
	}
}
