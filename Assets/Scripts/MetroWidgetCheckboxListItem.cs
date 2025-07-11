using System;

public class MetroWidgetCheckboxListItem : MetroWidget
{
	protected MetroIcon _checkbox;

	protected MetroButtonToggle _button;

	public string Id
	{
		get;
		protected set;
	}

	public Action OnItemToggle
	{
		get;
		set;
	}

	protected void UpdateCheckbox()
	{
		MetroIcon checkbox = _checkbox;
		MetroIcon metroIcon = CreateCheckboxIcon((!_button.State()) ? MetroSkin.IconUnchecked : MetroSkin.IconChecked);
		_checkbox.Replace(metroIcon).Destroy();
		_checkbox = metroIcon;
		_checkbox.SetLayer(checkbox.GetLayer());
		_checkbox.Parent.Reflow();
	}

	protected MetroIcon CreateCheckboxIcon(string iconName)
	{
		MetroIcon metroIcon = MetroIcon.Create(iconName);
		metroIcon.SetScale(0.5f);
		return metroIcon;
	}

	public bool IsSelected()
	{
		return _button.State();
	}

	public void Select(bool selected)
	{
		_button.Toggle(selected);
		UpdateCheckbox();
	}

	public void OnToggle()
	{
		UpdateCheckbox();
		if (OnItemToggle != null)
		{
			OnItemToggle();
		}
	}
}
