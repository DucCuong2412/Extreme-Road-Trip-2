using System;
using System.Collections;
using System.Collections.Generic;

public class Replay
{
	private List<ReplayFrame> _samples;

	private Car _car;

	private string _playerId;

	private string _playerAlias;

	private int _distance;

	private int _coins;

	private int _stunts;

	private int _duration;

	private string _blobKey;

	private int _rate;

	private int _seed;

	public Replay(Car car, string playerId, string playerAlias)
	{
		_car = car;
		_playerId = playerId;
		_playerAlias = playerAlias;
		_rate = 15;
		_seed = SeedUtil.GetSeed();
		_samples = new List<ReplayFrame>();
	}

	private Replay(Car car, string playerId, string playerAlias, int distance, int coins, int stunts, int duration, string blobKey, int rate, int seed)
		: this(car, playerId, playerAlias)
	{
		_distance = distance;
		_coins = coins;
		_stunts = stunts;
		_duration = duration;
		_blobKey = blobKey;
		_rate = rate;
		_seed = seed;
	}

	public string PlayerAlias()
	{
		return _playerAlias;
	}

	public string PlayerId()
	{
		return _playerId;
	}

	public Car Car()
	{
		return _car;
	}

	public int Rate()
	{
		return _rate;
	}

	public int Seed()
	{
		return _seed;
	}

	public int Distance()
	{
		return _distance;
	}

	public string BlobKey()
	{
		return _blobKey;
	}

	public bool IsReadyToPlay()
	{
		return _samples != null && _samples.Count > 0;
	}

	public void LoadData(byte[] data)
	{
		int num = data.Length / 12;
		byte[] array = new byte[12];
		for (int i = 0; i < num; i++)
		{
			Buffer.BlockCopy(data, i * 12, array, 0, 12);
			_samples.Add(new ReplayFrame(array));
		}
	}

	public void Complete(int distance, int coins, int stunts, int duration)
	{
		_distance = distance;
		_coins = coins;
		_stunts = stunts;
		_duration = duration;
	}

	public void AddSample(CarController car)
	{
		ReplayFrame item = new ReplayFrame(car.transform);
		_samples.Add(item);
	}

	public ReplayFrame GetFrame(int index)
	{
		return _samples[index];
	}

	public int GetLength()
	{
		return _samples.Count;
	}

	public byte[] GetByteArray()
	{
		int count = _samples.Count;
		byte[] array = new byte[count * 12];
		int num = 0;
		foreach (ReplayFrame sample in _samples)
		{
			Buffer.BlockCopy(sample.ToByteArray(), 0, array, num, 12);
			num += 12;
		}
		return array;
	}

	private string GetBase64()
	{
		byte[] byteArray = GetByteArray();
		return Convert.ToBase64String(byteArray);
	}

	public Hashtable ToJsonData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["Type"] = GetType().ToString();
		hashtable["Car"] = _car.Id;
		hashtable["PlayerId"] = _playerId;
		hashtable["PlayerAlias"] = _playerAlias;
		hashtable["Distance"] = _distance;
		hashtable["Coins"] = _coins;
		hashtable["Stunts"] = _stunts;
		hashtable["Duration"] = _duration;
		hashtable["Rate"] = _rate;
		hashtable["Seed"] = _seed;
		hashtable["Platform"] = Device.GetDeviceType();
		return hashtable;
	}

	public static Replay FromJsonData(Hashtable data)
	{
		string text = JsonUtil.ExtractString(data, "PlayerId", null);
		string text2 = JsonUtil.ExtractString(data, "Car", null);
		int num = JsonUtil.ExtractInt(data, "Seed", 0);
		if (text == null || text2 == null || num == 0)
		{
			return null;
		}
		string playerAlias = JsonUtil.ExtractString(data, "PlayerAlias", "unknown alias");
		int distance = JsonUtil.ExtractInt(data, "Distance", 0);
		int coins = JsonUtil.ExtractInt(data, "Coins", 0);
		int stunts = JsonUtil.ExtractInt(data, "Stunts", 0);
		int duration = JsonUtil.ExtractInt(data, "Duration", 0);
		string text3 = JsonUtil.ExtractString(data, "BlobKey", string.Empty);
		int rate = JsonUtil.ExtractInt(data, "Rate", 60);
		Car car = AutoSingleton<CarManager>.Instance.GetCar(text2);
		if (text3 != string.Empty)
		{
			return new Replay(car, text, playerAlias, distance, coins, stunts, duration, text3, rate, num);
		}
		return null;
	}

	public static List<Replay> ListFromJson(string json)
	{
		List<Replay> list = new List<Replay>();
		Hashtable hashtable = json.hashtableFromJson();
		if (hashtable != null && hashtable.Contains("Replays"))
		{
			ArrayList arrayList = hashtable["Replays"] as ArrayList;
			{
				foreach (Hashtable item in arrayList)
				{
					Replay replay = FromJsonData(item);
					if (replay != null)
					{
						list.Add(replay);
					}
				}
				return list;
			}
		}
		return list;
	}
}
