using UnityEngine;

public class LoadRandomCarSprite : MonoBehaviour
{
	private void Start()
	{
		AutoSingleton<CarManager>.Instance.Create();
		Car randomCar = AutoSingleton<CarManager>.Instance.GetRandomCar();
		tk2dSpriteCollection component = (Resources.Load("CarsSpriteCollection") as GameObject).GetComponent<tk2dSpriteCollection>();
		tk2dSpriteCollectionData spriteCollection = component.spriteCollection;
		GameObject gameObject = new GameObject();
		tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
		tk2dSprite.Collection = spriteCollection;
		tk2dSprite.spriteId = spriteCollection.GetSpriteIdByName(randomCar.Id + "Sprite");
	}
}
