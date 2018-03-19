using System;
using System.Reflection;

namespace Interop.Swig
{
	class Program
	{
		static void Main(string[] args)
		{
			var random = new Random();

			Console.WriteLine($"Next random number is {random.Next()}");
		}
	}
}
