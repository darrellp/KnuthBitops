using System.Collections.Generic;

namespace KnuthBitops
{
	class Swapper : PermutationNetwork
	{
		internal Swapper(byte[] permutation, int phase)
		{
			Masks.Add((permutation[0] == 1) ? 1ul << phase : 0);
		}
	}
}
