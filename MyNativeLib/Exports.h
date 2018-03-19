#ifdef MYNATIVELIB_EXPORTS
#define MYAPI __declspec(dllexport)
#else
#define MYAPI __declspec(dllimport)
#endif
