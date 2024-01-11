#include "pch.h"
#include <Windows.h>

extern "C" __declspec(dllexport) const char* ClipBoardData() { 
    const char* clipboardData = nullptr; 
    if (OpenClipboard(0)) { 
        HANDLE hClipboardData = GetClipboardData(CF_TEXT); 
        if (hClipboardData) { 
            const char* newClipboardData = static_cast<const char*>(GlobalLock(hClipboardData)); 
            if (newClipboardData) { 
                clipboardData = newClipboardData; 
                size_t clipboardLength = strlen(clipboardData); 
                HGLOBAL hNewClipboardData = GlobalAlloc(GMEM_MOVEABLE, clipboardLength + 1); 
                if (hNewClipboardData) { 
                    char* newClipboardDataCopy = static_cast<char*>(GlobalLock(hNewClipboardData)); 
                    if (newClipboardDataCopy) { 
                        memcpy(newClipboardDataCopy, clipboardData, clipboardLength + 1); 
                        GlobalUnlock(hNewClipboardData); 
                        SetClipboardData(CF_TEXT, hNewClipboardData); 
                    }
                }
                GlobalFree(hNewClipboardData);
            }
            GlobalUnlock(hClipboardData);
        }
        CloseClipboard();
    }
    return clipboardData;
}