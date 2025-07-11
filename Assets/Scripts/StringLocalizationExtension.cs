public static class StringLocalizationExtension
{
	public static string Localize(this string s)
	{
		return AutoSingleton<LocalizationManager>.Instance.Localize(s);
	}
}
