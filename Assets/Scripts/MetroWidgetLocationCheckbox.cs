using UnityEngine;

public class MetroWidgetLocationCheckbox : MetroWidgetCheckboxListItem
{
	public static MetroWidgetLocationCheckbox Create(Location location, int index, Color evenRowColor, Color oddRowColor, bool isSelected)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetLocationCheckbox).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroWidgetLocationCheckbox>().Setup(location, index, evenRowColor, oddRowColor, isSelected);
	}

	public MetroWidgetLocationCheckbox Setup(Location location, int index, Color evenRowColor, Color oddRowColor, bool isSelected)
	{
		base.Id = location.name;
		Color color = (index % 2 != 0) ? oddRowColor : evenRowColor;
		_button = MetroButtonToggle.Create(delegate
		{
			OnToggle();
		});
		_button.AddSolidBackground().SetColor(color);
		_button.Toggle(isSelected);
		Add(_button);
		MetroWidget metroWidget = MetroLayout.Create(Direction.horizontal);
		_button.Add(metroWidget);
		MetroMenu.AddKeyNavigationBehaviour(_button, MetroSkin.ButtonColorAlert1, color);
		MetroLabel metroLabel = MetroLabel.Create(location.DisplayName);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetAlignment(MetroAlign.Left);
		metroWidget.Add(metroLabel);
		metroWidget.Add(MetroSpacer.Create(5f));
		_checkbox = CreateCheckboxIcon((!_button.State()) ? MetroSkin.IconUnchecked : MetroSkin.IconChecked);
		metroWidget.Add(_checkbox);
		metroWidget.Add(MetroSpacer.Create());
		return this;
	}
}
