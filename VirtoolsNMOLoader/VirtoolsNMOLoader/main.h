#pragma once
#include "pch.h"

#ifdef __cplusplus
EXTERN_C {
#endif //  __cplusplus
  API_EXPORT int Loader_Init(HWND hWnd, char* currentPath);
  API_EXPORT int Loader_Destroy();
  API_EXPORT void* Loader_SolveNmoFileGetNext(void* filePtr, int* classIdOut);
  API_EXPORT void Loader_SolveNmoFileReset(void* filePtr);
#ifdef __cplusplus
}
#endif //  __cplusplus
