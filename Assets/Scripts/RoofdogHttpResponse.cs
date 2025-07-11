// using Roofdog;
using System;
using System.Collections.Generic;
using System.Text;

public class RoofdogHttpResponse
{
	private int StatusCode;

	private Dictionary<string, string> Headers;

	private byte[] Body;

	public RoofdogHttpResponse(int statusCode, Dictionary<string, string> headers, byte[] body)
	{
		StatusCode = statusCode;
		Headers = headers;
		Body = body;
	}

	public static RoofdogHttpResponse FromBytes(byte[] bytes)
	{
		UTF8Encoding utf = new UTF8Encoding();
		int idx = 0;
		string text = readUtf8Line(bytes, ref idx, utf);
		string value = text.Substring(0, text.IndexOf(' '));
		int statusCode = Convert.ToInt32(value);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string text2;
		while ((text2 = readUtf8Line(bytes, ref idx, utf)).Length > 0)
		{
			int num = text2.IndexOf(": ");
			string key = text2.Substring(0, num);
			string text4 = dictionary[key] = text2.Substring(num + 2);
		}
		int num2 = bytes.Length - idx;
		byte[] array = (num2 != 0) ? new byte[num2] : null;
		if (array != null)
		{
			Array.Copy(bytes, idx, array, 0, num2);
		}
		return new RoofdogHttpResponse(statusCode, dictionary, array);
	}

	private static string readUtf8Line(byte[] bytes, ref int idx, UTF8Encoding utf8)
	{
		int num = idx;
		while (idx < bytes.Length && bytes[idx] != 10)
		{
			idx++;
		}
		idx++;
		int count = idx - num - 1;
		return utf8.GetString(bytes, num, count);
	}

	// public HttpResponse ToHttpResponse(HttpMethod requestMethod, string requestUrl, Dictionary<string, string> headersToMerge)
	// {
	// 	Dictionary<string, string> dictionary = new Dictionary<string, string>();
	// 	foreach (string key in headersToMerge.Keys)
	// 	{
	// 		dictionary[key.ToUpper()] = headersToMerge[key];
	// 	}
	// 	foreach (string key2 in Headers.Keys)
	// 	{
	// 		dictionary[key2.ToUpper()] = Headers[key2];
	// 	}
	// 	return new HttpResponse(requestMethod, requestUrl, StatusCode, dictionary, Body);
	// }
}
