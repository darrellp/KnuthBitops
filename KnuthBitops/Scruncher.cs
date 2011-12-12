namespace KnuthBitops
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Scruncher. </summary>
	///
	/// <remarks>	
	/// Implements Knuth's algorithm for "extract[ing] and compress[ing] an arbitrary subset of a
	/// register's bits" as discussed on p. 148 of Volume 4A of "The Art of Computer Programming".
	/// Knuth ascribes the algorithm to Guy Steele.  Darrellp, 12/12/2011. 
	/// </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////
	public class Scruncher
	{
		readonly ulong[] _masks = new ulong[6];

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Constructor. </summary>
		///
		/// <remarks>	
		/// Determines the set of masks necessary for the scrunching to happen. This is the solution
		/// Knuth gives for the algorithm in exercise 70 of section 7.1.3. Darrellp, 12/12/2011. 
		/// </remarks>
		///
		/// <param name="scrunchMask">	The scrunch mask. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public Scruncher(ulong scrunchMask)
		{
			ulong phi = ~scrunchMask;
			int iShiftK = 1;
			for (int k = 0; k < _masks.Length; k++, iShiftK <<= 1)
			{
				if (phi == 0)
				{
					break;
				}
				ulong x = phi;
				int iShift = 1;
				for (int l = 0; l < 6; l++, iShift <<= 1)
				{
					x = x ^ (x << iShift);
				}
				_masks[k] = x;
				phi = (phi & ~x) >> iShiftK;
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Scrunches the input according to the mask passed to the constructor. </summary>
		///
		/// <remarks>	Darrellp, 12/12/2011. </remarks>
		///
		/// <param name="x">	The value to be scrunched. </param>
		///
		/// <returns>	The scrunched result. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public ulong Scrunch(ulong x)
		{
			int iShift = 1;
			for (int iMask = 0; iMask < _masks.Length; iMask++, iShift <<= 1)
			{
				if (iMask != 0)
				{
					x = Bitops.ShiftBitsMask(x, iShift, _masks[iMask]);
				}
			}
			return x;
		}
	}
}
