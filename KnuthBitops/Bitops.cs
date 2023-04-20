using System.Numerics;

namespace KnuthBitops
{
	public static class Bitops
	{
		private static readonly int[] DeBruijn32 = new[]
		    {
		        00, 01, 28, 02, 29, 14, 24, 03, 30, 22, 20, 15, 25, 17, 04, 8,
		        31, 27, 13, 23, 21, 19, 16, 07, 26, 12, 18, 06, 11, 05, 10, 9
		    };

		private const int RulerConst32 = 0x077cb531;

		private static readonly int[] DeBruijn64 = new[]
		    {
		        00, 01, 56, 02, 57, 49, 28, 03, 61, 58, 42, 50, 38, 29, 17, 04,
		        62, 47, 59, 36, 45, 43, 51, 22, 53, 39, 33, 30, 24, 18, 12, 05,
		        63, 55, 48, 27, 60, 41, 37, 16, 46, 35, 44, 21, 52, 32, 23, 11,
		        54, 26, 40, 15, 34, 20, 31, 10, 25, 14, 19, 9, 13, 8, 07, 06
		    };

		private const long RulerConst64 = 0x03f79d71b4ca8b09L;

		private const ulong HighBits = 0x8080808080808080UL;
		private const ulong LowBits = 0x0101010101010101UL;

		public const ulong Mu0 = 0x5555555555555555UL;
		public const ulong Mu1 = 0x3333333333333333UL;
		public const ulong Mu2 = 0x0f0f0f0f0f0f0f0fUL;
		public const ulong Mu3 = 0x00ff00ff00ff00ffUL;
		public const ulong Mu4 = 0x0000ffff0000ffffUL;
		public const ulong Mu5 = 0x00000000ffffffffUL;

		public static readonly ulong[] Mus = new[] {Mu0, Mu1, Mu2, Mu3, Mu4, Mu5};

		// DropRightmostOne
		public static long DropRightmostOne(long n)
		{
			return n & (n - 1);
		}

		public static int DropRightmostOne(int n)
		{
			return n & (n - 1);
		}

		public static short DropRightmostOne(short n)
		{
			return (short)(n & (n - 1));
		}

		public static byte DropRightmostOne(byte n)
		{
			return (byte)(n & (n - 1));
		}

		public static BigInteger DropRightmostOne(BigInteger n)
		{
			return n & (n - 1);
		}

		// ExtractRightmostOne
		public static long ExtractRightmostOne(long n)
		{
			return n & (-n);
		}
	
		public static int ExtractRightmostOne(int n)
		{
			return n & (-n);
		}
	
		public static short ExtractRightmostOne(short n)
		{
			return (short) (n & (-n));
		}
	
		public static byte ExtractRightmostOne(byte n)
		{
			return (byte) (n & (-n));
		}
	
		public static BigInteger ExtractRightmostOne(BigInteger n)
		{
			return n & (-n);
		}
	
		// smearRightmostOneLeft
		public static long SmearRightmostOneLeft(long n)
		{
			return n | (-n);
		}
	
		public static int SmearRightmostOneLeft(int n)
		{
			return n | (-n);
		}
	
		public static short SmearRightmostOneLeft(short n)
		{
			return (short) (n | (-n));
		}
	
		public static byte SmearRightmostOneLeft(byte n)
		{
			return (byte) (n | (-n));
		}
	
		public static BigInteger SmearRightmostOneLeft(BigInteger n)
		{
			return n | (-n);
		}
	
		// removeAndSmearRightmostOneLeft
		public static long RemoveAndSmearRightmostOneLeft(long n)
		{
			return n ^ (-n);
		}
	
		public static int RemoveAndSmearRightmostOneLeft(int n)
		{
			return n ^ (-n);
		}
	
		public static short RemoveAndSmearRightmostOneLeft(short n)
		{
			return (short) (n ^ (-n));
		}
	
		public static byte RemoveAndSmearRightmostOneLeft(byte n)
		{
			return (byte) (n ^ (-n));
		}
	
		public static BigInteger RemoveAndSmearRightmostOneLeft(BigInteger n)
		{
			return n ^ (-n);
		}
	
		// smearRightmostOneRight
		public static long SmearRightmostOneRight(long n)
		{
			return n | (n - 1);
		}
	
		public static int SmearRightmostOneRight(int n)
		{
			return n | (n - 1);
		}
	
		public static short SmearRightmostOneRight(short n)
		{
			return (short) (n | (n - 1));
		}
	
		public static byte SmearRightmostOneRight(byte n)
		{
			return (byte) (n | (n - 1));
		}
	
		public static BigInteger SmearRightmostOneRight(BigInteger n)
		{
			return n | (n - 1);
		}
	
		// extractAndSmearRightmostOneRight
		public static long ExtractAndSmearRightmostOneRight(long n)
		{
			return n ^ (n - 1);
		}
	
		public static int ExtractAndSmearRightmostOneRight(int n)
		{
			return n ^ (n - 1);
		}
	
		public static short ExtractAndSmearRightmostOneRight(short n)
		{
			return (short) (n ^ (n - 1));
		}
	
