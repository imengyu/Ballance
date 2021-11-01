using Ballance2.Utils;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugBeginStats.cs
* 
* 用途：
* 游戏刚开始Debug模块还未加载时，此脚本负责显示输出日志。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.Debug
{
    public class DebugBeginStats : MonoBehaviour 
    {
        private Text text;

        private void Start() {
            text = GetComponent<Text>();
            currentLogObserver = Log.RegisterLogObserver((LogLevel level, string tag, string message, string stackTrace) => {
                text.text = string.Format("<color=#{0}>{1}/{2} {3}</color>", GetLogColor(level), level, tag, SubstractMessage(message));
            }, LogLevel.All);
        }
        private string GetLogColor(LogLevel level) {
            switch(level) {
                case LogLevel.Info: return "67CCFF";
                case LogLevel.Verbose: return "FFFFFF";
                case LogLevel.Warning: return "FFCE00";
                case LogLevel.Error: return "FF1B00";
                default: return "CFCFCF";
            }
        }
        private string SubstractMessage(string message) {
            var brPos = message.IndexOf('\n');
            var messageSb = "";
            if(brPos > 0) 
                messageSb = message.Substring(0, brPos - 1);
            else
                messageSb = message;       
            if(messageSb.Length > 256)
                messageSb = messageSb.Substring(0, 255);
            return messageSb;
        }
        private void OnDisable() {
            Log.UnRegisterLogObserver(currentLogObserver);
        }
        private int currentLogObserver = 0;
    }
}