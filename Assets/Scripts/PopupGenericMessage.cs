using System;
using UnityEngine;

public class PopupGenericMessage : MetroPopupPage
{
	private string _imageId;

	private Message _message;

	public PopupGenericMessage Setup(Message userMessage)
	{
		_message = userMessage;
		if (userMessage.ActionCancellable)
		{
			_onClose = delegate
			{
				AutoSingleton<Rooflog>.Instance.OnGenericMessageActionTaken(_message.Uid, _message.ContextId, GenericMessageActionTaken.dismissed);
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
			_useCloseButton = true;
		}
		else
		{
			_useCloseButton = false;
		}
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		PictureManager.StorePicture(userMessage.Uid, userMessage.ImageUrl);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		MetroLabel metroLabel = MetroLabel.Create(userMessage.Title);
		metroLabel.SetMass(0.15f);
		metroLayout2.Add(metroLabel);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		metroLayout3.SetMass(0.7f);
		metroLayout2.Add(metroLayout3);
		if (!string.IsNullOrEmpty(userMessage.ImageUrl))
		{
			_imageId = userMessage.Uid;
			WidgetLoaderContainer child = WidgetWebImage.Create(_imageId, _imageId, MetroSpinner.Create(MetroSkin.Spinner), MetroStretch.fullRatio);
			metroLayout3.Add(child);
		}
		string content = userMessage.Content;
		if (!string.IsNullOrEmpty(content))
		{
			string content2 = content.Localize().Wrap(40);
			float num = Mathf.Ceil((float)content.Length / 40f);
			float mass = Mathf.Max(num * 0.33f, 0f);
			MetroLabel metroLabel2 = MetroLabel.Create(content2);
			metroLabel2.SetMass(mass);
			metroLayout3.Add(metroLabel2);
		}
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.Add(metroLayout4);
		metroLayout2.Add(MetroSpacer.Create(0.05f));
		metroLayout4.SetMass(0.15f);
		Action buttonAction = MessageActionFactory.ProcessPayload(userMessage);
		metroLayout4.Add(MetroSpacer.Create(0.8f));
		string label = string.IsNullOrEmpty(userMessage.ActionText) ? "OK" : userMessage.ActionText;
		MetroButton metroButton = MetroButton.Create(label);
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			Exit(GenericMessageActionTaken.taken);
			if (buttonAction != null)
			{
				buttonAction();
			}
		};
		metroLayout4.Add(metroButton);
		metroLayout4.Add(MetroSpacer.Create(0.8f));
		metroLayout.Add(metroLayout2);
		AutoSingleton<Rooflog>.Instance.OnGenericMessageRead(_message.Uid, _message.ContextId);
		return this;
	}

	private void Exit(GenericMessageActionTaken action)
	{
		AutoSingleton<Rooflog>.Instance.OnGenericMessageActionTaken(_message.Uid, _message.ContextId, action);
		if (AutoSingleton<MetroMenuStack>.Instance.Peek() == this)
		{
			AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.none);
		}
	}

	public void OnDisable()
	{
		if (!string.IsNullOrEmpty(_imageId))
		{
			PictureManager.Delete(_imageId);
			_imageId = string.Empty;
		}
	}
}
