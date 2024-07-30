using Ballance2;
using Ballance2.Base;
using Ballance2.Config;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.UI.Core;
using Ballance2.Utils;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugStat.cs
* 
* 用途：
* 调试信息显示工具类。
*
* 作者：
* mengyu
*/


namespace Ballance2.DebugTools 
{
  class DebugStat : MonoBehaviour
  {
    public static DebugStat Instance { get; private set; }

    public TMP_Text StatText = null;

    private StringBuilder sb = new StringBuilder();

    private int logCollectTick = 100;
    private int logObserver = 0;
    private bool logForceDisable = true;

    private Text DebugStatsText;
    private Text DebugSysinfoText;

    private GameUIManager GameUIManager;

    private void Start()
    {
      Instance = this;
      logForceDisable = false;

      var SystemPackage = GamePackage.GetSystemPackage();
      GameUIManager = GameManager.GetSystemService<GameUIManager>();
      StatText.text = "";

      GameManager.GameMediator.SubscribeSingleEvent(SystemPackage, "DebugToolsClear", "DebugStat", (evtName, param) => {
        StatText.text = "";
        return false;
      });

      //Log
      logObserver = Log.RegisterLogObserver((LogLevel level, string tag, string message, string stackTrace) => AppendLoadToStat(level, tag, message), LogLevel.All);
    }
    private void OnDestroy()
    {
      logForceDisable = true;
      if (logObserver > 0) {
        Log.UnRegisterLogObserver(logObserver);
        logObserver = 0;
      }
    }
    private void Update() {
      if (logStatItems.Count > 0) {
        if (logCollectTick > 0)
          logCollectTick--;
        else
          logCollectTick = 100;

        if (logCollectTick == 100) {
          logStatItems.RemoveAt(0);
          FlushLogText();
        } 
      }
    }

    private List<string> logStatItems = new List<string>();

    private void FlushLogText() {
      StringBuilder sb = new StringBuilder();
      foreach(var s in logStatItems) 
        sb.AppendLine(s);
      StatText.text = sb.ToString();
    }
    private void AppendLoadToStat(LogLevel level, string tag, string message) {
      if (logForceDisable)
        return;
      if(logStatItems.Count >= 10) 
        logStatItems.RemoveAt(0);

      string str = string.Format("<color=#ccc>{1}/{2}</color> <color=#{0}>{3}</color>", Log.GetLogColor(level), Log.LogLevelToString(level), tag, message.Replace('\n', ' ').Substring(0, message.Length > 128 ? 128 : message.Length));
      logStatItems.Add(str);
      FlushLogText();
    }
  }
}
