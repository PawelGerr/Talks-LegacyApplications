%module MyNativeLib_Swig
%{
#include "../MyNativeLib/Random.h"
%}

#define MYAPI
%include "../MyNativeLib/Random.h"
