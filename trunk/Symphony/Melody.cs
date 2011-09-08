using System.Collections.Generic;
namespace SongCraft
{
	//A stream is a pattern on notes defined by a function or series
	public class Melody : Procedural<int[]>
	{
		public int[] notes;
		public bool bStretch;

		public Melody()
		{
		}

		public Melody(int[] p, bool stretch)
		{
			bStretch = stretch;
			notes = p;
		}

		public override int[] Generate(Scope scope)
		{
			int size = scope.signature.count * scope.signature.measure;
			int[] n = new int[size];
			for (int idx = 0; idx < size; idx++)
			{
				n[idx] = scope.key + scope.scale.Ascent[rnd.Next(7)];
			}
			return n;
		}

		public override int[] Compose(int[] first, int[] other)
		{
			List<int> l = new List<int>(first);
			l.AddRange(new List<int>(other));
			return l.ToArray();
		}
	}

}