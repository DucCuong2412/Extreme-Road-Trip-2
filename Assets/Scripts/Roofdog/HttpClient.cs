using System;
using System.Collections;
using UnityEngine;

namespace Roofdog
{
	public class HttpClient : AutoSingleton<HttpClient>
	{
		protected override void OnAwake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			base.OnAwake();
		}

		public void Get(string url, Action<HttpResponse> onResponse = null, Action<string> onError = null, HttpRetryOptions retryOptions = null)
		{
			SendRequest(HttpMethod.GET, url, null, onResponse, onError, retryOptions, 0, DateTime.UtcNow);
		}

		public void Post(string url, byte[] body = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, HttpRetryOptions retryOptions = null)
		{
			SendRequest(HttpMethod.POST, url, body, onResponse, onError, retryOptions, 0, DateTime.UtcNow);
		}

		private void SendRequest(HttpMethod method, string url, byte[] body, Action<HttpResponse> onResponse, Action<string> onError, HttpRetryOptions retryOptions, int retryCount, DateTime startTime)
		{
			retryOptions = (retryOptions ?? HttpRetryOptions.NO_RETRY);
			AutoSingleton<HttpClient>.Instance.StartCoroutine(SendRequestCR(method, url, body, onResponse, onError, retryOptions, retryCount, startTime));
		}

		private IEnumerator SendRequestCR(HttpMethod method, string url, byte[] body, Action<HttpResponse> onResponse, Action<string> onError, HttpRetryOptions retryOptions, int retryCount, DateTime startTime)
		{
			if (method == HttpMethod.GET && body != null)
			{
				throw new ArgumentException("Sending body with HTTP method GET is not supported by Unity. URL: " + url);
			}
			if (retryCount == 0)
			{
				onResponse = (Action<HttpResponse>)Delegate.Combine(onResponse, (Action<HttpResponse>)delegate
				{
					string empty = string.Empty;
				});
				onError = (Action<string>)Delegate.Combine(onError, (Action<string>)delegate(string error)
				{
					SilentDebug.LogWarning("HTTP error on " + base.method + " " + base.url + ": " + error);
				});
			}
			if (retryCount < retryOptions.MaxNbRetries && !url.StartsWith("file:"))
			{
				onError = delegate(string error)
				{
					SilentDebug.LogWarning("HTTP retry " + (base.retryCount + 1) + "/" + base.retryOptions.MaxNbRetries + ", URL: " + base.url + ", error: " + error);
					base._003C_003Ef__this.SendRequest(base.method, base.url, base.body, base.onResponse, base._003CoriginalOnError_003E__0, base.retryOptions, base.retryCount + 1, base.startTime);
				};
			}
			int delayInSeconds = 0;
			if (retryCount > 0)
			{
				delayInSeconds = ((!retryOptions.ExponentialBackoff) ? retryOptions.DelayInSeconds : ((int)Math.Pow(2.0, retryCount - 1) * retryOptions.DelayInSeconds));
			}
			float absoluteElapseTime = (float)(DateTime.UtcNow - startTime).TotalSeconds + (float)delayInSeconds;
			float absoluteTimeLeft = (float)retryOptions.AbsoluteTimeoutInSeconds - absoluteElapseTime;
			if (retryCount > 0 && absoluteTimeLeft <= 0f)
			{
				onError("Absolute timeout reached (" + retryOptions.AbsoluteTimeoutInSeconds + " seconds)");
				yield break;
			}
			if (delayInSeconds > 0)
			{
				RealtimeDuration delay = new RealtimeDuration(delayInSeconds);
				while (!delay.IsDone())
				{
					yield return null;
				}
			}
			float requestTimeout = Math.Min(retryOptions.RequestTimeoutInSeconds, absoluteTimeLeft);
			RealtimeDuration requestDuration = new RealtimeDuration(requestTimeout);
			WWW www = (method != 0) ? new WWW(url, body) : new WWW(url);
			while (!www.isDone)
			{
				if (requestDuration.IsDone())
				{
					onError("Request timeout reached (" + requestTimeout + " seconds)");
					yield break;
				}
				yield return null;
			}
			if (!string.IsNullOrEmpty(www.error))
			{
				onError(www.error);
				yield break;
			}
			HttpResponse httpResponse = new HttpResponse(body: www.bytes, requestMethod: method, requestUrl: url, statusCode: 200, headers: www.responseHeaders);
			try
			{
				onResponse(httpResponse);
			}
			catch (RetryException ex)
			{
				RetryException e = ex;
				onError("Error while handling response, retry required: " + e.Message);
			}
		}
	}
}
