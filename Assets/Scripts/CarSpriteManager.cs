using UnityEngine;

public class CarSpriteManager : AutoSingleton<CarSpriteManager>
{
	private GameObject _template;

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_template = (Resources.Load("CarSpriteTemplate") as GameObject);
		base.OnAwake();
	}

	public GameObject MakeCarSprite(Car car)
	{
		GameObject gameObject = Object.Instantiate(_template) as GameObject;
		tk2dSprite componentInChildren = gameObject.GetComponentInChildren<tk2dSprite>();
		if (componentInChildren != null)
		{
			componentInChildren.SetSprite(componentInChildren.Collection.GetSpriteIdByName(car.Id + "Sprite"));
		}
		return gameObject;
	}
}
