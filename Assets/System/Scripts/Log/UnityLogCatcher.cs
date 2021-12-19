using Ballance2.Utils;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* UnityLogCatcher.cs
* 
* 用途：
* Unity日志捕捉器，用于捕捉Unity日志并将其显示至游戏内控制台。
*
* 作者：
* mengyu
*/

namespace Ballance2.Log.Utils
{
  class UnityLogCatcher
  {
    public static void Init()
    {
      Application.logMessageReceived += Application_logMessageReceived;
    }
    public static void Destroy()
    {
      Application.logMessageReceived -= Application_logMessageReceived;
    }

    private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
      switch (type)
      {
        case LogType.Error: Log.LogWrite(LogLevel.Error, "Application", condition, stackTrace); break;
        case LogType.Assert: Log.LogWrite(LogLevel.Error, "Assert", condition, stackTrace); break;
        case LogType.Warning: Log.LogWrite(LogLevel.Warning, "Application", condition, stackTrace); break;
        case LogType.Log: Log.LogWrite(LogLevel.Debug, "Application", condition, stackTrace); break;
        case LogType.Exception: Log.LogWrite(LogLevel.Error, "Exception", condition, stackTrace); break;
      }
    }
  }
}
