using System.Collections;
using UnityEngine;

public class FakeCarStunt : MonoBehaviour
{
	private StuntHud _stuntHud;

	public void Awake()
	{
		_stuntHud = GetComponent<StuntHud>();
	}

	public void Start()
	{
		StartCoroutine(StartCR());
	}

	private IEnumerator Stunt(StuntEvent stunt)
	{
		UnityEngine.Debug.Log("Stunt: " + stunt.ToString());
		_stuntHud.OnStuntEvent(stunt);
		yield return new WaitForSeconds(0.3f);
	}

	private IEnumerator StartCR()
	{
		yield return StartCoroutine(Stunt(StuntEvent.frontFlip));
		yield return StartCoroutine(Stunt(StuntEvent.backFlip));
		yield return StartCoroutine(Stunt(StuntEvent.bigAir));
		yield return StartCoroutine(Stunt(StuntEvent.frontFlip));
		yield return StartCoroutine(Stunt(StuntEvent.backFlip));
		yield return StartCoroutine(Stunt(StuntEvent.bigAir));
		yield return StartCoroutine(Stunt(StuntEvent.perfectLanding));
	}
}
