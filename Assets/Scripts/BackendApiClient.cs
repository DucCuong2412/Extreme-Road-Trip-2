////using Roofdog;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Security.Cryptography;
//using System.Text;
//using UnityEngine;

//public class BackendApiClient : AutoSingleton<BackendApiClient>
//{
//	private enum OnlineStatus
//	{
//		unknown,
//		online,
//		offline
//	}

//	private byte[] secret = new UTF8Encoding().GetBytes("mkECIQDto2APhd8LjJoYP7Zf4gOVu5yGoHaWyu8E3ZPuegKmCQIhANKwP7b+lmYQ");

//	private string _redirectLocation;

//	private Dictionary<string, Action<Action>> _directiveListeners = new Dictionary<string, Action<Action>>();

//	private Dictionary<int, Action> _errorHandlers = new Dictionary<int, Action>();

//	private OnlineStatus _onlineStatus;

//	private Action<OnlineStatusChangeEvent> _onlineListener;

//	private TimeSpan _lastBackendTimeDelta = TimeSpan.Zero;

//	private int _pauseSeq;

//	private BackendApiClient()
//	{
//	}

//	protected override void OnAwake()
//	{
//		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
//		base.OnAwake();
//	}

//	private void OnApplicationPause(bool pause)
//	{
//		if (pause)
//		{
//			_onlineStatus = OnlineStatus.unknown;
//		}
//		else
//		{
//			_pauseSeq++;
//		}
//	}

//	private string GetUrl(string path)
//	{
//		if (_redirectLocation != null)
//		{
//			return _redirectLocation + path;
//		}
//		return "https://roofdog-ert2.appspot.com" + path;
//	}

//	private ClientInfo GetCurrentClientInfo()
//	{
//		return new ClientInfo(GameVersion.VERSION, Device.GetDeviceType(), SystemInfo.deviceUniqueIdentifier, AutoSingleton<LocalizationManager>.Instance.Language.ToString(), AutoSingleton<NotificationManager>.Instance.GetDeviceToken(), DateTime.Now);
//	}

//	public bool IsOnline()
//	{
//		return _onlineStatus == OnlineStatus.online;
//	}

//	public void SetDirectiveListener(string directive, Action<Action> listener)
//	{
//		directive = directive.ToUpper();
//		if (_directiveListeners.ContainsKey(directive))
//		{
//			SilentDebug.LogError("Listener already set for directive: " + directive);
//		}
//		else
//		{
//			_directiveListeners[directive] = listener;
//		}
//	}

//	public void SetErrorHandler(int statusCode, Action handler)
//	{
//		if (_errorHandlers.ContainsKey(statusCode))
//		{
//			SilentDebug.LogError("Error handler already set for status code: " + statusCode);
//		}
//		else
//		{
//			_errorHandlers[statusCode] = handler;
//		}
//	}

//	public void RemoveErrorHandler(int statusCode)
//	{
//		if (_errorHandlers.ContainsKey(statusCode))
//		{
//			_errorHandlers.Remove(statusCode);
//		}
//	}

//	public void AddOnlineListener(Action<OnlineStatusChangeEvent> listener)
//	{
//		_onlineListener = (Action<OnlineStatusChangeEvent>)Delegate.Combine(_onlineListener, listener);
//		if (_onlineStatus == OnlineStatus.online)
//		{
//			OnlineStatusChangeEvent obj = default(OnlineStatusChangeEvent);
//			obj.IsOnline = (_onlineStatus == OnlineStatus.online);
//			obj.WasOffline = false;
//			obj.BackendTimeDelta = _lastBackendTimeDelta;
//			listener(obj);
//		}
//	}

//	public void RemoveOnlineListener(Action<OnlineStatusChangeEvent> listener)
//	{
//		_onlineListener = (Action<OnlineStatusChangeEvent>)Delegate.Remove(_onlineListener, listener);
//	}

//	public void Get(string path, Hashtable headers = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, int[] expectedStatusCode = null, HttpRetryOptions retryOptions = null)
//	{
//		SendRequest(HttpMethod.GET, path, headers, null, null, onResponse, onError, expectedStatusCode, retryOptions);
//	}

//	public void Post(string path, Hashtable headers = null, byte[] body = null, string contentType = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, int[] expectedStatusCode = null, HttpRetryOptions retryOptions = null)
//	{
//		SendRequest(HttpMethod.POST, path, headers, body, contentType, onResponse, onError, expectedStatusCode, retryOptions);
//	}

//	public void PostJson(string path, Hashtable headers = null, string jsonString = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, int[] expectedStatusCode = null, HttpRetryOptions retryOptions = null)
//	{
//		byte[] array = (!string.IsNullOrEmpty(jsonString)) ? new UTF8Encoding().GetBytes(jsonString) : null;
//		string contentType = (array != null) ? "application/json; charset=UTF-8" : null;
//		Post(path, headers, array, contentType, onResponse, onError, expectedStatusCode, retryOptions);
//	}

