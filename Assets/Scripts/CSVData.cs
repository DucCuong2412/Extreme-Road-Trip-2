using System.Collections.Generic;
using UnityEngine;

public class CSVData
{
	private Dictionary<string, int> _index;

	private Dictionary<int, List<string>> _data;

	public int Count => _data.Count;

	public CSVData(string file)
	{
		_index = new Dictionary<string, int>();
		_data = new Dictionary<int, List<string>>();
		TextAsset textAsset = Resources.Load(file) as TextAsset;
		if (textAsset != null)
		{
			string text = textAsset.text;
			string[] array = text.Split('\n');
			bool flag = true;
			int num = 0;
			int num2 = 0;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				string[] array3 = text2.Split('\t');
				if (flag)
				{
					flag = false;
					num = array3.Length;
					for (int j = 0; j < array3.Length; j++)
					{
						string key = array3[j];
						_index[key] = j;
					}
				}
				else
				{
					if (array3.Length < num)
					{
						break;
					}
					_data[num2] = new List<string>(array3);
					num2++;
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing CSV file " + file);
		}
	}

	public string GetString(int line, string field, string def = null)
	{
		bool flag = false;
		string result = def;
		if (_index != null && _index.ContainsKey(field))
		{
			int num = _index[field];
			if (_data != null && _data.ContainsKey(line))
			{
				List<string> list = _data[line];
				if (num < list.Count)
				{
					result = list[num];
					flag = true;
				}
			}
		}
		if (!flag)
		{
			UnityEngine.Debug.LogWarning("CSV WARNING: Could not extract string.");
			return def;
		}
		return result;
	}

	public int GetInt(int line, string field, int def = 0)
	{
		bool flag = false;
		int result = def;
		if (_index != null && _index.ContainsKey(field))
		{
			int num = _index[field];
			if (_data != null && _data.ContainsKey(line))
			{
				List<string> list = _data[line];
				if (num < list.Count)
				{
					flag = int.TryParse(list[num], out result);
				}
			}
		}
		if (!flag)
		{
			UnityEngine.Debug.LogWarning("CSV WARNING: Could not extract int.");
			return def;
		}
		return result;
	}

	public float GetFloat(int line, string field, float def = 0f)
	{
		bool flag = false;
		float result = def;
		if (_index != null && _index.ContainsKey(field))
		{
			int num = _index[field];
			if (_data != null && _data.ContainsKey(line))
			{
				List<string> list = _data[line];
				if (num < list.Count)
				{
					flag = float.TryParse(list[num], out result);
				}
			}
		}
		if (!flag)
		{
			UnityEngine.Debug.LogWarning("CSV WARNING: Could not extract float.");
			return def;
		}
		return result;
	}
}
