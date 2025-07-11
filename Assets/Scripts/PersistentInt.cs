public class PersistentInt
{
	private int _value;

	private string _key;

	public PersistentInt(string key)
		: this(key, 0)
	{
	}

	public PersistentInt(string key, int def)
	{
		_key = key;
		_value = Preference.GetInt(_key, def);
	}

	public int Get()
	{
		return _value;
	}

	public void Set(int v)
	{
		_value = v;
		Preference.SetInt(_key, _value);
	}
}
