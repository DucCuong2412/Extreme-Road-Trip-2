using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroWidgetCarUpgrade : MetroWidget
{
	private List<tk2dSprite> _upgradeSlots;

	public bool IsAnimating
	{
		get;
		private set;
	}

	public static MetroWidgetCarUpgrade Create(int level, float scale)
	{
		GameObject gameObject = new GameObject(typeof(MetroWidgetCarUpgrade).ToString());
		gameObject.transform.position = Vector3.zero;
		MetroWidgetCarUpgrade metroWidgetCarUpgrade = gameObject.AddComponent<MetroWidgetCarUpgrade>();
		metroWidgetCarUpgrade.Setup(level, scale);
		return metroWidgetCarUpgrade;
	}

	private void Setup(int level, float scale)
	{
		MetroSpacer metroSpacer = MetroSpacer.Create();
		Add(metroSpacer);
		Transform transform = (Object.Instantiate(Resources.Load(MetroSkin.UpgradeBar)) as GameObject).transform;
		transform.parent = metroSpacer.transform;
		transform.localScale = Vector3.one * scale;
		transform.localPosition += Vector3.back * 0.1f;
		_upgradeSlots = new List<tk2dSprite>();
		for (int i = 1; i <= 5; i++)
		{
			tk2dSprite component = transform.Find("Slot" + i.ToString()).Find("On").GetComponent<tk2dSprite>();
			SetSlotAlpha(component, (i > level) ? 0f : 1f);
			_upgradeSlots.Add(component);
		}
	}

	public void ShowLevel(int level)
	{
		for (int i = 0; i < _upgradeSlots.Count; i++)
		{
			SetSlotAlpha(_upgradeSlots[i], (i >= level) ? 0f : 1f);
		}
	}

	private void SetSlotAlpha(tk2dSprite slot, float alpha)
	{
		Color color = slot.color;
		color.a = alpha;
		slot.color = color;
	}

	public void StartUpgradeAnim(int level)
	{
		StartCoroutine(UpgradeCR(level));
	}

	private IEnumerator UpgradeCR(int level)
	{
		IsAnimating = true;
		yield return null;
		int slot = level - 1;
		Duration delay = new Duration(0.35f);
		while (!delay.IsDone())
		{
			SetSlotAlpha(alpha: Mathf.Lerp(0f, 1f, delay.Value01()), slot: _upgradeSlots[slot]);
			PrefabSingleton<GameSpecialFXManager>.Instance.PlayGUISparksFX(_upgradeSlots[slot].transform.position);
			yield return null;
		}
		IsAnimating = false;
	}
}
