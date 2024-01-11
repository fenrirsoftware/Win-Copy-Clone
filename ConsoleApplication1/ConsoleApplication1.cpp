#include <iostream>
#include <Windows.h>
#include <locale.h>

HHOOK g_hook;

LRESULT CALLBACK KeyboardProc(int nCode, WPARAM wParam, LPARAM lParam) {
    if (nCode == HC_ACTION) {
        KBDLLHOOKSTRUCT* keyboardStruct = reinterpret_cast<KBDLLHOOKSTRUCT*>(lParam);
        if (wParam == WM_KEYDOWN && keyboardStruct->vkCode == 'C' && GetAsyncKeyState(VK_CONTROL) & 0x8000) {
            wchar_t* clipboardData = nullptr;
            setlocale(LC_ALL, "Turkish");
            if (OpenClipboard(0)) {
                HANDLE hClipboardData = GetClipboardData(CF_UNICODETEXT);
                if (hClipboardData) {
                    const wchar_t* newClipboardData = static_cast<const wchar_t*>(GlobalLock(hClipboardData));
                    if (newClipboardData) {
                        size_t dataSize = wcslen(newClipboardData) + 1; // +1 for null terminator
                        clipboardData = new wchar_t[dataSize];
                        wcscpy_s(clipboardData, dataSize, newClipboardData);
                        GlobalUnlock(hClipboardData);
                    }
                }
                CloseClipboard();
            }

            if (clipboardData) {
                std::wcout << "Clipboard Data: " << clipboardData << std::endl;
                delete[] clipboardData; // Free allocated memory
            }
            else {
                std::wcout << "Failed to retrieve clipboard data." << std::endl;
            }
        }
    }

    return CallNextHookEx(g_hook, nCode, wParam, lParam);
}

int main() {
    g_hook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardProc, NULL, 0);

    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}
