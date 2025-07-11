using System;
using System.Text;

public class StringUtil
{
	public static string DecodeHtmlChars(string text)
	{
		string[] array = text.Split(new string[1]
		{
			"&#x"
		}, StringSplitOptions.None);
		for (int i = 1; i < array.Length; i++)
		{
			int num = array[i].IndexOf(';');
			string value = array[i].Substring(0, num);
			try
			{
				int num2 = Convert.ToInt32(value, 16);
				array[i] = (char)num2 + array[i].Substring(num + 1);
			}
			catch
			{
			}
		}
		return string.Join(string.Empty, array);
	}

	public static string Trunc(string toTrunc, int maxLenght)
	{
		if (toTrunc != null && toTrunc.Length > maxLenght)
		{
			return toTrunc.Substring(0, maxLenght) + ".";
		}
		return toTrunc;
	}

	public static string Cleanup(string toClean)
	{
		if (toClean != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			char[] array = toClean.ToCharArray();
			foreach (char c in array)
			{
				if (!char.IsControl(c) && !char.IsSurrogate(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
		return toClean;
	}
}
