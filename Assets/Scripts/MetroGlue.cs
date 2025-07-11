using System.Collections.Generic;
using UnityEngine;

public class MetroGlue : MetroWidget
{
	private Direction _flow;

	protected void SetFlow(Direction flow)
	{
		_flow = flow;
	}

	public static MetroGlue Create(Direction flow)
	{
		return Create("Glue", flow);
	}

	public static MetroGlue Create(string id, Direction flow)
	{
		GameObject gameObject = new GameObject(id);
		gameObject.transform.position = Vector3.zero;
		MetroGlue metroGlue = gameObject.AddComponent<MetroGlue>();
		metroGlue.SetFlow(flow);
		return metroGlue;
	}

	public void Update()
	{
		Reflow();
	}

	public override void Layout(Rect zone)
	{
		foreach (MetroWidget child in _childs)
		{
			if (_flow == Direction.vertical)
			{
				MetroWidget metroWidget = child;
				Vector3 size = RendererBounds.ComputeBounds(child.transform).size;
				metroWidget.Mass = size.y;
			}
			else
			{
				MetroWidget metroWidget2 = child;
				Vector3 size2 = RendererBounds.ComputeBounds(child.transform).size;
				metroWidget2.Mass = size2.x;
			}
		}
		base.Layout(zone);
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
		float width = zone.width;
		float height = zone.height;
		float num = ChildsMass();
		float num2 = 0f;
		float num3 = base.AlignmentOffset;
		if (_alignment == MetroAlign.Left)
		{
			num3 -= width * 0.5f + num * 0.5f;
		}
		else if (_alignment == MetroAlign.Right)
		{
			num3 += width * 0.5f - num * 0.5f;
		}
		for (int i = 0; i < count; i++)
		{
			MetroWidget metroWidget = childs[i];
			metroWidget.Layout(new Rect(num3 + num * -0.5f + num2, zone.y, metroWidget.Mass, height));
			num2 += metroWidget.Mass;
		}
	}

	private void LayoutVertical(List<MetroWidget> childs, Rect zone)
	{
		int count = childs.Count;
		float width = zone.width;
		float num = ChildsMass();
		float num2 = ChildsMass();
		for (int i = 0; i < count; i++)
		{
			MetroWidget metroWidget = childs[i];
			num2 -= metroWidget.Mass;
			metroWidget.Layout(new Rect(zone.x, num * -0.5f + num2, width, metroWidget.Mass));
		}
	}
}
