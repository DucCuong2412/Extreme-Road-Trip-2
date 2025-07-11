using System.Collections.Generic;
using UnityEngine;

public class MetroWidgetBestTime : MetroWidget
{
	private const int _playerNameMaxLength = 12;

	public static MetroWidgetBestTime Create(LeaderboardType leaderboard, float timer)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetBestTime).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetBestTime metroWidgetBestTime = gameObject.AddComponent<MetroWidgetBestTime>();
		metroWidgetBestTime.Setup(leaderboard, timer);
		return metroWidgetBestTime;
	}

	public void Setup(LeaderboardType leaderboard, float timer)
	{
		MetroWidget metroWidget = MetroLayout.Create(Direction.vertical);
		Add(metroWidget);
		string leaderboardTitle = GetLeaderboardTitle(leaderboard);
		MetroWidget child = MetroLabel.Create(leaderboardTitle.Localize() + TimeUtil.Format(timer)).SetFont(MetroSkin.SmallFont).SetColor(Color.black);
		metroWidget.Add(child);
		List<LeaderboardScore> mainLeaderboardScores = AutoSingleton<LeaderboardsManager>.Instance.GetMainLeaderboardScores(leaderboard);
		if (mainLeaderboardScores != null)
		{
			int num = Mathf.Min(3, mainLeaderboardScores.Count);
			for (int i = 0; i < num; i++)
			{
				metroWidget.Add(Player("#" + (i + 1).ToString(), StringUtil.Trunc(mainLeaderboardScores[i]._username, 12), (float)mainLeaderboardScores[i]._value / 100f));
			}
		}
		else
		{
			float min = AutoSingleton<GameStatsManager>.Instance.Overall.GetMin(GetStatType(leaderboard));
			float min2 = AutoSingleton<GameStatsManager>.Instance.CurrentRun.GetMin(GetStatType(leaderboard));
			float time = (!(min <= 0f)) ? Mathf.Min(min, min2) : min2;
			metroWidget.Add(Player(string.Empty, "Best".Localize() + " ", time));
		}
	}

	private GameStats.CarStats.Type GetStatType(LeaderboardType leaderboard)
	{
		switch (leaderboard)
		{
		case LeaderboardType.best2kTime:
			return GameStats.CarStats.Type.best2kTime;
		case LeaderboardType.best5kTime:
			return GameStats.CarStats.Type.best5kTime;
		case LeaderboardType.best10kTime:
			return GameStats.CarStats.Type.best10kTime;
		default:
			UnityEngine.Debug.Log("This leaderboard time isn't supported: " + leaderboard.ToString());
			return GameStats.CarStats.Type.best2kTime;
		}
	}

	private MetroWidget Player(string rank, string playerName, float time)
	{
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal);
		bool flag = rank.Length > 0;
		MetroFont font = (!flag) ? MetroSkin.SmallFont : MetroSkin.VerySmallFont;
		float mass = (!flag) ? 0.2f : 0f;
		metroWidget.Add(MetroSpacer.Create(mass));
		if (flag)
		{
			MetroWidget metroWidget2 = MetroLabel.Create(rank).SetFont(font).SetMass(0.25f);
			metroWidget2.SetColor(Color.black);
			metroWidget2.SetAlignment(MetroAlign.Left);
			metroWidget2.SetPadding(0f);
			metroWidget.Add(metroWidget2);
		}
		if (playerName == null)
		{
			playerName = "???";
		}
		MetroWidget metroWidget3 = MetroLabel.Create(playerName).SetFont(font).SetMass(0.35f);
		metroWidget3.SetColor(Color.black);
		metroWidget3.SetAlignment(MetroAlign.Left);
		metroWidget3.SetPadding(0f);
		metroWidget.Add(metroWidget3);
		metroWidget3 = MetroLabel.Create(TimeUtil.Format(time)).SetFont(font);
		metroWidget3.SetColor(Color.black);
		metroWidget3.SetAlignment(MetroAlign.Right);
		metroWidget.Add(metroWidget3);
		metroWidget.Add(MetroSpacer.Create(mass));
		return metroWidget;
	}

	private string GetLeaderboardTitle(LeaderboardType leaderboard)
	{
		switch (leaderboard)
		{
		case LeaderboardType.best2kTime:
			return "2K TIME".Localize() + " - ";
		case LeaderboardType.best5kTime:
			return "5K TIME".Localize() + " - ";
		case LeaderboardType.best10kTime:
			return "10K TIME".Localize() + " - ";
		default:
			return string.Empty;
		}
	}
}
