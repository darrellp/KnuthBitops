using System;
using System.Collections.Generic;

namespace KnuthBitops
{
	public class PermutationNetwork
	{
		ulong _inputMask;
		ulong _outputMask;
		protected List<ulong> Masks = new List<ulong>();
		int _delta;
		protected int Phase;
	
		protected PermutationNetwork()
		{
		}
	
		public PermutationNetwork(byte[] permutation)
		{
			if (!CheckPermutation(permutation))
			{
				throw new ArgumentException("Invalid permutation to PermutationNetwork");
			}
			Initialize(permutation, 0);
		}
	
		private static bool CheckPermutation(byte[] permutation)
		{
			if (permutation.Length != 64)
			{
				return false;
			}
			long check = 0;
			for (int i = 0; i < 64; i++)
			{
				check |= (1 << i);
			}
			return check == -1;
		}
	
		private PermutationNetwork(byte[] permutation, int phaseParm)
		{
			Initialize(permutation, phaseParm);
		}
	
		// The toParms are expressed in terms of indices of the outgoing "wires" rather
		// than actual bits.  Given a wire index, i, the corresponding bit is
		// i * delta + phase.
		private void Initialize(byte[] permutation, int phaseParm)
		{
			PermutationNetwork permutationNetworkEven;
			PermutationNetwork permutationNetworkOdd;
			Phase = phaseParm;
			int size = permutation.Length;
			byte[] reversePermutation = new byte[size];
			for(byte i = 0; i < size; i++)
			{
				reversePermutation[permutation[i]] = i;
			}
		
			_delta = 64 / size;
		
			byte[][] permutationsInner = new byte[2][];
			permutationsInner[0] = new byte[size / 2];
			permutationsInner[1] = new byte[size / 2];
		
			bool[] mappedToDest = new bool[size];
			int cMapped = 0;
		
			while (cMapped < size)
			{
				bool fInput = true;
				byte network = 0;
				byte bit;

				for (bit = 0; bit < size; bit++)
				{
					if (!mappedToDest[bit])
					{
						break;
					}
				}

				byte startBit = bit;
				byte switcher = (byte)(bit / 2);
				byte mapbit = permutation[bit];
			
				while (true)
				{
					fInput = !fInput;
					byte switcherPrev = switcher;
					switcher = (byte) (mapbit / 2);
					if ((mapbit & 1) != network)
					{
						if (fInput)
						{
							_inputMask = AddSwap(_inputMask, switcher);
						}
						else
						{
							_outputMask = AddSwap(_outputMask, switcher);
						}
					}
					mappedToDest[fInput ? mapbit : bit] = true;
					permutationsInner[network][fInput ? switcherPrev : switcher] = fInput ? switcher : switcherPrev;
					cMapped++;
					bit = (byte) (mapbit ^ 1);
					if (bit == startBit && fInput)
					{
						break;
					}
					network = (byte) (1 - network);
					mapbit = fInput ? permutation[bit] : reversePermutation[bit];
				}
			}
		
			if (size == 4)
			{
				permutationNetworkEven = new Swapper(permutationsInner[0]);
				permutationNetworkOdd = new Swapper(permutationsInner[1]);
			}
			else
			{
				permutationNetworkEven = new PermutationNetwork(permutationsInner[0], Phase);
				permutationNetworkOdd = new PermutationNetwork(permutationsInner[1], Phase + _delta);
			}
			SetMasks(permutationNetworkEven, permutationNetworkOdd);
		}
	
		private ulong AddSwap(ulong maskCur, int switcher)
		{
			return maskCur | (1ul << (2 * switcher * _delta + Phase));
		}
	
		void SetMasks(PermutationNetwork even, PermutationNetwork odd)
		{
			List<ulong> masksEven = even.GetMasks();
			List<ulong> masksOdd = odd.GetMasks();
			Masks.Add(_inputMask);
			for (int i = 0; i < masksEven.Count; i++)
			{
				Masks.Add(masksEven[i] | masksOdd[i]);
			}
			Masks.Add(_outputMask);
		}

		internal virtual List<ulong> GetMasks()
		{
			return Masks;
		}
	
		static readonly int[] Deltas = new[] {1, 2, 4, 8, 16, 32, 16, 8, 4, 2, 1};

		public ulong Permute(ulong n)
		{
			for (int i = 0; i < Deltas.Length; i++)
			{
				ulong l = Masks[i];
			
				if (l != 0)
				{
					n = Bitops.SwapBitsMask(n, Deltas[i], l);
				}
			}
			return n;
		}
	}
}
