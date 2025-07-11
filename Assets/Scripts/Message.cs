using System.Collections;

public class Message
{
	public enum MessageAction
	{
		invalidAction,
		openUrlAction,
		openGameAction,
		purchaseProductAction,
		redeemRewardAction,
		emptyAction
	}

	public string Uid
	{
		get;
		private set;
	}

	public string ContextId
	{
		get;
		private set;
	}

	public long CreationDate
	{
		get;
		private set;
	}

	public string Title
	{
		get;
		private set;
	}

	public string Content
	{
		get;
		private set;
	}

	public string ImageUrl
	{
		get;
		private set;
	}

	public MessageAction Action
	{
		get;
		private set;
	}

	public string ActionData
	{
		get;
		private set;
	}

	public string ActionText
	{
		get;
		private set;
	}

	public bool ActionCancellable
	{
		get;
		private set;
	}

	public bool ActionConsumable
	{
		get;
		private set;
	}

	public bool ActionConsumed
	{
		get;
		private set;
	}

	public bool ShowOnReception
	{
		get;
		private set;
	}

	public long ExpirationDate
	{
		get;
		private set;
	}

	public static Message FromJsonData(Hashtable data)
	{
		if (data == null)
		{
			return null;
		}
		Message message = new Message();
		message.Uid = JsonUtil.ExtractString(data, "Uid", string.Empty);
		message.ContextId = JsonUtil.ExtractString(data, "ContextId", string.Empty);
		message.CreationDate = JsonUtil.ExtractLong(data, "CreationDate", 0L);
		message.Title = JsonUtil.ExtractString(data, "Title", string.Empty);
		message.Content = JsonUtil.ExtractString(data, "Content", string.Empty);
		message.ImageUrl = JsonUtil.ExtractString(data, "ImageUrl", string.Empty);
		message.Action = EnumUtil.Parse(JsonUtil.ExtractString(data, "Action", string.Empty), MessageAction.invalidAction);
		message.ActionData = JsonUtil.ExtractString(data, "ActionData", string.Empty);
		message.ActionText = JsonUtil.ExtractString(data, "ActionText", string.Empty);
		message.ActionCancellable = JsonUtil.ExtractBool(data, "ActionCancellable", def: true);
		message.ActionConsumable = JsonUtil.ExtractBool(data, "ActionConsumable", def: false);
		message.ActionConsumed = JsonUtil.ExtractBool(data, "ActionConsumed", def: false);
		message.ShowOnReception = JsonUtil.ExtractBool(data, "ShowOnReception", def: false);
		message.ExpirationDate = JsonUtil.ExtractLong(data, "ExpirationDate", 0L);
		return message;
	}

	public void Consume()
	{
		ActionConsumed = true;
		ActionConsumable = false;
	}
}