		public static byte ExtractAndSmearRightmostOneRight(byte n)
		{
			return (byte) (n ^ (n - 1));
		}
	
		public static BigInteger ExtractAndSmearRightmostOneRight(BigInteger n)
		{
			return n ^ (n - 1);
		}
	
		// extractRemoveSmearRightmostOneRight
		public static long ExtractRemoveSmearRightmostOneRight(long n)
		{
			return (~n) & (n - 1);
		}
	
		public static int ExtractRemoveSmearRightmostOneRight(int n)
		{
			return (~n) & (n - 1);
		}
	
		public static short ExtractRemoveSmearRightmostOneRight(short n)
		{
			return (short) ((~n) & (n - 1));
		}
	
		public static byte ExtractRemoveSmearRightmostOneRight(byte n)
		{
			return (byte) ((~n) & (n - 1));
		}
	
		public static BigInteger ExtractRemoveSmearRightmostOneRight(BigInteger n)
		{
			return (~n) & (n - 1);
		}
	
		// removeRightmostOnes
		public static long RemoveRightmostOnes(long n)
		{
			return ((n | (n - 1)) + 1) & n;
		}
	
		public static int RemoveRightmostOnes(int n)
		{
			return ((n | (n - 1)) + 1) & n;
		}
	
		public static short RemoveRightmostOnes(short n)
		{
			return (short) (((n | (n - 1)) + 1) & n);
		}
	
		public static byte RemoveRightmostOnes(byte n)
		{
			return (byte) (((n | (n - 1)) + 1) & n);
		}
	
		public static BigInteger RemoveRightmostOnes(BigInteger n)
		{
			return ((n | (n - 1)) + 1) & n;
		}
	
		// This is Knuth's Ruler or Rho function
		public static int RightmostOneIndex(long n)
		{
			if (n == 0)
			{
				return -1;
			}
			return DeBruijn64[(((ulong)(RulerConst64 * (n & (-n))) >> 58))];
		}

		public static int RightmostOneIndex(int n)
		{
			if (n == 0)
			{
				return -1;
			}
			return DeBruijn32[(uint)(RulerConst32 * (n & (-n))) >> 27];
		}
	
		public static int RightmostOneIndex(short sn)
		{
			ushort n = (ushort) sn;
			if (n == 0)
			{
				return -1;
			}
			for (int i = 0; n != 0; n >>= 1, i++)
			{
				if ((n & 1) != 0)
				{
					return i;
				}
			}
			// We should never get here
			return 0;
		}
	
		public static int RightmostOneIndex(byte n)
		{
			return RightmostOneIndex((short)n);
		}
	
		public static int RightmostOneIndex(BigInteger n)
		{
			if (n == BigInteger.Zero)
			{
				return -1;
			}
			byte[] bytes = n.ToByteArray();
			int zeroBits = 0;
			int i;
			for (i = 0; i < bytes.Length; i++)
			{
				if (bytes[i] != 0)
				{
					break;
				}
				zeroBits += 8;
			}
			return zeroBits + RightmostOneIndex(bytes[i]);
		}
	
		// lefmostOneIndex
		public static int LeftmostOneIndex(long l)
		{
			int shifted = (int)((ulong)l >> 32);
			if (shifted != 0)
			{
				return 32 + LeftmostOneIndex(shifted);
			}
			return LeftmostOneIndex((int)l);
		}
	
		public static int LeftmostOneIndex(int i)
		{
			short shifted = (short)((uint)i >> 16);
			if (shifted != 0)
			{
				return 16 + LeftmostOneIndex(shifted);
			}
			return LeftmostOneIndex((short)i);
		}
	
		public static int LeftmostOneIndex(short i)
		{
			byte shifted = (byte)(i >> 8);
			if (shifted != 0)
			{
				return 8 + LeftmostOneIndex(shifted);
			}
			return LeftmostOneIndex((byte)i);
		}
	
		public static int LeftmostOneIndex(byte i)
		{
			if (i < 16)
			{
				return LeftmostOneIndexNybble(i);
			}
			return 4 + LeftmostOneIndexNybble((byte)(i >> 4));
		}
	
		static readonly int[] NybbleVals = new[]
				{-1, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3};

		private static int LeftmostOneIndexNybble(byte nybble)
		{
			return NybbleVals[nybble];
		}
	
		public static int LeftmostOneIndex(BigInteger n)
		{
			if (n == BigInteger.Zero)
			{
				return -1;
			}
			byte[] rgb = n.ToByteArray();
			return (rgb.Length - 1) * 8 + LeftmostOneIndex(rgb[rgb.Length - 1]);
		}

		// The positions here correspond to the powers of 2 so that
		// 0 corresponds to the units bit.
		public static ulong SwapBits(ulong n, int pos1, int pos2)
		{
			if (pos1 == pos2)
			{
				return n;
			}
			if (pos1 > pos2)
			{
				int t = pos2;
				pos2 = pos1;
				pos1 = t;
			}
			return SwapBitsMask(n, pos1, pos2, 1ul << pos1);
		}
	
