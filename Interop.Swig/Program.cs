using System;

namespace Interop.Swig
{
	class Program
	{
		static void Main(string[] args)
		{
			var p = new Point(1, 2);
			p.Add(3, 4);

			Console.WriteLine($"Point({p.X}, {p.Y})");
		}
	}
}
