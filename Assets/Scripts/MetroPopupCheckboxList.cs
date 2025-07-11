using System;
using System.Collections.Generic;
using UnityEngine;

public class MetroPopupCheckboxList : MetroPopupPage
{
	private List<MetroWidgetCheckboxListItem> _itemWidgetList;

	private MetroButton _closeButton;

	private string _confirmButtonString;

	private Action _onConfirmButtonClicked;

	protected override void OnAwake()
	{
		_width = 0.95f;
		_height = 0.95f;
		base.OnAwake();
	}

	public MetroPopupCheckboxList Setup(List<MetroWidgetCheckboxListItem> itemList, Action onConfirmButtonClicked, string titleString, string confirmButtonString)
	{
		_itemWidgetList = itemList;
		_confirmButtonString = confirmButtonString;
		_onConfirmButtonClicked = onConfirmButtonClicked;
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel child = MetroLabel.Create(titleString);
		metroLayout2.Add(child);
		if (itemList != null && itemList.Count > 0)
		{
			MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal).SetMass(5f).AddSlice9Background(MetroSkin.Slice9PopupBackground)
				.SetPadding(2f, 0.2f);
			metroLayout.Add(metroWidget);
			MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
			metroWidget.Add(MetroSpacer.Create(0.05f));
			metroWidget.Add(metroLayout3);
			metroWidget.Add(MetroSpacer.Create(0.05f));
			MetroViewport metroViewport = MetroViewport.Create(MetroSkin.ClippedGUILayer1);
			metroLayout3.Add(MetroSpacer.Create(0.05f));
			metroLayout3.Add(metroViewport);
			metroLayout3.Add(MetroSpacer.Create(0.05f));
			float onScreenCount = 3.5f;
			MetroSlider metroSlider = MetroSlider.Create(Direction.vertical, onScreenCount);
			metroViewport.Add(metroSlider);
			foreach (MetroWidgetCheckboxListItem item in itemList)
			{
				MetroWidgetCheckboxListItem metroWidgetCheckboxListItem = item;
				metroWidgetCheckboxListItem.OnItemToggle = (Action)Delegate.Combine(metroWidgetCheckboxListItem.OnItemToggle, new Action(OnItemSelect));
				metroSlider.Add(item);
			}
		}
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		metroLayout4.Add(MetroSpacer.Create());
		_closeButton = CreateCancelButton();
		_closeButton.SetMass(2f);
		metroLayout4.Add(_closeButton);
		metroLayout4.Add(MetroSpacer.Create());
		metroLayout.Add(MetroSpacer.Create(0.2f));
		return this;
	}

	private MetroButton CreateCancelButton()
	{
		MetroButton metroButton = MetroButton.Create("CANCEL");
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		};
		MetroMenu.AddKeyNavigationBehaviour(metroButton, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
		return metroButton;
	}

	private MetroButton CreateConfirmButton()
	{
		MetroButton metroButton = MetroButton.Create(_confirmButtonString);
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.SetMass(0.4f);
		MetroMenu.AddKeyNavigationBehaviour(metroButton, MetroSkin.Slice9ButtonRed, MetroSkin.Slice9Button);
		metroButton.OnButtonClicked += delegate
		{
			if (_onConfirmButtonClicked != null)
			{
				_onConfirmButtonClicked();
			}
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
		};
		return metroButton;
	}

	private void OnItemSelect()
	{
		MetroButton metroButton = (!HasSelectedItems()) ? CreateCancelButton() : CreateConfirmButton();
		_closeButton.Replace(metroButton).Destroy();
		_closeButton = metroButton;
		_closeButton.Parent.Reflow();
	}

	private bool HasSelectedItems()
	{
		foreach (MetroWidgetCheckboxListItem itemWidget in _itemWidgetList)
		{
			if (itemWidget.IsSelected())
			{
				return true;
			}
		}
		return false;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.8f, (0f - base.Camera.HalfScreenHeight) * 0.9f, base.Camera.ScreenWidth * 0.8f, base.Camera.ScreenHeight * 0.9f);
	}
}
