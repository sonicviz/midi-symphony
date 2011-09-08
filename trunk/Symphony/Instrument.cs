using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{

	public abstract class Instrument
	{
		static float tempo = 0.75f;
		public MObject melody;
		public Melody[] cachedNotes;
		private List<Message> messagesForOneMeasure;
		public BarMeasure signature;

		public void Init(MObject s, BarMeasure signature)
		{
			this.melody = s;
			this.signature = signature;
			cachedNotes = this.melody.GetOutput();

			this.messagesForOneMeasure = new List<Message>();
			for (int streamIdx = 0; streamIdx < cachedNotes.Length; streamIdx++)
			{
				Melody p = cachedNotes[streamIdx];
				int totalNotes = p.signature.measure * p.signature.count;
				int notesInPattern = p.notes.Length;
				int idx = 0;
				if (p.bStretch)
				{
					while (idx < notesInPattern)
					{
						int note = p.notes[idx];

						if (note != 0)
						{
							float time = (float)idx / (float)notesInPattern;

							AddMessage(SoundEngine.Instance.outputDevice, p, tempo * time * ((float)p.signature.count / (float)p.signature.measure), note, messagesForOneMeasure);
						}
						idx++;
					}
				}
				else
				{
					while (idx < totalNotes)
					{
						int note = p.notes[idx % notesInPattern];

						if (note != 0)
						{
							float time = (float)idx / (float)totalNotes;

							AddMessage(SoundEngine.Instance.outputDevice, p, tempo * time * ((float)p.signature.count / (float)p.signature.measure), note, messagesForOneMeasure);
						}
						idx++;
					}
				}

			}
			messagesForOneMeasure.Add(new CallbackMessage(
				new CallbackMessage.CallbackType(CallbackHandler), 0));
			SoundEngine.Instance.clock.Schedule(messagesForOneMeasure, 0);
		}

		protected abstract void AddMessage(OutputDevice outputDevice, Melody s, float time, int note, List<Message> messagesForOneMeasure);

		private void CallbackHandler(float time)
		{
			//Console.Out.WriteLine("----------------------");
			//foreach (Message m in messagesForOneMeasure)
			//{
			//    Console.Out.WriteLine("Event: " + m.ToString() + " - at: " + m.Time);
			//}
			

			// Round up to the next measure boundary.
			float timeOfNextMeasure = time + tempo * ((float)signature.count/(float)signature.measure);
			SoundEngine.Instance.clock.Schedule(messagesForOneMeasure, timeOfNextMeasure);
		}



	}
}
