using UnityEngine;

public class MetroGUITest : MonoBehaviour
{
	protected CameraGUI _camera;

	public void Awake()
	{
		_camera = PrefabSingleton<CameraGUI>.Instance;
		Rect zone = new Rect(0f - _camera.HalfScreenWidth, 0f - _camera.HalfScreenHeight, _camera.ScreenWidth, _camera.ScreenHeight);
		MetroLayout metroLayout = MetroLayout.Create("l", Direction.horizontal);
		MetroLayout metroLayout2 = MetroLayout.Create("l1", Direction.vertical);
		metroLayout2.Add(MetroLabel.Create("a"));
		metroLayout2.Add(MetroButton.Create("b"));
		metroLayout2.Add(MetroButton.Create("c"));
		MetroLayout metroLayout3 = MetroLayout.Create("l2", Direction.horizontal);
		metroLayout3.Add(MetroButton.Create("d"));
		metroLayout3.Add(MetroButton.Create("e"));
		metroLayout2.Add(metroLayout3);
		MetroLayout metroLayout4 = MetroLayout.Create("l3", Direction.horizontal);
		metroLayout4.Add(MetroButton.Create("f"));
		metroLayout4.Add(MetroButton.Create("g"));
		MetroLayout metroLayout5 = MetroLayout.Create("l4", Direction.vertical);
		metroLayout5.Add(MetroButton.Create("i"));
		MetroLayout metroLayout6 = MetroLayout.Create("l5", Direction.horizontal);
		metroLayout6.Add(MetroButton.Create("j"));
		metroLayout6.Add(MetroButton.Create("k"));
		metroLayout5.Add(metroLayout6);
		metroLayout4.Add(metroLayout5);
		metroLayout2.Add(metroLayout4);
		metroLayout.Add(MetroSpacer.Create("left"));
		metroLayout.Add(metroLayout2);
		metroLayout.Layout(zone);
	}
}
