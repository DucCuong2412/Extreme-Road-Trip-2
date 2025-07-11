using System.Collections.Generic;
using UnityEngine;

public class RoadSignManager : AutoSingleton<RoadSignManager>
{
	private GameObject _roadSignBackgroundTemplate;

	private float _roadSignLimitPos;

	private float _roadSignOffsetX;

	private List<Transform> _roadSignList;

	protected override void OnAwake()
	{
		_roadSignList = new List<Transform>();
		_roadSignBackgroundTemplate = (Resources.Load("RoadSignBackground") as GameObject);
		base.OnAwake();
	}

	public void Update()
	{
		for (int num = _roadSignList.Count - 1; num >= 0; num--)
		{
			Transform transform = _roadSignList[num];
			Vector3 position = transform.position;
			if (position.x < _roadSignLimitPos)
			{
				_roadSignList.Remove(transform);
				UnityEngine.Object.Destroy(transform.gameObject);
			}
			else
			{
				Transform transform2 = transform;
				Vector3 deltaCameraPosition = AutoSingleton<WorldManager>.Instance.GetDeltaCameraPosition();
				transform2.Translate(new Vector3(0.06f * (0f - deltaCameraPosition.x), 0f, 0f));
			}
		}
	}

	private void SpawnBackgroundRoadSign(MetroWidget widget)
	{
		Object @object = Object.Instantiate(_roadSignBackgroundTemplate, Vector3.zero, Quaternion.identity);
		if (!(@object == null))
		{
			Transform transform = (@object as GameObject).transform;
			Bounds bounds = RendererBounds.ComputeBounds(transform);
			float y = -14f;
			Vector3 extents = bounds.extents;
			float x = extents.x;
			_roadSignOffsetX = PrefabSingleton<CameraBackground>.Instance.ScreenWidth() * 0.5f + x;
			_roadSignLimitPos = 0f - _roadSignOffsetX;
			transform.position = new Vector3(_roadSignOffsetX, y, 75f);
			transform.parent = PrefabSingleton<CameraGUI>.Instance.transform;
			widget.SetPadding(0f);
			widget.transform.parent = transform;
			widget.SetLayer(13);
			float num = 1.5f;
			float num2 = x * 2f - num;
			Vector3 extents2 = bounds.extents;
			float num3 = extents2.y * 0.5f;
			float left = (0f - num2) * 0.5f;
			Vector3 extents3 = bounds.extents;
			float top = extents3.y - num3 * 0.5f;
			Rect zone = new Rect(left, top, num2, num3);
			widget.Layout(zone);
			_roadSignList.Add(transform);
		}
	}

	public void ShowBestTime(LeaderboardType timerType, float timer)
	{
		MetroWidget widget = MetroWidgetBestTime.Create(timerType, timer);
		SpawnBackgroundRoadSign(widget);
	}
}
