using UnityEngine;

internal class MetroWidgetPlayerChallenge : MetroWidgetCheckboxListItem
{
	public static MetroWidgetPlayerChallenge Create(User user, int index, Color evenRowColor, Color oddRowColor)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetPlayerChallenge).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroWidgetPlayerChallenge>().Setup(user, index, evenRowColor, oddRowColor);
	}

	public MetroWidgetPlayerChallenge Setup(User user, int index, Color evenRowColor, Color oddRowColor)
	{
		base.Id = user._id;
		Color color = (index % 2 != 0) ? oddRowColor : evenRowColor;
		_button = MetroButtonToggle.Create(delegate
		{
			OnToggle();
		});
		_button.AddSolidBackground().SetColor(color);
		Add(_button);
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal);
		_button.Add(metroWidget);
		metroWidget.Add(MetroSpacer.Create());
		MetroMenu.AddKeyNavigationBehaviour(_button, MetroSkin.ButtonColorAlert1, color);
		if (PictureManager.IsPictureLoaded(base.Id))
		{
			WidgetPlayerPicture child = WidgetPlayerPicture.Create(base.Id, 0.1f);
			metroWidget.Add(child);
		}
		else
		{
			metroWidget.Add(MetroSpacer.Create());
		}
		MetroLabel metroLabel = MetroLabel.Create(user._displayName);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetAlignment(MetroAlign.Left);
		metroLabel.SetMass(6f);
		metroWidget.Add(metroLabel);
		_checkbox = CreateCheckboxIcon((!_button.State()) ? MetroSkin.IconUnchecked : MetroSkin.IconChecked);
		metroWidget.Add(_checkbox);
		metroWidget.Add(MetroSpacer.Create());
		return this;
	}
}
