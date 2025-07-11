using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BackendSaveGame : AutoSingleton<BackendSaveGame>
{
	private struct SaveRequestMessage
	{
		public string PlayerID;

		public string GameVersion;

		public long BaseSaveVersion;

		public GameSaveInfo GameSave;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PlayerID"] = PlayerID;
			hashtable["GameVersion"] = GameVersion;
			hashtable["GameSave"] = GameSave.ToJsonData();
			hashtable["BaseSaveVersion"] = BaseSaveVersion;
			return hashtable;
		}
	}

	private struct SaveRequestAnswer
	{
		public string Status;

		public long Version;

		public static SaveRequestAnswer FromJsonData(Hashtable ht)
		{
			SaveRequestAnswer result = default(SaveRequestAnswer);
			result.Status = JsonUtil.ExtractString(ht, "Status", "invalid");
			result.Version = JsonUtil.ExtractLong(ht, "Version", -1L);
			return result;
		}
	}

	private struct LoadRequestMessage
	{
		public string PlayerID;

		public string GameVersion;

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["PlayerID"] = PlayerID;
			hashtable["GameVersion"] = GameVersion;
			return hashtable;
		}
	}

	public struct GameSaveInfo
	{
		public long Version;

		public string Data;

		public bool IsValid()
		{
			return Version != -1;
		}

		public static GameSaveInfo FromJsonData(Hashtable ht)
		{
			GameSaveInfo result = default(GameSaveInfo);
			result.Version = JsonUtil.ExtractLong(ht, "Version", 0L);
			result.Data = JsonUtil.ExtractString(ht, "Data", "{}");
			return result;
		}

		public Hashtable ToJsonData()
		{
			Hashtable hashtable = new Hashtable();
			hashtable["Version"] = Version;
			hashtable["Data"] = Data;
			return hashtable;
		}
	}

	private struct LoadAnswerMessage
	{
		public string Status;

		public GameSaveInfo Save;

		public static LoadAnswerMessage FromJsonData(Hashtable jsonData)
		{
			LoadAnswerMessage result = default(LoadAnswerMessage);
			result.Status = JsonUtil.ExtractString(jsonData, "Status", "invalid");
			Hashtable ht = JsonUtil.ExtractHashtable(jsonData, "GameSave", new Hashtable());
			result.Save = GameSaveInfo.FromJsonData(ht);
			return result;
		}
	}

	private string _saveHash;

	private Hashtable _save;

	private bool _modified;

	private bool _stopCoroutine;

	private bool _forceSave;

	private bool _isSaving;

	private bool _loaded;

	private bool _saveConflict;

	private long _localVersion;

	[method: MethodImpl(32)]
	public static event Action OnSaveGameLoadSuccess;

	[method: MethodImpl(32)]
	public static event Action OnSaveGameLoadFailed;

	[method: MethodImpl(32)]
	public static event Action OnSaveGameSaveSuccess;

	[method: MethodImpl(32)]
	public static event Action OnSaveGameSaveFailed;

	protected override void OnAwake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_localVersion = -1L;
		_saveHash = null;
		_save = null;
		StartCoroutine(SynchSaveFileCR());
		base.OnAwake();
	}

	public void SaveLocal()
	{
		string a = Security.ComputeHash(AutoSingleton<Preference>.Instance.GetData().toJson());
		if (a != _saveHash)
		{
			_save = AutoSingleton<Preference>.Instance.GetData();
			_saveHash = Security.ComputeHash(_save.toJson());
			_modified = true;
			if (_localVersion == 0L)
			{
				SaveGame();
			}
		}
	}

	protected override void OnDestroy()
	{
		_stopCoroutine = true;
		base.OnDestroy();
	}

	private IEnumerator SynchSaveFileCR()
	{
		Duration delay = new Duration(10f);
		while (!_stopCoroutine)
		{
			if (delay.IsDone() || _forceSave)
			{
				if (_modified || _forceSave)
				{
					SaveGame();
				}
				delay = new Duration(10f);
			}
			yield return new WaitForSeconds(1f);
		}
	}

	public void SaveGame()
	{
		if (!_isSaving && _modified)
		{
			_isSaving = true;
			_modified = false;
			if (AutoSingleton<BackendManager>.Instance.IsLoggedIn())
			{
				GameSaveInfo gameSaveInfo = default(GameSaveInfo);
				gameSaveInfo.Version = _localVersion + 1;
				gameSaveInfo.Data = _save.toJson();
				GameSaveInfo gameSave = gameSaveInfo;
				SaveRequestMessage saveRequestMessage = default(SaveRequestMessage);
				saveRequestMessage.PlayerID = AutoSingleton<BackendManager>.Instance.PlayerIdentifier();
				saveRequestMessage.GameVersion = GameVersion.VERSION;
				saveRequestMessage.BaseSaveVersion = _localVersion;
				saveRequestMessage.GameSave = gameSave;
				SaveRequestMessage saveRequestMessage2 = saveRequestMessage;
				AutoSingleton<BackendManager>.Instance.Post("/save", saveRequestMessage2.ToJsonData(), OnSaveGameSuccess, OnSaveGameFailed);
			}
		}
	}

	private void OnSaveGameSuccess(string response)
	{
		_isSaving = false;
		_forceSave = false;
		SaveRequestAnswer saveRequestAnswer = SaveRequestAnswer.FromJsonData(response.hashtableFromJson());
		if (saveRequestAnswer.Status == "Conflict")
		{
			_saveConflict = true;
			LoadGame();
			TriggerEventSaveFailed();
		}
		else
		{
			_localVersion = saveRequestAnswer.Version;
			TriggerEventSaveSuccess();
		}
	}

	private void OnSaveGameFailed(string error)
	{
		_isSaving = false;
		_forceSave = false;
		TriggerEventSaveFailed();
		SilentDebug.LogWarning("OnSaveFailed: error = " + error);
	}

	public void LoadGame()
	{
		LoadRequestMessage loadRequestMessage = default(LoadRequestMessage);
		loadRequestMessage.PlayerID = AutoSingleton<BackendManager>.Instance.PlayerIdentifier();
		loadRequestMessage.GameVersion = GameVersion.VERSION;
		LoadRequestMessage loadRequestMessage2 = loadRequestMessage;
		AutoSingleton<BackendManager>.Instance.Post("/load", loadRequestMessage2.ToJsonData(), OnLoadGame, OnLoadGameFailed);
	}

	private void OnLoadGame(string response)
	{
		LoadAnswerMessage loadAnswerMessage = LoadAnswerMessage.FromJsonData(response.hashtableFromJson());
		OnLoad(loadAnswerMessage.Save.Data.hashtableFromJson(), loadAnswerMessage.Save.Version);
		if (BackendSaveGame.OnSaveGameLoadSuccess != null)
		{
			BackendSaveGame.OnSaveGameLoadSuccess();
		}
	}

	private void OnLoadGameFailed(string error)
	{
		SilentDebug.LogWarning("OnLoadFailed: error = " + error);
		if (BackendSaveGame.OnSaveGameLoadFailed != null)
		{
			BackendSaveGame.OnSaveGameLoadFailed();
		}
		_saveConflict = false;
	}

	private void OnLoad(Hashtable save, long version)
	{
		_save = save;
		_saveHash = Security.ComputeHash(_save.toJson());
		_localVersion = version;
		_modified = false;
		if (_saveConflict)
		{
			AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.main));
		}
		_saveConflict = false;
	}

	public void OnLogin(GameSaveInfo saveInfo)
	{
		if (!saveInfo.IsValid())
		{
			return;
		}
		Hashtable save = saveInfo.Data.hashtableFromJson();
		long version = saveInfo.Version;
		if (_localVersion == -1 || saveInfo.Version == _localVersion)
		{
			if (version == 0L)
			{
				save = AutoSingleton<Preference>.Instance.GetData();
				OnLoad(save, saveInfo.Version);
				_modified = true;
			}
			else
			{
				OnLoad(save, saveInfo.Version);
			}
		}
		else
		{
			_saveConflict = true;
			OnLoad(save, saveInfo.Version);
		}
	}

	public void OnApplicationQuit()
	{
		SaveGame();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveGame();
		}
	}

	private void TriggerEventSaveSuccess()
	{
		if (BackendSaveGame.OnSaveGameSaveSuccess != null)
		{
			BackendSaveGame.OnSaveGameSaveSuccess();
		}
	}

	private void TriggerEventSaveFailed()
	{
		if (BackendSaveGame.OnSaveGameSaveFailed != null)
		{
			BackendSaveGame.OnSaveGameSaveFailed();
		}
	}

	public bool ForceSaveGameIfModified()
	{
		bool modified = _modified;
		if (modified)
		{
			SaveGame();
		}
		return modified || _isSaving;
	}
}
