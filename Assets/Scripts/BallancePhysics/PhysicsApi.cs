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
    private const string DLL_NNAME = "BallancePhysicsEditor";
#elif UNITY_IPHONE
	  private const string DLL_NNAME = "_Internal";
#else
    private const string DLL_NNAME = "BallancePhysics";
#endif

    public const int sError = 0;
    public const int sWarning = 1;
    public const int sInfo = 2;

    public delegate int ErrorReportCallback(int level, IntPtr msg);
    public delegate void InitFinishCallback();

    [DllImport(DLL_NNAME, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ballance_physics_entry(int init, IntPtr options);

    public static InitFinishCallback InitFinish;

    private static int ErrorReport(int level, IntPtr _msg)
    {
      string msg = Marshal.PtrToStringAnsi(_msg);
      if (level == sInfo)
        Debug.Log(msg);
      else if (level == sWarning)
        Debug.LogWarning(msg);
      else if (level == sError)
        Debug.LogError(msg);
      else 
        Debug.Log(msg);
      return 1;
    }

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

    public static void PhysicsApiInit()
    {
      ErrorReportCallback callback = ErrorReport;

      //拷贝初始配置结构
      sInitStruct initStruct = new sInitStruct();
      initStruct.smallPoolSize = PhysicsOptions.Instance.SmallPoolSize;
      initStruct.showConsole = boolToSbool(PhysicsOptions.Instance.ShowConsole);
      initStruct.eventCallback = Marshal.GetFunctionPointerForDelegate(callback);

      IntPtr initStructPtr = Marshal.AllocHGlobal(Marshal.SizeOf<sInitStruct>());
      Marshal.StructureToPtr(initStruct, initStructPtr, false);

      //调用初始化
      IntPtr apiStructPtr = ballance_physics_entry(sTrue, initStructPtr);

      //获取所有函数指针
      API.initAll(apiStructPtr, 256);
      
      //释放
      Marshal.FreeHGlobal(initStructPtr);
    }
    public static void PhysicsApiDestroy()
    {
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