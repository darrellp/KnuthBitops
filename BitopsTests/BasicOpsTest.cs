using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KnuthBitops;

namespace BitopsTests
{
	[TestClass]
	public class BasicOpsTest
	{
		[TestMethod]
		public void TestSwapBits()
		{
			Assert.AreEqual(1ul, Bitops.SwapBits(2, 0, 1));
			Assert.AreEqual(1ul, Bitops.SwapBits(2, 1, 0));
			Assert.AreEqual(3ul, Bitops.SwapBits(3, 0, 1));
			Assert.AreEqual(3ul, Bitops.SwapBits(3, 1, 0));
			Assert.AreEqual(5ul, Bitops.SwapBits(6, 0, 1));
			Assert.AreEqual(8ul, Bitops.SwapBits(8, 0, 1));
		}

		[TestMethod]
		public void TestRemoveRightmostOne()
		{
			BigInteger n = new BigInteger(12);
			BigInteger r = new BigInteger(8);
			BigInteger nBig = BigInteger.Parse("135235123512353451235123512517");
			BigInteger rBig = BigInteger.Parse("135235123512353451235123512516");
		
			Assert.AreEqual(8, Bitops.DropRightmostOne(12));
			Assert.AreEqual(0, Bitops.DropRightmostOne(0));
			Assert.AreEqual(6, Bitops.DropRightmostOne(7));
			Assert.AreEqual(8, Bitops.DropRightmostOne((byte)12));
			Assert.AreEqual(r, Bitops.DropRightmostOne(n));
			Assert.AreEqual(rBig, Bitops.DropRightmostOne(nBig));
		}
	
		[TestMethod]
		public void TestExtractRightmostOne()
		{
			BigInteger n = new BigInteger(12);
			BigInteger r = new BigInteger(4);
		
			Assert.AreEqual(4, Bitops.ExtractRightmostOne(12));
			Assert.AreEqual(r, Bitops.ExtractRightmostOne(n));
		}
	
		[TestMethod]
		public void TestSmearRightmostOneLeft()
		{
			BigInteger n = new BigInteger(12);
			BigInteger r = new BigInteger(-4);
		
			Assert.AreEqual(-4, Bitops.SmearRightmostOneLeft(12));
			Assert.AreEqual(r, Bitops.SmearRightmostOneLeft(n));
		}
	
		[TestMethod]
		public void TestRemoveAndSmearRightmostOneLeft()
		{
			BigInteger n = new BigInteger(12);
			BigInteger r = new BigInteger(-8);
		
			Assert.AreEqual(-8, Bitops.RemoveAndSmearRightmostOneLeft(12));
			Assert.AreEqual(r, Bitops.RemoveAndSmearRightmostOneLeft(n));
		}
	
		[TestMethod]
		public void TestSmearRightmostOneRight()
		{
			BigInteger n = new BigInteger(12);
			BigInteger r = new BigInteger(15);
		
			Assert.AreEqual(15, Bitops.SmearRightmostOneRight(12));
			Assert.AreEqual(r, Bitops.SmearRightmostOneRight(n));
		}
	
		[TestMethod]
		public void TestExtractAndSmearRightmostOneRight()
		{
			BigInteger n = new BigInteger(12);
			BigInteger r = new BigInteger(7);
		
			Assert.AreEqual(7, Bitops.ExtractAndSmearRightmostOneRight(12));
			Assert.AreEqual(r, Bitops.ExtractAndSmearRightmostOneRight(n));
		}
	
		[TestMethod]
		public void TestExtractRemoveSmearRightmostOneRight()
		{
			BigInteger n = new BigInteger(12);
			BigInteger r = new BigInteger(3);
		
			Assert.AreEqual(3, Bitops.ExtractRemoveSmearRightmostOneRight(12));
			Assert.AreEqual(r, Bitops.ExtractRemoveSmearRightmostOneRight(n));
		}
	
