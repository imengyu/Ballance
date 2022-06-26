
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Ballance2
{

  /// <summary>
  /// NMO loader
  /// </summary>
  public class VirtoolsLoader
  {
#if UNITY_STANDALONE_WIN
    private static bool loaderInited = false;

    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    private static extern int SetDllDirectoryA([MarshalAs(UnmanagedType.LPStr)] string path);
    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    private static extern int LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string dll);

    public static bool Init(string ck2DllPath)
    {
      if (!loaderInited)
      { 
        string dllDir = Path.GetDirectoryName(ck2DllPath).Replace('/', '\\');

        VirtoolsLoader.SetDllDirectoryA(dllDir);
		    VirtoolsLoader.LoadLibraryA(dllDir + "\\VirtoolsNMOLoader.dll");

        IntPtr stringPointer = Marshal.StringToHGlobalUni(ck2DllPath);
		
        if (VirtoolsLoaderApi.Loader_Init(IntPtr.Zero, stringPointer) == 0)
        {
          Marshal.FreeHGlobal(stringPointer);
          loaderInited = true;
          Debug.Log("Loader_Init success");
          return true;
        }
        else
        {
          Marshal.FreeHGlobal(stringPointer);
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
          -verticesPure[j * 2 + 1]
        ));
      return vertices.ToArray();
    }
    private static string getObjNameOrPtrName(IntPtr stringPtr, IntPtr objPtr, bool addPtrString = false)
    {
      try {
        return Marshal.PtrToStringAnsi(stringPtr) + (addPtrString ? objPtr.ToInt32() : "");
      } catch {
        return "Object" + objPtr.ToInt32();
      }
    }

    private const int CKCID_GROUP = 23;
    private const int CKCID_MATERIAL = 30;
    private const int CKCID_TEXTURE = 31;
    private const int CKCID_MESH = 32;
    private const int CKCID_3DENTITY = 33;
    private const int CKCID_3DOBJECT = 41;

    /// <summary>
    /// load NMO 3d object and group info to scense
    /// </summary>
    /// <param name="fileFullPath"></param>
    public static VirtoolsLoaderLoadNMOResult LoadNMOToScense(string fileFullPath, 
      LoadNMOToScenseMaterialCallback matCallback,
      LoadNMOToScenseTextureCallback texCallback, 
      LoadNMOToScenseErrorCallback errorCallback,
      Shader materialShader)
    {

      //Read file
      IntPtr stringPointer = Marshal.StringToHGlobalUni(fileFullPath.Replace('/', '\\'));
      IntPtr nmoFile = VirtoolsLoaderApi.Loader_SolveNmoFileRead(stringPointer, IntPtr.Zero);
      if (nmoFile == IntPtr.Zero)
      {
        Marshal.FreeHGlobal(stringPointer);
        errorCallback("Load nmo " + fileFullPath + " failed: " + VirtoolsLoaderApi.Loader_GetLastError());
        return null;
      }
      Marshal.FreeHGlobal(stringPointer);
      Debug.Log("Load nmo " + fileFullPath + " success");

      try {
        VirtoolsLoaderLoadNMOResult result = new VirtoolsLoaderLoadNMOResult();

        //Create default res
        Material materialDefault = new Material(Shader.Find("Standard"));

        //Cache texture and material for reuse 
        Dictionary<string, Texture> texturePool = new Dictionary<string, Texture>();
        Dictionary<string, Material> materialPool = new Dictionary<string, Material>();

        //host object
        GameObject main = new GameObject("NMO");
        result.mainObj = main;

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
          classId = Marshal.ReadInt32(classIdPtr);

          switch (classId)
          {
            case CKCID_3DOBJECT:
              {
                objName = getObjNameOrPtrName(objNamePtr, objPtr, true);

                //Read 3d Entity
                //================================
                IntPtr infoPtr = VirtoolsLoaderApi.Loader_SolveNmoFile3dEntity(objPtr);
                Loader_3dEntityInfo info = (Loader_3dEntityInfo)Marshal.PtrToStructure(infoPtr, typeof(Loader_3dEntityInfo));
                VirtoolsLoaderApi.Loader_Free(infoPtr);

                //Create object 
                //================================
                GameObject go = new GameObject(objName);
                go.transform.SetParent(main.transform);
                result.objectNameList[objName] = go;
                go.transform.position = new Vector3(info.positionX, info.positionY, info.positionZ);
                //go.transform.eulerAngles = new Vector3(info.eulerX * Mathf.Rad2Deg, info.eulerY * Mathf.Rad2Deg, info.eulerZ * Mathf.Rad2Deg);
                go.transform.rotation = new Quaternion(info.quaternionX, info.quaternionY, info.quaternionZ, -info.quaternionW);


                //if (objName == "A04_Floor_Wood_01")
                //  Debug.Log("eulerAngles: " + info.eulerX + "," + info.eulerY + "," + info.eulerZ);
                go.transform.localScale = new Vector3(info.scaleX, info.scaleY, info.scaleZ);
                MeshFilter meshFilter = go.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

                //Hidden object
                if (VirtoolsLoaderApi.Loader_CKObjectIsHidden(objPtr) > 0) {
                  meshRenderer.enabled = false;
                  continue;
                }

                //Read Mesh
                //================================
                for (int i = 0; i < info.meshCount;)
                {
                  //Get mesh info
                  IntPtr meshPtr = VirtoolsLoaderApi.Loader_CK3dEntityGetMeshObj(objPtr, i);
                  IntPtr meshInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileMesh(meshPtr);
                  IntPtr meshNamePtr = VirtoolsLoaderApi.Loader_CKObjectGetName(meshPtr);
                  string meshName = getObjNameOrPtrName(meshNamePtr, meshPtr);
                  Loader_MeshInfo meshInfo = (Loader_MeshInfo)Marshal.PtrToStructure(meshInfoPtr, typeof(Loader_MeshInfo));
                  VirtoolsLoaderApi.Loader_Free(meshInfoPtr);

                  Mesh mesh = new Mesh();
                  mesh.name = meshName;

                  /*Debug.Log(
                    objName + ": vertexCount: " + meshInfo.vertexCount + "\nfaceCount: " + meshInfo.faceCount + 
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

                  result.meshList[meshName] = mesh;
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
                        case 2: mesh.uv3 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                        case 3: mesh.uv4 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                        case 4: mesh.uv5 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                        case 5: mesh.uv6 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                        case 6: mesh.uv7 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
                        case 7: mesh.uv8 = floatPtrToVec2Array(uvsArr[k], meshInfo.vertexCount); break;
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
                      //For MaterialFaces
                      IntPtr outMaterialFacesCountPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                      IntPtr outMaterialFacesPtrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));

                      //Read info
                      IntPtr matPtr = VirtoolsLoaderApi.Loader_CKMeshGetMaterialObj(meshPtr, j, outMaterialFacesCountPtr, outMaterialFacesPtrPtr);
                      IntPtr matNamePtr = VirtoolsLoaderApi.Loader_CKObjectGetName(matPtr);
                      string matName = getObjNameOrPtrName(matNamePtr, matPtr);
                      int copyIndex = matName.IndexOf(".Copy");
                      if (copyIndex > 0)
                        matName = matName.Substring(0, copyIndex);

                      int outMaterialFacesCount = Marshal.ReadInt32(outMaterialFacesCountPtr);
                      IntPtr outMaterialFacesPtr = Marshal.ReadIntPtr(outMaterialFacesPtrPtr);

                      Marshal.FreeHGlobal(outMaterialFacesCountPtr);
                      Marshal.FreeHGlobal(outMaterialFacesPtrPtr);

                      //Add submesh for different material
                      //================================
                      if (outMaterialFacesCount > 0)
                      {
                        int[] indices = new int[outMaterialFacesCount * 3];
                        Marshal.Copy(outMaterialFacesPtr, indices, 0, outMaterialFacesCount * 3);
                        mesh.subMeshCount += 1;
                        mesh.SetIndices(indices, MeshTopology.Triangles, j);
                      }
                      VirtoolsLoaderApi.Loader_Free(outMaterialFacesPtr);

                      //If mat name is game internl materials, use our materials.
                      //================================
                      Material baseMat = matCallback(matName);
                      if (baseMat == null)
                      {
                        //Try get from cache
                        materialPool.TryGetValue(matName, out baseMat);
                        if (baseMat == null) { 

                          //Read material from virtools
                          //================================
                          IntPtr matInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileMaterial(matPtr, nmoFile);
                          Loader_MaterialInfo matInfo = (Loader_MaterialInfo)Marshal.PtrToStructure(matInfoPtr, typeof(Loader_MaterialInfo));
                          VirtoolsLoaderApi.Loader_Free(matInfoPtr);

                          baseMat = new Material(materialShader);
                          baseMat.color = new Color(matInfo.diffuseR, matInfo.diffuseG, matInfo.diffuseB, matInfo.diffuseA);
                          baseMat.SetColor("_AmbientColor", new Color(matInfo.ambientR, matInfo.ambientG, matInfo.ambientB, matInfo.ambientA));
                          baseMat.SetColor("_Color", baseMat.color);
                          baseMat.SetColor("_SpecColor", new Color(matInfo.specularR, matInfo.specularG, matInfo.specularB, matInfo.specularA));
                          baseMat.SetFloat("_Gloss", matInfo.power);
                          baseMat.SetColor("_Emission", new Color(matInfo.emissiveR, matInfo.emissiveG, matInfo.emissiveB, matInfo.emissiveA));

                          baseMat.name = matName;
                          materialPool[matName] = baseMat;
                          result.materialList[matName] = baseMat;

                          //Read texture object
                          if (matInfo.textureObject != IntPtr.Zero)
                          {
                            IntPtr texNamePtr = VirtoolsLoaderApi.Loader_CKObjectGetName(matInfo.textureObject);
                            string texName = getObjNameOrPtrName(texNamePtr, matInfo.textureObject);
                            copyIndex = texName.IndexOf(".Copy");
                            if (copyIndex > 0)
                              texName = texName.Substring(0, copyIndex);

                            //Try find our texture
                            Texture tex = texCallback(texName);
                            if (tex == null)
                            {
                              //Try get from cache
                              texturePool.TryGetValue(texName, out tex);
                              if (tex == null)
                              {
                                //Otherwise, read texture from virtools
                                IntPtr texInfoPtr = VirtoolsLoaderApi.Loader_SolveNmoFileTexture(matInfo.textureObject);
                                Loader_TextureInfo texInfo = (Loader_TextureInfo)Marshal.PtrToStructure(texInfoPtr, typeof(Loader_TextureInfo));
                                
                                /*Debug.Log(
                                  "Texture: " + texName +
                                  "\n" + texInfo.width + "x" + texInfo.height + "\nbufferSize: " + texInfo.bufferSize +
                                  "\nvideoPixelFormat: " + texInfo.videoPixelFormat
                                );*/

                                if (texInfo.bufferSize > 0 && texInfo.width > 0 && texInfo.height > 0)
                                {
                                  //Instance texture
                                  Texture2D texVt = new Texture2D(texInfo.width, texInfo.height, TextureFormat.ARGB32, false);
                                  texVt.name = texName;
                                  //Save to pool
                                  texturePool[texName] = texVt;
                                  result.textureList[texName] = texVt;
                                  //Read buffer
                                  IntPtr dataBuffer = Marshal.AllocHGlobal(texInfo.bufferSize);
                                  VirtoolsLoaderApi.Loader_DirectReadCKTextureData(matInfo.textureObject, texInfoPtr, dataBuffer);

                                  //Read pixes
                                  for (int x = 0; x < texInfo.width; x++)
                                  {
                                    for (int y = 0; y < texInfo.height; y++)
                                    {
                                      uint color = (uint)Marshal.ReadInt32(dataBuffer, (x + y * texInfo.width) * 4);
                                      texVt.SetPixel(x, y, new Color32(
                                        (byte)((color & 0x00ff0000) >> 16),
                                        (byte)((color & 0x0000ff00) >> 8),
                                        (byte)((color & 0x000000ff) >> 0),
                                        (byte)((color & 0xff000000) >> 24)
                                      ));
                                    }
                                  }

                                  //LoadRawTextureData has some problems, so do not use it
                                  //byte[] dateBufferManaged = new byte[texInfo.bufferSize];
                                  //Marshal.Copy(dataBuffer, dateBufferManaged, 0, texInfo.bufferSize);
                                  //texVt.LoadRawTextureData(dateBufferManaged);

                                  texVt.Apply();
                                  tex = texVt;

                                  Marshal.FreeHGlobal(dataBuffer);
                                }

                                VirtoolsLoaderApi.Loader_Free(texInfoPtr);
                              }
                            }

                            baseMat.mainTexture = tex;
                            baseMat.SetTexture("_MainTex", tex);
                          }
                        }
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
                objName = getObjNameOrPtrName(objNamePtr, objPtr);

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
                    groupName.Add(getObjNameOrPtrName(groupObjNamePtr, objGroupPtr, true));//添加名称到组
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

        texturePool.Clear();
        materialPool.Clear();

        return result;
      } catch (Exception e) {
        errorCallback(e.ToString());
        return null;
      } finally {

        //Close file
        VirtoolsLoaderApi.Loader_SolveNmoFileDestroy(nmoFile);

        Debug.Log("Nmo file closed");
      }
    }

#else 
    public static bool Init(string ck2DllPath)
    {
      throw new System.Exception("Not implemented");
    }
    public static bool Destroy()
    {
      throw new System.Exception("Not implemented");
    }
    public static VirtoolsLoaderLoadNMOResult LoadNMOToScense(string fileFullPath, 
      LoadNMOToScenseMaterialCallback matCallback,
      LoadNMOToScenseTextureCallback texCallback, 
      Shader materialShader)
    {
      throw new System.Exception("Not implemented");
    }
#endif

    public delegate Material LoadNMOToScenseMaterialCallback(string matName);
    public delegate Texture LoadNMOToScenseTextureCallback(string texName);
    public delegate void LoadNMOToScenseErrorCallback(string err);

    public class VirtoolsLoaderLoadNMOResult
    {
      public GameObject mainObj;
      public List<GameObject> objectList = new List<GameObject>();
      public Dictionary<string, GameObject> objectNameList = new Dictionary<string, GameObject>();
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
