using System;
using System.Security.Cryptography;
using System.Text;

public class Security
{
	private static string secret = "mkECIQDto2APhd8LjJoYP7Zf4gOVu5yGoHaWyu8E3ZPuegKmCQIhANKwP7b+lmYQ";

	public static string ComputeHash(string message)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		byte[] bytes = uTF8Encoding.GetBytes(secret);
		HMACSHA1 hMACSHA = new HMACSHA1(bytes);
		byte[] bytes2 = uTF8Encoding.GetBytes(message);
		byte[] inArray = hMACSHA.ComputeHash(bytes2);
		return Convert.ToBase64String(inArray);
	}
}
