
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ballance2 {

  /// <summary>
  /// NMO 加载器
  /// </summary>
  public class VirtoolsLoader {
    private static bool loaderInited = false;

    public static bool Init(string ck2DllPath) {
      if (!loaderInited) {
        if (VirtoolsLoaderApi.Loader_Init(IntPtr.Zero, ck2DllPath) == 0) {
          loaderInited = true;
          return true;
        } else {
          Debug.LogError("Loader_Init failed: " + VirtoolsLoaderApi.Loader_GetLastError());
        }
      }
      return false;
    }
    public static bool Destroy() {
      if (loaderInited) {
        if (VirtoolsLoaderApi.Loader_Destroy() == 0) {
          loaderInited = false;
          return true;
        } else {
          Debug.LogError("Loader_Destroy failed: " + VirtoolsLoaderApi.Loader_GetLastError());
        }
      }
      return false;
    }
  
    private const int CKCID_GROUP = 23;
    private const int CKCID_MATERIAL = 30;
    private const int CKCID_TEXTURE = 31;
    private const int CKCID_MESH = 32;
    private const int CKCID_3DENTITY = 33;
    private const int CKCID_3DOBJECT = 41;

    /// <summary>
    /// 加载NMO至场景
    /// </summary>
    /// <param name="fileFullPath"></param>
    public static VirtoolsLoaderLoadNMOResult LoadNMOToScense(string fileFullPath) {
      
      //读取文件
      IntPtr nmoFile = VirtoolsLoaderApi.Loader_SolveNmoFileRead(fileFullPath, IntPtr.Zero);
      if (nmoFile == IntPtr.Zero) {
        Debug.LogError("Load nom failed: " + VirtoolsLoaderApi.Loader_GetLastError());
        return null;
      }

      VirtoolsLoaderLoadNMOResult result = new VirtoolsLoaderLoadNMOResult();

      //文件对象循环
      int classId = 0;
      string objName = "";
      IntPtr objPtr = IntPtr.Zero;
      IntPtr objNamePtr = Marshal.AllocHGlobal(512);
      IntPtr classIdPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>());
      do {
        objPtr = VirtoolsLoaderApi.Loader_SolveNmoFileGetNext(nmoFile, classIdPtr, objNamePtr);
        objName = Marshal.PtrToStringAnsi(objNamePtr);
        classId = Marshal.ReadInt32(classIdPtr);

        switch (classId)
        {
        case CKCID_3DENTITY:
          break;
        case CKCID_3DOBJECT:
          break;
        case CKCID_MESH:
          break;
        case CKCID_MATERIAL:
          break;
        case CKCID_TEXTURE:
          break;
        case CKCID_GROUP: {
          int groupObjCount = VirtoolsLoaderApi.Loader_CKGroupGetObjectCount(objPtr);
          char objGroupObjName[512];
          int objGroupClassId = 0;
          for (int i = 0; i < groupObjCount; i++)
          {
            void* objGroupPtr = VirtoolsLoaderApi.Loader_CKGroupGetObject(objPtr, i, &objGroupClassId, objGroupObjName);
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
      VirtoolsLoaderApi.Loader_SolveNmoFileDestroy(nmoFile);

      return result;
    }

    public class VirtoolsLoaderLoadNMOResult {
      public List<GameObject> objectList;
      public Dictionary<string, string[]> groupList;

      public string[] GetGroupList(string groupName) {
        if (groupList.TryGetValue(groupName, out var l))
          return l;
        return null;
      }
      public string[] GetGroupNames() {
        return new List<string>(groupList.Keys).ToArray();
      }
    }
  }



}
