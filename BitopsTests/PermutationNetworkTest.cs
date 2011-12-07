using Microsoft.VisualStudio.TestTools.UnitTesting;
using KnuthBitops;

namespace BitopsTests
{
	[TestClass]
	public class PermutationNetworkTest
	{
		[TestMethod]
		public void TestPermutationNetwork()
		{
			byte[] permutation = new byte[64];
			for (byte i = 0; i < 64; i++)
			{
				permutation[i] = i;
			}
		
			PermutationNetwork pnwk = new PermutationNetwork(permutation);
			Assert.AreEqual(56ul, pnwk.Permute(56));
			permutation[0] = 1;
			permutation[1] = 0;
			pnwk = new PermutationNetwork(permutation);
			Assert.AreEqual(1ul, pnwk.Permute(2));
			Assert.AreEqual(2ul, pnwk.Permute(1));
			Assert.AreEqual(3ul, pnwk.Permute(3));
			Assert.AreEqual(5ul, pnwk.Permute(6));
			Assert.AreEqual(6ul, pnwk.Permute(5));
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
		}
	}
}
