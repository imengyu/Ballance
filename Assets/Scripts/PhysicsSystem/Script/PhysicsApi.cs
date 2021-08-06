using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PhysicsRT
{
    public static class PhysicsApi
    {
        #region 基础定义
        
#if UNITY_IPHONE && !UNITY_EDITOR
	    private const string DLL_NNAME = "_Internal";
#else
        private const string DLL_NNAME = "Physics_RT";
#endif

        private const CallingConvention _CallingConvention = CallingConvention.Cdecl;

        public delegate void ErrorReportCallback([MarshalAs(UnmanagedType.LPStr)] string msg);

        [DllImport(DLL_NNAME, CallingConvention = _CallingConvention)]
        private static extern IntPtr init(IntPtr pInitStruct);
        [DllImport(DLL_NNAME, CallingConvention = _CallingConvention)]
        private static extern bool quit();
        [DllImport(DLL_NNAME, CallingConvention = _CallingConvention)]
        public static extern int isInitSuccess();
        ///errBuffer: char*
        ///size: size_t->unsigned int
        [DllImport(DLL_NNAME, CallingConvention = _CallingConvention)]
        public static extern int checkException(IntPtr errBuffer, [MarshalAs(UnmanagedType.SysUInt)] uint size);

        public static string checkException() {
            if(checkException(assertMsgBuffer, 1024) > 0) 
                return Marshal.PtrToStringAnsi(assertMsgBuffer);
            return null;
        }

        private static void ErrorReport(string msg) {
            if(msg.Contains("Report"))
                Debug.Log(msg);
            else if(msg.Contains("Warn") || msg.Contains("WARN"))
                Debug.LogWarning(msg); 
            else
                Debug.LogError(msg);
        }

        private static IntPtr assertMsgBuffer = IntPtr.Zero;
 
        #endregion
        
        #region API定义

        /// <summary>
        /// 释放
        /// </summary>
        public static void PhysicsApiDestroy() {
            if(assertMsgBuffer != IntPtr.Zero) {
                Marshal.FreeHGlobal(assertMsgBuffer);
                assertMsgBuffer = IntPtr.Zero;
            }
            if(isInitSuccess() > 0) 
                quit();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void PhysicsApiInit() {
            //初始化
            sInitStruct intStruct = new sInitStruct();
            intStruct.mulithread = PhysicsOptions.Instance.EnableMultithreaded;
            intStruct.smallPoolSize = PhysicsOptions.Instance.SmallPoolSize;
            ErrorReportCallback callback = ErrorReport;
            intStruct.errCallback = Marshal.GetFunctionPointerForDelegate(callback);

            //拷贝结构体至托管内存
            int nSizeOfPerson = Marshal.SizeOf(intStruct);
            IntPtr intStructPtr = Marshal.AllocHGlobal(nSizeOfPerson);
            Marshal.StructureToPtr(intStruct, intStructPtr, false);

            //调用初始化
            IntPtr apiStructPtr =  init(intStructPtr);

            Marshal.FreeHGlobal(intStructPtr);
            
            //获取所有函数指针
            API.initAll(apiStructPtr, 256);

            if(assertMsgBuffer == IntPtr.Zero) 
                assertMsgBuffer = Marshal.AllocHGlobal(1024);
        }
        /// <summary>
        /// 所有的API
        /// </summary>
        /// <returns></returns>
        public static ApiStruct API { get; } = new ApiStruct();      
        
        #endregion
    }
}