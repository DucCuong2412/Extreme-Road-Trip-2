using System.Collections;

public class SpecialOffer
{
	public string OfferId;

	public Car Car;

	public Price Price;

	public string Title;

	public string Pitch;

	public SpecialOffer(string offerId, Car car, Price price, string title, string pitch)
	{
		OfferId = offerId;
		Car = car;
		Price = price;
		Title = title;
		Pitch = pitch;
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["OfferId"] = OfferId;
		hashtable["CarId"] = Car.Id;
		hashtable["Price"] = Price.ToJsonData();
		hashtable["Title"] = Title;
		hashtable["Pitch"] = Pitch;
		return hashtable;
	}

	public static SpecialOffer FromJsonData(Hashtable ht)
	{
		string text = JsonUtil.ExtractString(ht, "OfferId", null);
		string text2 = JsonUtil.ExtractString(ht, "CarId", null);
		if (text != null && text2 != null)
		{
			Car car = AutoSingleton<CarManager>.Instance.GetCar(text2);
			Price price = new Price(JsonUtil.ExtractHashtable(ht, "Price", new Price(10, Currency.bucks).ToJsonData()));
			string title = JsonUtil.ExtractString(ht, "Title", string.Empty);
			string pitch = JsonUtil.ExtractString(ht, "Pitch", string.Empty);
			return new SpecialOffer(text, car, price, title, pitch);
		}
		return null;
	}
}
