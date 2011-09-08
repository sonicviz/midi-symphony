using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SongCraft
{
	public abstract class Operation
	{
		public abstract Melody[] Process(Melody[] p);
	}

	public class StretchPattern : Operation
	{
		public BarMeasure signature;

		public override Melody[] Process(Melody[] melody)
		{
			List<Melody> newMelody = new List<Melody>();
			foreach (Melody m in melody)
			{
				newMelody.Add(new Melody(m.notes, true));
			}

			return newMelody.ToArray();
		}
	}

	public class LoopPattern : Operation
	{
		public BarMeasure signature;

		public override Melody[] Process(Melody[] melody)
		{
			List<Melody> newMelody = new List<Melody>();
			foreach (Melody m in melody)
			{
				newMelody.Add(new Melody(m.notes, false));
			}

			return newMelody.ToArray();
		}
	}

}
