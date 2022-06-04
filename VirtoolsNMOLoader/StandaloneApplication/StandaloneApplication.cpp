#include <iostream>
#include "main.h"

int main()
{
  char fullPath[512];
  GetModuleFileNameA(0, fullPath, 0);
  for (int i = strlen(fullPath) - 1; i >= 0; i--)
    if (fullPath[i] == '\\')
      fullPath[i + 1] = '\0';

  if (Loader_Init(GetConsoleWindow(), fullPath))
    return -1;

  //if (Loader_SolveNmoFileLogic((char*)"E:\\Programming\\GameProjects\\Ballance2\\VirtoolsNMOLoader\\Debug\\1.NMO"))
  //  printf("Load failed");


  return Loader_Destroy();
}
