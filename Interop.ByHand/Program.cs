using System;

namespace Interop.ByHand
{
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine("Adding 1 and 2");

			var result = MyClassInterop.add(1, 2, PrintMessage);

			Console.WriteLine($"Result is {result}");
		}

		private static void PrintMessage(string message)
		{
			Console.WriteLine($"From native lib: {message}");
		}
	}
}
