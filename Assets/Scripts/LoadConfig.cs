using System.Collections;
using UnityEngine;

public class LoadConfig
{
	public virtual IEnumerator PreLoad()
	{
		AutoSingleton<PauseManager>.Instance.Resume();
		AutoSingleton<FadingManager>.Instance.FadeOut();
		yield return new WaitForSeconds(0.3f);
	}

	public virtual IEnumerator PostLoad()
	{
		AutoSingleton<FadingManager>.Instance.FadeIn();
		yield return new WaitForSeconds(0.3f);
	}

	public virtual IEnumerator Load()
	{
		yield return null;
	}

	public virtual IEnumerator Unload()
	{
		yield return null;
	}
}
