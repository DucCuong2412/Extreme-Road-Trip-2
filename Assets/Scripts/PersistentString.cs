public class PersistentString
{
	private string _value;

	private string _key;

	public PersistentString(string key, string def)
	{
		_key = key;
		_value = Preference.GetString(_key, def);
	}

	public string Get()
	{
		return _value;
	}

	public void Set(string v)
	{
		_value = v;
		Preference.SetString(_key, _value);
	}
}
