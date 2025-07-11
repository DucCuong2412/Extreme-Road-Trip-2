using System.Collections;
using UnityEngine;

public class Price
{
	public int Amount
	{
		get;
		set;
	}

	public Currency Currency
	{
		get;
		set;
	}

	public Price(string json)
		: this(json.hashtableFromJson())
	{
	}

	public Price(Hashtable pricing)
	{
		Amount = JsonUtil.ExtractInt(pricing, "amount", 9999);
		Currency = EnumUtil.Parse(JsonUtil.ExtractString(pricing, "currency", "bucks"), Currency.bucks);
	}

	public Price(int amount, Currency currency)
	{
		Amount = amount;
		Currency = currency;
	}

	public bool IsFree()
	{
		return Amount == 0;
	}

	public bool IsCoins()
	{
		return Currency == Currency.coins;
	}

	public bool IsBucks()
	{
		return Currency == Currency.bucks;
	}

	public bool IsPrestigeTokens()
	{
		return Currency == Currency.prestigeTokens;
	}

	public bool IsRealMoney()
	{
		return Currency == Currency.realMoney;
	}

	public bool IsCoalOre()
	{
		return Currency == Currency.coalOre;
	}

	public bool IsDiamondOre()
	{
		return Currency == Currency.diamondOre;
	}

	public bool IsHealBlock()
	{
		return Currency == Currency.healBlock;
	}

	public bool IsRockBlock()
	{
		return Currency == Currency.rockBlock;
	}

	public bool IsDirtBlock()
	{
		return Currency == Currency.dirtBlock;
	}

	public bool IsCrateBlock()
	{
		return Currency == Currency.crateBlock;
	}

	public bool IsFishMinnow()
	{
		return Currency == Currency.fishMinnow;
	}

	public bool IsFishPeamouth()
	{
		return Currency == Currency.fishPeamouth;
	}

	public bool IsFishTrout()
	{
		return Currency == Currency.fishTrout;
	}

	public bool IsFishBarracuda()
	{
		return Currency == Currency.fishBarracuda;
	}

	public bool IsFishClownfish()
	{
		return Currency == Currency.fishClownfish;
	}

	public bool IsFishGreatWhiteShark()
	{
		return Currency == Currency.fishGreatWhiteShark;
	}

	public bool IsPocketMineCurrency()
	{
		return IsCoalOre() || IsCrateBlock() || IsDiamondOre() || IsDirtBlock() || IsHealBlock() || IsRockBlock();
	}

	public bool IsPocketMine3Currency()
	{
		return Currency == Currency.monsterGreenOrc || Currency == Currency.monsterSlime || Currency == Currency.monsterBlackSpider || Currency == Currency.monsterGoblin || Currency == Currency.monsterSentinel || Currency == Currency.monsterBerserker;
	}

	public bool IsFishingCurrency()
	{
		return IsFishMinnow() || IsFishPeamouth() || IsFishTrout() || IsFishBarracuda() || IsFishClownfish() || IsFishGreatWhiteShark();
	}

	public static Currency ConvertStringToCurrency(string priceType)
	{
		switch (priceType)
		{
		case "bucks":
			return Currency.bucks;
		case "coins":
			return Currency.coins;
		case "prestige":
			return Currency.prestigeTokens;
		case "realMoney":
			return Currency.realMoney;
		case "coalOre":
			return Currency.coalOre;
		case "diamondOre":
			return Currency.diamondOre;
		case "healBlock":
			return Currency.healBlock;
		case "rockBlock":
			return Currency.rockBlock;
		case "dirtBlock":
			return Currency.dirtBlock;
		case "crateBlock":
			return Currency.crateBlock;
		case "distance":
			return Currency.distance;
		case "fishPeamouth":
			return Currency.fishPeamouth;
		case "fishMinnow":
			return Currency.fishMinnow;
		case "fishTrout":
			return Currency.fishTrout;
		case "fishBarracuda":
			return Currency.fishBarracuda;
		case "fishClownfish":
			return Currency.fishClownfish;
		case "fishGreatWhiteShark":
			return Currency.fishGreatWhiteShark;
		case "monsterGreenOrc":
			return Currency.monsterGreenOrc;
		case "monsterSlime":
			return Currency.monsterSlime;
		case "monsterBlackSpider":
			return Currency.monsterBlackSpider;
		case "monsterGoblin":
			return Currency.monsterGoblin;
		case "monsterSentinel":
			return Currency.monsterSentinel;
		case "monsterBerserker":
			return Currency.monsterBerserker;
		default:
			UnityEngine.Debug.LogError("Expected coins or bucks as currency");
			return Currency.coins;
		}
	}

	public string GetCurrencyDisplayName()
	{
		switch (Currency)
		{
		case Currency.bucks:
			return "Bucks".Localize();
		case Currency.coins:
			return "Coins".Localize();
		case Currency.prestigeTokens:
			return "Prestiges".Localize();
		case Currency.coalOre:
			return "Coal".Localize();
		case Currency.diamondOre:
			return "Diamonds".Localize();
		case Currency.healBlock:
			return "Pick Repairs".Localize();
		case Currency.rockBlock:
			return "Rock blocks".Localize();
		case Currency.dirtBlock:
			return "Dirt blocks".Localize();
		case Currency.crateBlock:
			return "Crates".Localize();
		case Currency.distance:
			return "Meters".Localize();
		case Currency.fishMinnow:
			return "Minnow".Localize();
		case Currency.fishPeamouth:
			return "Peamouth".Localize();
		case Currency.fishTrout:
			return "Trout".Localize();
		case Currency.fishBarracuda:
			return "Barracuda".Localize();
		case Currency.fishClownfish:
			return "Clownfish".Localize();
		case Currency.fishGreatWhiteShark:
			return "Great White Shark".Localize();
		case Currency.monsterGreenOrc:
			return "Green Orc".Localize();
		case Currency.monsterSlime:
			return "Slime".Localize();
		case Currency.monsterBlackSpider:
			return "Black Spider".Localize();
		case Currency.monsterGoblin:
			return "Goblin".Localize();
		case Currency.monsterSentinel:
			return "Sentinel".Localize();
		case Currency.monsterBerserker:
			return "Berserker".Localize();
		default:
			UnityEngine.Debug.LogError("Expected coins or bucks as currency");
			return "NOT";
		}
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["amount"] = Amount;
		hashtable["currency"] = Currency.ToString();
		return hashtable;
	}

	public override string ToString()
	{
		string str = (Currency != Currency.distance) ? string.Empty : (" " + GetCurrencyDisplayName());
		return Amount.ToString() + str;
	}
}
