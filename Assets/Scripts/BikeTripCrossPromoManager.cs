using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeTripCrossPromoManager : AutoSingleton<BikeTripCrossPromoManager>
{
	public enum PromoType
	{
		Popup,
		MultiplayerButton,
		Unknow
	}

	public enum PopupPlacementId
	{
		MainMenu,
		EndRun,
		Unknown
	}

	public enum MultiplayerButtonPlacementId
	{
		MainMenu,
		EndRun,
		Unknown
	}

	private struct BikeTripPromoConfigRequestMessage
	{
		public string GameCenterId;

		public string FacebookId;

		public string DeviceId;

		public string Platform;

		public string GameVersion;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["GameCenterId"] = GameCenterId;
			hashtable["FacebookId"] = FacebookId;
			hashtable["DeviceId"] = DeviceId;
			hashtable["Platform"] = Platform;
			hashtable["GameVersion"] = GameVersion;
			return hashtable;
		}
	}

	private struct BikeTripPromoConfigResponseMessage
	{
		public BikeTripPromoConfig Config;

		public static BikeTripPromoConfigResponseMessage FromJsonData(Hashtable ht)
		{
			BikeTripPromoConfigResponseMessage result = default(BikeTripPromoConfigResponseMessage);
			Hashtable jsonData = JsonUtil.ExtractHashtable(ht, "Data", new Hashtable());
			result.Config = BikeTripPromoConfig.FromJsonData(jsonData);
			return result;
		}
	}

	private struct BikeTripPromoConfig
	{
		private PopupConfig _popupConfig;

		private MutliplayerButtonConfig _buttonConfig;

		public bool IsEnabled()
		{
			return _popupConfig.IsEnabled() || _buttonConfig.IsEnabled();
		}

		public static BikeTripPromoConfig FromJsonData(Hashtable jsonData)
		{
			BikeTripPromoConfig result = default(BikeTripPromoConfig);
			ArrayList arrayList = JsonUtil.ExtractArrayList(jsonData, "BikeTripPromoConfig", new ArrayList());
			foreach (Hashtable item in arrayList)
			{
				switch (EnumUtil.Parse(JsonUtil.ExtractString(item, "Type", string.Empty), PromoType.Unknow))
				{
				case PromoType.Popup:
					result._popupConfig = PopupConfig.FromJsonData(item);
					break;
				case PromoType.MultiplayerButton:
					result._buttonConfig = MutliplayerButtonConfig.FromJsonData(item);
					break;
				default:
					SilentDebug.LogWarning("BikeTripPromo: Unknown promo type config");
					break;
				}
			}
			return result;
		}

		public PopupConfig GetPopupConfig()
		{
			return _popupConfig;
		}

		public MutliplayerButtonConfig GetMutliplayerButtonConfig()
		{
			return _buttonConfig;
		}
	}

	private struct PopupConfig
	{
		private struct PlacementConfig
		{
			public bool Enable;

			public int Frequence;

			public int MaxDisplay;

			public int CloseButtonSec;
		}

		private bool _enable;

		private Dictionary<PopupPlacementId, PlacementConfig> _placements;

		public bool IsEnabled()
		{
			return _enable && _placements != null && _placements.Count > 0;
		}

		public bool IsPlacementEnabled(PopupPlacementId id)
		{
			bool result = false;
			if (IsEnabled() && _placements != null && _placements.ContainsKey(id))
			{
				PlacementConfig placementConfig = _placements[id];
				result = placementConfig.Enable;
			}
			return result;
		}

		public int GetFrequency(PopupPlacementId id)
		{
			if (IsEnabled() && _placements != null && _placements.ContainsKey(id))
			{
				PlacementConfig placementConfig = _placements[id];
				return placementConfig.Frequence;
			}
			return -1;
		}

		public int GetMaxDisplay(PopupPlacementId id)
		{
			if (IsEnabled() && _placements != null && _placements.ContainsKey(id))
			{
				PlacementConfig placementConfig = _placements[id];
				return placementConfig.MaxDisplay;
			}
			return -1;
		}

		public int GetCloseButtonSec(PopupPlacementId id)
		{
			if (IsEnabled() && _placements != null && _placements.ContainsKey(id))
			{
				PlacementConfig placementConfig = _placements[id];
				return placementConfig.CloseButtonSec;
			}
			return 0;
		}

		public static PopupConfig FromJsonData(Hashtable jsonData)
		{
			PopupConfig result = default(PopupConfig);
			result._enable = false;
			if (EnumUtil.Parse(JsonUtil.ExtractString(jsonData, "Type", string.Empty), PromoType.Unknow) == PromoType.Popup)
			{
				result._enable = JsonUtil.ExtractBool(jsonData, "Enable", def: false);
				if (result._enable)
				{
					ArrayList arrayList = JsonUtil.ExtractArrayList(jsonData, "Placements", new ArrayList());
					result._placements = new Dictionary<PopupPlacementId, PlacementConfig>();
					{
						foreach (Hashtable item in arrayList)
						{
							PopupPlacementId popupPlacementId = EnumUtil.Parse(JsonUtil.ExtractString(item, "PlacementId", string.Empty), PopupPlacementId.Unknown);
							if (popupPlacementId != PopupPlacementId.Unknown)
							{
								bool flag = JsonUtil.ExtractBool(item, "Enable", def: false);
								int num = JsonUtil.ExtractInt(item, "Frequency", -1);
								int maxDisplay = JsonUtil.ExtractInt(item, "MaxDisplay", 0);
								int closeButtonSec = JsonUtil.ExtractInt(item, "CloseButtonSec", 0);
								if (num > 0 && flag)
								{
									PlacementConfig value = default(PlacementConfig);
									value.Enable = flag;
									value.Frequence = num;
									value.MaxDisplay = maxDisplay;
									value.CloseButtonSec = closeButtonSec;
									result._placements[popupPlacementId] = value;
								}
							}
						}
						return result;
					}
				}
			}
			return result;
		}
	}

	private struct MutliplayerButtonConfig
	{
		private bool _enable;

		private Dictionary<MultiplayerButtonPlacementId, bool> _placements;

		public bool IsEnabled()
		{
			return _enable && _placements != null && _placements.Count > 0;
		}

		public bool IsEnabled(MultiplayerButtonPlacementId id)
		{
			return IsEnabled() && _placements != null && _placements.ContainsKey(id) && _placements[id];
		}

		public static MutliplayerButtonConfig FromJsonData(Hashtable jsonData)
		{
			MutliplayerButtonConfig result = default(MutliplayerButtonConfig);
			result._enable = false;
			PromoType promoType = EnumUtil.Parse(JsonUtil.ExtractString(jsonData, "Type", string.Empty), PromoType.Unknow);
			if (promoType == PromoType.MultiplayerButton)
			{
				result._enable = JsonUtil.ExtractBool(jsonData, "Enable", def: false);
				if (result._enable)
				{
					ArrayList arrayList = JsonUtil.ExtractArrayList(jsonData, "Placements", new ArrayList());
					result._placements = new Dictionary<MultiplayerButtonPlacementId, bool>();
					{
						foreach (Hashtable item in arrayList)
						{
							MultiplayerButtonPlacementId multiplayerButtonPlacementId = EnumUtil.Parse(JsonUtil.ExtractString(item, "PlacementId", string.Empty), MultiplayerButtonPlacementId.Unknown);
							if (multiplayerButtonPlacementId != MultiplayerButtonPlacementId.Unknown)
							{
								bool flag = JsonUtil.ExtractBool(item, "Enable", def: false);
								if (flag)
								{
									result._placements[multiplayerButtonPlacementId] = flag;
								}
							}
						}
						return result;
					}
				}
			}
			return result;
		}
	}

	private const string _crossPromoID = "BikeTripPromoConfig";

	private BikeTripPromoConfig _config;

	private Dictionary<PopupPlacementId, int> _displayAttemptCount;

	private Dictionary<PopupPlacementId, int> _displayCount;

	private PersistentBool _bikeTripHaveBeenInstalled;

	private PersistentBool _bikeTripPromoUrlClicked;

	private DateTime _lastUpdateTime;

	private int _deltaTimeUpdateMinute = 60;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		TextAsset textAsset = Resources.Load("bikeTripPromo.json", typeof(TextAsset)) as TextAsset;
		BikeTripPromoConfig config = BikeTripPromoConfig.FromJsonData(textAsset.text.hashtableFromJson());
		_bikeTripHaveBeenInstalled = new PersistentBool("_isBikeTripHaveBeenInstalled", def: false);
		_bikeTripPromoUrlClicked = new PersistentBool("_bikeTripPromoUrlClicked", def: false);
		UpdateConfig(config);
		LoadDisplayCount();
		GetServerConfig();
	}

	private void LoadDisplayCount()
	{
		string @string = Preference.GetString("BikeTripPromoData", "{}");
		Hashtable data = @string.hashtableFromJson();
		Hashtable hashtable = JsonUtil.ExtractHashtable(data, "DisplayCount", new Hashtable());
		_displayCount = new Dictionary<PopupPlacementId, int>();
		Hashtable data2 = JsonUtil.ExtractHashtable(data, "DisplayAttemptCount", new Hashtable());
		_displayAttemptCount = new Dictionary<PopupPlacementId, int>();
		foreach (int value in Enum.GetValues(typeof(PopupPlacementId)))
		{
			if (hashtable.Contains(((PopupPlacementId)value).ToString()))
			{
				_displayCount[(PopupPlacementId)value] = JsonUtil.ExtractInt(hashtable, ((PopupPlacementId)value).ToString(), 0);
				_displayAttemptCount[(PopupPlacementId)value] = JsonUtil.ExtractInt(data2, ((PopupPlacementId)value).ToString(), 0);
			}
		}
	}

	private void SaveDisplayCount()
	{
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		foreach (KeyValuePair<PopupPlacementId, int> item in _displayCount)
		{
			hashtable[item.Key.ToString()] = item.Value;
		}
		foreach (KeyValuePair<PopupPlacementId, int> item2 in _displayAttemptCount)
		{
			hashtable2[item2.Key.ToString()] = item2.Value;
		}
		string @string = Preference.GetString("BikeTripPromoData", "{}");
		Hashtable hashtable3 = @string.hashtableFromJson();
		hashtable3["DisplayCount"] = hashtable;
		hashtable3["DisplayAttemptCount"] = hashtable2;
		Preference.SetString("BikeTripPromoData", hashtable3.toJson());
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			RefreshIsBikeTripInstalled();
			if ((DateTime.Now - _lastUpdateTime).TotalMinutes >= (double)_deltaTimeUpdateMinute)
			{
				GetServerConfig();
			}
		}
	}

	private void GetServerConfig()
	{
		if (!IsPromoConsumed())
		{
			_lastUpdateTime = DateTime.Now;
			BikeTripPromoConfigRequestMessage bikeTripPromoConfigRequestMessage = default(BikeTripPromoConfigRequestMessage);
			bikeTripPromoConfigRequestMessage.DeviceId = Device.GetDeviceId();
			bikeTripPromoConfigRequestMessage.GameCenterId = AutoSingleton<BackendManager>.Instance.GetGameCenterID();
			bikeTripPromoConfigRequestMessage.FacebookId = AutoSingleton<BackendManager>.Instance.GetFacebookID();
			bikeTripPromoConfigRequestMessage.Platform = Device.GetDeviceType();
			bikeTripPromoConfigRequestMessage.GameVersion = GameVersion.VERSION;
			AutoSingleton<BackendManager>.Instance.Post("/crosspromo/ebt/getconfig", bikeTripPromoConfigRequestMessage.ToJsonData(), OnGetServerConfigSuccess, OnGetServerConfigFailed);
		}
	}

	public void OnGetServerConfigSuccess(string dataJson)
	{
		BikeTripPromoConfigResponseMessage bikeTripPromoConfigResponseMessage = BikeTripPromoConfigResponseMessage.FromJsonData(dataJson.hashtableFromJson());
		UpdateConfig(bikeTripPromoConfigResponseMessage.Config);
	}

	public void OnGetServerConfigFailed(string error)
	{
	}

	private void UpdateConfig(BikeTripPromoConfig config)
	{
		RefreshIsBikeTripInstalled();
		_config = config;
	}

	private void RefreshIsBikeTripInstalled()
	{
		if (!_bikeTripHaveBeenInstalled.Get())
		{
			_bikeTripHaveBeenInstalled.Set(IsBikeTripInstalled());
			if (!_bikeTripHaveBeenInstalled.Get())
			{
			}
		}
	}

	private bool IsBikeTripInstalled()
	{
		return AutoSingleton<ExternalAppLauncher>.Instance.IsGameInstalled("ebt");
	}

	public bool DisplayMultiplayerButtonPlacement(MultiplayerButtonPlacementId id)
	{
		return IsPromoEnabled() && _config.GetMutliplayerButtonConfig().IsEnabled(id);
	}

	private bool DisplayPopupPlacement(PopupPlacementId id)
	{
		bool result = false;
		if (IsPromoEnabled() && _config.GetPopupConfig().IsPlacementEnabled(id))
		{
			if (!_displayCount.ContainsKey(id))
			{
				_displayCount[id] = 0;
				_displayAttemptCount[id] = 0;
			}
			if (_displayCount[id] < _config.GetPopupConfig().GetMaxDisplay(id))
			{
				Dictionary<PopupPlacementId, int> displayAttemptCount;
				Dictionary<PopupPlacementId, int> dictionary = displayAttemptCount = _displayAttemptCount;
				PopupPlacementId key;
				PopupPlacementId key2 = key = id;
				int num = displayAttemptCount[key];
				dictionary[key2] = num + 1;
				if (_displayAttemptCount[id] % _config.GetPopupConfig().GetFrequency(id) == 0)
				{
					Dictionary<PopupPlacementId, int> displayCount;
					Dictionary<PopupPlacementId, int> dictionary2 = displayCount = _displayCount;
					PopupPlacementId key3 = key = id;
					num = displayCount[key];
					dictionary2[key3] = num + 1;
					result = true;
				}
				SaveDisplayCount();
			}
		}
		return result;
	}

	private bool IsPromoConsumed()
	{
		return _bikeTripHaveBeenInstalled.Get() || _bikeTripPromoUrlClicked.Get();
	}

	public static bool DisplayMainMenuPopup()
	{
		return AutoSingleton<BikeTripCrossPromoManager>.Instance.DisplayPopupPlacement(PopupPlacementId.MainMenu);
	}

	public static int GetMainMenuCloseDelay()
	{
		return AutoSingleton<BikeTripCrossPromoManager>.Instance._config.GetPopupConfig().GetCloseButtonSec(PopupPlacementId.MainMenu);
	}

	public static bool DisplayEndRunPopup()
	{
		return AutoSingleton<BikeTripCrossPromoManager>.Instance.DisplayPopupPlacement(PopupPlacementId.EndRun);
	}

	public static int GetEndRunCloseDelay()
	{
		return AutoSingleton<BikeTripCrossPromoManager>.Instance._config.GetPopupConfig().GetCloseButtonSec(PopupPlacementId.EndRun);
	}

	public static bool IsMainMenuMutiplayerButtonEnabled()
	{
		return AutoSingleton<BikeTripCrossPromoManager>.Instance.DisplayMultiplayerButtonPlacement(MultiplayerButtonPlacementId.MainMenu);
	}

	public static bool IsEndRunMultipluerButtonEnabled()
	{
		return AutoSingleton<BikeTripCrossPromoManager>.Instance.DisplayMultiplayerButtonPlacement(MultiplayerButtonPlacementId.EndRun);
	}

	public static bool IsSupported()
	{
		return true;
	}

	public static bool IsEnabled()
	{
		if (!AutoSingleton<BikeTripCrossPromoManager>.IsCreated())
		{
			SilentDebug.LogWarning("BikeTripCrossPromoManager not created: Enabled False");
			return false;
		}
		return AutoSingleton<BikeTripCrossPromoManager>.Instance.IsPromoEnabled();
	}

	public string GetMobileTrackingUrl()
	{
		return AutoSingleton<ExternalAppLauncher>.Instance.GetExternalStoreURL("ebt");
	}

	private bool IsPromoEnabled()
	{
		return !IsPromoConsumed() && _config.IsEnabled();
	}

	public void ConsumePromo()
	{
		_bikeTripPromoUrlClicked.Set(v: true);
	}
}
