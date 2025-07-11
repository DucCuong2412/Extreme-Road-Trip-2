using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class BackendCrashLogApiClient
{
	public static void SendCrashLog(long timestamp, string crashData, Action onSuccess = null, Action<string> onError = null)
	{
		//string jsonString = new CrashLogJson(timestamp, crashData).ToJsonData().toJson();
		//AutoSingleton<BackendApiClient>.Instance.PostJson("/rest/crashLogs", null, jsonString, delegate
		//{
		//	if (onSuccess != null)
		//	{
		//		onSuccess();
		//	}
		//}, onError, new int[1]
		//{
		//	204
		//}, HttpRetryOptions.DEFAULT_RETRY);
	}
}

public class BackendApiClient
{
    //public void PostJson(string endpoint, object queryParams, string jsonData, Action onSuccess, Action<string> onError, int[] validStatusCodes, HttpRetryOptions retryOptions)
    //{
    //    // Implementation of POST request logic
    //    // This is a basic implementation - expand based on your needs
    //    UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
    //    request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
    //    request.downloadHandler = new DownloadHandlerBuffer();
    //    request.SetRequestHeader("Content-Type", "application/json");

    //    // Send request and handle response
    //    var operation = request.SendWebRequest();
    //    operation.completed += (AsyncOperation op) =>
    //    {
    //        if (Array.IndexOf(validStatusCodes, (int)request.responseCode) != -1)
    //        {
    //            onSuccess?.Invoke();
    //        }
    //        else
    //        {
    //            onError?.Invoke(request.error);
    //        }
    //        request.Dispose();
    //    };
    //}
}
