using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{
	public class Music : Procedural<Music>
	{
		
		public List<Clip> clips = new List<Clip>();

		public override void Generate(Scope scope)
		{
			scope.signature = new BarMeasure(1, 4);//(int)Math.Pow(2, rnd.Next(4)), rnd.Next(2) > 0 ? 4 : 3);
			scope.key = 36 + rnd.Next(12);

			clips.AddRange(GeneratePart(scope, 0));
			//clips.AddRange(GeneratePart(scope, 64));
		}

		public List<Clip> GeneratePart(Scope scope, float startTime)
		{
			List<Clip> part = new List<Clip>();

			scope.scale = Scale.NaturalMinor;

			MObject m = CreateDrummer(scope, startTime, part);

			BarMeasure tempo;
			Clip i1;
			CreateBassGuitar(scope, startTime, part, m, out tempo, out i1);
			
			//Clip i2 = CreateDistortionGuitar(scope, startTime, part, tempo, i1);

			CreateCelloBackingTrack(startTime, part, i1);
			return part;
		}

		private static MObject CreateDrummer(Scope scope, float startTime, List<Clip> part)
		{
			Drummer drummer = new Drummer();
			part.Add(drummer);
			scope.instrument = drummer;
			MObject m = new MObject();
			drummer.startTime = startTime;
			drummer.endTime = startTime + 64;

			NoteStream downBeat = new NoteStream(new int[4] { 35, 0, 0, 0 }, false);
			downBeat.signature = scope.signature;
			m.melody.Add(downBeat);

			NoteStream upBeat = new NoteStream(new int[4] { 0, 0, 40, 0 }, false);
			upBeat.signature = scope.signature;
			m.melody.Add(upBeat);

			NoteStream straighBeat = new NoteStream(new int[4] { 44, 44, 44, 44 }, false);
			straighBeat.signature = scope.signature;
			m.melody.Add(straighBeat);

			//NoteStream doubleKick = new NoteStream(new int[12]
			//{
			//    36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36
			//}, false);
			//doubleKick.signature = scope.signature;
			//doubleKick.bStretch = false;
			//m.melody.Add(doubleKick);

			drummer.melody = m;
			drummer.signature = new BarMeasure(64*4, 1);
			drummer.Init(0);

			return m;
		}

		private static void CreateBassGuitar(Scope scope, float startTime, List<Clip> part, MObject m, out BarMeasure tempo, out Clip i1)
		{
			tempo = new BarMeasure(4, 1);
			i1 = new BassGuitar();
			i1.startTime = startTime;
			i1.endTime = startTime + 64;
			part.Add(i1);
			scope.instrument = i1;
			scope.parent = m;
			i1.melody = new MObject();
			//tempo.measure = 1;
			GenerateMelody2(scope, tempo, 1, i1.melody);
			//i1.melody.operations.Add(new Arpeggiator());

			Multiply mult = new Multiply();
			mult.other = new MObject();

			NoteStream km = new NoteStream(new int[] { 0, 1, 2, 1 }, false);
			km.signature = new BarMeasure(1, 4);
			km.scale = scope.scale.Ascent;
			km.key = scope.key;
			mult.glib = false;
			mult.other.melody.Add(km);
			mult.other.operations.Add(new PolyMeter());
			//i1.melody.operations.Add(new PolyMeter());
			i1.melody.operations.Add(mult);
			//i1.melody.melody[0].bStretch = true;

			//i1.tempoScale = 4;
			i1.volume = 90;
			i1.signature = new BarMeasure(256, 1);
			i1.melody.melody[0].bStretch = false;
			i1.Init(Midi.Instrument.DistortionGuitar);
		}

		private static Clip CreateCelloBackingTrack2(Midi.Instrument inst, float startTime, List<Clip> part, Scope scope, int[] notes)
		{
			Clip i3 = new BassGuitar();
			i3.startTime = startTime;
			i3.endTime = startTime + 64;

			part.Add(i3);
			i3.volume = 70;
			//i3.scale = 4;
			i3.melody = new MObject();
			i3.melody.melody = new List<NoteStream>();
			i3.melody.melody.Add(new NoteStream(notes, false));
			i3.signature = new BarMeasure(4, 1);
			//i3.melody.melody[0].signature = new BarMeasure(1,32);
			i3.melody.melody[0].scale = scope.scale.Ascent;
			i3.melody.melody[0].key = scope.key;
			//i3.melody.melody[0].octave = 0;
			i3.Init(inst);

			return i3;
		}

		private static void CreateCelloBackingTrack(float startTime, List<Clip> part, Clip i2)
		{
			Clip i3 = new BassGuitar();
			i3.startTime = startTime;
			i3.endTime = startTime + 64;

			part.Add(i3);
			i3.volume = 70;
			i3.tempoScale = 1;
			i3.melody = new MObject();
			i3.melody.melody = new List<NoteStream>(i2.melody.melody);
			i3.melody.melody[0].octave = 0;
			i3.melody.melody[0].bStretch = true;

			NoteStream harmony = new NoteStream(i3.melody.melody[0]);
			for (int idx = 0; idx < harmony.notes.Count; idx++)
			{
				harmony.notes[idx] = new NoteStream.Note(i3.melody.melody[0].notes[idx]);
				harmony.notes[idx].note += 2;
			}
			harmony.signature = i3.melody.melody[0].signature;
			i3.melody.melody.Add(harmony);

			harmony = new NoteStream(i3.melody.melody[0]);
			for (int idx = 0; idx < harmony.notes.Count; idx++)
			{
				harmony.notes[idx] = new NoteStream.Note(i3.melody.melody[0].notes[idx]);
				harmony.notes[idx].note += 4;
			}
			harmony.signature = i3.melody.melody[0].signature;
			i3.melody.melody.Add(harmony);


			i3.signature = new BarMeasure(32, 8);
			i3.Init(Instrument.Cello);
			//tempo.count = 8;
			//tempo.measure = 1;
			//AddInstrument(scope, tempo, 2);
		}

		private static Clip CreateDistortionGuitar(Scope scope, float startTime, List<Clip> part, BarMeasure tempo, Clip i1)
		{
			Clip i2 = new BassGuitar();
			i2.tempoScale = 4.0f;
			i2.startTime = startTime;
			i2.endTime = startTime + 64;

			part.Add(i2);
			i2.volume = 70;
			scope.instrument = i2;
			i2.melody = new MObject();
			i2.melody.melody = new List<NoteStream>(i1.melody.melody);



			Multiply mult = new Multiply();
			mult.other = new MObject();
			tempo.measure = 8;
			tempo.count = 1;

			GenerateMelody2(scope, tempo, 1, mult.other);
			i2.melody.operations.Add(mult);
			//i2.melody.melody[0].bStretch = true;
			//i2.melody.melody[0].signature = new BarMeasure(1, 32);
			//i2.signature = new BarMeasure(1, 32);

			i2.Init(Midi.Instrument.DistortionGuitar);
			return i2;
		}

		public void Start()
		{
			SoundEngine.Instance.clock.Schedule(new CallbackMessage(CallbackHandler, SoundEngine.Instance.clock.Time + 0.1f));
		}

		private static void GenerateMelody2(Scope scope, BarMeasure tempo, int octave, MObject m)
		{
			NoteStream gen = GenerateMelody(scope, octave, tempo);
			gen.signature = tempo;
			m.melody.Add(gen);
		}

		private static NoteStream GenerateMelody(Scope scope, int octave, BarMeasure tempo)
		{
			NoteStream fourBar = new NoteStream();
			fourBar.key = scope.key;
			fourBar.octave = octave;
			fourBar.scale = scope.scale.Ascent;
			fourBar.signature = tempo;

			fourBar.notes = new List<NoteStream.Note>();

			int root = 0;// rnd.Next(7);

			int motion = 2;// 1 + rnd.Next(6);

			if (rnd.Next(2) == 0)
			{
				motion = -motion;
			}

			int noteCount = fourBar.signature.count * fourBar.signature.measure;

			for (int idx = 0; idx < noteCount; idx++)
			{
				if (root > 0)
				{
					fourBar.notes.Add(new NoteStream.Note(root % fourBar.scale.Length, 1.0f/(float)noteCount));
				}
				else
				{
					fourBar.notes.Add(new NoteStream.Note(fourBar.scale.Length + (root % fourBar.scale.Length), 1.0f / (float)noteCount));
				}

				int j = rnd.Next(4);
				switch (j)
				{
					case 0:
						root += motion;
						break;
					case 1:
						root += motion;
						break;
					case 2:
						do
						{
							motion = (rnd.Next(6) - 3);
						} while (motion == 0);
						root += motion;
						break;
					case 3:
						root = 0;
						break;
				}
			}
			return fourBar;
		}

		public override Music Compose(Music first, Music other)
		{
			throw new NotImplementedException();
		}

		float nextMeasure = 0;
		private void CallbackHandler(float time)
		{
			if (time >= 128)
			{
				SoundEngine.Instance.clock.Stop();
				SoundEngine.Instance.clock.Reset();
				time = 0;
			}

			Console.Out.WriteLine("Callback: " + time);

			int size = clips.Max(clip => clip.signature.count);
			Console.Out.WriteLine("... again at: " + size);

			foreach (Clip c in clips)
			{
				if (c.startTime <= nextMeasure && c.endTime > nextMeasure)
				{
					SoundEngine.Instance.clock.Schedule(c.messagesForOneMeasure, nextMeasure);
				}
			}
			nextMeasure += size;
			SoundEngine.Instance.clock.Schedule(new CallbackMessage(CallbackHandler, nextMeasure - 0.1f));

		}
	}
}
