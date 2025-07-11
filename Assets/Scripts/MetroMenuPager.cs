using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroMenuPager : MetroMenuPage
{
	protected MetroPager _pager;

	protected List<MetroButton> _pagerButtons;

	private bool _isSnapped = true;

	protected float RowCount
	{
		get;
		set;
	}

	protected float ColCount
	{
		get;
		set;
	}

	protected override void OnAwake()
	{
		_pagerButtons = new List<MetroButton>();
		base.OnAwake();
	}

	protected MetroPager CreateMetroPager(string name)
	{
		return MetroPager.Create(name);
	}

	protected MetroGrid CreateGrid()
	{
		return MetroGrid.Create((int)ColCount, (int)RowCount);
	}

	protected MetroLayout CreatePagerChild(MetroGrid grid, bool isFirst, bool isLast)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		if (!isFirst)
		{
			MetroButton left = MetroButton.Create();
			left.IsKeyNavigatorAccessible = false;
			left.SetMass(0.5f);
			MetroIcon child = MetroIcon.Create(MetroSkin.Arrow);
			left.Add(child);
			left.OnButtonClicked += delegate
			{
				_pager.ChangePage(next: false);
				_isSnapped = false;
				tk2dSprite componentInChildren2 = left.GetComponentInChildren<tk2dSprite>();
				Color color2 = componentInChildren2.color;
				color2.a = 0f;
				componentInChildren2.color = color2;
			};
			metroLayout.Add(left);
			_pagerButtons.Add(left);
		}
		else
		{
			metroLayout.Add(MetroSpacer.Create(0.5f));
		}
		grid.SetMass(9f);
		metroLayout.Add(grid);
		if (!isLast)
		{
			MetroButton right = MetroButton.Create();
			right.IsKeyNavigatorAccessible = false;
			right.SetMass(0.5f);
			MetroIcon metroIcon = MetroIcon.Create(MetroSkin.Arrow);
			metroIcon.transform.Rotate(0f, 0f, 180f);
			right.Add(metroIcon);
			right.OnButtonClicked += delegate
			{
				_pager.ChangePage(next: true);
				_isSnapped = false;
				tk2dSprite componentInChildren = right.GetComponentInChildren<tk2dSprite>();
				Color color = componentInChildren.color;
				color.a = 0f;
				componentInChildren.color = color;
			};
			metroLayout.Add(right);
			_pagerButtons.Add(right);
		}
		else
		{
			metroLayout.Add(MetroSpacer.Create(0.5f));
		}
		return metroLayout;
	}

	protected override void OnMenuUpdate()
	{
		bool flag = _pager.IsSnapped();
		if (_isSnapped != flag)
		{
			_isSnapped = flag;
			foreach (MetroButton pagerButton in _pagerButtons)
			{
				pagerButton.transform.localScale = Vector3.one;
				pagerButton.SetActive(flag);
				pagerButton.gameObject.SetActive(flag);
				if (flag)
				{
					StartCoroutine(ShowPagerButtonCR(pagerButton.GetComponentInChildren<tk2dSprite>()));
				}
			}
		}
		base.OnMenuUpdate();
	}

	private IEnumerator ShowPagerButtonCR(tk2dSprite s)
	{
		Duration delay = new Duration(0.3f);
		float from = 0f;
		float to = 1f;
		while (!delay.IsDone() && _isSnapped)
		{
			float a = Mathf.Lerp(from, to, delay.Value01());
			Color c = s.color;
			c.a = a;
			s.color = c;
			yield return null;
		}
	}
}
