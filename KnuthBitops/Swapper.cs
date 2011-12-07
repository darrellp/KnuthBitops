using System.Collections.Generic;

namespace KnuthBitops
{
	class Swapper : PermutationNetwork
	{
		internal Swapper(byte[] permutation)
		{
			Masks.Add((permutation[0] == 1) ? 1ul << Phase : 0);
		}

		internal override List<ulong> GetMasks()
		{
			return Masks;
		}
	}
}
