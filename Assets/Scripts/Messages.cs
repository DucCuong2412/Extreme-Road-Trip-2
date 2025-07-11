using System.Collections;
using System.Collections.Generic;

public class Messages
{
	public List<Message> PendingMessages
	{
		get;
		private set;
	}

	public static Messages FromJsonData(Hashtable data)
	{
		if (data == null)
		{
			return null;
		}
		List<Message> list = new List<Message>();
		ArrayList arrayList = JsonUtil.ExtractArrayList(data, "Messages");
		foreach (Hashtable item in arrayList)
		{
			list.Add(Message.FromJsonData(item));
		}
		Messages messages = new Messages();
		messages.PendingMessages = list;
		return messages;
	}
}
