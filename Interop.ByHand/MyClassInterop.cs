using System.Runtime.InteropServices;

namespace Interop.ByHand
{
	public class MyClassInterop
	{
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void ProgressCallback(string message);

		[DllImport("MyNativeLib.dll")]
		public static extern int add(int a, int b, ProgressCallback callback);
	}
}
