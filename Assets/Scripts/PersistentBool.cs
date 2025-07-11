public class PersistentBool
{
	private bool _value;

	private string _key;

	public PersistentBool(string key, bool def)
	{
		_key = key;
		if (!Preference.HasKey(key))
		{
			Set(def);
		}
		_value = ((Preference.GetInt(_key) != 0) ? true : false);
	}

	public bool Get()
	{
		return _value;
	}

	public void Set(bool v)
	{
		_value = v;
		Preference.SetInt(_key, _value ? 1 : 0);
	}
}
