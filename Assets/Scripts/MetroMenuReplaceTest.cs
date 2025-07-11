using UnityEngine;

public class MetroMenuReplaceTest : MetroMenuPage
{
	protected override void OnStart()
	{
		MetroButton a = MetroButton.Create("a");
		a.SetColor(Color.black);
		MetroButton b = MetroButton.Create("b");
		b.SetColor(Color.black);
		MetroLayout menu = MetroLayout.Create(Direction.horizontal);
		menu.Add(a);
		menu.Add(b);
		a.OnButtonClicked += delegate
		{
			UnityEngine.Debug.Log("'a' button clicked");
			MetroButton metroButton3 = MetroButton.Create("d");
			metroButton3.SetColor(Color.green);
			UnityEngine.Object.Destroy(menu.Replace(a, metroButton3).gameObject);
			menu.Reflow();
		};
		b.OnButtonClicked += delegate
		{
			UnityEngine.Debug.Log("'b' button clicked");
			UnityEngine.Object.Destroy(menu.Remove(b).gameObject);
			MetroButton metroButton = MetroButton.Create("c");
			metroButton.SetColor(Color.white);
			MetroButton metroButton2 = MetroButton.Create("d");
			metroButton2.SetColor(Color.red);
			menu.Add(metroButton);
			menu.Add(metroButton2);
			menu.Reflow();
		};
		Add(menu);
		base.OnStart();
	}
}
