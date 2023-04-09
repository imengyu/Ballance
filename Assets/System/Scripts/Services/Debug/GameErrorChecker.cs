using Ballance2.Entry;
using Ballance2.UI.CoreUI;
using Ballance2.Utils;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
  /// 错误检查器
  /// </summary>
  public class GameErrorChecker
  {
    private static GameGlobalErrorUI gameGlobalErrorUI;
    private static List<string> gameErrorLogPools;

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
      HideInternalObjects();
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

    private static void HideInternalObjects() {
      var GameGlobalMask = GameObject.Find("GameGlobalMask");
      if(GameGlobalMask != null)
        GameGlobalMask.SetActive(false);
      var GameGlobalIngameLoading = GameObject.Find("GameGlobalIngameLoading");
      if(GameGlobalIngameLoading != null)
        GameGlobalIngameLoading.SetActive(false);
    }

    /// <summary>
    /// 显示系统错误信息提示提示对话框
    /// </summary>
    /// <param name="message">错误信息</param>
    public static void ShowSystemErrorMessage(string message)
    {
      gameGlobalErrorUI.ShowErrorUI(message);
      HideInternalObjects();
    }
    /// <summary>
    /// 显示脚本错误信息提示提示对话框
    /// </summary>
    /// <param name="message">错误信息</param>
    public static void ShowScriptErrorMessage(string fileName, string packName, string message)
    {
      GameEntry entry = GameEntry.Instance;
      if(entry) {
        GameSystem.ForceInterruptGame();
        HideInternalObjects();

        string err = string.Format("代码异常：\n发生错误的文件：{0} 模块：{1}\n",fileName, packName) + message;
        entry.GlobalGameScriptErrDialog.Show(err);
        UnityEngine.Debug.LogError(err);
      }
    }
  }
}
