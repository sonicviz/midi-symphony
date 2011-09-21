using System.Collections.Generic;
using System;
namespace SongCraft
{
	public class MObject
	{
		public List<NoteStream> melody = new List<NoteStream>();
		public List<Operation> operations = new List<Operation>();

		public virtual NoteStream[] GetOutput()
		{
			NoteStream[] output = new List<NoteStream>(melody).ToArray();


			foreach (Operation o in operations)
			{
				output = o.Process(output);
			}
			return output;
		}



	}

	///// <summary>
	///// A composite is an m-object that mixes clips and synchronizes them
	///// </summary>
	//public class Composite : MObject
	//{
	//    public struct CompositeData
	//    {
	//        public MObject clip;
	//        public bool stretch;
	//        public int LoopCount;
	//    }

	//    public override Melody[] GetOutput()
	//    {
	//        foreach (var l in list)
	//        {
	//            BarMeasure a = BarMeasure.CalculateLength(l.clip.melody);
	//            if (l.clip.CalculateLength() != CalculateLength())
	//            {
	//            }
	//        }
	//        Melody[] output = new List<Melody>(melody).ToArray();

	//        foreach (Operation o in operations)
	//        {
	//            output = o.Process(output);
	//        }
	//        return output;
	//    }


	//    public List<CompositeData> list = new List<CompositeData>();
	//}
}