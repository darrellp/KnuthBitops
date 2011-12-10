using System.Numerics;

namespace KnuthBitops
{
	public class Bitops
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
	
		// ruler
		public static int Ruler(long n)
		{
			return DeBruijn64[(((ulong)(RulerConst64 * (n & (-n))) >> 58))];
		}

		public static int Ruler(int n)
		{
			return DeBruijn32[(uint)(RulerConst32 * (n & (-n))) >> 27];
		}
	
		public static int Ruler(short sn)
		{
			ushort n = (ushort) sn;
			if (n == 0)
			{
				return 0;
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
	
		public static int Ruler(byte n)
		{
			return Ruler((short)n);
		}
	
		public static int Ruler(BigInteger n)
		{
			if (n == BigInteger.Zero)
			{
				return 0;
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
			return zeroBits + Ruler(bytes[i]);
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
			short shifted = (short)((ushort)i >> 16);
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
			if (i < 16 && i >= 0)
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
			//int delta = pos2 - pos1;
			//ulong y =  (n ^ (n >> delta)) & mask;
			//return n ^ y ^ (y << delta);
			return SwapBitsMask(n, pos2 - pos1, mask);
		}
	
		internal static ulong SwapBitsMask(ulong n, int delta, ulong mask)
		{
			//return SwapBitsMask(n, 0, delta, mask);
			ulong y = (n ^ (n >> delta)) & mask;
			return n ^ y ^ (y << delta);
		}
	}
}