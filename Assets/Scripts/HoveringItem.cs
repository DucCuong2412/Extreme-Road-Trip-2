using System.Collections;
using UnityEngine;

public class HoveringItem : BreakableItem
{
	private RandomRange _startHeight = new RandomRange(40f, 120f);

	private RandomRange _xDelay = new RandomRange(0.5f, 1.5f);

	private RandomRange _yDelay = new RandomRange(0.2f, 0.5f);

	private float _xOffset = 5f;

	private float _yOffset = 1f;

	private bool _anim;

	private float _baseX;

	private float _baseY;

	public override void Reset()
	{
		base.Reset();
		_anim = false;
	}

	public override void Activate()
	{
		base.Activate();
		_transform.position += Vector3.up * _startHeight.Pick();
		_transform.rotation = Quaternion.identity;
		Vector3 position = _transform.position;
		_baseX = position.x;
		Vector3 position2 = _transform.position;
		_baseY = position2.y;
		_anim = true;
		StartCoroutine(HoveringCR());
	}

	private IEnumerator HoveringCR()
	{
		float fromX = 0f - _xOffset;
		float toX = _xOffset;
		float fromY = 0f - _yOffset;
		float toY = _yOffset;
		Duration xDelay = new Duration(_xDelay.Pick());
		Duration yDelay = new Duration(_yDelay.Pick());
		while (_anim)
		{
			float x = Mathfx.Lerp(fromX, toX, xDelay.Value01());
			float y = Mathfx.Lerp(fromY, toY, yDelay.Value01());
			Transform transform = _transform;
			float x2 = _baseX + x;
			float y2 = _baseY + y;
			Vector3 position = _transform.position;
			transform.position = new Vector3(x2, y2, position.z);
			if (xDelay.IsDone())
			{
				float tempX = fromX;
				fromX = toX;
				toX = tempX;
				xDelay = new Duration(_xDelay.Pick());
			}
			if (yDelay.IsDone())
			{
				float tempY = fromY;
				fromY = toY;
				toY = tempY;
				yDelay = new Duration(_yDelay.Pick());
			}
			yield return null;
		}
	}
}
