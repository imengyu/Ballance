using System.Collections.Generic;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.InputManager;
using Ballance2.UI.Core.Controls;
using Ballance2.UI.Utils;
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
* 调试窗口控制类。
*
* 作者：
* mengyu
*/

namespace Ballance2
{
  public class DebugWindow : MonoBehaviour
  {
    public GameObject LogItemPrefab = null;
    public InputField CommandInputField = null;
    public InputField FilterLogInputField = null;
    public RectTransform LogContentView = null;
    public ScrollRect LogScrollView = null;
    public ScrollRect LogStacktraceScrollView = null;
    public RectTransform LogStacktraceView = null;
    public Text LogStacktraceText = null;
    public Toggle CheckBoxWarningAndErr = null;
    public Toggle CheckBoxMessages = null;
    public Toggle CheckBoxSysInfo = null;
    public Toggle CheckBoxMemInfo = null;
    public ToggleEx ToggleAutoScroll = null;

    private struct LogItem
    {
      public GameObject go;
      public string message;
      public LogLevel level;
      public string stackTrace;
    }
    private Font logFont;
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
    private int commandInputListenKey1;
    private int commandInputListenKey2;
    private GameUIManager GameUIManager;

    private void Start()
    {
      this.GameUIManager = GameManager.Instance.GetSystemService<GameUIManager>();
      this.logFont = GameStaticResourcesPool.FindStaticAssets("FontConsole") as Font;

      // 注册日志观察者
      this.logObserver = Log.RegisterLogObserver((level, tag, message, stackTrace) =>
      {
        if (this.logForceDisable)
          return;

        if (this.logItems.Count > this.logMaxCount)
        {
          var nitem = logItems[0];
          this.logItems.RemoveAt(0);
          UnityEngine.Object.Destroy(nitem.go);
        }

        if (message.Length > 32768)
          message = message.Substring(0, 32767);

        var logColor = Log.GetLogColor(level);
        var t = string.Format("<color=#{0}>{1}/{2} {3}</color>", Log.GetLogColor(level), Log.LogLevelToString(level), tag, message);

        var newEle = CloneUtils.CloneNewObjectWithParent(LogItemPrefab, this.LogContentView);
        var text = newEle.GetComponent<Text>();
        text.text = t;
        UIAnchorPosUtils.SetUIAnchor((RectTransform)newEle.transform, UIAnchor.Stretch, UIAnchor.Top);
        UIAnchorPosUtils.SetUIPivot((RectTransform)newEle.transform, UIPivot.TopLeft);

        //-日志点击事件
        EventTriggerListener.Get(newEle).onClick = (go) => this.ShowLogStackTrace(go.transform.GetSiblingIndex());

        var item = new LogItem();
        item.go = newEle;
        item.message = message;
        item.level = level;
        item.stackTrace = stackTrace;
        this.logItems.Add(item);

        //重新布局
        this.RelayoutLogContent();

        text.gameObject.SetActive(true);
        //滚动到末尾
        if (this.logAutoScroll)
          this.LogScrollView.normalizedPosition = Vector2.zero;
      }, LogLevel.All);

      this.logForceDisable = false;

      //命令服务的准备
      this.commandServer = GameManager.Instance.GameDebugCommandServer;
      commandServer.RegisterCommand("clear", (keyword, fullCmd, argsCount, args) => {
        ClearCommand();
        commandHistory.Clear();
        commandHistoryIndex = 0;
        return true;
      }, 0, "clear 清空控制台");

      //发送没有抓取到的日志
      Log.SendLogsInTemporary();

      commandHistory.Add("");
      commandInputListenKey1 = this.GameUIManager.ListenKey(KeyCode.UpArrow, (KeyCode key, bool downed) => {
        if(downed && CommandInputField.isFocused) UpCommandHistory();
      });
      commandInputListenKey2 = this.GameUIManager.ListenKey(KeyCode.DownArrow, (KeyCode key, bool downed) => {
        if(downed && CommandInputField.isFocused) DownCommandHistory();
      });
    }
    private void OnDestroy()
    {
      if(this.GameUIManager != null) {
        this.GameUIManager.DeleteKeyListen(commandInputListenKey1);
        this.GameUIManager.DeleteKeyListen(commandInputListenKey2);
      }
      logForceDisable = true;
      Log.UnRegisterLogObserver(logObserver);
    }

    //布局日志界面
    private void RelayoutLogContent()
    {
      var transform = LogContentView;
      var h = 0;
      var logFilterEmpty = logFilter == "";
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
          child.anchoredPosition = new Vector2(0, -h);
          UIAnchorPosUtils.SetUILeftBottom(child, 10, UIAnchorPosUtils.GetUIBottom(child));
          child.sizeDelta = new Vector2(0, 20);
          h = h + 20;
        }
        else
          child.gameObject.SetActive(false);
      }
      transform.sizeDelta = new Vector2(transform.sizeDelta.x, h < 200 ? 200 : h);
    }
    //清空日志条目
    private void ClearLogContent()
    {
      var transform = LogContentView;
      var c = transform.childCount;
      for (var i = c - 1; i >= 0; i--)
        UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
      transform.sizeDelta = new Vector2(transform.sizeDelta.x, 300);
    }
    //显示日志StackTrace
    private void ShowLogStackTrace(int index)
    {
      var data = logItems[index];
      LogStacktraceText.text = data.message + "\n= StackTrace ====\n" + data.stackTrace;
      LayoutRebuilder.ForceRebuildLayoutImmediate(LogStacktraceText.rectTransform);
      var size = LogStacktraceText.rectTransform.sizeDelta;
      LogStacktraceView.sizeDelta = new Vector2(size.x + 40, size.y + 40);
      LogStacktraceScrollView.normalizedPosition = new Vector2(0, 1);
    }
    private void SelectLastLog()
    {
      ShowLogStackTrace(LogContentView.childCount - 1);
    }
    
    //AutoScroll更改
    public void OnAutoScrollChanged()
    {
      logAutoScroll = ToggleAutoScroll.isOn;
    }
    //显示错误Checkbox更改事件
    public void OnShowErrCheckChange()
    {
      logShowErrAndWarn = CheckBoxWarningAndErr.isOn;
      RelayoutLogContent();
    }
    //显示信息Checkbox更改事件
    public void OnShowMessagesCheckChange()
    {
      logShowMessage = CheckBoxMessages.isOn;
      RelayoutLogContent();
    }
    //筛选Input输入完成事件
    public void OnFilterInputEndInput()
    {
      logFilter = FilterLogInputField.text;
      RelayoutLogContent();
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

    public void ForceGC() {
      System.GC.Collect();
      Log.D("Debug", "GC Finish");
    }
    public void OnShowSysInfoCheckChange()
    {
      GameManager.Instance.GameActionStore.CallAction("DbgStatShowSystemInfo", CheckBoxSysInfo.isOn);
    }
    public void OnShowMemInfoCheckChange()
    {
      GameManager.Instance.GameActionStore.CallAction("DbgStatShowStats", CheckBoxMemInfo.isOn);
    }

    public void CopyStacktrace()
    {
      UnityEngine.GUIUtility.systemCopyBuffer = LogStacktraceText.text;
      GameUIManager.GlobalToast("已复制到剪贴板", 1);
    }
    //执行命令
    public void ExecCommand()
    {
      if (commandServer.ExecuteCommand(CommandInputField.text))
      {
        commandHistory.Add(CommandInputField.text);
        commandHistoryIndex = 0;
        CommandInputField.text = "";
        SelectLastLog();
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
  }
}
