//using Roofdog;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class BackendMessageApiClient
{
	public static void GetPendingMessages(Action<List<Message>> onSuccess, Action<string> onError)
	{
		//AutoSingleton<BackendApiClient>.Instance.Get("/rest/messages", null, delegate(HttpResponse response)
		//{
		//	Messages messages = Messages.FromJsonData(response.GetStringBody().hashtableFromJson());
		//	if (onSuccess != null)
		//	{
		//		onSuccess(messages.PendingMessages);
		//	}
		//}, onError, new int[1]
		//{
		//	200
		//}, HttpRetryOptions.DEFAULT_RETRY);
	}

	public static void DeleteMessage(string messageUid, Action onSuccess, Action<string> onError)
	{
		//	AutoSingleton<BackendApiClient>.Instance.Delete("/rest/messages/" + WWW.EscapeURL(messageUid), null, delegate
		//	{
		//		if (onSuccess != null)
		//		{
		//			onSuccess();
		//		}
		//	}, onError, new int[1]
		//	{
		//		200
		//	}, HttpRetryOptions.DEFAULT_RETRY);
	}
}
