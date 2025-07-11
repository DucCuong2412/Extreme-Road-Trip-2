using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Localization/LocalizationManager")]
public class LocalizationManager : AutoSingleton<LocalizationManager>
{
	private Dictionary<string, string> _french = new Dictionary<string, string>();

	public LanguageType Language => AutoSingleton<PersistenceManager>.Instance.Language;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		string text = (Resources.Load("Localization") as TextAsset).text;
		string[] array = text.Split('\n');
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			string[] array3 = text2.Split('\t');
			if (array3.Length >= 3)
			{
				_french[array3[0].Replace("\\n", "\n")] = array3[2].Replace("\\n", "\n");
			}
		}
		base.OnAwake();
	}

	public void ChangeLanguage(LanguageType l)
	{
		AutoSingleton<PersistenceManager>.Instance.Language = l;
	}

	public void ToggleLanguage()
	{
		if (Language == LanguageType.english)
		{
			ChangeLanguage(LanguageType.french);
		}
		else
		{
			ChangeLanguage(LanguageType.english);
		}
	}

	public string Localize(string s)
	{
		if (s == null || s.Equals(string.Empty))
		{
			return s;
		}
		switch (Language)
		{
		case LanguageType.english:
			return s;
		case LanguageType.french:
			if (_french.ContainsKey(s))
			{
				return _french[s];
			}
			break;
		}
		return s;
	}

	public void DumpStrings()
	{
	}

	public string GetLanguagePrefix()
	{
		LanguageType language = Language;
		if (language != 0 && language == LanguageType.french)
		{
			return "FR";
		}
		return "EN";
	}
}
