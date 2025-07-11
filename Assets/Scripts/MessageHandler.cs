using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : AutoSingleton<MessageHandler>
{
	private List<Message> _msgQueue;

	private Message _currentMsg;

	private Message _pendingDelete;

	private bool _sessionStarted;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_msgQueue = new List<Message>();
		_currentMsg = null;
		_pendingDelete = null;
	}

	private void OnEnable()
	{
		BackendSessionManager.OnSessionStartedEvent += OnSessionStarted;
	}

	private void OnDisable()
	{
		BackendSessionManager.OnSessionStartedEvent -= OnSessionStarted;
	}

	private bool CanProcessMessage()
	{
		return _sessionStarted;
	}

	private Message GetMessage()
	{
		Message result = null;
		if (_msgQueue.Count > 0)
		{
			result = _msgQueue[0];
			_msgQueue.RemoveAt(0);
		}
		return result;
	}

	public bool ProcessServerMessage()
	{
		if (CanProcessMessage())
		{
			if (_currentMsg != null)
			{
				HandleGenericMsg(_currentMsg);
				_currentMsg = null;
				return true;
			}
			if (_pendingDelete == null)
			{
				_pendingDelete = GetMessage();
				if (_pendingDelete != null)
				{
					BackendMessageApiClient.DeleteMessage(_pendingDelete.Uid, OnMsgDeleteSuccess, OnMsgDeleteFail);
				}
			}
		}
		return false;
	}

	private void HandleGenericMsg(Message msg)
	{
		PopupGenericMessage page = MetroMenuPage.Create<PopupGenericMessage>().Setup(msg);
		AutoSingleton<MetroMenuStack>.Instance.Push(page);
	}

	public void UpdateGenericMessage(List<Message> messages)
	{
		_msgQueue = messages;
	}

	public static void OnSessionStarted()
	{
		if (AutoSingleton<MessageHandler>.IsCreated())
		{
			AutoSingleton<MessageHandler>.Instance.SessionStarted();
		}
	}

	private void SessionStarted()
	{
		_sessionStarted = true;
		FetchServerMessage();
	}

	private void FetchServerMessage()
	{
		_msgQueue.Clear();
		BackendMessageApiClient.GetPendingMessages(OnMsgListUpdated, null);
	}

	private void MsgDeleteSuccess()
	{
		if (_pendingDelete != null)
		{
			_currentMsg = _pendingDelete;
			_pendingDelete = null;
		}
	}

	private void MsgDeleteFail(string error)
	{
		_pendingDelete = null;
		FetchServerMessage();
	}

	public static void OnMsgDeleteSuccess()
	{
		if (AutoSingleton<MessageHandler>.IsCreated())
		{
			AutoSingleton<MessageHandler>.Instance.MsgDeleteSuccess();
		}
	}

	public static void OnMsgDeleteFail(string error)
	{
		if (AutoSingleton<MessageHandler>.IsCreated())
		{
			AutoSingleton<MessageHandler>.Instance.MsgDeleteFail(error);
		}
	}

	public static void OnMsgListUpdated(List<Message> messages)
	{
		if (AutoSingleton<MessageHandler>.IsCreated())
		{
			AutoSingleton<MessageHandler>.Instance.UpdateGenericMessage(messages);
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			_sessionStarted = false;
			_msgQueue.Clear();
		}
	}
}
