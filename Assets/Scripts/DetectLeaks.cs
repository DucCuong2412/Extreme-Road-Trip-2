using System.Collections.Generic;
using UnityEngine;

public class DetectLeaks : MonoBehaviour
{
	private void OnGUI()
	{
		Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(Object));
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		Object[] array2 = array;
		foreach (UnityEngine.Object @object in array2)
		{
			string text = @object.GetType().ToString();
			if (dictionary.ContainsKey(text))
			{
				Dictionary<string, int> dictionary2;
				Dictionary<string, int> dictionary3 = dictionary2 = dictionary;
				string key;
				string key2 = key = text;
				int num = dictionary2[key];
				dictionary3[key2] = num + 1;
			}
			else
			{
				dictionary[text] = 1;
			}
		}
		List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(dictionary);
		list.Sort((KeyValuePair<string, int> firstPair, KeyValuePair<string, int> nextPair) => nextPair.Value.CompareTo(firstPair.Value));
		foreach (KeyValuePair<string, int> item in list)
		{
			GUILayout.Label(item.Key + ": " + item.Value);
		}
	}
}
