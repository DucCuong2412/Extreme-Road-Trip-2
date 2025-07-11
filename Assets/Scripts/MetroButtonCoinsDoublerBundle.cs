using UnityEngine;

public class MetroButtonCoinsDoublerBundle : MetroButton
{
	public static MetroButtonCoinsDoublerBundle Create(StoreItem item)
	{
		GameObject gameObject = new GameObject(typeof(MetroButtonCoinsDoublerBundle).ToString());
		return gameObject.AddComponent<MetroButtonCoinsDoublerBundle>().Setup(item);
	}

	private MetroButtonCoinsDoublerBundle Setup(StoreItem coinDoublerItem)
	{
		AddSlice9Background(MetroSkin.Slice9ButtonGreen);
		MetroWidget metroWidget = Add(MetroLayout.Create(Direction.vertical));
		metroWidget.Add(MetroSpacer.Create());
		MetroWidget metroWidget2 = metroWidget.Add(MetroLayout.Create(Direction.horizontal));
		metroWidget2.SetMass(5f);
		metroWidget2.Add(MetroSpacer.Create(0.25f));
		MetroIcon metroIcon = MetroIcon.Create(coinDoublerItem.AssetName);
		metroIcon.SetScale(0.4f);
		metroWidget2.Add(metroIcon);
		MetroWidget metroWidget3 = metroWidget2.Add(MetroLayout.Create(Direction.vertical));
		metroWidget3.SetMass(2.5f);
		MetroLabel metroLabel = MetroLabel.Create(coinDoublerItem.Description.Localize().Wrap(25));
		metroWidget3.Add(metroLabel);
		metroLabel.SetFont(MetroSkin.VerySmallFont);
		metroLabel.SetMass(2f);
		MetroWidgetPrice child = MetroWidgetPrice.Create(coinDoublerItem.Price);
		metroWidget3.Add(child);
		metroWidget2.Add(MetroSpacer.Create(0.25f));
		metroWidget.Add(MetroSpacer.Create());
		base.OnButtonClicked += delegate
		{
			if (coinDoublerItem.IsPurchased())
			{
				MetroPopupMessage page = MetroMenuPage.Create<MetroPopupMessage>().Setup("STORE", "You have already purchased this item.");
				AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
			}
			else
			{
				coinDoublerItem.TryPurchase();
			}
		};
		base.OnKeyFocusGained += delegate
		{
			AddSlice9Background(MetroSkin.Slice9ButtonRed);
			Berp();
		};
		base.OnKeyFocusLost += delegate
		{
			AddSlice9Background(MetroSkin.Slice9ButtonGreen);
		};
		return this;
	}
}
