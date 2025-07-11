using System;
using UnityEngine;

public struct ReplayFrame
{
	public const int Size = 12;

	private byte[] _data;

	public ReplayFrame(Transform t)
	{
		_data = new byte[12];
		Vector3 position = t.position;
		float x = position.x;
		Vector3 position2 = t.position;
		float y = position2.y;
		Vector3 eulerAngles = t.rotation.eulerAngles;
		float z = eulerAngles.z;
		Buffer.BlockCopy(BitConverter.GetBytes(x), 0, _data, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(y), 0, _data, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(z), 0, _data, 8, 4);
	}

	public ReplayFrame(byte[] data)
	{
		_data = new byte[12];
		Buffer.BlockCopy(data, 0, _data, 0, 12);
	}

	public Vector3 Position()
	{
		float x = BitConverter.ToSingle(_data, 0);
		float y = BitConverter.ToSingle(_data, 4);
		return new Vector3(x, y, 1f);
	}

	public Quaternion Rotation()
	{
		float z = BitConverter.ToSingle(_data, 8);
		return Quaternion.Euler(0f, 0f, z);
	}

	public byte[] ToByteArray()
	{
		return _data;
	}
}
