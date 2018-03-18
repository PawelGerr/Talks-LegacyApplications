#define DllExport __declspec(dllexport) 

extern "C" {
	typedef void(__stdcall * ProgressCallback)(const char* message);

	DllExport int __stdcall add(int a, int b, ProgressCallback callback);
}
