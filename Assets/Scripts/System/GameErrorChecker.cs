using Ballance2.System.Debug;
using Ballance2.UI.Parts;
using Ballance2.Utils;
using System.Text;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameErrorChecker.cs
* 
* 用途：
* 整个游戏的入口
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-14 创建
*
*/

namespace Ballance2.System
{
    /// <summary>
    /// 错误检查器
    /// </summary>
    [SLua.CustomLuaClass]
    public class GameErrorChecker
    {
        private static GameGlobalErrorUI gameGlobalErrorUI;

        internal static void SetGameErrorUI(GameGlobalErrorUI errorUI)
        {
            gameGlobalErrorUI = errorUI;
        }

        /// <summary>
        /// 抛出游戏异常，此操作会直接终止游戏
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public static void ThrowGameError(GameError code, string message) 
        {
            StringBuilder stringBuilder = new StringBuilder("错误代码：");
            stringBuilder.Append(code.ToString());
            stringBuilder.Append("\n");
            stringBuilder.Append(string.IsNullOrEmpty(message) ? GameErrorInfo.GetErrorMessage(code) : message);
            stringBuilder.Append("\n");
            stringBuilder.Append(DebugUtils.GetStackTrace(1));

            GameSystem.ForceInterruptGame();
            gameGlobalErrorUI.ShowErrorUI(stringBuilder.ToString());
        }

        /// <summary>
        /// 上一个操作的错误
        /// </summary>
        public static GameError LastError { get; set; }
        /// <summary>
        /// 上一个操作的错误说明文字
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
        /// <param name="tag">TAG</param>
        /// <param name="message">错误信息</param>
        /// <param name="param">日志信息</param>
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
    }
}
