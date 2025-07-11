using UnityEngine;

public class MetroPagerIndicator : MetroWidget
{
	private MetroPager _pager;

	private int _numberOfPages;

	private int _current;

	private MetroWidget[] _icons;

	public static MetroPagerIndicator Create(MetroPager pager)
	{
		GameObject gameObject = new GameObject(typeof(MetroPagerIndicator).ToString());
		gameObject.transform.position = Vector3.zero;
		return gameObject.AddComponent<MetroPagerIndicator>().Setup(pager);
	}

	private string GetIconName(int i, int current)
	{
		string result = MetroSkin.IconOff;
		if ((i == 15 && AutoSingleton<PocketMine2PromoManager>.Instance.AreCarsAvailable()) || (i == 17 && AutoSingleton<PRTPromoManager>.Instance.AreCarsAvailable()) || (i == 19 && AutoSingleton<PocketMine3PromoManager>.Instance.AreCarsAvailable()) || (i == 18 && AutoSingleton<FishingBreakPromoManager>.Instance.AreCarsAvailable()))
		{
			result = MetroSkin.IconOffSpecial;
		}
		if (i == current)
		{
			result = MetroSkin.IconOn;
		}
		return result;
	}

	public MetroPagerIndicator Setup(MetroPager pager)
	{
		_pager = pager;
		_numberOfPages = _pager.GetNumberOfPages();
		_current = _pager.ComputePage();
		_icons = new MetroWidget[_numberOfPages];
		if (_numberOfPages > 1)
		{
			MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
			Add(metroLayout);
			for (int i = 0; i < _numberOfPages; i++)
			{
				MetroButton metroButton = MetroButton.Create(string.Empty);
				MetroIcon metroIcon = CreatePagerIcon(GetIconName(i, _current), i + 1);
				metroButton.Add(metroIcon);
				int iCopy = i;
				metroButton.OnButtonClicked += delegate
				{
					_pager.ChangePage(iCopy, fastSpeed: true);
				};
				metroLayout.Add(metroButton);
				_icons[i] = metroIcon;
			}
		}
		return this;
	}

	private MetroIcon CreatePagerIcon(string iconName, int n)
	{
		MetroIcon metroIcon = MetroIcon.Create(iconName);
		metroIcon.SetScale(0.8f);
		MetroLabel metroLabel = MetroLabel.Create(n.ToString());
		metroLabel.SetFont(MetroSkin.SmallFont);
		metroIcon.Add(metroLabel);
		return metroIcon;
	}

	public void Update()
	{
		int num = _pager.ComputePage();
		if (_current != num)
		{
			MetroIcon metroIcon = CreatePagerIcon(GetIconName(_current, num), _current + 1);
			_icons[_current].Replace(metroIcon).Destroy();
			_icons[_current] = metroIcon;
			_current = num;
			MetroIcon metroIcon2 = CreatePagerIcon(GetIconName(_current, num), _current + 1);
			_icons[_current].Parent.transform.localScale = Vector3.one;
			_icons[_current].Replace(metroIcon2).Destroy();
			_icons[_current] = metroIcon2;
			Reflow();
		}
	}
}
