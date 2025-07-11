using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MetroPopupPage : MetroMenuPage
{
	protected float _width = 0.8f;

	protected float _height = 0.8f;

	protected bool _useCloseButton = true;

	protected Action _onClose;

	protected MetroWidget _closeXButton;

	[method: MethodImpl(32)]
	public event Action OnFocusGained;

	public override void Layout(Rect zone)
	{
		float width = zone.width;
		float height = zone.height;
		zone.width *= _width;
		zone.height *= _height;
		base.Layout(zone);
		if (_useCloseButton)
		{
			_useCloseButton = false;
			if (_onClose == null)
			{
				_onClose = (Action)Delegate.Combine(_onClose, (Action)delegate
				{
					AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
				});
			}
			_closeXButton = MetroSkin.CreatePopupCloseButton(_onClose);
			Add(_closeXButton);
			zone.width = width;
			zone.height = height;
			_zone = zone;
			_closeXButton.Layout(zone);
		}
	}

	public override void OnFocus()
	{
		if (this.OnFocusGained != null)
		{
			this.OnFocusGained();
		}
		base.OnFocus();
	}
}
