using System.Collections.Generic;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.InputManager;
using Ballance2.UI.Utils;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* Copyright(c) 2022  mengyu
*
* 模块名：     
* DebugConsole.cs
* 
* 用途：
* 调试控制台逻辑管理。
*
* 作者：
* mengyu
*/

namespace Ballance2.DebugTools 
{
  public class DebugConsole : MonoBehaviour 
  {
    public ScrollRect ConsoleScrollView;
    public RectTransform ConsoleContainerContent;
    public Button ExecButton;
    public TMP_InputField CommandInputField;
    public GameObject DebugLogItemPrefab;
    public Toggle CheckBoxAutoScroll;
    public Toggle CheckBoxWarningAndErr;
    public Toggle CheckBoxMessages;
    public TMP_InputField FilterLogInputField;
    public TMP_Text CommandHelpText;

    private struct LogItem
    {
      public GameObject go;
      public string message;
      public LogLevel level;
      public string stackTrace;
    }
    private int logObserver = 0;
    private bool logAutoScroll = true;
    private bool logShowErrAndWarn = true;
    private bool logShowMessage = true;
    private List<LogItem> logItems = new List<LogItem>();
    private bool logForceDisable = true;
    private int logMaxCount = 256;
    private string logFilter = "";

    private int commandHistoryIndex = 0;
    private List<string> commandHistory = new List<string>();
    private GameDebugCommandServer commandServer = null;

    private void Start()
    {
      // 注册日志观察者
      logObserver = Log.RegisterLogObserver((level, tag, message, stackTrace) =>
      {
        if (logForceDisable)
          return;

        if (logItems.Count > logMaxCount)
        {
          var nitem = logItems[0];
          logItems.RemoveAt(0);
          Destroy(nitem.go);
        }

        if (message.Length > 8192)
          message = message.Substring(0, 8192) + "...";

        var logColor = Log.GetLogColor(level);
        var t = string.Format("<color=#999>{1}/{2}</color> <color=#{0}>{3}</color>{4}", 
          Log.GetLogColor(level), 
          Log.LogLevelToString(level), 
          tag, 
          message, 
          level >= LogLevel.Warning ? $"\n{stackTrace}" : ""
        );

        var newEle = CloneUtils.CloneNewObjectWithParent(DebugLogItemPrefab, ConsoleContainerContent);
        var text = newEle.GetComponent<TMP_Text>();
        text.text = t;
        UIAnchorPosUtils.SetUIAnchor((RectTransform)newEle.transform, UIAnchor.Left, UIAnchor.Top);
        UIAnchorPosUtils.SetUIPivot((RectTransform)newEle.transform, UIPivot.TopLeft);

        var item = new LogItem
        {
          go = newEle,
          message = message,
          level = level,
          stackTrace = stackTrace
        };
        logItems.Add(item);

        //重新布局
        RelayoutLogContent();

        text.gameObject.SetActive(true);
        //滚动到末尾
        if (logAutoScroll)
          ConsoleScrollView.normalizedPosition = Vector2.zero;
      }, LogLevel.All);

      logForceDisable = false;

      //命令服务的准备
      commandServer = GameManager.Instance.GameDebugCommandServer;
      commandServer.RegisterCommand("clear", (keyword, fullCmd, argsCount, args) => {
        ClearCommand();
        commandHistory.Clear();
        commandHistoryIndex = 0;
        return true;
      }, 0, "clear 清空控制台");

      //发送没有抓取到的日志
      Log.SendLogsInTemporary();

      commandHistory.Add("");
      /*TODO: commandInputListenKey1 = GameUIManager.ListenKey(KeyCode.UpArrow, (KeyCode key, bool downed) => {
        if(downed && CommandInputField.isFocused) UpCommandHistory();
      });
      commandInputListenKey2 = GameUIManager.ListenKey(KeyCode.DownArrow, (KeyCode key, bool downed) => {
        if(downed && CommandInputField.isFocused) DownCommandHistory();
      });*/
    }
    private void OnDestroy()
    {
      logForceDisable = true;
      Log.UnRegisterLogObserver(logObserver);
      /*if(GameUIManager != null) {
        GameUIManager.DeleteKeyListen(commandInputListenKey1);
        GameUIManager.DeleteKeyListen(commandInputListenKey2);
      }*/
    }

