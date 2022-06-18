
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ballance2.Package;
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

    private static Vector3[] floatPtrToVec3Array(IntPtr floatPtr, int vertexCount) {
      float[] verticesPure = new float[3 * vertexCount];
      Marshal.Copy(floatPtr, verticesPure, 0, 3 * vertexCount);
      List<Vector3> vertices = new List<Vector3>();
      for (var j = 0; j < vertexCount; j++)
        vertices.Add(new Vector3(
          verticesPure[j * 3],
          verticesPure[j * 3 + 1],
          verticesPure[j * 3 + 2]
        ));
      return vertices.ToArray();
    }
    private static Vector2[] floatPtrToVec2Array(IntPtr floatPtr, int vec2Count) {
      float[] verticesPure = new float[2 * vec2Count];
      Marshal.Copy(floatPtr, verticesPure, 0, 2 * vec2Count);
      List<Vector2> vertices = new List<Vector2>();
      for (var j = 0; j < vec2Count; j++)
        vertices.Add(new Vector2(
          verticesPure[j * 2],
          verticesPure[j * 2 + 1]
        ));
      return vertices.ToArray();
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
    public static VirtoolsLoaderLoadNMOResult LoadNMOToScense(string fileFullPath, GamePackage basePackage) {
      
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
        case CKCID_3DOBJECT: {
          //读取3d对象
          //================================
          IntPtr infoPtr = VirtoolsLoaderApi.Loader_SolveNmoFile3dEntity(objPtr);
          Loader_3dEntityInfo info =  Marshal.PtrToStructure<Loader_3dEntityInfo>(infoPtr);

          //创建对象
          //================================
          GameObject go = new GameObject();
          go.name = objName;
          go.transform.position = new Vector3(info.position[0], info.position[1], info.position[2]);
          go.transform.rotation = new Quaternion(info.quaternion[0], info.quaternion[1], info.quaternion[2], info.quaternion[3]);
          go.transform.localScale = new Vector3(info.scale[0], info.scale[1], info.scale[2]);
          MeshFilter meshFilter = go.AddComponent<MeshFilter>();
          MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

          //读取Mesh
          //================================
          for (int i = 0; i < info.meshCount; ) {
            IntPtr meshPtr = VirtoolsLoaderApi.Loader_CK3dEntityGetMeshObj(objPtr, i);
            IntPtr meshInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileMesh(meshPtr);
            Loader_MeshInfo meshInfo =  Marshal.PtrToStructure<Loader_MeshInfo>(infoPtr);

            Mesh mesh = new Mesh();

            //读取顶点数组、三角形数组、法线数组
            //================================
            IntPtr verticesPtr = Marshal.AllocHGlobal(Marshal.SizeOf<float>() * 3 * meshInfo.vertexCount);
            IntPtr tranglesPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>() * 3 * meshInfo.faceCount);
            IntPtr normalsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<float>() * 3 * meshInfo.vertexCount);
            IntPtr uvsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * meshInfo.channelCount);
            List<IntPtr> uvsArr =new List<IntPtr>();
            for (var k = 0; k < meshInfo.channelCount; k++) {
              IntPtr uvsArrPtr = Marshal.AllocHGlobal(Marshal.SizeOf<float>() * 2 * meshInfo.vertexCount);
              uvsArr.Add(uvsArrPtr);
            }

            VirtoolsLoaderApi.Loader_DirectReadCKMeshData(meshPtr, verticesPtr, tranglesPtr, normalsPtr, uvsPtr);

            //转换所有数据至 Unity
            //================================

            //== vertices
            mesh.vertices = floatPtrToVec3Array(verticesPtr, meshInfo.vertexCount);
            //== trangles
            int[] tranglesPure = new int[3 * meshInfo.faceCount];
            Marshal.Copy(tranglesPtr, tranglesPure, 0, 3 * meshInfo.faceCount);
            mesh.triangles = tranglesPure;
            //== normals
            mesh.normals = floatPtrToVec3Array(normalsPtr, meshInfo.vertexCount);
            //UV 1-8
            for (var k = 0; k < meshInfo.channelCount && i < 8; k++) {
              switch(k) {
                case 0: mesh.uv = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
                case 1: mesh.uv2 = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
                case 2: mesh.uv3 = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
                case 3: mesh.uv4 = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
                case 4: mesh.uv5 = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
                case 5: mesh.uv6 = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
                case 6: mesh.uv7 = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
                case 7: mesh.uv8 = floatPtrToVec2Array(uvsArr[i], meshInfo.vertexCount); break;
              }
            }

            meshFilter.mesh = mesh;

            //读取Material
            for (int j = 0; j < meshInfo.materialCount; j++) {
              IntPtr matPtr = VirtoolsLoaderApi.Loader_CK3dEntityGetMeshObj(meshPtr, i);
              IntPtr matNamePtr = VirtoolsLoaderApi.Loader_CKObjectGetName(matPtr);
              string matName = Marshal.PtrToStringAnsi(matNamePtr);

              //如果材质名称是内定材质，则直接使用，否则从 virtools 读取材质
              //================================
              Material baseMat = basePackage.GetMaterialAsset(matName);
              if (baseMat) {

              } else {
                //从 virtools 读取材质
                //================================
                IntPtr matInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileMesh(matPtr);
                Loader_MaterialInfo matInfo = Marshal.PtrToStructure<Loader_MaterialInfo>(matInfoPtr);

                //baseMat = new Material();

                
              }
            }
            break;
          }
          break;
        }
        case CKCID_GROUP: {
          
          //创建组信息
          if(!result.groupList.ContainsKey(objName))
            result.groupList.Add(objName, new List<string>());

          List<string> groupName = result.groupList[objName];

          //循环组对象
          int groupObjCount = VirtoolsLoaderApi.Loader_CKGroupGetObjectCount(objPtr);
          IntPtr groupObjNamePtr = Marshal.AllocHGlobal(512);
          IntPtr groupObjClassIdPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>());
          for (int i = 0; i < groupObjCount; i++)
          {
            IntPtr objGroupPtr = VirtoolsLoaderApi.Loader_CKGroupGetObject(objPtr, i, groupObjClassIdPtr, groupObjNamePtr);
            if (objGroupPtr != IntPtr.Zero && Marshal.ReadInt32(groupObjClassIdPtr) == CKCID_3DOBJECT) {
              groupName.Add(Marshal.PtrToStringAnsi(groupObjNamePtr));//添加名称到组
            }
          }
          Marshal.FreeHGlobal(groupObjNamePtr);
          Marshal.FreeHGlobal(groupObjClassIdPtr);
          break;
        }
        }

      } while (objPtr != IntPtr.Zero);

      Marshal.FreeHGlobal(objNamePtr);
      Marshal.FreeHGlobal(classIdPtr);

      //关闭文件
      VirtoolsLoaderApi.Loader_SolveNmoFileDestroy(nmoFile);

      return result;
    }

    public class VirtoolsLoaderLoadNMOResult {
      public List<GameObject> objectList = new List<GameObject>();
      public Dictionary<string, Material> materialList = new Dictionary<string, Material>();
      public Dictionary<string, Mesh> meshList = new Dictionary<string, Mesh>();
      public Dictionary<string, Texture> textureList = new Dictionary<string, Texture>();
      public Dictionary<string, List<string>> groupList = new Dictionary<string, List<string>>();

      public string[] GetGroupList(string groupName) {
        if (groupList.TryGetValue(groupName, out var l))
          return l.ToArray();
        return null;
      }
      public string[] GetGroupNames() {
        return new List<string>(groupList.Keys).ToArray();
      }
    }
  }



}
