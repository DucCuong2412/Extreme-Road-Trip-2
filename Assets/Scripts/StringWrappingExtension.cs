public static class StringWrappingExtension
{
	public static string Wrap(this string s, int limit)
	{
		string[] array = s.Split('\n');
		string text = string.Empty;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (text != string.Empty)
			{
				text += "\n";
			}
			text += StringWrapper.WrapString(text2, limit, " ");
		}
		return text;
	}
}
