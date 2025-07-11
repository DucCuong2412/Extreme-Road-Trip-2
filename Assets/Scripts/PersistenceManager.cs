public class PersistenceManager : AutoSingleton<PersistenceManager>
{
	private PersistentInt _currentLanguage;

	private PersistentInt _musicVolume;

	private PersistentInt _soundsVolume;

	private PersistentInt _facebookLoginPopupAttemptCount;

	private PersistentInt _facebookPublishPopupAttemptCount;

	private PersistentInt _runCount;

	private PersistentBool _hasSeenTutorial;

	private PersistentBool _hasSeenShowroom;

	private PersistentBool _hasHeardAboutNewCarV3_9_0;

	private PersistentBool _hasHeardAboutNewCarV3_14_0;

	private PersistentBool _hasHeardAboutNewCarV3_16_0;

	private PersistentBool _hasSeenStore;

	private PersistentBool _hasHeardAboutStore;

	private PersistentBool _facebookRewardCollected;

	private PersistentBool _facebookInviteSent;

	private PersistentBool _mustShowFacebookInvitationToLoginPopup;

	private PersistentBool _mustShowFacebookInvitationToPublishPopup;

	private PersistentBool _spentMoney;

	private PersistentBool _showroomNotLoggedInPopup;

	private PersistentBool _hasUsedEpicChoppa;

	private PersistentBool _invertedControl;

	private bool _isDestroyed;

	public LanguageType Language
	{
		get
		{
			return (LanguageType)_currentLanguage.Get();
		}
		set
		{
			_currentLanguage.Set((int)value);
		}
	}

	public bool HasSeenTutorial
	{
		get
		{
			return _hasSeenTutorial.Get();
		}
		set
		{
			_hasSeenTutorial.Set(value);
		}
	}

	public bool HasSeenStore
	{
		get
		{
			return _hasSeenStore.Get();
		}
		set
		{
			_hasSeenStore.Set(value);
		}
	}

	public bool HasHeardAboutStore
	{
		get
		{
			return _hasHeardAboutStore.Get();
		}
		set
		{
			_hasHeardAboutStore.Set(value);
		}
	}

	public bool HasSeenShowroom
	{
		get
		{
			return _hasSeenShowroom.Get();
		}
		set
		{
			_hasSeenShowroom.Set(value);
		}
	}

	public bool HasHeardAboutNewCarV3_9_0
	{
		get
		{
			return _hasHeardAboutNewCarV3_9_0.Get();
		}
		set
		{
			_hasHeardAboutNewCarV3_9_0.Set(value);
		}
	}

	public bool HasHeardAboutNewCarV3_14_0
	{
		get
		{
			return _hasHeardAboutNewCarV3_14_0.Get();
		}
		set
		{
			_hasHeardAboutNewCarV3_14_0.Set(value);
		}
	}

	public bool HasHeardAboutNewCarV3_16_0
	{
		get
		{
			return _hasHeardAboutNewCarV3_16_0.Get();
		}
		set
		{
			_hasHeardAboutNewCarV3_16_0.Set(value);
		}
	}

	public bool FacebookRewardCollected
	{
		get
		{
			return _facebookRewardCollected.Get();
		}
		set
		{
			_facebookRewardCollected.Set(value);
		}
	}

	public bool FacebookInviteSent
	{
		get
		{
			return _facebookInviteSent.Get();
		}
		set
		{
			_facebookInviteSent.Set(value);
		}
	}

	public bool MustShowFacebookInvitationToLoginPopup
	{
		get
		{
			return _mustShowFacebookInvitationToLoginPopup.Get();
		}
		set
		{
			_mustShowFacebookInvitationToLoginPopup.Set(value);
		}
	}

	public int FacebookLoginPopupAttemptCount
	{
		get
		{
			return _facebookLoginPopupAttemptCount.Get();
		}
		set
		{
			_facebookLoginPopupAttemptCount.Set(value);
		}
	}

	public bool MustShowFacebookInvitationToPublishPopup
	{
		get
		{
			return _mustShowFacebookInvitationToPublishPopup.Get();
		}
		set
		{
			_mustShowFacebookInvitationToPublishPopup.Set(value);
		}
	}

	public int FacebookPublishPopupAttemptCount
	{
		get
		{
			return _facebookPublishPopupAttemptCount.Get();
		}
		set
		{
			_facebookPublishPopupAttemptCount.Set(value);
		}
	}

	public bool SpentMoney
	{
		get
		{
			return _spentMoney.Get();
		}
		set
		{
			_spentMoney.Set(value);
		}
	}

	public int MusicVolume
	{
		get
		{
			return _musicVolume.Get();
		}
		set
		{
			_musicVolume.Set(value);
		}
	}

	public int SoundsVolume
	{
		get
		{
			return _soundsVolume.Get();
		}
		set
		{
			_soundsVolume.Set(value);
		}
	}

	public bool ShowroomNotLoggedInPopup
	{
		get
		{
			return _showroomNotLoggedInPopup.Get();
		}
		set
		{
			_showroomNotLoggedInPopup.Set(value);
		}
	}

	public bool HasUsedEpicChoppa
	{
		get
		{
			return _hasUsedEpicChoppa.Get();
		}
		set
		{
			_hasUsedEpicChoppa.Set(value);
		}
	}

	public bool UseInvertedControl
	{
		get
		{
			return _invertedControl.Get();
		}
		set
		{
			_invertedControl.Set(value);
		}
	}

	public int RunCount
	{
		get
		{
			return _runCount.Get();
		}
		set
		{
			_runCount.Set(value);
		}
	}

	protected override void OnAwake()
	{
		_isDestroyed = false;
		_currentLanguage = new PersistentInt("_currentLanguage");
		_musicVolume = new PersistentInt("_musicVolume", 1);
		_soundsVolume = new PersistentInt("_soundsVolume", 1);
		_facebookLoginPopupAttemptCount = new PersistentInt("_facebookLoginInformationAttemptCount", 1);
		_facebookPublishPopupAttemptCount = new PersistentInt("_facebookPublishInformationAttemptCount", 0);
		_runCount = new PersistentInt("_runCount", 0);
		_hasSeenTutorial = new PersistentBool("_hasSeenTutorial", def: false);
		_hasSeenShowroom = new PersistentBool("_hasSeenShowroom", def: false);
		_hasHeardAboutNewCarV3_9_0 = new PersistentBool("_hasHeardAboutNewCarV3_9_0", def: false);
		_hasHeardAboutNewCarV3_14_0 = new PersistentBool("_hasHeardAboutNewCarV3_14_0", def: false);
		_hasHeardAboutNewCarV3_16_0 = new PersistentBool("_hasHeardAboutNewCarV3_16_0", def: false);
		_hasSeenStore = new PersistentBool("_hasSeenStore", def: false);
		_hasHeardAboutStore = new PersistentBool("_hasHeardAboutStore", def: false);
		_facebookRewardCollected = new PersistentBool("_facebookRewardCollected", def: false);
		_facebookInviteSent = new PersistentBool("_facebookInviteSent", def: false);
		_mustShowFacebookInvitationToLoginPopup = new PersistentBool("_mustShowFacebookLoginPopup", def: true);
		_mustShowFacebookInvitationToPublishPopup = new PersistentBool("_mustShowFacebookPublishPopup", def: true);
		_spentMoney = new PersistentBool("_spentMoney", def: false);
		_showroomNotLoggedInPopup = new PersistentBool("_showroomNotLoggedInPopup", def: true);
		_hasUsedEpicChoppa = new PersistentBool("_hasUsedEpicChoppa", def: false);
		_invertedControl = new PersistentBool("_invertedControl", def: false);
		base.OnAwake();
	}

	public override void Destroy()
	{
		_isDestroyed = true;
		base.Destroy();
	}

	public void Save()
	{
		if (!_isDestroyed)
		{
			Preference.Save();
		}
	}
}
