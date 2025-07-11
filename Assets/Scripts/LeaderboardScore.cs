using System;

public class LeaderboardScore : IComparable
{
	public SocialPlatform _userPlatform;

	public string _username;

	public string _userId;

	public long _value;

	public LeaderboardScore(string username, string userId, long val, SocialPlatform userPlatform)
	{
		_username = username;
		_userId = userId;
		_value = val;
		_userPlatform = userPlatform;
	}

	public override string ToString()
	{
		return $"LeaderboardScore: name: {_username}, id: {_userId}, score: {_value}";
	}

	public int CompareTo(object obj)
	{
		LeaderboardScore leaderboardScore = obj as LeaderboardScore;
		if (leaderboardScore != null)
		{
			return (int)(leaderboardScore._value - _value);
		}
		return 1;
	}
}
