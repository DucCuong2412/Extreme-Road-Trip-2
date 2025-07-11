using UnityEngine;

public class Simplex
{
	private static int[,] grad3;

	private static int[] p;

	private static int[] perm;

	private static float F2;

	private static float G2;

	private static int seedx;

	private static int seedy;

	static Simplex()
	{
		grad3 = new int[12, 3]
		{
			{
				1,
				1,
				0
			},
			{
				-1,
				1,
				0
			},
			{
				1,
				-1,
				0
			},
			{
				-1,
				-1,
				0
			},
			{
				1,
				0,
				1
			},
			{
				-1,
				0,
				1
			},
			{
				1,
				0,
				-1
			},
			{
				-1,
				0,
				-1
			},
			{
				0,
				1,
				1
			},
			{
				0,
				-1,
				1
			},
			{
				0,
				1,
				-1
			},
			{
				0,
				-1,
				-1
			}
		};
		p = new int[256]
		{
			151,
			160,
			137,
			91,
			90,
			15,
			131,
			13,
			201,
			95,
			96,
			53,
			194,
			233,
			7,
			225,
			140,
			36,
			103,
			30,
			69,
			142,
			8,
			99,
			37,
			240,
			21,
			10,
			23,
			190,
			6,
			148,
			247,
			120,
			234,
			75,
			0,
			26,
			197,
			62,
			94,
			252,
			219,
			203,
			117,
			35,
			11,
			32,
			57,
			177,
			33,
			88,
			237,
			149,
			56,
			87,
			174,
			20,
			125,
			136,
			171,
			168,
			68,
			175,
			74,
			165,
			71,
			134,
			139,
			48,
			27,
			166,
			77,
			146,
			158,
			231,
			83,
			111,
			229,
			122,
			60,
			211,
			133,
			230,
			220,
			105,
			92,
			41,
			55,
			46,
			245,
			40,
			244,
			102,
			143,
			54,
			65,
			25,
			63,
			161,
			1,
			216,
			80,
			73,
			209,
			76,
			132,
			187,
			208,
			89,
			18,
			169,
			200,
			196,
			135,
			130,
			116,
			188,
			159,
			86,
			164,
			100,
			109,
			198,
			173,
			186,
			3,
			64,
			52,
			217,
			226,
			250,
			124,
			123,
			5,
			202,
			38,
			147,
			118,
			126,
			255,
			82,
			85,
			212,
			207,
			206,
			59,
			227,
			47,
			16,
			58,
			17,
			182,
			189,
			28,
			42,
			223,
			183,
			170,
			213,
			119,
			248,
			152,
			2,
			44,
			154,
			163,
			70,
			221,
			153,
			101,
			155,
			167,
			43,
			172,
			9,
			129,
			22,
			39,
			253,
			19,
			98,
			108,
			110,
			79,
			113,
			224,
			232,
			178,
			185,
			112,
			104,
			218,
			246,
			97,
			228,
			251,
			34,
			242,
			193,
			238,
			210,
			144,
			12,
			191,
			179,
			162,
			241,
			81,
			51,
			145,
			235,
			249,
			14,
			239,
			107,
			49,
			192,
			214,
			31,
			181,
			199,
			106,
			157,
			184,
			84,
			204,
			176,
			115,
			121,
			50,
			45,
			127,
			4,
			150,
			254,
			138,
			236,
			205,
			93,
			222,
			114,
			67,
			29,
			24,
			72,
			243,
			141,
			128,
			195,
			78,
			66,
			215,
			61,
			156,
			180
		};
		perm = new int[512];
		seedx = 0;
		seedy = 0;
		for (int i = 0; i < 512; i++)
		{
			perm[i] = p[i & 0xFF];
		}
		F2 = 0.5f * (Mathf.Sqrt(3f) - 1f);
		G2 = (3f - Mathf.Sqrt(3f)) / 6f;
	}

	private static int fastfloor(float x)
	{
		return (!(x > 0f)) ? ((int)x - 1) : ((int)x);
	}

	private static float dot(int g0, int g1, float x, float y)
	{
		return (float)g0 * x + (float)g1 * y;
	}

	public static void SetSeed(int x, int y)
	{
		seedx = x;
		seedy = y;
	}

	public static float Noise(float xin, float yin)
	{
		xin += (float)seedx;
		yin += (float)seedy;
		float num = (xin + yin) * F2;
		int num2 = fastfloor(xin + num);
		int num3 = fastfloor(yin + num);
		float num4 = (float)(num2 + num3) * G2;
		float num5 = (float)num2 - num4;
		float num6 = (float)num3 - num4;
		float num7 = xin - num5;
		float num8 = yin - num6;
		int num9;
		int num10;
		if (num7 > num8)
		{
			num9 = 1;
			num10 = 0;
		}
		else
		{
			num9 = 0;
			num10 = 1;
		}
		float num11 = num7 - (float)num9 + G2;
		float num12 = num8 - (float)num10 + G2;
		float num13 = num7 - 1f + 2f * G2;
		float num14 = num8 - 1f + 2f * G2;
		int num15 = num2 & 0xFF;
		int num16 = num3 & 0xFF;
		int num17 = perm[num15 + perm[num16]] % 12;
		int num18 = perm[num15 + num9 + perm[num16 + num10]] % 12;
		int num19 = perm[num15 + 1 + perm[num16 + 1]] % 12;
		float num20 = 0.5f - num7 * num7 - num8 * num8;
		float num21;
		if (num20 < 0f)
		{
			num21 = 0f;
		}
		else
		{
			num20 *= num20;
			num21 = num20 * num20 * dot(grad3[num17, 0], grad3[num17, 1], num7, num8);
		}
		float num22 = 0.5f - num11 * num11 - num12 * num12;
		float num23;
		if (num22 < 0f)
		{
			num23 = 0f;
		}
		else
		{
			num22 *= num22;
			num23 = num22 * num22 * dot(grad3[num18, 0], grad3[num18, 1], num11, num12);
		}
		float num24 = 0.5f - num13 * num13 - num14 * num14;
		float num25;
		if (num24 < 0f)
		{
			num25 = 0f;
		}
		else
		{
			num24 *= num24;
			num25 = num24 * num24 * dot(grad3[num19, 0], grad3[num19, 1], num13, num14);
		}
		return 70f * (num21 + num23 + num25);
	}

	public static float Noise01(float xin, float yin)
	{
		return (Noise(xin, yin) + 1f) * 0.5f;
	}
}
