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

		public override BarMeasure Generate(Scope scope)
		{
			throw new NotImplementedException();
		}

		public override BarMeasure Compose(BarMeasure first, BarMeasure other)
		{
			throw new NotImplementedException();
		}
	}

	public class Note
	{
		public Note(BarMeasure inStart, BarMeasure inDuration)
		{
			start = inStart;
			duration = inDuration;
		}
		public BarMeasure start;
		public BarMeasure duration;

		public List<int> pitches;
	}

	class Drummer : Instrument
	{
		protected override void AddMessage(OutputDevice outputDevice, Melody s, float time, int note, List<Message> messagesForOneMeasure)
		{
			messagesForOneMeasure.Add(new PercussionMessage(outputDevice, (Percussion)note,
										50, time));
		}
	}

	class BassGuitar : Instrument
	{
		protected override void AddMessage(OutputDevice outputDevice, Melody s, float time, int note, List<Message> messagesForOneMeasure)
		{
			messagesForOneMeasure.Add(new NoteOnMessage(outputDevice, (Channel)0, (Pitch)note,
										50, time));
			messagesForOneMeasure.Add(new NoteOffMessage(outputDevice, (Channel)0, (Pitch)note,
										50, time+0.5f));
		}
	}

}
