using System.Collections.Generic;
namespace SongCraft
{

	public class Composition
	{
		public List<Instrument> instruments = new List<Instrument>();
		
		public class Part : Dictionary<Instrument, Melody>
		{
		}

		public List<Part> parts;
	}

}