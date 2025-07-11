using System;

[Serializable]
public struct AndroidSkuDetails
{
	public string title;

	public string price;

	public string type;

	public string description;

	public double price_amount_micros;

	public string price_currency_code;

	public string productId;

	public AndroidSkuDetails(string title, string price, string type, string description, double price_amount_micros, string price_currency_code, string productId)
	{
		this.title = title;
		this.price = price;
		this.type = type;
		this.description = description;
		this.price_amount_micros = price_amount_micros;
		this.price_currency_code = price_currency_code;
		this.productId = productId;
	}
}
