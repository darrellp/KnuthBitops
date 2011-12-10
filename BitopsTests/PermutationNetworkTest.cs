using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KnuthBitops;

namespace BitopsTests
{
	[TestClass]
	public class PermutationNetworkTest
	{
		private static readonly byte[] Identity = new byte[64];

		[TestMethod]
		public void TestPermutationNetwork()
		{
			byte[] permutation = new byte[64];

			// Identity permutation
			for (byte i = 0; i < 64; i++)
			{
				Identity[i] = i;
			}

			for (int i1 = 0; i1 < 63; i1++)
			{
				for (int i2 = i1; i2 < 64; i2++)
				{
					TestTwoCycle(i1, i2, permutation);
				}
			}

			Array.Copy(Identity, permutation, 64);

			PermutationNetwork pnwk = new PermutationNetwork(permutation);
			Assert.AreEqual(56ul, pnwk.Permute(56));

			// Swap unit and two's bits
			permutation[0] = 1;
			permutation[1] = 0;
			pnwk = new PermutationNetwork(permutation);
			Assert.AreEqual(1ul, pnwk.Permute(2));
			Assert.AreEqual(2ul, pnwk.Permute(1));
			Assert.AreEqual(3ul, pnwk.Permute(3));
			Assert.AreEqual(5ul, pnwk.Permute(6));
			Assert.AreEqual(6ul, pnwk.Permute(5));

			// Cycle the bottom four bits
			permutation[0] = 1;
			permutation[1] = 2;
			permutation[2] = 3;
			permutation[3] = 0;
			pnwk = new PermutationNetwork(permutation);
			Assert.AreEqual(2ul, pnwk.Permute(1));
			Assert.AreEqual(4ul, pnwk.Permute(2));
			Assert.AreEqual(8ul, pnwk.Permute(4));
			Assert.AreEqual(1ul, pnwk.Permute(8));
			Assert.AreEqual(3ul, pnwk.Permute(9));

			// Random permutation
			Random rnd = new Random(0);
			for (int i = 0; i < 63; i++)
			{
				int swapIndex = rnd.Next(64);
				byte b = permutation[i];
				permutation[i] = permutation[swapIndex];
				permutation[swapIndex] = b;
			}
			pnwk = new PermutationNetwork(permutation);
			ulong testVal = ((ulong)rnd.Next() << 32) | (ulong)rnd.Next();
			ulong resultVal = pnwk.Permute(testVal);
			TestPerm(permutation, testVal, resultVal);
		}

		private static void TestPerm(byte[] permutation, ulong testVal, ulong resultVal)
		{
			for (int i = 0; i < 64; i++ )
			{
				if (((testVal >> i) & 1UL) != ((resultVal >> permutation[i]) & 1UL))
				{
					Console.WriteLine("TestPerm fails at i = {0}", i);
				}
				Assert.AreEqual((testVal >> i) & 1UL, (resultVal >> permutation[i]) & 1UL);
			}
		}

		private static void TestTwoCycle(int i1, int i2, byte[] permutation)
		{
			Array.Copy(Identity, permutation, 64);
			permutation[i1] = (byte)i2;
			permutation[i2] = (byte)i1;
			PermutationNetwork pnwk = new PermutationNetwork(permutation);
			Assert.AreEqual(1UL << i2, pnwk.Permute(1UL << i1));
			Assert.AreEqual(1UL << i1, pnwk.Permute(1UL << i2));
		}
	}
}
