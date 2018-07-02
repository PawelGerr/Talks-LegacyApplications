using System;

namespace Interop.ByHand
{
	public class Program
	{
		public static void Main()
		{
			var random = RandomInterop.Random();
			var number = RandomInterop.Next(random);

			Console.WriteLine($"Next random number is {number}");

         Console.WriteLine("\nPress ENTER to exit.");
		   Console.ReadLine();
		}
	}
}
