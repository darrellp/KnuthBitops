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
		public void TestRuler()
		{
			Assert.AreEqual(0, Bitops.Ruler((long)0));
			Assert.AreEqual(0, Bitops.Ruler((short)0));
			Assert.AreEqual(0, Bitops.Ruler((byte)0));
			Assert.AreEqual(0, Bitops.Ruler(0));
			Assert.AreEqual(0, Bitops.Ruler(new BigInteger(0)));
			Assert.AreEqual(0, Bitops.Ruler((long)(-1)));
			Assert.AreEqual(0, Bitops.Ruler((short)(-1)));
			Assert.AreEqual(0, Bitops.Ruler((byte)(0xff)));
			Assert.AreEqual(0, Bitops.Ruler(-1));
			Assert.AreEqual(0, Bitops.Ruler(new BigInteger(-1)));
			for (long i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.Ruler(l));
			}
			for (int i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.Ruler(l));
			}
			for (short i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.Ruler(l));
			}
			for (byte i = 0, l = 1; l != 0; l <<= 1, i++)
			{
				Assert.AreEqual(i, Bitops.Ruler(l));
			}
			Assert.AreEqual(8, Bitops.Ruler(new BigInteger(256)));
		}
	
		[TestMethod]
		public void TestLeftmostOneIndex()
		{
			Assert.AreEqual(36, Bitops.LeftmostOneIndex(0x10ccccccccL));
			Assert.AreEqual(-1, Bitops.LeftmostOneIndex((long)0));
			Assert.AreEqual(10, Bitops.LeftmostOneIndex(new BigInteger(1024)));
			Assert.AreEqual(-1, Bitops.LeftmostOneIndex(BigInteger.Zero));
		}
	}
}
