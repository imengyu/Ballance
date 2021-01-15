using Ballance2.Utils;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* UnityLogCatcher.cs
* 
* 用途：
* Unity日志捕捉器，用于捕捉Unity日志并将其显示至控制台
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-15 创建
*
*/

namespace Ballance2.System.Utils
{
    class UnityLogCatcher
    {
        private static int logObserverId = 0;
        private static bool logLock = false;

        public static void Init()
        {
            Application.logMessageReceived += Application_logMessageReceived;
            logObserverId = Log.RegisterLogObserver(LogObserver, LogLevel.All);
        }
        public static void Destroy()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
            Log.UnRegisterLogObserver(logObserverId);
        }

        private static void LogObserver(LogLevel level, string tag, string message, string stackTrace)
        {
#if UNITY_EDITOR
            string logTagStr;
            switch (level)
            {
                default:
                case LogLevel.Verbose: logTagStr = "V";  break;
                case LogLevel.Debug: logTagStr = "D"; break;
                case LogLevel.Info: logTagStr = "I"; break;
                case LogLevel.Warning: logTagStr = "W"; break;
                case LogLevel.Error: logTagStr = "E"; break;
            }
            string finalMessage = string.Format("{0}/{1} {2}", logTagStr, tag, message);
            logLock = true;
            switch (level)
            {
                default:
                case LogLevel.Verbose: UnityEngine.Debug.Log(finalMessage); break;
                case LogLevel.Debug: UnityEngine.Debug.Log(finalMessage); break;
                case LogLevel.Info: UnityEngine.Debug.Log(finalMessage); break;
                case LogLevel.Warning: UnityEngine.Debug.LogWarning(finalMessage); break;
                case LogLevel.Error: UnityEngine.Debug.LogError(finalMessage); break;
            }
#endif
        }
        private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            //如果已经锁定，则返回，防止unitylog循环输出
            if(logLock)
            {
                logLock = false;
                return;
            }
            //输出到Logger
            switch (type)
            {
                case LogType.Error: Log.LogWrite(LogLevel.Error, "Application", condition, stackTrace); break;
                case LogType.Assert: Log.LogWrite(LogLevel.Error, "Assert", condition, stackTrace); break;
                case LogType.Warning: Log.LogWrite(LogLevel.Error, "Application", condition, stackTrace); break;
                case LogType.Log: Log.LogWrite(LogLevel.Error, "Application", condition, stackTrace); break;
                case LogType.Exception: Log.LogWrite(LogLevel.Error, "Exception", condition, stackTrace); break;
            }
        }
    }
}
