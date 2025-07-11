using System.Collections.Generic;
using UnityEngine;

public class MetroWidgetPrice : MetroWidget
{
	private MetroLabel _label;

	private MetroIcon _icon;

	public static MetroWidgetPrice Create(Price price, MetroFont font = null)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetPrice).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		MetroWidgetPrice metroWidgetPrice = gameObject.AddComponent<MetroWidgetPrice>();
		List<Price> list = new List<Price>();
		list.Add(price);
		metroWidgetPrice.Setup(list, font ?? MetroSkin.MediumFont);
		return metroWidgetPrice;
	}

	public static MetroWidgetPrice Create(List<Price> prices, MetroFont font = null)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetPrice).ToString());
		gameObject.transform.localPosition = Vector3.zero;
		MetroWidgetPrice metroWidgetPrice = gameObject.AddComponent<MetroWidgetPrice>();
		metroWidgetPrice.Setup(prices, font ?? MetroSkin.MediumFont);
		return metroWidgetPrice;
	}

	public void Setup(List<Price> prices, MetroFont font)
	{
		MetroWidget metroWidget = MetroGlue.Create(Direction.horizontal);
		Add(metroWidget);
		foreach (Price price in prices)
		{
			string priceIcon = GetPriceIcon(price);
			if (priceIcon != string.Empty)
			{
				_icon = MetroIcon.Create(priceIcon);
				metroWidget.Add(_icon);
			}
			_label = MetroLabel.Create(string.Empty);
			metroWidget.Add(_label);
			_label.SetFont(font);
			_label.SetText(price.ToString());
		}
	}

	private string GetPriceIcon(Price price)
	{
		string result = string.Empty;
		switch (price.Currency)
		{
		case Currency.bucks:
			result = MetroSkin.IconBucks;
			break;
		case Currency.coalOre:
			result = MetroSkin.IconCoal;
			break;
		case Currency.coins:
			result = MetroSkin.IconCoin;
			break;
		case Currency.crateBlock:
			result = MetroSkin.IconCrate2;
			break;
		case Currency.diamondOre:
			result = MetroSkin.IconDiamond;
			break;
		case Currency.dirtBlock:
			result = MetroSkin.IconDirt;
			break;
		case Currency.healBlock:
			result = MetroSkin.IconHeal;
			break;
		case Currency.prestigeTokens:
			result = MetroSkin.IconPrestigeToken;
			break;
		case Currency.rockBlock:
			result = MetroSkin.IconRock;
			break;
		case Currency.fishPeamouth:
			result = MetroSkin.IconPeamouth;
			break;
		case Currency.fishMinnow:
			result = MetroSkin.IconMinnow;
			break;
		case Currency.fishTrout:
			result = MetroSkin.IconTrout;
			break;
		case Currency.fishBarracuda:
			result = MetroSkin.IconBarracuda;
			break;
		case Currency.fishClownfish:
			result = MetroSkin.IconClownfish;
			break;
		case Currency.fishGreatWhiteShark:
			result = MetroSkin.IconGreatWhiteShark;
			break;
		case Currency.realMoney:
		case Currency.distance:
			result = string.Empty;
			break;
		case Currency.monsterGreenOrc:
			result = MetroSkin.IconGreenOrc;
			break;
		case Currency.monsterSlime:
			result = MetroSkin.IconPinky;
			break;
		case Currency.monsterBlackSpider:
			result = MetroSkin.IconBlackSpider;
			break;
		case Currency.monsterGoblin:
			result = MetroSkin.IconGoblin;
			break;
		case Currency.monsterSentinel:
			result = MetroSkin.IconSentinel;
			break;
		case Currency.monsterBerserker:
			result = MetroSkin.IconBerserker;
			break;
		default:
			UnityEngine.Debug.Log("Currency not supported! " + price.Currency.ToString());
			break;
		}
		return result;
	}

	public MetroWidgetPrice SetFont(MetroFont font)
	{
		if (_label != null)
		{
			_label.SetFont(font);
		}
		return this;
	}

	public MetroWidgetPrice SetIconScale(float scale)
	{
		if (_icon != null)
		{
			_icon.SetScale(scale);
		}
		return this;
	}

	public void SetTextColor(Color color)
	{
		if (_label != null)
		{
			_label.SetColor(color);
		}
	}

	public void AddOutline()
	{
		if (_label != null)
		{
			_label.AddOutline();
		}
	}

	public void UpdatePrice(Price price)
	{
		if (_label != null)
		{
			_label.SetText(price.ToString());
		}
	}
}