    //布局日志界面
    private void RelayoutLogContent()
    {
      var transform = ConsoleContainerContent;
      var logFilterEmpty = logFilter == "";
      float h = 0.0f, w = transform.sizeDelta.x;
      for (var i = 0; i < transform.childCount; i++)
      {
        var child = transform.GetChild(i) as RectTransform;
        if(i >= logItems.Count)
          break;
        var log = logItems[i];
        if (
          (logShowErrAndWarn && (log.level == LogLevel.Warning || log.level == LogLevel.Error)) ||
          (logShowMessage && !(log.level == LogLevel.Warning || log.level == LogLevel.Error)) &&
          (logFilterEmpty || log.message.Contains(logFilter))
        )
        {
          child.gameObject.SetActive(true);
          child.anchoredPosition = new Vector2(5, -h);
          if (child.sizeDelta.x > w) 
            w = child.sizeDelta.x;
          h = h + LayoutUtility.GetPreferredSize(child, 1);
        }
        else
          child.gameObject.SetActive(false);
      }
      transform.sizeDelta = new Vector2(w, h < 200 ? 200 : h);
    }
    //清空日志条目
    private void ClearLogContent()
    {
      var transform = ConsoleContainerContent;
      var c = transform.childCount;
      for (var i = c - 1; i >= 0; i--)
        Destroy(transform.GetChild(i).gameObject);
      transform.sizeDelta = new Vector2(transform.sizeDelta.x, 300);
    }
    
    //AutoScroll更改
    public void OnAutoScrollChanged()
    {
      logAutoScroll = CheckBoxAutoScroll.isOn;
    }
    //显示错误Checkbox更改事件
    public void OnShowErrCheckChange()
    {
      logShowErrAndWarn = CheckBoxWarningAndErr.isOn;
      RelayoutLogContent();
      SwitchCommandHelp(false);
    }
    //显示信息Checkbox更改事件
    public void OnShowMessagesCheckChange()
    {
      logShowMessage = CheckBoxMessages.isOn;
      RelayoutLogContent();
      SwitchCommandHelp(false);
    }
    //筛选Input输入完成事件
    public void OnFilterInputEndInput()
    {
      logFilter = FilterLogInputField.text;
      RelayoutLogContent();
      SwitchCommandHelp(false);
    }
    
    private void UpCommandHistory() {
      if(commandHistoryIndex == 0) {
        commandHistory[0] = CommandInputField.text;
        commandHistoryIndex = commandHistory.Count - 1;
      } else {
        if(commandHistoryIndex > 1) {
          commandHistory[commandHistoryIndex] = CommandInputField.text;
          commandHistoryIndex--;
          CommandInputField.text = commandHistory[commandHistoryIndex];
        }
      }
    }    
    private void DownCommandHistory() {
      if(commandHistoryIndex > 0) {
        if(commandHistoryIndex < commandHistory.Count - 1) {
          commandHistory[commandHistoryIndex] = CommandInputField.text;
          commandHistoryIndex ++;
          CommandInputField.text = commandHistory[commandHistoryIndex];
        } else {
          commandHistoryIndex = 0;
          CommandInputField.text = commandHistory[0];
        }
      }
    }
    private void SwitchCommandHelp(bool show) {
      if (show) {
        CommandHelpText.gameObject.SetActive(true);
        ConsoleScrollView.gameObject.SetActive(false);
      } else {
        CommandHelpText.gameObject.SetActive(false);
        ConsoleScrollView.gameObject.SetActive(true);
      }
    }
    private void GenCommandHelp(string text) {
      //生成指令帮助
      CommandHelpText.text = commandServer.GenCommandHelp(text);
    }

    //聚焦输入框
    public void OnConsoleShow() {
      RelayoutLogContent();
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(CommandInputField.gameObject);
    }
    //Input输入事件
    public void OnCommandInputFieldChange() {
      string text = CommandInputField.text;
      if (text.StartsWith('/')) {
        SwitchCommandHelp(true);
        GenCommandHelp(text.Substring(1));
      } else {
        SwitchCommandHelp(false);
      }
    }
    //强制GC
    public void ForceGC() {
      System.GC.Collect();
      Log.D("Debug", "GC Finish");
    }
    //执行命令
    public void ExecCommand()
    {
      string enter = CommandInputField.text;
      if (string.IsNullOrEmpty(enter))
        return;
      if (enter.StartsWith('/')) 
      {
        if (commandServer.ExecuteCommand(enter.Substring(1)))
        {
          commandHistory.Add(CommandInputField.text);
          commandHistoryIndex = 0;
          CommandInputField.text = "";
          SwitchCommandHelp(false);
        }
      } 
      else 
      {
        Log.I("User", enter);
        CommandInputField.text = "";
        SwitchCommandHelp(false);
      }
    }
    //清空命令窗口
    public void ClearCommand()
    {
      //清空日志条目
      ClearLogContent();
      GameManager.GameMediator.NotifySingleEvent("DebugToolsClear");
      //重新布局
      RelayoutLogContent();
    }
    //关闭命令窗口
    public void CloseWindow()
    {
      DebugManager.SwitchConsoleVisible();
    }
  }
}