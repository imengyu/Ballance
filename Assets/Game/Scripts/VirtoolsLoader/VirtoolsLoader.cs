
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ballance2
{

  /// <summary>
  /// NMO loader
  /// </summary>
  public class VirtoolsLoader
  {
    private static bool loaderInited = false;

    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    public static extern int SetDllDirectoryA([MarshalAs(UnmanagedType.LPStr)] string path);
    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    public static extern int LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string dll);

    public static bool Init(string ck2DllPath)
    {
      if (!loaderInited)
      {
        if (VirtoolsLoaderApi.Loader_Init(IntPtr.Zero, ck2DllPath) == 0)
        {
          loaderInited = true;
          Debug.Log("Loader_Init success");
          return true;
        }
        else
        {
          Debug.LogError("Loader_Init failed: " + VirtoolsLoaderApi.Loader_GetLastError());
        }
      }
      return false;
    }
    public static bool Destroy()
    {
      if (loaderInited)
      {
        if (VirtoolsLoaderApi.Loader_Destroy() == 0)
        {
          loaderInited = false;
          return true;
        }
        else
        {
          Debug.LogError("Loader_Destroy failed: " + VirtoolsLoaderApi.Loader_GetLastError());
        }
      }
      return false;
    }

    private static Vector3[] floatPtrToVec3Array(IntPtr floatPtr, int vertexCount)
    {
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
    private static Vector2[] floatPtrToVec2Array(IntPtr floatPtr, int vec2Count)
    {
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

    public delegate Material LoadNMOToScenseMaterialCallback(string matName);
    public delegate Texture LoadNMOToScenseTextureCallback(string texName);

    /// <summary>
    /// load NMO 3d object and group info to scense
    /// </summary>
    /// <param name="fileFullPath"></param>
    public static VirtoolsLoaderLoadNMOResult LoadNMOToScense(string fileFullPath, 
      LoadNMOToScenseMaterialCallback matCallback,
      LoadNMOToScenseTextureCallback texCallback)
    {

      //Read file
      IntPtr nmoFile = VirtoolsLoaderApi.Loader_SolveNmoFileRead(fileFullPath, IntPtr.Zero);
      if (nmoFile == IntPtr.Zero)
      {
        Debug.LogError("Load nmo " + fileFullPath + " failed: " + VirtoolsLoaderApi.Loader_GetLastError());
        return null;
      }
      Debug.Log("Load nmo " + fileFullPath + " success");

      try {
        VirtoolsLoaderLoadNMOResult result = new VirtoolsLoaderLoadNMOResult();

        //Create default res
        Material materialDefault = new Material(Shader.Find("Diffuse"));
        Shader materialShader = Shader.Find("Diffuse");

        //File object loop
        int classId = 0;
        string objName = "";
        IntPtr objPtr = IntPtr.Zero;
        IntPtr objNamePtr = Marshal.AllocHGlobal(512);
        IntPtr classIdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
        do
        {
          objPtr = VirtoolsLoaderApi.Loader_SolveNmoFileGetNext(nmoFile, classIdPtr, objNamePtr);
          if (objPtr == IntPtr.Zero)
            continue;
          objName = Marshal.PtrToStringAnsi(objNamePtr);
          classId = Marshal.ReadInt32(classIdPtr);

          switch (classId)
          {
            case CKCID_3DOBJECT:
              {
                //Read 3d Entity
                //================================
                IntPtr infoPtr = VirtoolsLoaderApi.Loader_SolveNmoFile3dEntity(objPtr);
                Loader_3dEntityInfo info = (Loader_3dEntityInfo)Marshal.PtrToStructure(infoPtr, typeof(Loader_3dEntityInfo));
                VirtoolsLoaderApi.Loader_Free(infoPtr);

                //Create object 
                //================================
                GameObject go = new GameObject();
                go.name = objName;
                go.transform.position = new Vector3(info.positionX, info.positionY, info.positionZ);
                //go.transform.eulerAngles = new Vector3(info.eulerX * Mathf.Rad2Deg, info.eulerY * Mathf.Rad2Deg, info.eulerZ * Mathf.Rad2Deg);
                go.transform.rotation = new Quaternion(info.quaternionX, info.quaternionY, info.quaternionZ, -info.quaternionW);

                //if (objName == "A04_Floor_Wood_01")
                //  Debug.Log("eulerAngles: " + info.eulerX + "," + info.eulerY + "," + info.eulerZ);
                go.transform.localScale = new Vector3(info.scaleX, info.scaleY, info.scaleZ);
                MeshFilter meshFilter = go.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

                //Read Mesh
                //================================
                for (int i = 0; i < info.meshCount;)
                {
                  IntPtr meshPtr = VirtoolsLoaderApi.Loader_CK3dEntityGetMeshObj(objPtr, i);
                  IntPtr meshInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileMesh(meshPtr);
                  Loader_MeshInfo meshInfo = (Loader_MeshInfo)Marshal.PtrToStructure(meshInfoPtr, typeof(Loader_MeshInfo));
                  VirtoolsLoaderApi.Loader_Free(meshInfoPtr);

                  Mesh mesh = new Mesh();

                  /*Debug.Log(
                    "vertexCount: " + meshInfo.vertexCount + "\nfaceCount: " + meshInfo.faceCount + 
                    "\nchannelCount: " + meshInfo.channelCount
                  );*/

                  //Read mesh info
                  //================================
                  IntPtr verticesPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 3 * meshInfo.vertexCount);
                  IntPtr tranglesPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 3 * meshInfo.faceCount);
                  IntPtr normalsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 3 * meshInfo.vertexCount);

                  List<IntPtr> uvsArr = new List<IntPtr>();
                  IntPtr uvsPtr;
                  if (meshInfo.channelCount > 0) {
                    uvsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * meshInfo.channelCount);
                    for (var k = 0; k < meshInfo.channelCount; k++)
                    {
                      IntPtr uvsArrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 2 * meshInfo.vertexCount);
                      uvsArr.Add(uvsArrPtr);
                    }
                    Marshal.Copy(uvsArr.ToArray(), 0, uvsPtr, uvsArr.Count);
                  } 
                  else
                  {
                    uvsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * 1);
                    IntPtr uvsArrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 2 * meshInfo.vertexCount);
                    uvsArr.Add(uvsArrPtr);
                    Marshal.Copy(uvsArr.ToArray(), 0, uvsPtr, 1);
                  }

                  VirtoolsLoaderApi.Loader_DirectReadCKMeshData(meshPtr, verticesPtr, tranglesPtr, normalsPtr, uvsPtr);

                  //Copy mesh info to Unity
                  //================================

                  //== vertices
                  mesh.vertices = floatPtrToVec3Array(verticesPtr, meshInfo.vertexCount);
                  //== trangles
                  int[] tranglesPure = new int[3 * meshInfo.faceCount];
                  Marshal.Copy(tranglesPtr, tranglesPure, 0, 3 * meshInfo.faceCount);
                  mesh.triangles = tranglesPure;
                  //== normals
                  mesh.normals = floatPtrToVec3Array(normalsPtr, meshInfo.vertexCount);
                  if (meshInfo.channelCount > 0)
                  {
                    //UV 1-8
                    for (var k = 0; k < meshInfo.channelCount && i < 8; k++)
                    {
                      switch (k)
                      {
                        case 0: mesh.uv = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                        case 1: mesh.uv2 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                          //case 2: mesh.uv3 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                          //case 3: mesh.uv4 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                          //case 4: mesh.uv5 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                          //case 5: mesh.uv6 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                          //case 6: mesh.uv7 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                          //case 7: mesh.uv8 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                      }
                    }
                  } 
                  else
                  {
                    mesh.uv = floatPtrToVec2Array(uvsArr[0], meshInfo.vertexCount);
                  }

                  meshFilter.mesh = mesh;

                  //Free buffer
                  Marshal.FreeHGlobal(verticesPtr);
                  Marshal.FreeHGlobal(tranglesPtr);
                  Marshal.FreeHGlobal(normalsPtr);
                  for (int l = 0; l < uvsArr.Count; l++)
                    Marshal.FreeHGlobal(uvsArr[l]);
                  uvsArr.Clear();
                  if (uvsPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(uvsPtr);

                  if (meshInfo.materialCount == 0) {
                    meshRenderer.material = materialDefault;
                  } else {
                    List<Material> matArr = new List<Material>();
                    //Read Material
                    for (int j = 0; j < meshInfo.materialCount; j++)
                    {
                      IntPtr matPtr = VirtoolsLoaderApi.Loader_CKMeshGetMaterialObj(meshPtr, i);
                      IntPtr matNamePtr = VirtoolsLoaderApi.Loader_CKObjectGetName(matPtr);
                      string matName = Marshal.PtrToStringAnsi(matNamePtr);

                      //If mat name is game internl materials, use our materials.
                      //================================
                      Material baseMat = matCallback(matName);
                      if (baseMat == null) 
                      {
                        //Read material from virtools
                        //================================
                        IntPtr matInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileMaterial(matPtr, nmoFile);
                        Loader_MaterialInfo matInfo = (Loader_MaterialInfo)Marshal.PtrToStructure(matInfoPtr, typeof(Loader_MaterialInfo));

                        baseMat = new Material(materialShader);
                        baseMat.color = new Color(matInfo.diffuseR, matInfo.diffuseG, matInfo.diffuseB, matInfo.ambientA);

                        //Read texture object
                        if (matInfo.textureObject != IntPtr.Zero)
                        {
                          IntPtr texInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileTexture(matInfo.textureObject);
                          IntPtr texNamePtr = VirtoolsLoaderApi.Loader_CKObjectGetName(matInfo.textureObject);
                          string texName = Marshal.PtrToStringAnsi(texNamePtr);

                          //Try find our texture
                          Texture tex = texCallback(texName);
                          if (tex == null)
                          {
                            //Otherwise, read texture from virtools
                            Loader_TextureInfo texInfo = (Loader_TextureInfo)Marshal.PtrToStructure(texInfoPtr, typeof(Loader_TextureInfo));
                            if (texInfo.bufferSize > 0)
                            {
                              //Instance texture
                              Texture2D texVt = new Texture2D(texInfo.width, texInfo.height);
                              //Read buffer
                              IntPtr dataBuffer = Marshal.AllocHGlobal(texInfo.bufferSize);
                              long ptr = dataBuffer.ToInt64(), count = texInfo.width * texInfo.height;
                              List<Color> colorArray = new List<Color>();

                              Debug.Log(
                                "Texture: " + texName + 
                                "\n" + texInfo.width + "x" + texInfo.height + "\bufferSize: " + texInfo.bufferSize +
                                "\videoPixelFormat: " + texInfo.videoPixelFormat
                              );


                              switch (texInfo.videoPixelFormat)
                              {
                                case 3: // _24_RGB888:
                                  for (int k = 0; k < count; k++)
                                  {
                                    colorArray.Add(new Color32(
                                      Marshal.ReadByte(new IntPtr(ptr)),
                                      Marshal.ReadByte(new IntPtr(ptr + 1)),
                                      Marshal.ReadByte(new IntPtr(ptr + 2)),
                                      255
                                    ));
                                    ptr += 3;
                                  }
                                  break;
                                case 14: // _24_BGR888:
                                  for (int k = 0; k < count; k++)
                                  {
                                    colorArray.Add(new Color32(
                                      Marshal.ReadByte(new IntPtr(ptr + 2)),
                                      Marshal.ReadByte(new IntPtr(ptr + 1)),
                                      Marshal.ReadByte(new IntPtr(ptr)),
                                      255
                                    ));
                                    ptr += 3;
                                  }
                                  break;
                                case 10: // _32_ABGR8888:
                                  for (int k = 0; k < count; k++)
                                  {
                                    colorArray.Add(new Color32(
                                      Marshal.ReadByte(new IntPtr(ptr + 2)),
                                      Marshal.ReadByte(new IntPtr(ptr + 1)),
                                      Marshal.ReadByte(new IntPtr(ptr)),
                                      Marshal.ReadByte(new IntPtr(ptr + 3))
                                    ));
                                    ptr += 4;
                                  }
                                  break;
                                case 1: // _32_ARGB8888:
                                  for (int k = 0; k < count; k++)
                                  {
                                    colorArray.Add(new Color32(
                                      Marshal.ReadByte(new IntPtr(ptr)),
                                      Marshal.ReadByte(new IntPtr(ptr + 3)),
                                      Marshal.ReadByte(new IntPtr(ptr + 1)),
                                      Marshal.ReadByte(new IntPtr(ptr + 2))
                                    ));
                                    ptr += 4;
                                  }
                                  break;
                                case 11: // _32_RGBA8888:
                                  for (int k = 0; k < count; k++)
                                  {
                                    colorArray.Add(new Color32(
                                      Marshal.ReadByte(new IntPtr(ptr)),
                                      Marshal.ReadByte(new IntPtr(ptr + 1)),
                                      Marshal.ReadByte(new IntPtr(ptr + 2)),
                                      Marshal.ReadByte(new IntPtr(ptr + 3))
                                    ));
                                    ptr += 4;
                                  }
                                  break;
                                case 2: // _32_RGB888:
                                  for (int k = 0; k < count; k++)
                                  {
                                    colorArray.Add(new Color32(
                                      Marshal.ReadByte(new IntPtr(ptr)),
                                      Marshal.ReadByte(new IntPtr(ptr + 1)),
                                      Marshal.ReadByte(new IntPtr(ptr + 2)),
                                      255
                                    ));
                                    ptr += 4;
                                  }
                                  break;
                              }

                              texVt.SetPixels(colorArray.ToArray());
                              colorArray.Clear();
                              tex = texVt;

                              Marshal.FreeHGlobal(dataBuffer);
                            }
                          }

                          baseMat.mainTexture = tex;
                        }

                        VirtoolsLoaderApi.Loader_Free(matInfoPtr);
                      }
                      matArr.Add(baseMat);
                    }

                    meshRenderer.materials = matArr.ToArray();
                  }

                  break;
                }
                break;
              }
            case CKCID_GROUP:
              {

                //Build group info
                if (!result.groupList.ContainsKey(objName))
                  result.groupList.Add(objName, new List<string>());

                List<string> groupName = result.groupList[objName];

                //Loop group objects
                int groupObjCount = VirtoolsLoaderApi.Loader_CKGroupGetObjectCount(objPtr);
                IntPtr groupObjNamePtr = Marshal.AllocHGlobal(512);
                IntPtr groupObjClassIdPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                for (int i = 0; i < groupObjCount; i++)
                {
                  IntPtr objGroupPtr = VirtoolsLoaderApi.Loader_CKGroupGetObject(objPtr, i, groupObjClassIdPtr, groupObjNamePtr);
                  if (objGroupPtr != IntPtr.Zero && Marshal.ReadInt32(groupObjClassIdPtr) == CKCID_3DOBJECT)
                  {
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

        return result;
      } finally {

        //Close file
        VirtoolsLoaderApi.Loader_SolveNmoFileDestroy(nmoFile);

        Debug.Log("Nmo file closed");
      }
    }

    public class VirtoolsLoaderLoadNMOResult
    {
      public List<GameObject> objectList = new List<GameObject>();
      public Dictionary<string, Material> materialList = new Dictionary<string, Material>();
      public Dictionary<string, Mesh> meshList = new Dictionary<string, Mesh>();
      public Dictionary<string, Texture> textureList = new Dictionary<string, Texture>();
      public Dictionary<string, List<string>> groupList = new Dictionary<string, List<string>>();

      public string[] GetGroupList(string groupName)
      {
        List<string> list = null;
        if (groupList.TryGetValue(groupName, out list))
          return list.ToArray();
        return null;
      }
      public string[] GetGroupNames()
      {
        return new List<string>(groupList.Keys).ToArray();
      }
    }
  }



}
