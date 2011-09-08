using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{
	public class Music : Procedural<Composition>
	{

		MObject playing;
		List<Instrument> instruments = new List<Instrument>();
		/// <summary>
		/// Generates a piece of music with a top level structure fitting of my (Thomas') composition techniques.
		/// Generates a top level structure,
		/// </summary>
		/// <param name="scope"></param>
		/// <returns></returns>
		public override Composition Generate(Scope scope)
		{
			Composition c = new Composition();

			scope.signature = new BarMeasure((int)Math.Pow(2, rnd.Next(4)), rnd.Next(2) > 0 ? 4 : 3);
			scope.key = 36 + rnd.Next(12);
			scope.scale = Scale.NaturalMinor;

			Drummer drummer = new Drummer();
			c.instruments.Add(drummer);
			scope.instrument = drummer;
			MObject m = new MObject();

			Melody downBeat = new Melody();
			downBeat.signature = scope.signature;
			downBeat.notes = new int[4] { 36, 0, 0, 0 };
			m.melody.Add(downBeat);

			Melody upBeat = new Melody();
			upBeat.signature = scope.signature;
			upBeat.notes = new int[4] { 0, 0, 40, 0 };
			m.melody.Add(upBeat);

			Melody straighBeat = new Melody();
			straighBeat.signature = scope.signature;
			straighBeat.notes = new int[4] { 44, 44, 44, 44 };
			m.melody.Add(straighBeat);

			Melody doubleKick = new Melody();
			doubleKick.signature = scope.signature;
			doubleKick.signature.measure *= 3;
			doubleKick.notes = new int[12] { 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36 };
			m.melody.Add(doubleKick);

			BarMeasure tempo = new BarMeasure(4, 1);
			drummer.Init(m, tempo);

			BassGuitar g = new BassGuitar();
			c.instruments.Add(g);
			scope.instrument = g;
			m = new MObject();

			Melody fourBar = new Melody();
			fourBar.signature = scope.signature;
			fourBar.notes = new int[4] { 0, 0, 0, 0 };
			for (int idx = 0; idx < 4; idx++)
			{
				fourBar.notes[idx] = scope.key + scope.scale.Ascent[rnd.Next(7)];
			}
			m.melody.Add(fourBar);
			g.Init(m, tempo);

			return c;
		}

		public override Composition Compose(Composition first, Composition other)
		{
			throw new NotImplementedException();
		}
	}
}
