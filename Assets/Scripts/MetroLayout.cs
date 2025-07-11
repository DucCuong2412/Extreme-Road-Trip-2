using System.Collections.Generic;
using UnityEngine;

public class MetroLayout : MetroWidget
{
	private Direction _flow;

	protected void SetFlow(Direction flow)
	{
		_flow = flow;
	}

	public static MetroLayout Create(Direction flow)
	{
		return Create("Layout", flow);
	}

	public static MetroLayout Create(string id, Direction flow)
	{
		GameObject gameObject = new GameObject(id);
		gameObject.transform.position = Vector3.zero;
		MetroLayout metroLayout = gameObject.AddComponent<MetroLayout>();
		metroLayout.SetFlow(flow);
		return metroLayout;
	}

	public override void LayoutChilds()
	{
		if (_flow == Direction.vertical)
		{
			LayoutVertical(_childs, _childsZone);
		}
		else
		{
			LayoutHorizontal(_childs, _childsZone);
		}
	}

	private float ChildsMass()
	{
		float num = 0f;
		foreach (MetroWidget child in _childs)
		{
			num += child.Mass;
		}
		return num;
	}

	private void LayoutHorizontal(List<MetroWidget> childs, Rect zone)
	{
		int count = childs.Count;
		float num = zone.width / ChildsMass();
		float height = zone.height;
		float num2 = 0f;
		for (int i = 0; i < count; i++)
		{
			MetroWidget metroWidget = childs[i];
			metroWidget.Layout(new Rect(zone.x + num * num2, zone.y, num * metroWidget.Mass, height));
			num2 += metroWidget.Mass;
		}
	}

	private void LayoutVertical(List<MetroWidget> childs, Rect zone)
	{
		int count = childs.Count;
		float num = ChildsMass();
		float width = zone.width;
		float num2 = zone.height / num;
		for (int i = 0; i < count; i++)
		{
			MetroWidget metroWidget = childs[i];
			num -= metroWidget.Mass;
			metroWidget.Layout(new Rect(zone.x, zone.y + num2 * num, width, num2 * metroWidget.Mass));
		}
	}
}
