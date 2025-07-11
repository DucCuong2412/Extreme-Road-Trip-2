using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPropDatabase : AutoSingleton<ActionPropDatabase>
{
	private List<PickupElement> _elements;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_elements = new List<PickupElement>();
		TextAsset textAsset = Resources.Load("actionProps.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			ArrayList arrayList = textAsset.text.arrayListFromJson();
			if (arrayList != null)
			{
				foreach (Hashtable item in arrayList)
				{
					PickupElement pickupElement = new PickupElement();
					pickupElement._frequency = JsonUtil.ExtractInt(item, "frequency", 0);
					string text = JsonUtil.ExtractString(item, "prefab", null);
					if (text != null)
					{
						GameObject gameObject = Resources.Load(text) as GameObject;
						pickupElement._prefab = gameObject.transform;
					}
					_elements.Add(pickupElement);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file actionProps.json");
		}
		base.OnAwake();
	}

	public List<PickupElement> GetElements()
	{
		return _elements;
	}

	public WeightedList<Transform> GetWeightedElements()
	{
		return PickupElement.ArrayToWeightedList(_elements.ToArray());
	}
}
