public class Maybe<T>
{
	private bool _set;

	private T _data;

	public T Get()
	{
		return _data;
	}

	public bool IsSet()
	{
		return _set;
	}

	public void Set(T data)
	{
		_set = true;
		_data = data;
	}

	public void Reset()
	{
		_set = false;
		_data = default(T);
	}
}