//	public void Put(string path, Hashtable headers = null, byte[] body = null, string contentType = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, int[] expectedStatusCode = null, HttpRetryOptions retryOptions = null)
//	{
//		SendRequest(HttpMethod.PUT, path, headers, body, contentType, onResponse, onError, expectedStatusCode, retryOptions);
//	}

//	public void PutJson(string path, Hashtable headers = null, string jsonString = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, int[] expectedStatusCode = null, HttpRetryOptions retryOptions = null)
//	{
//		byte[] array = (!string.IsNullOrEmpty(jsonString)) ? new UTF8Encoding().GetBytes(jsonString) : null;
//		string contentType = (array != null) ? "application/json; charset=UTF-8" : null;
//		Put(path, headers, array, contentType, onResponse, onError, expectedStatusCode, retryOptions);
//	}

//	public void Delete(string path, Hashtable headers = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, int[] expectedStatusCode = null, HttpRetryOptions retryOptions = null)
//	{
//		SendRequest(HttpMethod.DELETE, path, headers, null, null, onResponse, onError, expectedStatusCode, retryOptions);
//	}

//	public void Head(string path, Hashtable headers = null, Action<HttpResponse> onResponse = null, Action<string> onError = null, int[] expectedStatusCode = null, HttpRetryOptions retryOptions = null)
//	{
//		SendRequest(HttpMethod.HEAD, path, headers, null, null, onResponse, onError, expectedStatusCode, retryOptions);
//	}

//	private void SendRequest(HttpMethod method, string path, Hashtable headers, byte[] body, string contentType, Action<HttpResponse> onResponse, Action<string> onError, int[] expectedStatusCode, HttpRetryOptions retryOptions)
//	{
//		int requestPauseSeq = _pauseSeq;
//		if (_onlineStatus == OnlineStatus.offline)
//		{
//			if (onError != null)
//			{
//				onError("Is in offline mode.");
//			}
//			return;
//		}
//		if (onResponse == null)
//		{
//			onResponse = delegate
//			{
//			};
//		}
//		onError = (Action<string>)Delegate.Combine(onError, (Action<string>)delegate(string error)
//		{
//			SilentDebug.LogWarning("Backend error on " + method + " " + path + " (enabling offline mode): " + error);
//		});
//		onResponse = (Action<HttpResponse>)Delegate.Combine(onResponse, (Action<HttpResponse>)delegate(HttpResponse response)
//		{
//			if (requestPauseSeq == _pauseSeq)
//			{
//				_lastBackendTimeDelta = GetServerTimeDelta(response);
//				if (_onlineStatus == OnlineStatus.unknown)
//				{
//					_onlineStatus = OnlineStatus.online;
//					if (_onlineListener != null)
//					{
//						OnlineStatusChangeEvent obj2 = default(OnlineStatusChangeEvent);
//						obj2.IsOnline = true;
//						obj2.WasOffline = false;
//						obj2.BackendTimeDelta = _lastBackendTimeDelta;
//						_onlineListener(obj2);
//					}
//				}
//			}
//		});
//		onError = (Action<string>)Delegate.Combine(onError, (Action<string>)delegate
//		{
//			if (_onlineStatus != OnlineStatus.offline)
//			{
//				_onlineStatus = OnlineStatus.offline;
//				if (_onlineListener != null)
//				{
//					OnlineStatusChangeEvent obj = default(OnlineStatusChangeEvent);
//					obj.IsOnline = false;
//					obj.WasOffline = false;
//					obj.BackendTimeDelta = TimeSpan.Zero;
//					_onlineListener(obj);
//				}
//				AutoSingleton<BackendApiClient>.Instance.StartCoroutine(CheckOnlineStatusCR());
//			}
//		});
//		Hashtable hashtable = (headers != null) ? new Hashtable(headers) : new Hashtable();
//		hashtable["X-Roofdog-ClientInfo"] = GetSignedHeaderValue(GetCurrentClientInfo().ToJsonData());
//		if (AutoSingleton<BackendSessionManager>.Instance.SessionToken != null)
//		{
//			hashtable["X-Roofdog-SessionToken"] = AutoSingleton<BackendSessionManager>.Instance.SessionToken;
//		}
//		if (body != null && body.Length > 0)
//		{
//			hashtable["X-Roofdog-Hash"] = ComputeSignature(body);
//			hashtable["Content-Type"] = ((!string.IsNullOrEmpty(contentType)) ? contentType : "application/octet-stream");
//		}
//		path = path + ((!path.Contains("?")) ? "?" : "&") + "roofdogHttp=true";
//		RoofdogHttpRequest roofdogHttpRequest = new RoofdogHttpRequest(method.ToString(), path, hashtable, body);
//		string url = GetUrl(path);
//		AutoSingleton<HttpClient>.Instance.Post(url, roofdogHttpRequest.ToBytes(), delegate(HttpResponse response)
//		{
//			try
//			{
//				RoofdogHttpResponse roofdogHttpResponse = RoofdogHttpResponse.FromBytes(response.Body);
//				response = roofdogHttpResponse.ToHttpResponse(method, url, response.Headers);
//			}
//			catch (Exception ex)
//			{
//				throw new RetryException("Backend error on " + method + " " + path + ": unable to parse Roofdog HTTP response: " + ex);
//				IL_0098:;
//			}
//			string value = null;
//			response.Headers.TryGetValue("Location".ToUpper(), out value);
//			if (response.StatusCode == 301 && value != null)
//			{
//				_redirectLocation = value;
//				SendRequest(method, path, headers, body, contentType, onResponse, onError, expectedStatusCode, retryOptions);
//			}
//			else if (_errorHandlers.ContainsKey(response.StatusCode))
//			{
//				_errorHandlers[response.StatusCode]();
//			}
//			else
//			{
//				if (expectedStatusCode != null && Array.IndexOf(expectedStatusCode, response.StatusCode) < 0)
//				{
//					throw new RetryException("Backend error on " + method + " " + path + ": unexpected status code: " + response.StatusCode);
//				}
//				bool flag = true;
//				if (response.Body != null && response.Body.Length > 0)
//				{
//					string value2 = string.Empty;
//					response.Headers.TryGetValue("X-Roofdog-Hash".ToUpper(), out value2);
//					flag = (value2 == ComputeSignature(response.Body));
//				}
//				if (flag)
//				{
//					List<Action<Action>> list = new List<Action<Action>>();
//					string value3 = null;
//					response.Headers.TryGetValue("X-Roofdog-ServerDirective".ToUpper(), out value3);
//					if (value3 != null)
//					{
//						string[] array = value3.Split(',');
//						string[] array2 = array;
//						foreach (string text in array2)
//						{
//							string key = text.Trim().ToUpper();
//							if (_directiveListeners.ContainsKey(key))
//							{
//								list.Add(_directiveListeners[key]);
//							}
//						}
//					}
//					int remaining = list.Count;
//					if (remaining == 0)
//					{
//						onResponse(response);
//					}
//					else
//					{
//						foreach (Action<Action> item in list)
//						{
//							item(delegate
//							{
//								if (--remaining <= 0)
//								{
//									onResponse(response);
//								}
//							});
//						}
//					}
//				}
//				else
//				{
//					onError("Received invalid signature.");
//				}
//			}
//		}, onError, retryOptions);
//	}

