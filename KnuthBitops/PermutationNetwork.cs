using System;
using System.Collections.Generic;

namespace KnuthBitops
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Permutation network. </summary>
	///
	/// <remarks>	
	/// An implementation of the permutation network as explained on pp. 145-147 of Vol. 4A of "The
	/// Art of Computer Programming" by Knuth.  It efficiently permutes the bits of a ulong according
	/// to an arbitrary permutation passed to the constructor.
	/// 
	/// It does this by calculating eleven masks and calling SwapBitsMask using each mask.  The masks
	/// are calculated in two phases.  First, a system of recursive permutation networks is set up
	/// with each network containing two half sized networks internally.  The inputs and outputs of
	/// the outer network are wired to the inner network using masks which are used in the
	/// SwapBitsMask operation. When there are only four inputs and outputs, two swappers are used
	/// internally rather than the more general permutation networks.
	/// 
	/// After the system has been set up, each outer network determines a single mask from the masks
	/// used by it's two inner networks.  These masks are kept in a list from the innermost networks
	/// outward.  When we reach the outermost network (of size 64 with two 32 sized networks internal
	/// to it) that array contains the masks we need to effect the permutation.  At that point the
	/// internal networks are no longer necessary and are discarded.
	/// 
	/// It might be a teeny bit more efficient to do all this in one pass - i.e., not build up inner
	/// permutation networks, but keep track of everything in a single array but I think the code
	/// would suffer a loss of clarity in the process and this is a one time processing task so in
	/// general, shouldn't be a bottleneck.  Therefore I'm leaving things the way they are.
	/// 
	/// Darrellp, 12/7/2011. 
	/// </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	public class PermutationNetwork
	{
		// The list of masks for the final permutation
		protected List<ulong> Masks = new List<ulong>();
	
		protected PermutationNetwork()
		{
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Constructor. </summary>
		///
		/// <remarks>	Darrellp, 12/7/2011. </remarks>
		///
		/// <exception cref="ArgumentException">	Thrown when the array representing the permutation is
		/// 										not a valid permutation. </exception>
		///
		/// <param name="permutation">	The permutation we will use to permute the bits. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public PermutationNetwork(byte[] permutation)
		{
			if (!CheckPermutation(permutation))
			{
				throw new ArgumentException("Invalid permutation to PermutationNetwork");
			}
			Initialize(permutation, 0);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Validates a passed in permutation in the form of an array. </summary>
		///
		/// <remarks>	Darrellp, 12/7/2011. </remarks>
		///
		/// <param name="permutation">	The permutation we will use to permute the bits. </param>
		///
		/// <returns>	true if it's a valid permutation, false if not. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
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

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Initializes the permutation network. </summary>
		///
		/// <remarks>	
		/// This is where the real work is done.  The permutation network is an abstraction of a set of
		/// incoming wires and outgoing wires.  The permutation is given in terms of those wires, but
		/// each indivicual wire represents a particular bit in the input.  The bits represented by
		/// adjacent wires are separated by a value delta which is equal to 64 / (the size of the
		/// permutation). The first wire represents the bit indexed by phase.  Thus the bit corresponding
		/// to wire i is phase + i * delta.  Since size can be calculated by the size of the permutation
		/// the only thing we need to make this mapping are the phase and the permutation.  Key point is
		/// that the permutation references wire numbers running consecutively from 0 to (size - 1), not
		/// actual bit positions.  Darrellp, 12/7/2011. 
		/// </remarks>
		///
		/// <param name="permutation">	The permutation we will use to permute the bits. </param>
		/// <param name="phase">		The index of the first bit to be permuted. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		private void Initialize(byte[] permutation, int phase)
		{
			PermutationNetwork permutationNetworkEven;
			PermutationNetwork permutationNetworkOdd;
			int size = permutation.Length;
			int delta = 64 / size;
			ulong inputMask;
			ulong outputMask;

			// Get the wiring needed for our two inner networks
			byte[][] permutationsInner = GetInnerPermutations(permutation, delta, phase, out inputMask, out outputMask);

			// Recursively create those two inner networks
			if (size == 4)
			{
				// The recursion ends here by creating two swappers rather than normal permutation networks
				permutationNetworkEven = new Swapper(permutationsInner[0], phase);
				permutationNetworkOdd = new Swapper(permutationsInner[1], phase);
			}
			else
			{
				// For larger sizes, the inner networks are standard permutation networks
				permutationNetworkEven = new PermutationNetwork(permutationsInner[0], phase);
				permutationNetworkOdd = new PermutationNetwork(permutationsInner[1], phase + delta);
			}

			// Extract the masks from our inner permutation networks into our list of masks
			ExtractMasks(permutationNetworkEven, permutationNetworkOdd, inputMask, outputMask);
		}

		private static byte[][] GetInnerPermutations(byte[] permutationOuter, int delta, int phase, out ulong inputMask, out ulong outputMask)
		{
			inputMask = 0;
			outputMask = 0;

			byte[][] permutationsInner = new byte[2][];
			int size = permutationOuter.Length;
			permutationsInner[0] = new byte[size / 2];
			permutationsInner[1] = new byte[size / 2];

			byte[] reversePermutation = GetReversePermutation(permutationOuter);

			// We map out cycles until we've covered all the inputs and their
			// connections to the outputs.  mappedToDest keeps track of which
			// inputs have been mapped and cMapped keeps track of the count of
			// mapped inputs.  When we end a cycle, if cMapped == size then
			// we're done.  If it's not, then we find an unmapped input and
			// map out the cycle starting from that input.
			bool[] mappedToDest = new bool[size];
			int cMapped = 0;

			while (cMapped < size)
			{
				bool fInput = true;
				byte network = 0;
				byte bit;

				// Find the next unmapped input
				for (bit = 0; bit < size; bit++)
				{
					if (!mappedToDest[bit])
					{
						break;
					}
				}

				// Keep track of our starting bit
				byte startBit = bit;

				// Each pair of inputs/outputs is potentially connected to a
				// switcher which swaps them around.  Get the index for the
				// switcher we're attached to.
				byte switcher = (byte)(bit / 2);

				// This is the bit we need to be mapped to
				byte mapbit = permutationOuter[bit];

				// Trace the cycle starting at startBit
				while (true)
				{
					// We alternate from input to output of the current permutation network
					fInput = !fInput;

					// Where we came from
					byte switcherPrev = switcher;
					switcher = (byte)(mapbit / 2);

					// If we're connected to the wrong network currently...
					if ((mapbit & 1) != network)
					{
						// If we're on the input side...
						if (fInput)
						{
							// Swap the two input connections around
							inputMask = AddSwap(inputMask, switcher, delta, phase);
						}
						else
						{
							// Swap the two output connections around
							outputMask = AddSwap(outputMask, switcher, delta, phase);
						}
					}

					// Mark this input bit as mapped
					mappedToDest[fInput ? mapbit : bit] = true;

					// mark the connections that need to be made in the inner network
					permutationsInner[network][fInput ? switcherPrev : switcher] = fInput ? switcher : switcherPrev;

					// We've mapped one more pin
					cMapped++;

					// Get the other pin on this switcher
					bit = (byte)(mapbit ^ 1);

					// It needs to go to the other network
					network = (byte)(1 - network);
					mapbit = fInput ? permutationOuter[bit] : reversePermutation[bit];

					// If we're back to startBit on the input side, then we've completed the cycle
					if (bit == startBit && fInput)
					{
						break;
					}
				}
			}
			return permutationsInner;
		}

		private static byte[] GetReversePermutation(byte[] permutation)
		{
			int size = permutation.Length;
			byte[] reversePermutation = new byte[size];
			for(byte i = 0; i < size; i++)
			{
				reversePermutation[permutation[i]] = i;
			}
			return reversePermutation;
		}

		private static ulong AddSwap(ulong maskCur, int switcher, int delta, int phase)
		{
			return maskCur | (1ul << (2 * switcher * delta + phase));
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Add this network's contribute to the mask list. </summary>
		///
		/// <remarks>	
		/// Extract the masks from our inner networks, interleave them and wrap the resultant list with
		/// our own masks.  Darrellp, 12/7/2011. 
		/// </remarks>
		///
		/// <param name="even">			The even inner permutation network. </param>
		/// <param name="odd">			The odd inner permutation network. </param>
		/// <param name="inputMask">	Our own input mask. </param>
		/// <param name="outputMask">	Our own output mask. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		private void ExtractMasks(PermutationNetwork even, PermutationNetwork odd, ulong inputMask, ulong outputMask)
		{
			List<ulong> masksEven = even.GetMasks();
			List<ulong> masksOdd = odd.GetMasks();
			Masks.Add(inputMask);
			for (int i = 0; i < masksEven.Count; i++)
			{
				Masks.Add(masksEven[i] | masksOdd[i]);
			}
			Masks.Add(outputMask);
		}

		internal virtual List<ulong> GetMasks()
		{
			return Masks;
		}

		private static readonly int[] Deltas = new[] { 1, 2, 4, 8, 16, 32, 16, 8, 4, 2, 1 };

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
