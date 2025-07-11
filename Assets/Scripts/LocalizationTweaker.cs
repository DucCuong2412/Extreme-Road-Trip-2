using UnityEngine;

[AddComponentMenu("Localization/LocalizationTweaker")]
public class LocalizationTweaker : MonoBehaviour
{
	public LanguageType _language = LanguageType.french;

	public L10nTweakAction _action;

	public float _number = 0.8f;

	public void Awake()
	{
		if (_language == AutoSingleton<LocalizationManager>.Instance.Language && _action == L10nTweakAction.scale)
		{
			base.transform.localScale *= _number;
		}
	}
}
