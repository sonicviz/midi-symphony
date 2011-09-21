using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{
	public class BarMeasure : Procedural<BarMeasure>
	{
		public BarMeasure(int inCount, int inMeasure)
		{
			measure = inMeasure;
			count = inCount;
		}

		public BarMeasure()
		{
		}

		
		public int measure;
		public int count;

		public override void Generate(Scope scope)
		{
			throw new NotImplementedException();
		}

		public override BarMeasure Compose(BarMeasure first, BarMeasure other)
		{
			throw new NotImplementedException();
		}

		public static int LCM(int a, int b)
		{
			return LCM2(Math.Min(a, b), Math.Max(a, b));
		}

		public static int LCM2(int min, int max)
		{
			if (max % min == 0)
			{
				return max;
			}

			for (int multiple = 1; multiple < max; multiple++)
			{
				int commonMultiple = min * multiple;
				if (max % commonMultiple == 0)
				{
					return commonMultiple;
				}
			}
			throw new Exception("no LCM?");

			return max*min;
		}

		public static BarMeasure CalculateLength(IEnumerable<NoteStream> melody)
		{

			int minRequiredBars = 1;
			while (true)
			{
				bool failure = false;
				foreach (var l in melody)
				{
					if (minRequiredBars % l.signature.count != 0)
					{
						minRequiredBars = (int)BarMeasure.LCM(minRequiredBars, l.signature.count);
						failure = true;
						break;
					}
				}
				//retry with increased minRequiredBars
				if (failure)
				{
					continue;
				}
				//If we get here, we have 
				break;
			}

			int minRequiredMeasures = 1;
			while (true)
			{
				bool failure = false;
				foreach (var l in melody)
				{
					if (minRequiredMeasures % l.signature.measure != 0)
					{
						minRequiredMeasures = (int)BarMeasure.LCM(minRequiredMeasures, l.signature.measure);
						failure = true;
						break;
					}
				}
				//retry with increased minRequiredBars
				if (failure)
				{
					continue;
				}
				//If we get here, we have 
				break;
			}

			return new BarMeasure(minRequiredBars, minRequiredMeasures);

		}
	}

	//public class Note
	//{
	//    public Note(BarMeasure inStart, BarMeasure inDuration)
	//    {
	//        start = inStart;
	//        duration = inDuration;
	//    }
	//    public BarMeasure start;
	//    public BarMeasure duration;

	//    public List<int> pitches;
	//}

	class Drummer : Clip
	{
		protected override void AddMessage(OutputDevice outputDevice, NoteStream s, int channel, float time, float noteLength, float tempo, int note, List<Message> messagesForOneMeasure)
		{
			messagesForOneMeasure.Add(new PercussionMessage(outputDevice, (Percussion)note,
										volume, time * tempo));
		}
	}

	class BassGuitar : Clip
	{
		
		protected override void AddMessage(OutputDevice outputDevice, NoteStream s, int channel, float time, float noteLength, float tempo, int note, List<Message> messagesForOneMeasure)
		{

			messagesForOneMeasure.Add(new NoteOnMessage(outputDevice, (Channel)channel, (Pitch)note,
										volume, time * tempo));
			messagesForOneMeasure.Add(new NoteOffMessage(outputDevice, (Channel)channel, (Pitch)note,
										volume, (time + noteLength*1.0f) * tempo));
		}
	}

}
