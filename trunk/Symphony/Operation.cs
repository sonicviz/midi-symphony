using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SongCraft
{
	public class TOperation<TSubclass> : Procedural<TSubclass> where TSubclass : TOperation<TSubclass>, new()
	{
		public override void Generate(Scope scope)
		{
			throw new NotImplementedException();
		}

		public override TSubclass Compose(TSubclass first, TSubclass other)
		{
			throw new NotImplementedException();
		}
	}

	public class Operation : TOperation<Operation>
	{
		public virtual NoteStream[] Process(NoteStream[] p) 
		{
			throw new NotImplementedException();
		}

		public override Operation Compose(Operation first, Operation other)
		{
			throw new NotImplementedException();
		}
	}

	public class StretchPattern : Operation
	{
		public BarMeasure signature;

		public override NoteStream[] Process(NoteStream[] melody)
		{
			List<NoteStream> newMelody = new List<NoteStream>();
			foreach (NoteStream m in melody)
			{
				newMelody.Add(new NoteStream(m.notes, true));
			}

			return newMelody.ToArray();
		}

		public override void Generate(Scope scope)
		{
			throw new NotImplementedException();
		}
	}

	public class LoopPattern : Operation
	{

		public override NoteStream[] Process(NoteStream[] melody)
		{
			List<NoteStream> newMelody = new List<NoteStream>();
			foreach (NoteStream m in melody)
			{
				newMelody.Add(new NoteStream(m.notes, false));
			}

			return newMelody.ToArray();
		}

		public override void Generate(Scope scope)
		{
			throw new NotImplementedException();
		}
	}

	public class Arpeggiator : Operation
	{
		public int speed = 2;

		public override NoteStream[] Process(NoteStream[] melody)
		{
			List<NoteStream> newMelody = new List<NoteStream>();
			foreach (NoteStream m in melody)
			{
				NoteStream.Note[] newNotes = new NoteStream.Note[m.notes.Count*speed];
				for (int idx2 = 0; idx2 < m.notes.Count; idx2++)
				{
					for (int i = 0; i < speed; i++)
					{
						newNotes[idx2 * speed + i] = new NoteStream.Note(m.notes[idx2].note, m.notes[idx2].duration);
					}
				}
				NoteStream m2 = new NoteStream(newNotes, false);
				m2.bStretch = m.bStretch;
				m2.signature = m.signature;
				m2.octave = m.octave;
				m2.scale = m.scale;
				m2.key = m.key;
				m2.signature.measure = m2.signature.measure * speed;
				newMelody.Add(m2);
			}

			return newMelody.ToArray();
		}

		public override void Generate(Scope scope)
		{
			//TODO - implement patterns
			//ABAC
			//AABC
			//ABCC
			//ABABCBAB
			throw new NotImplementedException();
		}
	}

	public class PolyMeter : Operation
	{
		public List<int> meter;

		public override NoteStream[] Process(NoteStream[] melody)
		{
			GenerateRandomMeter(melody);
			List<NoteStream> newMelody = new List<NoteStream>();

			int noteCount = meter.Aggregate((sum, x) => sum += x);

			foreach (NoteStream m in melody)
			{
				List<NoteStream.Note> newNotes = new List<NoteStream.Note>();
				int index = 0;

				foreach(int beatLength in meter)
				{
					newNotes.Add(new NoteStream.Note(m.notes[index++ % m.notes.Count].note, (float)beatLength / (float)noteCount));
				}
				NoteStream m2 = new NoteStream(new List<NoteStream.Note>(newNotes), false);
				m2.bStretch = m.bStretch;
				m2.octave = m.octave;
				m2.scale = m.scale;
				m2.key = m.key;

				m2.signature = m.signature;
				m2.signature.measure *= noteCount;
				newMelody.Add(m2);
			}

			return newMelody.ToArray();
		}

		private void GenerateRandomMeter(NoteStream[] melody)
		{
			BarMeasure phraseSize = BarMeasure.CalculateLength(melody);

			int maxCount = phraseSize.measure * phraseSize.count * (int)Math.Pow(2,1+rnd.Next(2));
			int count = 0;
			meter = new List<int>();
			while (count < maxCount)
			{
				int c = 2 + rnd.Next(2);

				if (count + c > maxCount)
				{
					meter[meter.Count - 1] += maxCount - count;
					break;
				}
				meter.Add(c);
				count += c;
			}

		}

		/// <summary>
		/// All this does is make choices about the default settings, because it runs absolutely
		/// first in the life cycle of the object, before init.
		/// </summary>
		/// <param name="scope"></param>
		public override void Generate(Scope scope)
		{	
		}
	}

	

	public class Multiply : Operation
	{
		public MObject other;
		public bool glib;

		public override void Generate(Scope scope)
		{
			base.Generate(scope);
			glib = rnd.Next(2) == 0;
		}

		public override NoteStream[] Process(NoteStream[] melody)
		{


			NoteStream[] otherMelodies = other.GetOutput();

			List<NoteStream> output = new List<NoteStream>();

			foreach (NoteStream m in melody)
			{
				foreach (NoteStream m2 in otherMelodies)
				{
					output.Add(MultiplyMelody(m, m2, glib));

				}
			}
			return output.ToArray();
		}

		private NoteStream MultiplyMelody(NoteStream riff, NoteStream bassline, bool glib)
		{
			NoteStream o = new NoteStream(riff);
			o.notes = new List<NoteStream.Note>();
			o.signature = new BarMeasure(riff.notes.Count, bassline.notes.Count);
			o.octave = bassline.octave;

			int marker = 0;
			for (int idx = 0; idx < riff.notes.Count; idx++)
			{
				marker = idx * bassline.notes.Count;
				for (int idx2 = 0; idx2 < bassline.notes.Count; idx2++)
				{
					if (glib && idx2 != 0)
					{
						o.notes[marker + idx2] = bassline.notes[idx2];
					}
					else
					{
						//Use the duration of the riff, the note of the bassline + the riff
						o.notes.Add(new NoteStream.Note(bassline.notes[idx2].note + riff.notes[idx].note, riff.notes[idx].duration));
					}
				}
			}

			return o;
		}

	}

}
