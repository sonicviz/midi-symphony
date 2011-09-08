using System.Collections.Generic;
namespace SongCraft
{
	public class MObject
	{
		public List<Melody> melody = new List<Melody>();
		public List<Operation> operations = new List<Operation>();

		public Melody[] GetOutput()
		{
			Melody[] output = new List<Melody>(melody).ToArray();

			foreach (Operation o in operations)
			{
				output = o.Process(output);
			}
			return output;
		}
	}
}