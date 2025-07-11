using UnityEngine;

public class Showroom : MonoBehaviour
{
	private string _displayName;

	private string _description;

	public Material _previewMaterial;

	public float _carIconScale;

	public string DisplayName
	{
		get
		{
			return _displayName.Localize();
		}
		set
		{
			_displayName = value;
		}
	}

	public string Description
	{
		get
		{
			return _description.Localize();
		}
		set
		{
			_description = value;
		}
	}

	public Price Price
	{
		get;
		set;
	}

	public int SlotCount()
	{
		return base.transform.Find("CarSlots").childCount;
	}
}
