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
		protected List<ulong> Masks { get; set;}
	
		protected PermutationNetwork()
		{
			Masks = new List<ulong>();
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
		public PermutationNetwork(byte[] permutation) : this()
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
				check |= 1 << i;
			}
			return check == -1;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Private constructor. </summary>
		///
		/// <remarks>	
		/// Since this is only called internally and we check the permutation on the public constructor,
		/// there's no need to check the permutation here.  Darrellp, 12/10/2011. 
		/// </remarks>
		///
		/// <param name="permutation">	The permutation we will use to permute the bits. </param>
		/// <param name="phaseParm">	The phase parm. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		private PermutationNetwork(byte[] permutation, int phaseParm) : this()
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
				permutationNetworkOdd = new Swapper(permutationsInner[1], phase + delta);
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

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets the inner permutations. </summary>
		///
		/// <remarks>	
		/// Two things have to be calculated here in tandem: which switchers are turned on and the
		/// permutation in each inner permutation network.  Knuth gives a demo of how to do this.  It
		/// consists essentially of working our way back and forth across the permutation networks.  With
		/// each pass we have a start pin and an end pin.  Depending on which direction we're working,
		/// the start pin could be either and input or an output pin.  Whichever side it's on, the end
		/// pin is the connected pin on the other side.  When the switcher for a pin is disabled, the pin
		/// defaults to the network of the same parity as the pin.  In each pass we know which inner
		/// network the start pin is connected to.  The end pin must be connected to the same network as
		/// the start pin.  This determines whether the end switcher needs to be activated to ensure that
		/// the start and end pin are on the same inner network.  The actual pins in the inner
		/// permutation are just the indices of the corresponding switchers.  After making that
		/// determination, we have all the information we need to repeat the process using the other pin
		/// on the end pin's switcher as the start pin.  It's guaranteed that after making enough of
		/// these passes we'll end up back on our starting pin.  At that point, the cycle starting at
		/// that pin is complete. If we've handled all the input pins, then we're done.  If not, we have
		/// to pick an unmapped pin and repeat the cycling process at that pin.  Eventually we have
		/// handled all pins and the process is complete. Darrellp, 12/10/2011. 
		/// </remarks>
		///
		/// <param name="permutationOuter">	The permutation we're trying to achieve. </param>
		/// <param name="delta">			The distance between bits in the outer permutation. </param>
		/// <param name="phase">			The index of the first bit to be permuted. </param>
		/// <param name="inputMask">		[out] Our own input mask. </param>
		/// <param name="outputMask">		[out] Our own output mask. </param>
		///
		/// <returns>	The inner permutations. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
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

			// While there are cycles to map...
			while (cMapped < size)
			{
				bool fInput = true;
				byte network = 0;
				byte startPin;

				// Find the next unmapped input
				for (startPin = 0; startPin < size; startPin++)
				{
					if (!mappedToDest[startPin])
					{
						break;
					}
				}

				// Keep track of our starting pin
				byte cycleStartPin = startPin;

				// Adjacent pairs of pins are connected to a switcher which
				// may or may not swap them around.  Get the index for the
				// switcher we're attached to.
				byte switcher = (byte)(startPin / 2);

				// This is the pin we need to be mapped to
				byte endPin = permutationOuter[startPin];

				// Trace the cycle starting at startPin
				while (true)
				{
					// We alternate from input to output of the current permutation network
					fInput = !fInput;

					// Where we came from
					byte switcherPrev = switcher;
					switcher = (byte)(endPin / 2);

					// If we're connected to the wrong network currently...
					if ((endPin & 1) != network)
					{
						// If we're on the input side...);
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
					mappedToDest[fInput ? endPin : startPin] = true;

					// mark the connections that need to be made in the inner network
					permutationsInner[network][fInput ? switcher : switcherPrev] = fInput ? switcherPrev : switcher;

					// We've mapped one more pin
					cMapped++;

					// Get the other pin on this switcher
					startPin = (byte)(endPin ^ 1);

					// If we're back to startPin on the input side, then we've completed the cycle
					if (startPin == cycleStartPin && fInput)
					{
						break;
					}

					// The other pin on our switcher has to go to the other network
					network = (byte)(1 - network);

					// The pin we map to on the other side of the network
					endPin = fInput ? permutationOuter[startPin] : reversePermutation[startPin];
				}
			}
			return permutationsInner;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets the reverse permutation for an input permutation. </summary>
		///
		/// <remarks>	Darrellp, 12/10/2011. </remarks>
		///
		/// <param name="permutation">	Original permutation. </param>
		///
		/// <returns>	The reverse permutation. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
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

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Adds a swap to a mask. </summary>
		///
		/// <remarks>	
		/// Swaps are produced by the SwapBitsMask operation which takes a mask to indicate which bits
		/// are to be swapped.  This routine inserts the proper bit into the mask to force the requested
		/// swap.  Darrellp, 12/10/2011. 
		/// </remarks>
		///
		/// <param name="maskCur">	The current mask. </param>
		/// <param name="switcher">	The switcher to be swapped. </param>
		/// <param name="delta">	The distance between bits in the permutation. </param>
		/// <param name="phase">	The index of the first bit to be permuted. </param>
		///
		/// <returns>	New mask with the correct bit turned on to effect the swap. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		private static ulong AddSwap(ulong maskCur, int switcher, int delta, int phase)
		{
			return maskCur | (1ul << (2 * switcher * delta + phase));
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Add this network's contribution to the mask list. </summary>
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
			List<ulong> masksEven = even.Masks;
			List<ulong> masksOdd = odd.Masks;
			Masks.Add(inputMask);
			for (int i = 0; i < masksEven.Count; i++)
			{
				Masks.Add(masksEven[i] | masksOdd[i]);
			}
			Masks.Add(outputMask);
		}

		private static readonly int[] Deltas = new[] { 1, 2, 4, 8, 16, 32, 16, 8, 4, 2, 1 };

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Permutes the bits in the input. </summary>
		///
		/// <remarks>	Darrellp, 12/10/2011. </remarks>
		///
		/// <param name="n">	The input ulong to be permuted. </param>
		///
		/// <returns>	The input after it's bits have been permuted properly. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
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
