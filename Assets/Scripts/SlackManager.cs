//using Roofdog;
using System.Collections;
using System.Text;
using UnityEngine;

public class SlackManager : AutoSingleton<SlackManager>
{
	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public static void PostToBuildBot(string text)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["channel"] = "#build-bot";
		hashtable["text"] = text;
		string url = "https://hooks.slack.com/services/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";
		// AutoSingleton<HttpClient>.Instance.Post(url, Encoding.UTF8.GetBytes($"payload={hashtable.toJson()}"), delegate
		// {
		// }, delegate
		// {
		// }, HttpRetryOptions.NO_RETRY);
	}
}
