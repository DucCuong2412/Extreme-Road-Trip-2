using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroMenuJson : MetroMenu
{
	private Hashtable data;

	protected Dictionary<string, Action> _callbacks = new Dictionary<string, Action>();

	private MetroWidget ParseJson(string json)
	{
		data = json.hashtableFromJson();
		if (data != null)
		{
			return Parse(data);
		}
		UnityEngine.Debug.Log("data is null :(");
		return null;
	}

	public MetroWidget Parse(Hashtable data)
	{
		string text = data["id"].ToString();
		switch (data["type"].ToString())
		{
		case "layout":
		{
			string a3 = data["split"].ToString();
			MetroLayout metroLayout = MetroLayout.Create(text, (!(a3 == "vertical")) ? Direction.horizontal : Direction.vertical);
			ArrayList arrayList2 = data["content"] as ArrayList;
			{
				foreach (Hashtable item in arrayList2)
				{
					metroLayout.Add(Parse(item));
				}
				return metroLayout;
			}
		}
		case "button":
		{
			MetroButton metroButton = MetroButton.Create(text);
			if (_callbacks.ContainsKey(text))
			{
				metroButton.OnButtonClicked += _callbacks[text];
			}
			foreach (string key in data.Keys)
			{
				if (key == "color")
				{
					metroButton.SetColor(ParseColor(data["color"] as ArrayList));
				}
			}
			ArrayList arrayList = data["content"] as ArrayList;
			{
				foreach (Hashtable item2 in arrayList)
				{
					metroButton.Add(Parse(item2));
				}
				return metroButton;
			}
		}
		case "label":
		{
			MetroLabel metroLabel = MetroLabel.Create(text);
			{
				foreach (string key2 in data.Keys)
				{
					if (key2 == "text")
					{
						metroLabel.SetText(data["text"].ToString());
					}
					if (key2 == "color")
					{
						metroLabel.SetColor(ParseColor(data["color"] as ArrayList));
					}
				}
				return metroLabel;
			}
		}
		case "spacer":
			return MetroSpacer.Create(text);
		default:
			UnityEngine.Debug.Log("Unkown block type while parsing tree:" + data.toJson());
			return null;
		}
	}

	private float F(ArrayList data, int i)
	{
		return (float)(double)data[i];
	}

	private Color ParseColor(ArrayList data)
	{
		switch (data.Count)
		{
		case 3:
			return new Color(F(data, 0), F(data, 1), F(data, 2), 1f);
		case 4:
			return new Color(F(data, 0), F(data, 1), F(data, 2), F(data, 3));
		default:
			return Color.red;
		}
	}
}
