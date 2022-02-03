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

#if UNITY_EDITOR
    private const string DLL_NNAME = "bphysics_unity";
#elif UNITY_IPHONE
	  private const string DLL_NNAME = "_Internal";
#else
    private const string DLL_NNAME = "bphysics";
#endif

    public const int sError = 0;
    public const int sWarning = 1;
    public const int sInfo = 2;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ErrorReportCallback(int level, int len, IntPtr msg);

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

        //BUG. 不这样放，mono 生成的指针似乎不正确，c++那边回调会出现问题
        ErrorReportCallback callback = (int level, int len, IntPtr _msg) =>
        {
          string msg = Marshal.PtrToStringAnsi(_msg, len);
          if (level == sInfo)
            Debug.Log(msg);
          else if (level == sWarning)
            Debug.LogWarning(msg);
          else if (level == sError)
            Debug.LogError(msg);
          else 
            Debug.Log(msg);
          return 1;
        };

        //已经初始化过，则只需要更新回调函数
        apiStructPtr = ballance_physics_entry(5, Marshal.GetFunctionPointerForDelegate(callback));

        //获取所有函数指针
        API.initAll(apiStructPtr, 256);
      } else {

        //BUG. 不这样放，mono 生成的指针似乎不正确，c++那边回调会出现问题
        ErrorReportCallback callback = (int level, int len, IntPtr _msg) =>
        {
          string msg = Marshal.PtrToStringAnsi(_msg, len);
          if (level == sInfo)
            Debug.Log(msg);
          else if (level == sWarning)
            Debug.LogWarning(msg);
          else if (level == sError)
            Debug.LogError(msg);
          else 
            Debug.Log(msg);
          return 1;
        };

        //拷贝初始配置结构
        sInitStruct initStruct = new sInitStruct();
        initStruct.smallPoolSize = PhysicsOptions.Instance.SmallPoolSize;
#if UNITY_EDITOR
        initStruct.showConsole = boolToSbool(PhysicsOptions.Instance.ShowConsole);
#else
        initStruct.showConsole = boolToSbool(false);
#endif
        initStruct.eventCallback = Marshal.GetFunctionPointerForDelegate(callback);
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