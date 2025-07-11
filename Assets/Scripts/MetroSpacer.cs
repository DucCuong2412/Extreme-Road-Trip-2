using UnityEngine;

public class MetroSpacer : MetroWidget
{
	public MetroConstraint Constraint
	{
		get;
		set;
	}

	public static MetroSpacer Create()
	{
		return Create("Spacer");
	}

	public static MetroSpacer Create(float mass)
	{
		MetroSpacer metroSpacer = Create();
		metroSpacer.Mass = mass;
		return metroSpacer;
	}

	public static MetroSpacer Create(string id)
	{
		return Create(id, MetroSkin.Padding, MetroSkin.Padding);
	}

	public static MetroSpacer Create(string id, float paddingX, float paddingY)
	{
		GameObject gameObject = new GameObject(id);
		gameObject.transform.localPosition = Vector3.zero;
		MetroSpacer metroSpacer = gameObject.AddComponent<MetroSpacer>();
		metroSpacer.SetPadding(paddingX, paddingY);
		return metroSpacer;
	}

	public override void Layout(Rect zone)
	{
		if (Constraint == MetroConstraint.Square)
		{
			if (zone.width < zone.height)
			{
				float num = zone.height - zone.width;
				zone.height = zone.width;
				zone.y += num * 0.5f;
			}
			else
			{
				float num2 = zone.width - zone.height;
				zone.width = zone.height;
				zone.x += num2 * 0.5f;
			}
		}
		base.Layout(zone);
	}
}
