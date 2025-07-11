public class StringWrapper
{
	public static string WrapString(string text, int limit, string split)
	{
		if (text.Length < limit)
		{
			return text;
		}
		int length = text.Length;
		int num = 0;
		int num2;
		while ((num2 = text.Substring(0, length).LastIndexOf(split)) != -1)
		{
			if (num2 < limit)
			{
				num = num2 + split.Length;
				break;
			}
			length = num2;
		}
		if (num == 0)
		{
			return text;
		}
		return text.Substring(0, num) + "\n" + WrapString(text.Substring(num), limit, split);
	}
}
