

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
    public static extern IntPtr Loader_CK3dEntityGetMeshObj(IntPtr objPtr, int index);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_CKObjectGetName(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFileMesh(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFile3dEntity(IntPtr objPtr);
    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)] 
    public static extern IntPtr Loader_SolveNmoFileMaterial(IntPtr objPtr);
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
    float power;
    [MarshalAs(UnmanagedType.R4, SizeConst=4)]
    float[] ambient;
    [MarshalAs(UnmanagedType.R4, SizeConst=4)]
    float[] diffuse;
    [MarshalAs(UnmanagedType.R4, SizeConst=4)]
    float[] specular;
    [MarshalAs(UnmanagedType.R4, SizeConst=4)]
    float[] emissive;
    IntPtr textureObject;
  }
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  public struct Loader_TextureInfo
  {
    int width;
    int height;
    int videoPixelFormat;
    int bufferSize;
  }  
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  public struct Loader_MeshInfo
  {
    int vertexCount;
    int faceCount;
    int channelCount;
  }  
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
  public struct Loader_3dEntityInfo
  {
    [MarshalAs(UnmanagedType.R4, SizeConst=3)]
    float[] position;
    [MarshalAs(UnmanagedType.R4, SizeConst=3)]
    float[] quaternion;
    [MarshalAs(UnmanagedType.R4, SizeConst=3)]
    float[] scale;
	  int meshCount;
  }
}
