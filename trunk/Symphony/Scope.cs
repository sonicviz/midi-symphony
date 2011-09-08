

using Midi;
namespace SongCraft
{
	/// <summary>
	/// A scope is passed into a procedural "generate" call and specifies the
	/// environment (time, shape, color, key, whatever) that the element is required to fit in.
	/// </summary>
	public class Scope
	{
		/// <summary>
		/// The time box in which this procedural element must fit
		/// </summary>
		public BarMeasure signature;
		public int key;
		public ScalePattern scale;
		public Instrument instrument;
	}
}