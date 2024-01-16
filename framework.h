#pragma once

#ifndef COPY_PASTE_API
	#define COPY_PASTE_API		extern "C" __declspec(dllexport)
	#define WIN32_LEAN_AND_MEAN
	#define PRIVATE
#endif // COPY_PASTE_API

PRIVATE static const char* returnClipBoardData = nullptr;

COPY_PASTE_API const char* ClipBoardData();
//COPY_PASTE_API BYTE* GetScreenCapture(int &width, int &height);
