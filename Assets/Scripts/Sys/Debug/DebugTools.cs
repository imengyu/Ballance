using System.Collections.Generic;
using System.Text;
using Ballance2.Sys;
using Ballance2.Sys.Bridge;
using Ballance2.Sys.Package;
using Ballance2.Sys.Utils;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugTools.cs
* 
* 用途：
* 调试工具条控制类。
*
* 作者：
* mengyu
*/

namespace Ballance2
{
  public class DebugTools : MonoBehaviour
  {
    public Text DebugLogText = null;

    private int logObserver;
    private StringBuilder logStringBuffer = new StringBuilder();
    private List<int> logStringBrPos = new List<int>();

    private void Start()
    {
      var systemPackage = GamePackage.GetSystemPackage();

      // 注册日志观察者
      logObserver = Log.RegisterLogObserver((level, tag, message, stackTrace) => {
        if (logStringBrPos.Count >= 8)
        {
          logStringBuffer.Remove(0, logStringBrPos[0]);
          logStringBrPos.RemoveAt(0);
        }

        var logColor = Log.GetLogColor(level);
        var t = string.Format("<color=#{0}>{1}/{2} {3}</color>\n", logColor, Log.LogLevelToString(level), tag, SubstractMessage(message));
        logStringBrPos.Add(t.Length);
        logStringBuffer.Append(t);

        DebugLogText.text = logStringBuffer.ToString();
      }, LogLevel.All);

      DebugLogText.text = "";

      GameManager.GameMediator.SubscribeSingleEvent(systemPackage, "DebugToolsClear", "DebugTools", (evtName, par) => { Clear(); return false; });
    }
    private void OnDestroy()
    {
      if(GameManager.GameMediator != null)
        GameManager.GameMediator.UnRegisterSingleEvent("DebugToolsClear");
      Log.UnRegisterLogObserver(logObserver);
      Clear();
    }
    //清空
    private void Clear()
    {
      logStringBrPos.Clear();
      logStringBuffer.Clear();
      DebugLogText.text = "";
    }
    //日志文字提取
    private string SubstractMessage(string message)
    {
      var brPos = message.IndexOf('\n');
      var messageSb = "";
      if (brPos > 0)
        messageSb = message.Substring(0, brPos - 1);
      else
        messageSb = message;
      
      if (messageSb.Length > 256) 
        messageSb = messageSb.Substring(0, 255);
      
      return messageSb;
    }

  }
}