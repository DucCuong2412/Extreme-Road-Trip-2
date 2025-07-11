public class PersistentFloat
{
	private float _value;

	private string _key;

	public PersistentFloat(string key)
		: this(key, 0f)
	{
	}

	public PersistentFloat(string key, float def)
	{
		_key = key;
		_value = Preference.GetFloat(_key, def);
	}

	public float Get()
	{
		return _value;
	}

	public void Set(float v)
	{
		_value = v;
		Preference.SetFloat(_key, _value);
	}
}