		[TestMethod]
		public void TestRemoveRightmostOnes()
		{
			BigInteger n = new BigInteger(22);
			BigInteger r = new BigInteger(16);
		
			Assert.AreEqual(16, Bitops.RemoveRightmostOnes(22));
			Assert.AreEqual(r, Bitops.RemoveRightmostOnes(n));
		}
	
		[TestMethod]
		public void TestRightmostOneIndex()
		{
			Assert.AreEqual(-1, Bitops.RightmostOneIndex((long)0));
			Assert.AreEqual(-1, Bitops.RightmostOneIndex((short)0));
			Assert.AreEqual(-1, Bitops.RightmostOneIndex((byte)0));
			Assert.AreEqual(-1, Bitops.RightmostOneIndex(0));
			Assert.AreEqual(-1, Bitops.RightmostOneIndex(new BigInteger(0)));
			Assert.AreEqual(0, Bitops.RightmostOneIndex((long)(-1)));
			Assert.AreEqual(0, Bitops.RightmostOneIndex((short)(-1)));
			Assert.AreEqual(0, Bitops.RightmostOneIndex((byte)(0xff)));
			Assert.AreEqual(0, Bitops.RightmostOneIndex(-1));
			Assert.AreEqual(0, Bitops.RightmostOneIndex(new BigInteger(-1)));
			for (long i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.RightmostOneIndex(l));
			}
			for (int i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.RightmostOneIndex(l));
			}
			for (short i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.RightmostOneIndex(l));
			}
			for (byte i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.RightmostOneIndex(l));
			}
			Assert.AreEqual(8, Bitops.RightmostOneIndex(new BigInteger(256)));
		}
	
		[TestMethod]
		public void TestLeftmostOneIndex()
		{
			Assert.AreEqual(36, Bitops.LeftmostOneIndex(0x10ccccccccL));
			Assert.AreEqual(-1, Bitops.LeftmostOneIndex((long)0));
			Assert.AreEqual(10, Bitops.LeftmostOneIndex(new BigInteger(1024)));
			Assert.AreEqual(-1, Bitops.LeftmostOneIndex(BigInteger.Zero));
		}

		[TestMethod]
		public void TestReverseBits()
		{
			Assert.AreEqual(0x0123456789abcdefUL, Bitops.ReverseBits(0xf7b3d591e6a2c480UL));
		}

		[TestMethod]
		public void TestBitcount()
		{
			Assert.AreEqual(19, Bitops.BitCount(0x111111f111111111UL));
		}

		[TestMethod]
		public void NextFragmentedFieldTest()
		{
			ulong chi = 0x101010;
			ulong x = 0;
			x = Bitops.NextFragmentedField(x, chi);
			Assert.AreEqual(0x000010UL, x);
			x = Bitops.NextFragmentedField(x, chi);
			Assert.AreEqual(0x001000UL, x);
			x = Bitops.NextFragmentedField(x, chi);
			Assert.AreEqual(0x001010UL, x);
			x = Bitops.NextFragmentedField(x, chi);
			Assert.AreEqual(0x100000UL, x);
			x = Bitops.NextFragmentedField(x, chi);
			Assert.AreEqual(0x100010UL, x);
			x = Bitops.NextFragmentedField(x, chi);
			Assert.AreEqual(0x101000UL, x);
		}

		[TestMethod]
		public void NextSubcubeVertexTest()
		{
			ulong a = 0x101010;
			ulong b = 0xf;
			ulong x = b;
			x = Bitops.NextSubcubeVertex(x, a, b);
			Assert.AreEqual(0x00001fUL, x);
			x = Bitops.NextSubcubeVertex(x, a, b);
			Assert.AreEqual(0x00100fUL, x);
			x = Bitops.NextSubcubeVertex(x, a, b);
			Assert.AreEqual(0x00101fUL, x);
			x = Bitops.NextSubcubeVertex(x, a, b);
			Assert.AreEqual(0x10000fUL, x);
			x = Bitops.NextSubcubeVertex(x, a, b);
			Assert.AreEqual(0x10001fUL, x);
			x = Bitops.NextSubcubeVertex(x, a, b);
			Assert.AreEqual(0x10100fUL, x);
		}

		[TestMethod]
		public void AddFragmentedFieldsTest()
		{
			//       0x10f0302030e0a003
			//     + 0xf03020f040202005
			//  mask 0xf0f0f0f0f0f0f00f
			//       ------------------
			//       0x102050108000c008
			ulong result = Bitops.AddFragmentedFields(0x10f0302030e0a003, 0xf03020f040202005, 0xf0f0f0f0f0f0f00f);
			Assert.AreEqual(0x102060108000c008UL, result);
		}

		[TestMethod]
		public void AddBytesTest()
		{
			//   0xff 00 f0 32 20 30 40 50
			// + 0x02 33 10 24 f0 e0 d0 c0
			//   -------------------------
			//   0x01 33 00 56 10 10 10 10
			Assert.AreEqual(0x0133005610101010UL, Bitops.AddBytes(0xff00f03220304050UL, 0x02331024f0e0d0c0UL));
		}

		[TestMethod]
		public void SubtractBytesTest()
		{
			//   0xff 00 f0 32 20 30 40 50
			// - 0x02 33 10 24 f0 e0 d0 c0
			//   -------------------------
			//   0xfd cd e0 0e 30 50 70 90
			ulong result = Bitops.SubtractBytes(0xff00f03220304050UL, 0x02331024f0e0d0c0UL);
			Assert.AreEqual(0xfdcde00e30507090UL, result);
		}

		[TestMethod]
		public void AverageBytesTest()
		{
			//       0x10f0302030e0a003
			//       0xf03020f040202005
			//       ------------------
			//       0x8090288838806004
			ulong result = Bitops.AverageBytes(0x10f0302030e0a003, 0xf03020f040202005);
			Assert.AreEqual(0x8090288838806004UL, result);
		}

		[TestMethod]
		public void IsAnyByteZeroTest()
		{
			Assert.IsFalse(Bitops.IsAnyByteZero(0x123456789abcdef0UL));
			Assert.IsFalse(Bitops.IsAnyByteZero(0x123456700abcdef0UL));
			Assert.IsTrue(Bitops.IsAnyByteZero(0x123456009abcdef0UL));
		}

		[TestMethod]
		public void ContainsByteTest()
		{
			Assert.IsFalse(Bitops.ContainsByte(0x123456789abcdef0UL, 0xda));
			Assert.IsFalse(Bitops.ContainsByte(0x1234567daabcdef0UL, 0xda));
			Assert.IsTrue(Bitops.ContainsByte(0x123456da9abcdef0UL, 0xda));
		}

		[TestMethod]
		public void LocateBitsTest()
		{
			Assert.AreEqual(0UL, Bitops.LocateZeroes(0x123456789abcdef0UL));
			ulong result = Bitops.LocateZeroes(0x100400009abc00f0UL);
			Assert.AreEqual(0x0000808000008000UL, result);
		}

		[TestMethod]
		public void CompareBitsTest()
		{
			// 0x0011110011001100
			// 0x0000ff1111001111
			// ------------------
			// 0x0000808000000080
			Assert.AreEqual(0x0000808000000080UL, Bitops.CompareBytes(0x0011110011001100UL, 0x0000ff1111001111UL));
		}

		[TestMethod]
		public void IndexTValuesTest()
		{
			// 0x00 11 11 00 11 00 11 00
			// 0x00 00 ff 11 11 00 11 11
			// -------------------------
			// 0x00 00 80 80 00 00 00 80
			Assert.AreEqual(0, Bitops.RightIndexOfTValue(Bitops.CompareBytes(0x0011110011001100UL, 0x0000ff1111001111UL)));
			Assert.AreEqual(5, Bitops.LeftIndexOfTValue(Bitops.CompareBytes(0x0011110011001100UL, 0x0000ff1111001111UL)));
		}
	}
}
