using System.Collections;
using UnityEngine;

public class MetroTestSpinner : MetroMenuPage
{
	private MetroSpinner _spinner;

	protected override void OnStart()
	{
		MetroLayout metroLayout = MetroLayout.Create("center", Direction.vertical);
		Add(metroLayout);
		MetroLabel child = MetroLabel.Create("CLICK TO TEST SPINNER");
		metroLayout.Add(child);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		metroLayout2.Add(MetroSpacer.Create());
		MetroButton button = MetroButton.Create("SPIN IT");
		metroLayout2.Add(button);
		button.OnButtonClicked += delegate
		{
			_spinner = MetroSpinner.Create(MetroSkin.Spinner);
			button.Add(_spinner);
			button.SetActive(active: false);
			button.Reflow();
			ButtonClick();
		};
		metroLayout2.Add(MetroSpacer.Create());
		base.OnStart();
	}

	private void ButtonClick()
	{
		StartCoroutine(ButtonClickCR());
	}

	private IEnumerator ButtonClickCR()
	{
		yield return new WaitForSeconds(1f);
		if (_spinner != null)
		{
			((MetroButton)_spinner.Parent).SetActive(active: true);
			_spinner.Destroy();
			_spinner = null;
		}
	}
}
