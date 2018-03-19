using System;
using System.Runtime.InteropServices;

namespace Interop.ByHand
{
	public class RandomInterop
	{
		[DllImport("MyNativeLib", EntryPoint = "??4Random@@QEAAAEAV0@$$QEAV0@@Z")]
		public static extern IntPtr Random();

		[DllImport("MyNativeLib", EntryPoint = "?Next@Random@@QEAAHXZ")]
		public static extern int Next(IntPtr random);
	}
}
