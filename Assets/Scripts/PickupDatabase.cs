using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PickupDatabase : AutoSingleton<PickupDatabase>
{
	private List<PickupSequence> _introSequences;

	private List<PickupSequence> _gameSequences;

	public void Create(GameMode mode)
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_introSequences = new List<PickupSequence>();
		_gameSequences = new List<PickupSequence>();
		Dictionary<string, GameObject> lookup = new Dictionary<string, GameObject>();
		TextAsset textAsset = Resources.Load((mode != 0) ? "frenzyPickups.json" : "pickups.json", typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			Hashtable hashtable = textAsset.text.hashtableFromJson();
			if (hashtable != null)
			{
				ArrayList al = JsonUtil.ExtractArrayList(hashtable, "intro");
				FillSequences(lookup, al, _introSequences);
				ArrayList al2 = JsonUtil.ExtractArrayList(hashtable, "game");
				FillSequences(lookup, al2, _gameSequences);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Missing JSON file " + ((mode != 0) ? "frenzyPickups.json" : "pickups.json"));
		}
		Create();
	}

	private void FillSequences(Dictionary<string, GameObject> lookup, ArrayList al, List<PickupSequence> seqs)
	{
		if (al != null)
		{
			foreach (Hashtable item in al)
			{
				PickupSequence pickupSequence = new PickupSequence();
				pickupSequence._frequency = JsonUtil.ExtractInt(item, "frequency", 0);
				ArrayList arrayList = JsonUtil.ExtractArrayList(item, "prefabs", new ArrayList());
				pickupSequence._templates = new List<GameObject>();
				foreach (string item2 in arrayList)
				{
					if (!lookup.ContainsKey(item2))
					{
						lookup[item2] = (Resources.Load(item2) as GameObject);
					}
					if (lookup[item2] != null)
					{
						pickupSequence._templates.Add(lookup[item2]);
					}
				}
				seqs.Add(pickupSequence);
			}
		}
	}

	public List<PickupSequence> GetIntroSequences()
	{
		return _introSequences;
	}

	public List<PickupSequence> GetGameSequences()
	{
		return _gameSequences;
	}

	public WeightedList<List<GameObject>> GetWeightedIntroSequences()
	{
		WeightedList<List<GameObject>> weightedList = new WeightedList<List<GameObject>>();
		foreach (PickupSequence introSequence in _introSequences)
		{
			weightedList.Add(introSequence._templates, introSequence._frequency);
		}
		return weightedList;
	}

	public WeightedList<List<GameObject>> GetWeightedGameSequences()
	{
		WeightedList<List<GameObject>> weightedList = new WeightedList<List<GameObject>>();
		foreach (PickupSequence gameSequence in _gameSequences)
		{
			weightedList.Add(gameSequence._templates, gameSequence._frequency);
		}
		return weightedList;
	}
}
