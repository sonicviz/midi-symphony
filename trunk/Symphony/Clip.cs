using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{

	public abstract class Clip
	{
		public float startTime = 0;
		public float endTime = 64;

		public static float tempo = 2.0f;

		public float tempoScale = 1;
		public int volume = 127;
		public MObject melody;
		public NoteStream[] cachedNotes;
		public List<Message> messagesForOneMeasure;
		public BarMeasure signature;
		int channelStart;
		static int channelsUsed;

		public void Init(Midi.Instrument inst)
		{
			channelStart = channelsUsed;

			SoundEngine.Instance.outputDevice.SendProgramChange((Channel)channelStart, inst);
			channelsUsed += 1;
			Update();
		}

		public void Update()
		{

			cachedNotes = this.melody.GetOutput();
			
			BarMeasure clipSignature = BarMeasure.CalculateLength(cachedNotes);

			this.messagesForOneMeasure = new List<Message>();
			
			for (int idx2 = 0; idx2 < cachedNotes.Length; idx2++)
			{
				NoteStream p = cachedNotes[idx2];
				float dscale = ((float)signature.count / (float)p.signature.count);

				if (p.bStretch)
				{
					float time = 0;
					for (int idx3 = 0; idx3 < p.signature.count; idx3++)
					{
						int idx = 0;
						while (idx < p.notes.Count)
						{
							NoteStream.Note note = p.notes[idx];
							int playedNote = -1;
							if (p.scale != null)
							{
								if (note.note < 0)
								{
									playedNote = ((p.octave - 1 + (note.note / p.scale.Length)) * 12) + p.key + p.scale[((p.scale.Length - 1) - note.note) % p.scale.Length];
								}
								else
								{
									playedNote = ((p.octave + note.note / p.scale.Length) * 12) + p.key + p.scale[note.note % p.scale.Length];
								}
							}
							if (playedNote > 0)
							{
								AddMessage(SoundEngine.Instance.outputDevice, p, channelStart + idx2, time, note.duration*dscale, tempo * tempoScale, playedNote, messagesForOneMeasure);
							}
							time += note.duration*dscale;
							idx++;
						}
					}
				}
				else
				{
					int numRepeats = signature.count / p.signature.count;
					float time = 0;
					for (int repeat = 0; repeat < numRepeats; repeat++)
					{
						//int step = Math.Max(1, signature.measure / p.signature.measure);
						for (int idx = 0; idx < p.notes.Count; idx++)
						{
							NoteStream.Note note = p.notes[idx];
							if (note != null)
							{

								int playedNote = note.note;

								if (p.scale != null)
								{

									if (note.note < 0)
									{
										int index = p.scale.Length + note.note;
										playedNote = ((p.octave - 1) * 12) + p.key + p.scale[index % p.scale.Length];
									}
									else
									{
										playedNote = ((p.octave + (note.note / p.scale.Length)) * 12) + p.key + p.scale[note.note % p.scale.Length];
									}
								}
								if (playedNote > 0)
								{
									AddMessage(SoundEngine.Instance.outputDevice, p, channelStart + idx2, time, note.duration, tempo * tempoScale, playedNote, messagesForOneMeasure);
								}
							}
							time += note.duration;
						}

					}
				}

			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputDevice"></param>
		/// <param name="s"></param>
		/// <param name="time">between 0 and 1!</param>
		/// <param name="noteLength"></param>
		/// <param name="note"></param>
		/// <param name="messagesForOneMeasure"></param>
		protected abstract void AddMessage(OutputDevice outputDevice, NoteStream s, int channel, float time, float noteLength, float tempo, int note, List<Message> messagesForOneMeasure);

	}
}