		private static ulong SwapBitsMask(ulong n, int pos1, int pos2, ulong mask)
		{
			return SwapBitsMask(n, pos2 - pos1, mask);
		}
	
		internal static ulong SwapBitsMask(ulong n, int delta, ulong mask)
		{
			ulong y = (n ^ (n >> delta)) & mask;
			return n ^ y ^ (y << delta);
		}

		public static ulong ReverseBits(ulong x)
		{
			int iShift = 1;

			for (int iMu = 0; iMu < 5; iMu++, iShift *= 2)
			{
				x = ReverseStep(x, iMu, iShift);
			}
			return (x >> 32) | (x << 32);
		}

		private static ulong ReverseStep(ulong x, int iMu, int iShift)
		{
			ulong mu = Mus[iMu];
			ulong y = (x >> iShift) & mu;
			ulong z = (x & mu) << iShift;
			return y | z;
		}

		public static int BitCount(ulong x)
		{
			ulong y = x - ((x >> 1) & Mu0);
			y = (y & Mu1) + ((y >> 2) & Mu1);
			y = (y + (y >> 4)) & Mu2;
			return (int)((LowBits * y) >> 56);
		}

		internal static ulong ShiftBitsMask(ulong x, int delta, ulong mask)
		{
			return x ^ ((x ^ (x >> delta)) & mask);
		}

		// Increment the fragmented field represented by the mask inside
		// a word of zeroes as described on p. 150 of TAOCP V4A.
		public static ulong NextFragmentedField(ulong x, ulong mask)
		{
			return (x - mask) & mask;
		}

		// Increment the subcube vertex as described on p. 150 of TAOCP V4A.
		// Really, this is just the NextFragmentedField algorithm allowing
		// for potential ones in the word around the fragmented field.
		public static ulong NextSubcubeVertex(ulong x, ulong a, ulong b)
		{
			return ((x - (a + b)) & a) + b;
		}

		// Add two fragmented fields as described on p. 150 of TAOCP V4A.
		public static ulong AddFragmentedFields(ulong x, ulong y, ulong mask)
		{
			return ((x & mask) + (y | ~mask)) & mask;
		}

		// Add the 8 bytes in one long with the 8 bytes in another as described on
		// p. 151 of TAOCP V4A.
		public static ulong AddBytes(ulong x, ulong y)
		{
			return ((x ^ y) & HighBits) ^ ((x & ~HighBits) + (y & ~HighBits));
		}

		// Subtract the 8 bytes in one long with the 8 bytes in another as described in
		// exercise 88 of section 7.1.3 of TAOCP V4A.
		public static ulong SubtractBytes(ulong x, ulong y)
		{
			return ((x ^ ~y) & HighBits) ^ ((x | HighBits) - (y & ~HighBits));
		}

		// Average bytes as described on p. 151 of TAOCP V4A.
		public static ulong AverageBytes(ulong x, ulong y)
		{
			return (((x ^ y) & ~LowBits) >> 1) + (x & y);
		}

		public static bool IsAnyByteZero(ulong n)
		{
			return (HighBits & (n - LowBits) & ~n) != 0;
		}

		public static ulong LocateZeroes(ulong n)
		{
			return HighBits & ~(n | ((n | HighBits) - LowBits));
		}

		public static bool ContainsByte(ulong n, byte b)
		{
			return IsAnyByteZero(n ^ RepeatByte(b));
		}

		// Returns a long with b in every byte
		public static ulong RepeatByte(byte b)
		{
			// This appears to be barely faster than LowBits * b, but not by much
			ulong z = (ulong)((b << 8) | b);
			z = (z << 16) | z;
			return (z << 32) | z;
		}

		// Replaces the j'th byte with 128 * [ x[j] < y[j] ] as documented on p. 153 of TAOCP V4A.
		public static ulong CompareBytes(ulong x, ulong y)
		{
			ulong z = (x | HighBits) - (y & ~HighBits);
			return HighBits & ~Median(x, ~y, z);
		}

		private static int[] _rBits;
		private static int[] _lBits;

		private static ulong GatherHighBits(ulong t)
		{
			t = t + (t << 7);
			t = t + (t << 14);
			t = t + (t << 28);
			return t >> 56;
		}

		public static int LeftIndexOfTValue(ulong t)
		{
			if (t == 0)
			{
				return -1;
			}
			if (_lBits == null)
			{
				_lBits = new int[256];
				for (byte i = 0; i < 255; i++)
				{
					_lBits[i] = LeftmostOneIndex(i);
				}
			}
			return _lBits[GatherHighBits(t)];
		}

		public static int RightIndexOfTValue(ulong t)
		{
			if (t == 0)
			{
				return -1;
			}
			if (_rBits == null)
			{
				_rBits = new int[256];
				for (byte i = 0; i < 255; i++)
				{
					_rBits[i] = RightmostOneIndex(i);
				}
			}
			return _rBits[GatherHighBits(t)];
		}

		public static ulong Median(ulong x, ulong y, ulong z)
		{
			return (x & y) | (y & z) | (x & z);
		}
	}
}