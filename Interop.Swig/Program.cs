using System;

namespace Interop.Swig
{
	class Program
	{
		static void Main(string[] args)
		{
			var random = new Random();

			Console.WriteLine($"Next random number is {random.Next()}");

		   Console.WriteLine("\nPress ENTER to exit.");
		   Console.ReadLine();
		}
	}
}
