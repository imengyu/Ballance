using Ballance2.Entry;
using Ballance2.UI.CoreUI;
using Ballance2.Utils;
using System.Collections.Generic;
using System.Text;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameErrorChecker.cs
* 
* 用途：
* 错误检查器。
* 使用错误检查器获取游戏API的调用错误。
* 错误检查器还可负责弹出全局错误窗口以检查BUG.
*
* 作者：
* mengyu
*/

namespace Ballance2.Services.Debug
{
  /// <summary>
  /// 错误检查器。使用错误检查器获取游戏API的调用错误。
  /// </summary>
  [JSExport]
  public class GameErrorChecker
  {
    private static GameGlobalErrorUI gameGlobalErrorUI;
    private static List<string> gameErrorLogPools;
    private static int gameErrorLogObserver = 0;

    internal static void SetGameErrorUI(GameGlobalErrorUI errorUI)
    {
      gameGlobalErrorUI = errorUI;
    }

    /// <summary>
    /// 抛出游戏异常，此操作会直接停止游戏。类似于 Windows 蓝屏功能。
    /// </summary>
    /// <param name="code">错误代码</param>
    /// <param name="message">关于错误的异常信息</param>
    public static void ThrowGameError(GameError code, string message)
    {
      StringBuilder stringBuilder = new StringBuilder("错误代码：");
      stringBuilder.Append(code.ToString());
      stringBuilder.Append("\n");
      stringBuilder.Append(string.IsNullOrEmpty(message) ? GameErrorInfo.GetErrorMessage(code) : message);
      stringBuilder.Append("\n");
      stringBuilder.Append(DebugUtils.GetStackTrace(1));
      stringBuilder.Append("\n");

      if(gameErrorLogPools != null && gameErrorLogPools.Count > 0) {
        stringBuilder.Append("Errors:\n");
        foreach (var item in gameErrorLogPools)
        {
          stringBuilder.Append(item);
          stringBuilder.Append("\n");
        }
      }

      GameSystem.ForceInterruptGame();
      gameGlobalErrorUI.ShowErrorUI(stringBuilder.ToString());
      UnityEngine.Debug.LogError(stringBuilder.ToString());
    }

    /// <summary>
    /// 获取或设置上一个操作的错误
    /// </summary>
    public static GameError LastError { get; set; }
    /// <summary>
    /// 获取上一个操作的错误说明文字
    /// </summary>
    /// <returns></returns>
    public static string GetLastErrorMessage()
    {
      return GameErrorInfo.GetErrorMessage(LastError);
    }


    internal static void Init() {
      gameErrorLogPools = new List<string>();
      gameErrorLogObserver = Log.RegisterLogObserver(LogObserver, LogLevel.Error | LogLevel.Warning);
    }
    internal static void Destroy() {
      gameErrorLogPools.Clear();
      gameErrorLogPools = null;
      Log.UnRegisterLogObserver(gameErrorLogObserver);
    }

    private static void LogObserver(LogLevel level, string tag, string message, string stackTrace) {
      if(gameErrorLogPools.Count > 10)
        gameErrorLogPools.RemoveAt(0);
      StringBuilder sb = new StringBuilder();
      sb.Append('[');
      sb.Append(tag);
      sb.Append("] ");
      sb.Append(message);
      sb.Append('\n');
      sb.Append(stackTrace);
      gameErrorLogPools.Add(sb.ToString());
    }

    /// <summary>
    /// 设置错误码并打印日志
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="tag">日志标签</param>
    /// <param name="message">日志信息格式化字符串</param>
    /// <param name="param">日志信息格式化参数</param>
    public static void SetLastErrorAndLog(GameError code, string tag, string message, params object[] param)
    {
      LastError = code;
      Log.E(tag, message, param);
    }
    /// <summary>
    /// 设置错误码并打印日志
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="tag">TAG</param>
    /// <param name="message">错误信息</param>
    public static void SetLastErrorAndLog(GameError code, string tag, string message)
    {
      LastError = code;
      Log.E(tag, message);
    }

    /// <summary>
    /// 显示系统错误信息提示提示对话框
    /// </summary>
    /// <param name="message">错误信息</param>
    public static void ShowSystemErrorMessage(string message)
    {
      GameEntry entry = GameEntry.Instance;
      if(entry) {
        entry.GlobalGameSysErrMessageDebuggerTipDialogText.text = message;
        entry.GlobalGameSysErrMessageDebuggerTipDialog.SetActive(true);
      }
    }
  }
}
