#include "MyClass.h"

using namespace std;

DllExport int add(int a, int b, ProgressCallback callback)
{
	callback("Adding two parameters ...");

	int result = a + b;

	callback("Added two parameters");

	return result;
}
