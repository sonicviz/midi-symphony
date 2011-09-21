using System;
using Midi;
using System.Collections.Generic;
namespace SongCraft
{


	public abstract class Procedural<T> where T : Procedural<T>, new()
	{
		public BarMeasure signature;

		public static Random rnd = new Random();

		public abstract void Generate(Scope scope);
		public abstract T Compose(T first, T other);

		//public T GenerateComposite(Scope scope)
		//{
		//    T first = new T();
		//    first.Generate(scope);
		//    int divisions = rnd.Next((int)Math.Floor(Math.Log((double)scope.signature.count, 2.0)));
		//    Scope s = new Scope();

		//    for (int idx = 0; idx < divisions; idx++)
		//    {
		//        Scope sc = new Scope();
		//        sc.key = scope.key;
		//        sc.scale = scope.scale;
		//        sc.signature = scope.signature;
		//        sc.signature.count = scope.signature.count / divisions;
		//        Compose(first,Generate(sc));
		//    }

		//    return first;
		//}

	}
}