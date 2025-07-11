using System.Collections;
using System.Collections.Generic;

public class User
{
	public string _id;

	public string _alias;

	public string _displayName;

	public static User FromFacebookJson(Hashtable data)
	{
		User user = new User();
		user._id = JsonUtil.ExtractString(data, "id", string.Empty);
		user._alias = StringUtil.Trunc(StringUtil.Cleanup(JsonUtil.ExtractString(data, "name", string.Empty)), 32);
		user._displayName = user._alias;
		Hashtable data2 = new Hashtable(JsonUtil.ExtractDictionary(data, "picture", new Dictionary<string, object>()));
		Hashtable data3 = new Hashtable(JsonUtil.ExtractDictionary(data2, "data"));
		string url = JsonUtil.ExtractString(data3, "url", string.Empty);
		bool flag = JsonUtil.ExtractBool(data3, "is_silhouette", def: false);
		if (!string.IsNullOrEmpty(user._id) && user._id != "0" && !flag)
		{
			PictureManager.StorePicture(user._id, url);
		}
		return user;
	}

	public override string ToString()
	{
		return $"<Player> playerId: {_id}, alias: {_alias}, displayName: {_displayName}";
	}
}
