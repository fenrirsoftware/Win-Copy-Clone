#include "pch.h"

const char* ClipBoardData() {
    const char* clipboardData = nullptr; 
    if (OpenClipboard(NULL)) {
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

BYTE* GetScreenCapture(int &width, int &height) {
    int screenWidth = GetSystemMetrics(SM_CXSCREEN);
    int screenHeight = GetSystemMetrics(SM_CYSCREEN);
    HDC hdcScreen = GetDC(NULL);
    HDC hdcMem = CreateCompatibleDC(hdcScreen);
    HBITMAP hBitmap = CreateCompatibleBitmap(hdcScreen, screenWidth, screenHeight);

    SelectObject(hdcMem, hBitmap);
    BitBlt(hdcMem, 0, 0, screenWidth, screenHeight, hdcScreen, 0, 0, SRCCOPY);

    BYTE* lpbitmap = nullptr;

    if (GetAsyncKeyState(VK_SNAPSHOT) & 0x8000) {
        BITMAPINFOHEADER bi;
        bi.biSize = sizeof(BITMAPINFOHEADER);
        bi.biWidth = screenWidth;
        bi.biHeight = -screenHeight;
        bi.biPlanes = 1;
        bi.biBitCount = 24;
        bi.biCompression = BI_RGB;
        bi.biSizeImage = 0;
        bi.biXPelsPerMeter = 0;
        bi.biYPelsPerMeter = 0;
        bi.biClrUsed = 0;
        bi.biClrImportant = 0;

        DWORD dwBmpSize = ((screenWidth * bi.biBitCount + 31) / 32) * 4 * screenHeight;
        lpbitmap = new BYTE[dwBmpSize];
        GetDIBits(hdcScreen, hBitmap, 0, screenHeight, lpbitmap, (BITMAPINFO*)&bi, DIB_RGB_COLORS);
        width = screenWidth;
        height = screenHeight;
    }else {
        OpenClipboard(NULL);
        EmptyClipboard();
        SetClipboardData(CF_BITMAP, hBitmap);
        CloseClipboard();

        HBITMAP hClipboardBitmap = static_cast<HBITMAP>(GetClipboardData(CF_BITMAP));

        if (hClipboardBitmap) {
            BITMAP bitmapInfo;
            GetObject(hClipboardBitmap, sizeof(BITMAP), &bitmapInfo);
            lpbitmap = new BYTE[bitmapInfo.bmWidthBytes * bitmapInfo.bmHeight];
            GetBitmapBits(hClipboardBitmap, bitmapInfo.bmWidthBytes * bitmapInfo.bmHeight, lpbitmap);
            width = bitmapInfo.bmWidth;
            height = bitmapInfo.bmHeight;
        }

    }
    DeleteObject(hBitmap);
    DeleteDC(hdcMem);
    ReleaseDC(NULL, hdcScreen);

    return lpbitmap;
}
