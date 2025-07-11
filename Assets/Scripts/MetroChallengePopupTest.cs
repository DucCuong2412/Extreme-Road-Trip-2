using System.Collections.Generic;

public class MetroChallengePopupTest : MetroMenuPage
{
	protected override void OnStart()
	{
		base.OnStart();
		List<User> list = new List<User>();
		for (int i = 1; i < 20; i++)
		{
			list.Add(new User
			{
				_id = i.ToString(),
				_alias = i.ToString(),
				_displayName = i.ToString()
			});
		}
		List<MetroWidgetCheckboxListItem> list2 = new List<MetroWidgetCheckboxListItem>();
		int num = 0;
		List<Location> allLocations = AutoSingleton<LocationDatabase>.Instance.GetAllLocations();
		foreach (Location item2 in allLocations)
		{
			MetroWidgetLocationCheckbox item = MetroWidgetLocationCheckbox.Create(item2, num, MetroSkin.GameCenterPlayerEvenRowColor, MetroSkin.GameCenterPlayerOddRowColor, isSelected: true);
			list2.Add(item);
			num++;
		}
		MetroMenuPage metroMenuPage = MetroMenuPage.Create<MetroPopupCheckboxList>().Setup(list2, null, "MAPS LIST", "APPLY");
		metroMenuPage.Reflow();
		AutoSingleton<MetroMenuStack>.Instance.Push(metroMenuPage);
	}
}
