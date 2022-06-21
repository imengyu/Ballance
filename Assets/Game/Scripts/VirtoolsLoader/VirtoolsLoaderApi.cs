

using System;
using System.Runtime.InteropServices;

namespace Ballance2 {

  /// <summary>
  /// 加载器的DLL API
  /// </summary>
  public class VirtoolsLoaderApi {
    private const string DLL_NNAME = "VirtoolsNMOLoader";

    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Loader_Init(IntPtr hWnd, [MarshalAs(UnmanagedType.LPStr)] string ck2fullPath);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern int Loader_Destroy();
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Loader_Free(IntPtr ptr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern int Loader_GetLastError();
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Loader_SolveNmoFileReset(IntPtr filePtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFileGetNext(IntPtr filePtr, IntPtr/* int* */ classIdOut, IntPtr/* char* */ nameOutBuffer);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern void Loader_SolveNmoFileDestroy(IntPtr filePtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFileRead([MarshalAs(UnmanagedType.LPStr)] string filePath, IntPtr /* int* */ outErrCode);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern int Loader_CKGroupGetObjectCount(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_CKGroupGetObject(IntPtr objPtr, int pos, IntPtr/* int* */ classIdOut, IntPtr/* char* */ nameOutBuffer);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern int Loader_CK3dEntityGetMeshCount(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_CKMeshGetMaterialObj(IntPtr objPtr, int index, IntPtr/* int* */ outMaterialFacesCount, IntPtr/* void** */ outMaterialFacesPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_CK3dEntityGetMeshObj(IntPtr objPtr, int index);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_CKObjectGetName(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFileMesh(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFile3dEntity(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFileMaterial(IntPtr objPtr, IntPtr nmoFilePtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFileTexture(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern void Loader_DirectReadCKMeshData(IntPtr objPtr, IntPtr/* float* */ vertices, IntPtr/* int* */ triangles, IntPtr/* float* */ normals, IntPtr/* float** */ uvs);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)]
    public static extern void Loader_DirectReadCKTextureData(IntPtr objPtr, IntPtr/* Loader_TextureInfo* */ info, IntPtr/* unsigned char*  */dataBuffer);
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  public struct Loader_MaterialInfo
  {
    public float power;
    public float ambientR;
    public float ambientG;
    public float ambientB;
    public float ambientA;
    public float diffuseR;
    public float diffuseG;
    public float diffuseB;
    public float diffuseA;
    public float specularR;
    public float specularG;
    public float specularB;
    public float specularA;
    public float emissiveR;
    public float emissiveG;
    public float emissiveB;
    public float emissiveA;
    public IntPtr textureObject;
  }
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  public struct Loader_TextureInfo
  {
    public int width;
    public int height;
    public int videoPixelFormat;
    public int bufferSize;
  }  
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  public struct Loader_MeshInfo
  {
    public int vertexCount;
    public int faceCount;
    public int channelCount;
    public int materialCount;
  }  
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  public struct Loader_3dEntityInfo
  {
    public float positionX;
    public float positionY;
    public float positionZ;
    public float quaternionX;
    public float quaternionY;
    public float quaternionZ;
    public float quaternionW;
    public float eulerX;
    public float eulerY;
    public float eulerZ;
    public float scaleX;
    public float scaleY;
    public float scaleZ;
	  public int meshCount;
  }
}
