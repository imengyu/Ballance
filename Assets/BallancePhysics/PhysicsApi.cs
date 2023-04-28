using System;
using System.Runtime.InteropServices;
using BallancePhysics.Api;
using static BallancePhysics.Api.ApiStruct;
using UnityEngine;

namespace BallancePhysics
{
  public static class PhysicsApi
  {
    #region 基础定义

#if UNITY_EDITOR && UNITY_EDITOR_WIN
    private const string DLL_NNAME = "bphysics_unity";
#elif UNITY_EDITOR
    private const string DLL_NNAME = "bphysics";
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
    private const string DLL_NNAME = "libbphysics";
#elif UNITY_IPHONE
	  private const string DLL_NNAME = "_Internal";
#else
    private const string DLL_NNAME = "bphysics";
#endif

    public const int sError = 0;
    public const int sWarning = 1;
    public const int sInfo = 2;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int ErrorReportCallback(int level, int len, IntPtr msg);

    public delegate void InitFinishCallback();

    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ballance_physics_entry(int init, IntPtr options);

    public static InitFinishCallback InitFinish;

    public const int sTrue = 1;
    public const int sFalse = 0;

    public static int boolToSbool(bool b) {
      return b ? sTrue : sFalse;
    }

    #endregion

    /// <summary>
    /// 所有的API
    /// </summary>
    /// <returns></returns>
    public static ApiStruct API { get; } = new ApiStruct();
    /// <summary>
    /// 密钥
    /// </summary>
    /// <value></value>
    public static string SecretKey { get; set; }

    public static void PhysicsApiInit()
    {
      Debug.Log("PhysicsApiInit");

      IntPtr apiStructPtr = IntPtr.Zero;

      //检查是否已经初始化
      if(ballance_physics_entry(4, IntPtr.Zero).ToInt64() == 1) {
        
        //如果已经调用初始化，则需要重新生成函数指针，否则unity editor中运行会被释放
        apiStructPtr = ballance_physics_entry(5, IntPtr.Zero);

        //获取所有函数指针
        API.initAll(apiStructPtr, 256);

      } else {

        //拷贝初始配置结构
        sInitStruct initStruct = new sInitStruct();
        initStruct.smallPoolSize = PhysicsOptions.Instance.SmallPoolSize;
#if UNITY_EDITOR
        initStruct.showConsole = boolToSbool(PhysicsOptions.Instance.ShowConsole);
#else
        initStruct.showConsole = boolToSbool(false);
#endif
        initStruct.key = SecretKey;

        IntPtr initStructPtr = Marshal.AllocHGlobal(Marshal.SizeOf<sInitStruct>());
        Marshal.StructureToPtr(initStruct, initStructPtr, false);

        //调用初始化
        apiStructPtr = ballance_physics_entry(sTrue, initStructPtr);

        //获取所有函数指针
        API.initAll(apiStructPtr, 256);
        
        //释放
        Marshal.FreeHGlobal(initStructPtr);
      }
    }
    public static void PhysicsApiDestroy()
    {
      Debug.Log("PhysicsApiDestroy");

      ballance_physics_entry(sFalse, IntPtr.Zero);
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Ballance/Physics/Open Console")]
    public static void OpenConsole() {
      ballance_physics_entry(3, IntPtr.Zero);
    }
    [UnityEditor.MenuItem("Ballance/Physics/Close Console")]
    public static void CloseConsole() {
      ballance_physics_entry(2, IntPtr.Zero);
    }
#endif
  }
}