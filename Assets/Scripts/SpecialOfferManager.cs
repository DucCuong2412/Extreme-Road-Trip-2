using System.Collections;
using UnityEngine;

public class SpecialOfferManager : AutoSingleton<SpecialOfferManager>
{
	private PersistentInt _specialOfferAttemptCount;

	private PersistentString _lastSpecialOfferSeen;

	private SpecialOffer _currentSpecialOffer;

	private int SpecialOfferAttemptCount
	{
		get
		{
			return _specialOfferAttemptCount.Get();
		}
		set
		{
			_specialOfferAttemptCount.Set(value);
		}
	}

	private string LastSpecialOfferSeen
	{
		get
		{
			return _lastSpecialOfferSeen.Get();
		}
		set
		{
			_lastSpecialOfferSeen.Set(value);
		}
	}

	protected override void OnAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_currentSpecialOffer = null;
		_specialOfferAttemptCount = new PersistentInt("_specialOfferAttemptCount", 0);
		_lastSpecialOfferSeen = new PersistentString("_lastSpecialOfferSeen", string.Empty);
		AutoSingleton<BackendManager>.Instance.OnGetSpecialOffersSucceeded += OnGetSpecialOffersSucceeded;
		AutoSingleton<BackendManager>.Instance.GetSpecialOffers();
		base.OnAwake();
	}

	private void OnGetSpecialOffersSucceeded(string json)
	{
		Hashtable ht = json.hashtableFromJson();
		SpecialOffer specialOffer = SpecialOffer.FromJsonData(ht);
		if (specialOffer == null)
		{
			UnityEngine.Debug.LogWarning("Get Special Offer success but invalid offer data");
		}
		if (specialOffer.OfferId == LastSpecialOfferSeen)
		{
			specialOffer = null;
		}
		else
		{
			LastSpecialOfferSeen = specialOffer.OfferId;
		}
		_currentSpecialOffer = specialOffer;
	}

	public bool SpecialOfferAttempt()
	{
		SpecialOfferAttemptCount++;
		return MustShowSpecialOffer();
	}

	public bool MustShowSpecialOffer()
	{
		return _currentSpecialOffer != null;
	}

	public SpecialOffer ConsumeSpecialOffer()
	{
		SpecialOffer specialOffer = _currentSpecialOffer;
		_currentSpecialOffer = null;
		if (AutoSingleton<CarManager>.Instance.GetCarProfile(specialOffer.Car).IsUnlocked())
		{
			ReportResolution(specialOffer, SpecialOfferResolutionType.notapplicable);
			specialOffer = null;
		}
		return specialOffer;
	}

	public void ReportResolution(SpecialOffer offer, SpecialOfferResolutionType resolutionType)
	{
		AutoSingleton<BackendManager>.Instance.ReportSpecialOfferResolution(offer.OfferId, resolutionType);
	}
}