//	private TimeSpan GetServerTimeDelta(HttpResponse response)
//	{
//		string value = null;
//		response.Headers.TryGetValue("X-Roofdog-ServerTime".ToUpper(), out value);
//		if (value != null && DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime result))
//		{
//			return DateTime.UtcNow - result;
//		}
//		return TimeSpan.Zero;
//	}

//	private IEnumerator CheckOnlineStatusCR()
//	{
//		int waitSeconds = 1;
//		int maxWaitSeconds = 32;
//		while (_onlineStatus == OnlineStatus.offline)
//		{
//			yield return new WaitForSeconds(waitSeconds);
//			waitSeconds = Math.Min(waitSeconds * 2, maxWaitSeconds);
//			CheckOnlineStatus();
//		}
//	}

//	private void CheckOnlineStatus()
//	{
//		AutoSingleton<HttpClient>.Instance.Get(GetUrl("/rest/ping"), delegate(HttpResponse response)
//		{
//			if (_onlineStatus != OnlineStatus.online && response.StatusCode / 100 == 2 && response.GetStringBody() == "pong")
//			{
//				_onlineStatus = OnlineStatus.online;
//				if (_onlineListener != null)
//				{
//					OnlineStatusChangeEvent obj = default(OnlineStatusChangeEvent);
//					obj.IsOnline = true;
//					obj.WasOffline = true;
//					obj.BackendTimeDelta = GetServerTimeDelta(response);
//					_onlineListener(obj);
//				}
//			}
//		}, delegate
//		{
//		}, HttpRetryOptions.NO_RETRY);
//	}

//	private string GetSignedHeaderValue(Hashtable data)
//	{
//		string text = data.toJson();
//		byte[] bytes = new UTF8Encoding().GetBytes(text);
//		string str = ComputeSignature(bytes);
//		return str + "," + text;
//	}

//	private string ComputeSignature(byte[] bytes)
//	{
//		return Convert.ToBase64String(new HMACSHA1(secret).ComputeHash(bytes));
//	}
//}
