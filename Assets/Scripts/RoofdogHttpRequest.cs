using System.Collections;
using System.Text;

public class RoofdogHttpRequest
{
	private string Method;

	private string Path;

	private Hashtable Headers;

	private byte[] Body;

	public RoofdogHttpRequest(string method, string path, Hashtable headers, byte[] body)
	{
		Method = method;
		Path = path;
		Headers = headers;
		Body = body;
	}

	public byte[] ToBytes()
	{
		string text = Method + " " + Path + "\n";
		if (Headers != null)
		{
			foreach (object key in Headers.Keys)
			{
				string text2 = Headers[key] as string;
				text2 = text2.Replace("\n", string.Empty);
				string text3 = text;
				text = text3 + key + ": " + text2 + "\n";
			}
		}
		text += "\n";
		byte[] bytes = new UTF8Encoding().GetBytes(text);
		if (Body == null || Body.Length == 0)
		{
			return bytes;
		}
		byte[] array = new byte[bytes.Length + Body.Length];
		bytes.CopyTo(array, 0);
		Body.CopyTo(array, bytes.Length);
		return array;
	}
}
