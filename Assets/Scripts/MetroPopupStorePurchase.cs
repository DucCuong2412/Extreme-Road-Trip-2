using UnityEngine;

public class MetroPopupStorePurchase : MetroPopupPage
{
	public MetroPopupStorePurchase Setup(StoreItem item)
	{
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout metroLayout = MetroLayout.Create("content", Direction.vertical);
		metroLayout.SetPadding(2f);
		Add(metroLayout);
		metroLayout.Add(MetroSpacer.Create(0.1f));
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout2);
		MetroLabel child = MetroLabel.Create("STORE");
		metroLayout2.Add(child);
		metroLayout.Add(MetroSpacer.Create(0.1f));
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout3);
		metroLayout.Add(MetroSpacer.Create(0.1f));
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		MetroLabel metroLabel = MetroLabel.Create("Do you wish to purchase this item?");
		metroLayout3.Add(metroLabel);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetLineSpacing(0f);
		metroLayout3.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroWidgetStoreItem.Create(item, Color.white).SetMass(2.5f));
		metroLayout.Add(MetroSpacer.Create(0.1f));
		MetroLayout metroLayout4 = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(metroLayout4);
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		MetroButton metroButton = MetroButton.Create("PURCHASE");
		metroLayout4.Add(metroButton);
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		metroButton.OnButtonClicked += delegate
		{
			if (item.TryPurchase())
			{
				AutoSingleton<MetroMenuStack>.Instance.Pop();
			}
		};
		metroLayout4.Add(MetroSpacer.Create(0.5f));
		metroLayout.Add(MetroSpacer.Create(0.3f));
		return this;
	}

	protected override Rect ViewRect()
	{
		return new Rect((0f - base.Camera.HalfScreenWidth) * 0.95f, (0f - base.Camera.HalfScreenHeight) * 0.9f, base.Camera.ScreenWidth * 0.95f, base.Camera.ScreenHeight * 0.9f);
	}
}
