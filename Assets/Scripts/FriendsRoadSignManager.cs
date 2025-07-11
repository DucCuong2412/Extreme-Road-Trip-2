using System.Collections.Generic;
using UnityEngine;

public class FriendsRoadSignManager : AutoSingleton<FriendsRoadSignManager>
{
	private const int _friendRoadSignMax = 10;

	private const int _playerNameMaxLenght = 9;

	private Transform _roadSignTemplate;

	private List<Item> _usedItems;

	private Dictionary<SocialPlatform, List<LeaderboardScore>> _scores;

	protected override void OnAwake()
	{
		_usedItems = new List<Item>();
		_roadSignTemplate = (Resources.Load("FriendsRoadSign") as GameObject).transform;
		_scores = AutoSingleton<LeaderboardsManager>.Instance.GetLeaderboardScoresDict(LeaderboardType.longestRoadTrip);
		base.OnAwake();
	}

	public void SpawnRoadSign(Curve curve)
	{
		if (_scores != null)
		{
			List<CurvePoint> points = curve.GetPoints();
			CurvePoint curvePoint = points[0];
			float x = curvePoint.position.x;
			CurvePoint endPoint = curve.GetEndPoint();
			float x2 = endPoint.position.x;
			foreach (KeyValuePair<SocialPlatform, List<LeaderboardScore>> score2 in _scores)
			{
				int num = Mathf.Min(10, score2.Value.Count);
				for (int i = 0; i < num; i++)
				{
					LeaderboardScore leaderboardScore = score2.Value[i];
					if ((float)leaderboardScore._value >= x && (float)leaderboardScore._value <= x2)
					{
						int index = Mathf.RoundToInt((float)leaderboardScore._value - x);
						Transform transform = Object.Instantiate(_roadSignTemplate, Vector3.zero, Quaternion.identity) as Transform;
						if (transform == null)
						{
							break;
						}
						Transform transform2 = transform;
						CurvePoint curvePoint2 = points[index];
						transform2.position = curvePoint2.position;
						BreakableItem component = transform.GetComponent<BreakableItem>();
						if (component != null)
						{
							component.ZOffset = 0f;
							component.Activate();
							_usedItems.Add(component);
						}
						Transform transform3 = transform.Find("Pivot/piece1");
						Bounds bounds = RendererBounds.ComputeBounds(transform3);
						LeaderboardScore score = leaderboardScore;
						Vector3 extents = bounds.extents;
						MetroWidget metroWidget = CreatePictureWidget(score, extents.y);
						metroWidget.transform.parent = transform3;
						metroWidget.SetLayer(0);
						Vector3 extents2 = bounds.extents;
						float x3 = extents2.x;
						float num2 = 1.5f;
						float num3 = 0.35f;
						float num4 = x3 * 2f - num2;
						Vector3 size = bounds.size;
						float num5 = size.y - num3;
						float left = (0f - num4) * 0.5f;
						float top = (0f - num5) * 0.5f;
						Rect zone = new Rect(left, top, num4, num5);
						metroWidget.Layout(zone);
						Vector3 localPosition = metroWidget.transform.localPosition;
						metroWidget.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0f);
					}
				}
			}
		}
	}

	private MetroWidget CreatePictureWidget(LeaderboardScore score, float pngSize)
	{
		MetroWidget metroWidget = MetroLayout.Create(Direction.vertical);
		MetroWidget child = MetroLabel.Create(StringUtil.Trunc(score._username, 9)).SetFont(MetroSkin.VerySmallFont).SetColor(Color.black);
		metroWidget.Add(child);
		if (PictureManager.IsPictureLoaded(score._userId))
		{
			WidgetPlayerPicture widgetPlayerPicture = WidgetPlayerPicture.Create(score._userId);
			widgetPlayerPicture.SetMass(2f);
			metroWidget.Add(widgetPlayerPicture);
		}
		MetroWidget child2 = MetroLabel.Create(score._value + "m").SetFont(MetroSkin.VerySmallFont).SetColor(Color.black);
		metroWidget.Add(child2);
		return metroWidget;
	}

	public void FreeItems()
	{
		CameraGame cam = PrefabSingleton<CameraGame>.Instance;
		_usedItems.RemoveAll(delegate(Item item)
		{
			if (cam.IsLeftOfScreen(item.GetRightMostPosition()))
			{
				item.Reset();
				UnityEngine.Object.Destroy(item.gameObject);
				return true;
			}
			return false;
		});
	}
}
