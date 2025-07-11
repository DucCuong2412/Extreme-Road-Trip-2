using System.Collections.Generic;
using UnityEngine;

public class MetroMenuStack : AutoSingleton<MetroMenuStack>
{
	private Stack<MetroMenuPage> _stack;

	private List<MetroPopupPage> _popups;

	protected override void OnAwake()
	{
		_stack = new Stack<MetroMenuPage>();
		_popups = new List<MetroPopupPage>();
		base.OnAwake();
	}

	public void Push(MetroMenuPage page)
	{
		Push(page, MetroMenuPage.DefaultAnimation);
	}

	public void Push(MetroMenuPage page, MetroAnimation animation)
	{
		if (_stack.Count != 0)
		{
			_stack.Peek().OnBlur();
		}
		_stack.Push(page);
		if (page is MetroPopupPage && (_popups.Count == 0 || _popups[0] != page))
		{
			_popups.Insert(0, page as MetroPopupPage);
		}
		Debug.Log($"{_stack == null} vs {page == null}");
		page.Layer = _stack.Count;
		page.OnPush(animation);
	}

	public void Pop()
	{
		Pop(MetroMenuPage.DefaultAnimation);
	}

	public void Pop(MetroAnimation animation)
	{
		if (_stack.Count == 0)
		{
			return;
		}
		MetroMenuPage metroMenuPage = _stack.Pop();
		metroMenuPage.OnPop(animation);
		if (_stack.Count == 0)
		{
			return;
		}
		MetroMenuPage metroMenuPage2 = _stack.Peek();
		if (_popups.Count > 0 && metroMenuPage == _popups[0])
		{
			_popups.RemoveAt(0);
			if (_popups.Count != 0 && metroMenuPage2 != _popups[0])
			{
				metroMenuPage2 = _popups[0];
				Push(metroMenuPage2, animation);
			}
			else
			{
				metroMenuPage2.OnFocus();
			}
		}
		else
		{
			metroMenuPage2.OnFocus();
		}
	}

	public void Replace(MetroMenuPage page)
	{
		Pop();
		Push(page);
	}

	public void Replace(MetroMenuPage page, MetroAnimation animation)
	{
		Pop(animation);
		Push(page, animation);
	}

	public MetroMenuPage Peek()
	{
		if (_stack == null || _stack.Count == 0)
		{
			return null;
		}
		return _stack.Peek();
	}

	public void EnqueuePopup(MetroPopupPage popup)
	{
		_popups.Add(popup);
		if (_popups.Count == 1)
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(popup, MetroAnimation.popup);
		}
	}

	public bool HasPendingPopup()
	{
		return _popups.Count > 0;
	}
}
