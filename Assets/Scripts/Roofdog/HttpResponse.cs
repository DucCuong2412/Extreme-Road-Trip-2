using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Roofdog
{
	public class HttpResponse
	{
		private static Regex charsetRegex = new Regex("charset\\s*=\\s*\\\"?([^;\\\"\\s]+)\\\"?");

		public HttpMethod RequestMethod
		{
			get;
			private set;
		}

		public string RequestUrl
		{
			get;
			private set;
		}

		public int StatusCode
		{
			get;
			private set;
		}

		public Dictionary<string, string> Headers
		{
			get;
			private set;
		}

		public byte[] Body
		{
			get;
			private set;
		}

		public HttpResponse(HttpMethod requestMethod, string requestUrl, int statusCode, Dictionary<string, string> headers, byte[] body)
		{
			RequestMethod = requestMethod;
			RequestUrl = requestUrl;
			StatusCode = statusCode;
			Headers = headers;
			Body = body;
		}

		public string GetStringBody()
		{
			if (Body == null || Body.Length == 0)
			{
				return null;
			}
			Encoding encoding = null;
			string value = null;
			Headers.TryGetValue("CONTENT-TYPE", out value);
			if (!string.IsNullOrEmpty(value))
			{
				Match match = charsetRegex.Match(value);
				if (match.Success)
				{
					string value2 = match.Groups[1].Value;
					value2 = value2.Trim().ToLower();
					try
					{
						encoding = Encoding.GetEncoding(value2);
					}
					catch (ArgumentException)
					{
					}
				}
			}
			if (encoding == null)
			{
				encoding = new UTF8Encoding();
			}
			return encoding.GetString(Body);
		}
	}
}
