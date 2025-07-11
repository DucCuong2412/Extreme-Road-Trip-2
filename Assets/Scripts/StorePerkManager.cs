using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePerkManager : AutoSingleton<StorePerkManager>
{
	private List<StorePerk> _storePerkList;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_storePerkList = new List<StorePerk>();
		base.OnAwake();
	}

	protected override void OnStart()
	{
		PlayerProfile profile = AutoSingleton<Player>.Instance.Profile;
		UpdatePerkList(profile.StorePerkList);
	}

	public void ActivateStorePerk(StorePerkType perkType)
	{
		if (!IsStorePerkOwned(perkType))
		{
			StorePerk storePerk = StorePerk.Create(perkType);
			if (storePerk != null)
			{
				storePerk.Activate();
				_storePerkList.Add(storePerk);
				AutoSingleton<Player>.Instance.Profile.StorePerkList = GetJsonArrayList();
				AutoSingleton<Player>.Instance.SaveProfile();
			}
		}
	}

	public void DeactivateStorePerk(StorePerkType perkType)
	{
		StorePerk storePerk2 = _storePerkList.Find((StorePerk storePerk) => storePerk.StorePerkType == perkType);
		if (storePerk2 != null)
		{
			storePerk2.Deactivate();
			_storePerkList.Remove(storePerk2);
			AutoSingleton<Player>.Instance.Profile.StorePerkList = GetJsonArrayList();
			AutoSingleton<Player>.Instance.SaveProfile();
		}
	}

	public bool IsStorePerkOwned(StorePerkType type)
	{
		StorePerk storePerk2 = _storePerkList.Find((StorePerk storePerk) => storePerk.StorePerkType == type);
		return storePerk2 != null;
	}

	public ArrayList GetJsonArrayList()
	{
		ArrayList arrayList = new ArrayList();
		foreach (StorePerk storePerk in _storePerkList)
		{
			arrayList.Add(storePerk.ToJsonData());
		}
		return arrayList;
	}

	public ArrayList GetStringArrayList()
	{
		ArrayList arrayList = new ArrayList();
		foreach (StorePerk storePerk in _storePerkList)
		{
			arrayList.Add(storePerk.StorePerkType.ToString());
		}
		return arrayList;
	}

	private void UpdatePerkList(ArrayList jsonStorePerk)
	{
		foreach (StorePerk storePerk2 in _storePerkList)
		{
			storePerk2.Deactivate();
		}
		_storePerkList.Clear();
		foreach (Hashtable item in jsonStorePerk)
		{
			StorePerk storePerk = StorePerk.FromJsonData(item);
			if (storePerk != null)
			{
				storePerk.Activate();
				_storePerkList.Add(storePerk);
			}
		}
		MaybeAutoActivate(StorePerkType.permanentCoinDoubler, StorePerkType.adRemover);
		MaybeAutoActivate(StorePerkType.adRemover, StorePerkType.permanentCoinDoubler);
	}

	private void MaybeAutoActivate(StorePerkType perkToActivate, StorePerkType perkToWatch)
	{
		if (!IsStorePerkOwned(perkToActivate) && IsStorePerkOwned(perkToWatch))
		{
			ActivateStorePerk(perkToActivate);
		}
	}
}
