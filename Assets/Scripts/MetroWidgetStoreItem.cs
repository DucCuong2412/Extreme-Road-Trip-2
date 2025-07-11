using UnityEngine;

public class MetroWidgetStoreItem : MetroWidget
{
	private StoreItem _item;

	private MetroIcon _iconItem;

	private MetroIcon _iconSold;

	private MetroLabel _descriptionLabel;

	private MetroWidgetPrice _price;

	private Color _textColor;

	public static MetroWidgetStoreItem Create(StoreItem item, Color textColor)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetStoreItem).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetStoreItem metroWidgetStoreItem = gameObject.AddComponent<MetroWidgetStoreItem>();
		metroWidgetStoreItem.Setup(item, textColor);
		return metroWidgetStoreItem;
	}

	private void Setup(StoreItem item, Color textColor)
	{
		_textColor = textColor;
		_item = item;
		Color color = (!item.IsPurchased()) ? Color.white : new Color(1f, 1f, 1f, 0.5f);
		if (item.Type == StoreItemType.permanentCoinDoubler)
		{
			MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
			Add(metroLayout);
			metroLayout.Add(MetroSpacer.Create(0.5f));
			_iconItem = MetroIcon.Create(item.AssetName);
			metroLayout.Add(_iconItem);
			_iconItem.SetMass(3f);
			_iconItem.SetScale(0.5f);
			_iconItem.SetColor(color);
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
			metroLayout.Add(metroLayout2);
			metroLayout2.SetMass(10f);
			string content = item.Description.Localize().Wrap(25);
			_descriptionLabel = MetroLabel.Create(content);
			metroLayout2.Add(_descriptionLabel);
			_descriptionLabel.SetFont(MetroSkin.MediumFont);
			_descriptionLabel.SetMass(2f);
			_price = MetroWidgetPrice.Create(item.Price);
			metroLayout2.Add(_price);
			metroLayout.Add(MetroSpacer.Create(0.5f));
			return;
		}
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.vertical);
		Add(metroLayout3);
		metroLayout3.Add(MetroSpacer.Create());
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout4.SetMass(3f);
		metroLayout3.Add(metroLayout4);
		_iconItem = MetroIcon.Create(item.AssetName);
		metroLayout4.Add(_iconItem);
		_iconItem.SetScale(0.7f);
		_iconItem.SetColor(color);
		if (item.Type == StoreItemType.powerups)
		{
			MetroIcon metroIcon = MetroIcon.Create(MetroSkin.IconStoreTag);
			metroIcon.SetScale(0.8f);
			metroIcon.Add(MetroLabel.Create("x" + item.Quantity.ToString()).SetFont(MetroSkin.MediumFont).AddOutline());
			metroIcon.transform.parent = _iconItem.transform;
			metroIcon.Reflow();
			Bounds bounds = RendererBounds.ComputeBounds(_iconItem.transform);
			Transform transform = metroIcon.transform;
			Vector3 extents = bounds.extents;
			transform.localPosition = new Vector3(0f, 0f - extents.y * 0.5f, -0.1f);
		}
		metroLayout3.Add(MetroSpacer.Create());
		_price = MetroWidgetPrice.Create(item.Price);
		metroLayout3.Add(_price);
		metroLayout3.Add(MetroSpacer.Create());
	}

	public override void Layout(Rect zone)
	{
		UpdateVisual();
		base.Layout(zone);
	}

	private void UpdateVisual()
	{
		Color color = (!_item.IsPurchased()) ? Color.white : new Color(1f, 1f, 1f, 0.5f);
		Color color2 = (!_item.IsPurchased()) ? _textColor : new Color(0f, 0f, 0f, 0.5f);
		_iconItem.SetColor(color);
		if (_descriptionLabel != null)
		{
			_descriptionLabel.SetColor(color2);
		}
		if (_price != null)
		{
			_price.SetTextColor(color2);
		}
		if (_item.IsPurchased() && _iconSold == null)
		{
			_iconSold = MetroIcon.Create((AutoSingleton<LocalizationManager>.Instance.Language != 0) ? MetroSkin.IconStoreSoldFrench : MetroSkin.IconStoreSoldEnglish);
			_iconSold.SetScale(1.3f);
			_iconSold.transform.parent = base.transform;
			_iconSold.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
	}
}
