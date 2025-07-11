using System.Collections;

public class AGSLeaderboard
{
	public string name;

	public string id;

	public string displayText;

	public string scoreFormat;

	public string imageUrl;

	public static AGSLeaderboard fromHashtable(Hashtable hashtable)
	{
		AGSLeaderboard aGSLeaderboard = new AGSLeaderboard();
		aGSLeaderboard.name = hashtable["leaderboardName"].ToString();
		aGSLeaderboard.id = hashtable["leaderboardId"].ToString();
		aGSLeaderboard.displayText = hashtable["leaderboardDisplayText"].ToString();
		aGSLeaderboard.scoreFormat = hashtable["leaderboardScoreFormat"].ToString();
		aGSLeaderboard.imageUrl = hashtable["leaderboardImageUrl"].ToString();
		return aGSLeaderboard;
	}

	public static AGSLeaderboard GetBlankLeaderboard()
	{
		AGSLeaderboard aGSLeaderboard = new AGSLeaderboard();
		aGSLeaderboard.name = string.Empty;
		aGSLeaderboard.id = string.Empty;
		aGSLeaderboard.displayText = string.Empty;
		aGSLeaderboard.scoreFormat = string.Empty;
		aGSLeaderboard.imageUrl = string.Empty;
		return aGSLeaderboard;
	}

	public override string ToString()
	{
		return $"name: {name}, id: {id}, displayText: {displayText}, scoreFormat: {scoreFormat}, imageUrl: {imageUrl}";
	}
}
