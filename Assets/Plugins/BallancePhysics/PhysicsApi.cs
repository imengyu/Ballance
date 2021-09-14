using System;
using System.Runtime.InteropServices;
using BallancePhysics.Api;
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

    private const CallingConvention _CallingConvention = CallingConvention.Cdecl;

    public delegate void ErrorReportCallback([MarshalAs(UnmanagedType.LPStr)] string msg);
    public delegate void InitFinishCallback();

    [DllImport(DLL_NNAME, CallingConvention = _CallingConvention)]
    private static extern IntPtr entry(int init, IntPtr _eventCallback);

    public static InitFinishCallback InitFinish;

    private static void ErrorReport(string msg)
    {
      if (msg.Contains("Report"))
        Debug.Log(msg);
      else if (msg.Contains("Warn") || msg.Contains("WARN"))
        Debug.LogWarning(msg);
      else
        Debug.LogError(msg);
    }

    public const int sTrue = 1;
    public const int sFalse = 1;

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
      //调用初始化
      IntPtr apiStructPtr = entry(sTrue, Marshal.GetFunctionPointerForDelegate(callback));
      //获取所有函数指针
      API.initAll(apiStructPtr, 256);
    }
    public static void PhysicsApiDestroy()
    {
      entry(sFalse, IntPtr.Zero);
    }
  }
}