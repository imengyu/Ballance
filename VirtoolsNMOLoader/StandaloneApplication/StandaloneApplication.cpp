#include <iostream>
#include "main.h"

#define CKCID_GROUP 23
#define CKCID_MATERIAL 30
#define CKCID_TEXTURE 31
#define CKCID_MESH 32
#define CKCID_3DENTITY 33
#define CKCID_3DOBJECT 41

int main()
{
  //路径
  char fullPath[512];
  GetModuleFileName(0, fullPath, 512);
  for (int i = strlen(fullPath) - 1; i >= 0; i--)
    if (fullPath[i] == '\\') {
      fullPath[i + 1] = '\0';
      break;
    }

  //初始化库
  if (Loader_Init(GetConsoleWindow(), fullPath))
    return -1;

  char filePath[512];
  strcpy(filePath, fullPath);
  strcat(filePath, "\\Level_01.NMO");

  //读取文件
  void* nmoFile = Loader_SolveNmoFileRead(filePath, nullptr);
  if (!nmoFile) {
    printf("Load failed");
    return -1;
  }

  //文件对象循环
  int classId = 0;
  void *objPtr = 0;
  char objName[512];
  int index = 0;
  do {
    objPtr = Loader_SolveNmoFileGetNext(nmoFile, &classId, objName);
    printf("%d\n", index++);
    
    switch (classId)
    {
    case CKCID_3DENTITY:
      printf("  CKCID_3DENTITY: %s\n", objName);
      break;
    case CKCID_3DOBJECT:
      printf("  CKCID_3DOBJECT: %s\n", objName);
      break;
    case CKCID_MESH:
      printf("  CKCID_MESH: %s\n", objName);
      break;
    case CKCID_MATERIAL:
      printf("  CKCID_MATERIAL: %s\n", objName);
      break;
    case CKCID_TEXTURE:
      printf("  CKCID_TEXTURE: %s\n", objName);
      break;
    case CKCID_GROUP: {
      printf("  CKCID_GROUP: %s\n", objName);
      int groupObjCount = Loader_CKGroupGetObjectCount(objPtr);
      char objGroupObjName[512];
      int objGroupClassId = 0;
      for (int i = 0; i < groupObjCount; i++)
      {
        void* objGroupPtr = Loader_CKGroupGetObject(objPtr, i, &objGroupClassId, objGroupObjName);
        if (objGroupPtr && objGroupClassId == CKCID_3DOBJECT) {
          printf("    GroupObject: %s\n", objGroupObjName);
        }
      }
      break;
    }
    default:
      printf("  UNKNOW CKCID: %d %s\n", classId, objName);
      break;
    }

  } while (objPtr);


  //关闭文件
  Loader_SolveNmoFileDestroy(nmoFile);

  //卸载
  return Loader_Destroy();
}
