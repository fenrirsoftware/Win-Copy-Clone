#pragma once
#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#define COPY_PASTE_API		extern "C" __declspec(dllexport)

COPY_PASTE_API const char* ClipBoardData();
COPY_PASTE_API BYTE* GetScreenCapture(int &width, int &height);