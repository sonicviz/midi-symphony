using System.Collections.Generic;
using System;
namespace SongCraft
{
	//A stream is a pattern on notes defined by a function or series
	public class NoteStream : Procedural<NoteStream>
	{
		public class Note
		{
			public int note;
			public float duration;
			
			/// <summary>
			/// 
			/// </summary>
			/// <param name="note">note relative to root in scale</param>
			/// <param name="duration">duration as percentage of the bar</param>
			public Note(int note, float duration)
			{
				this.note = note;
				this.duration = duration;
			}

			public Note(Note other)
			{
				note = other.note;
				duration = other.duration;
			}
		}

		public List<Note> notes;
		public bool bStretch;
		public int octave;
		public int key;
		public int[] scale;
		private Note[] newNotes;
		private bool p;
		

		public NoteStream()
		{
		}

		/// <summary>
		/// ONLY FOR DRUMS
		/// </summary>
		/// <param name="p"></param>
		/// <param name="stretch"></param>
		public NoteStream(int[] p, bool stretch)
		{
			bStretch = stretch;

			notes = new List<Note>();
			foreach (int i in p)
			{
				notes.Add(new Note(i, 1.0f / (float)p.Length));
			}
		}

		public NoteStream(NoteStream m)
		{
			notes = new List<Note>(m.notes);
			signature = m.signature;
			octave = m.octave;
			scale = m.scale;
			key = m.key;
			bStretch = m.bStretch;
		}

		public NoteStream(List<Note> list, bool stretch)
		{
			this.notes = list;
			this.bStretch = stretch;
		}

		public NoteStream(Note[] newNotes, bool p)
		{
			// TODO: Complete member initialization
			this.newNotes = newNotes;
			this.p = p;
		}

		public List<int> PickAMotion()
		{
			List<int> l = new List<int>();
			l.Add(0); //Always add the root first, for now.

			int j = rnd.Next(3);
			l.Add(j);

			return l;

		}

		public override void Generate(Scope scope)
		{
			//int size = scope.signature.count * scope.signature.measure;
			//notes = new int[size];
			
			//int current = scope.scale.Ascent[rnd.Next(7)];

			//List<int> motion = PickAMotion();

			//for (int idx = 0; idx < size; idx++)
			//{
			//    notes[idx] = scope.key + current;
			//    int j = rnd.Next(4);
			//    switch (j)
			//    {
			//        case 0:
			//            break;
			//    }
			//}
		}

		public override NoteStream Compose(NoteStream first, NoteStream other)
		{
			//List<int> l = new List<int>(first);
			//l.AddRange(new List<int>(other));
			//return l.ToArray();\
			throw new NotImplementedException();
		}
	}

}