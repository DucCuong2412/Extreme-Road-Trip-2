using System.Collections;
using UnityEngine;

public class RoofdogAnalyticData : AutoSingleton<RoofdogAnalyticData>
{
	private PersistentFloat _sessionCount;

	private PersistentFloat _totalSecondInApp;

	private PersistentFloat _totalMoneySpentCent;

	private PersistentInt _totalAdsWatched;

	private PersistentInt _totalTapjoyRewards;

	private float _sessionLengthInSecond;

	protected override void OnAwake()
	{
		base.OnAwake();
		Object.DontDestroyOnLoad(base.gameObject);
		Init();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Init()
	{
		_sessionCount = new PersistentFloat("_sessionCount", 0f);
		_totalSecondInApp = new PersistentFloat("_totalSecondInApp", 0f);
		_totalMoneySpentCent = new PersistentFloat("_totalMoneySpentCent", 0f);
		_totalAdsWatched = new PersistentInt("_totalAdsWatched", 0);
		_totalTapjoyRewards = new PersistentInt("_totalTapjoyRewards", 0);
		_sessionLengthInSecond = 0f;
		StartCoroutine(TimerCR());
	}

	public void OnSessionStarted()
	{
		_sessionCount.Set(_sessionCount.Get() + 1f);
		_sessionLengthInSecond = 0f;
	}

	private IEnumerator TimerCR()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			_sessionLengthInSecond += 1f;
			_totalSecondInApp.Set(_totalSecondInApp.Get() + 1f);
		}
	}

	public float GetTotalSecondInApp()
	{
		return _totalSecondInApp.Get();
	}

	public float GetTimeSecondInSession()
	{
		return _sessionLengthInSecond;
	}

	public float GetSessionCount()
	{
		return _sessionCount.Get();
	}

	public void OnMoneySpend(double moneySpend)
	{
		float num = _totalMoneySpentCent.Get();
		int num2 = (int)(moneySpend * 100.0 * 0.7);
		_totalMoneySpentCent.Set(num + (float)num2);
	}

	public void OnAdReward()
	{
		_totalAdsWatched.Set(_totalAdsWatched.Get() + 1);
	}

	public void OnTapjoyReward(int amount)
	{
		_totalTapjoyRewards.Set(_totalTapjoyRewards.Get() + 1);
	}

	public float GetTapjoyRewardCount()
	{
		return _totalTapjoyRewards.Get();
	}

	public float GetTotalAdReward()
	{
		return _totalAdsWatched.Get();
	}

	public int GetTotalMoneySpendCent()
	{
		return Mathf.RoundToInt(_totalMoneySpentCent.Get());
	}
}
