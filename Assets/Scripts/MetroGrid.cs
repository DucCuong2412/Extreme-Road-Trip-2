using UnityEngine;

public class MetroGrid : MetroWidget
{
	private MetroWidget[,] _grid;

	private int _rows;

	private int _cols;

	private int _count;

	public bool IsFull()
	{
		return _count == _rows * _cols;
	}

	public bool IsEmpty()
	{
		return _count == 0;
	}

	public static MetroGrid Create(int cols, int rows)
	{
		GameObject gameObject = new GameObject(typeof(MetroGrid).ToString());
		gameObject.transform.position = Vector3.zero;
		return gameObject.AddComponent<MetroGrid>().Setup(cols, rows);
	}

	public MetroGrid Setup(int cols, int rows)
	{
		_cols = cols;
		_rows = rows;
		_grid = new MetroWidget[cols, rows];
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		base.Add(metroLayout);
		for (int i = 0; i < rows; i++)
		{
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
			metroLayout.Add(metroLayout2);
			for (int j = 0; j < cols; j++)
			{
				MetroSpacer metroSpacer = MetroSpacer.Create();
				metroLayout2.Add(metroSpacer);
				_grid[j, i] = metroSpacer;
			}
		}
		return this;
	}

	public override MetroWidget Add(MetroWidget child)
	{
		if (_count >= _cols * _rows)
		{
			UnityEngine.Debug.LogWarning("Trying to add element to full grid: ignoring new element");
			return null;
		}
		int num = _count % _cols;
		int num2 = _count / _cols;
		_grid[num, num2].Replace(child).Destroy();
		_count++;
		return child;
	}
}
