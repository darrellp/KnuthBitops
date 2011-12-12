namespace KnuthBitops
{
	public class Scruncher
	{
		readonly ulong[] _masks = new ulong[6];

		public Scruncher(ulong scrunchMask)
		{
			ulong phi = ~scrunchMask;
			int k = 0;
			int iShiftK = 1;
			while (phi != 0)
			{
				ulong x = phi;
				int iShift = 1;
				for (int l = 0; l < 6; l++, iShift <<= 1)
				{
					x = x ^ (x << iShift);
				}
				_masks[k] = x;
				phi = (phi & ~x) >> iShiftK;
				k++;
				iShiftK <<= 1;
			}
		}

		public ulong Scrunch(ulong x)
		{
			int iShift = 1;
			for (int iMask = 0; iMask < _masks.Length; iMask++, iShift <<= 1)
			{
				x = Bitops.ShiftBitsMask(x, iShift, _masks[iMask]);
			}
			return x;
		}
	}
}
